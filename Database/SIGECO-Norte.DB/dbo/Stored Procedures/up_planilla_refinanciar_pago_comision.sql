create proc [dbo].[up_planilla_refinanciar_pago_comision]
(
@codigo_detalle_cronograma int,
@monto_financiar decimal(10,2),
@numero_cuota int,
@usuario_registra varchar(50)
)
as
begin

declare @v_count_cuota int,
        @v_numero_cuota_maxima int,
        @v_codigo_cronograma int,
		@v_codigo_articulo int,     
		@v_codigo_tipo_cuota int,   
		@v_codigo_estado_cuota int,
		@v_saldo_actual decimal(10,2),
		@v_codigo_estado_cuota_auxiliar int,
		@v_codigo_planilla int,
		@v_cuota_mes decimal(10,2),
		@v_igv decimal(10,2),
		@v_monto_igv decimal(10,2),
		@v_monto_sin_igv decimal(10,2),
		-------------------------------
	    @v_mensaje varchar(200),
		@v_nro_contrato varchar(20),
        @v_codigo_equivalencia_empresa varchar(20),
		@v_total_nro_cuota_contrato int,
		@v_total_nro_cuota_comision int,
		-------------------------------
		@v_fecha_pago datetime ,
		@v_codidgo_detalle_planilla_anterior int;
		declare @v_codigo_detalle_cronograma_actual int;
		------------------------------	
set @v_count_cuota = 1;
set @v_fecha_pago=null;


/*******************************************************
PARA REFINANCIAR UNA CUOTA SOLO ESTA PERMITIDO EN ESTADO
1.-Pendiente
2.-En Proceso Pago
********************************************************/
select 
@v_codigo_cronograma=codigo_cronograma,
@v_saldo_actual=monto_neto,
@v_codigo_articulo=codigo_articulo,
@v_codigo_tipo_cuota=codigo_tipo_cuota,
@v_codigo_estado_cuota=codigo_estado_cuota,
@v_codigo_estado_cuota_auxiliar= codigo_estado_cuota 
from detalle_cronograma where codigo_detalle=@codigo_detalle_cronograma;


-----------------------------------------------------------------------
select 
	 --@v_fecha_pago=max(fecha_programada) ,
	 @v_numero_cuota_maxima=max(nro_cuota)
 from detalle_cronograma 
 where codigo_cronograma=@v_codigo_cronograma 
 and codigo_articulo=@v_codigo_articulo and codigo_estado_cuota in(1,2,3,4);

 
/***********************************************************************/
if @monto_financiar>@v_saldo_actual
begin
  set @v_mensaje='El monto a refinanciar ('+convert(varchar(20),@monto_financiar)+') debe ser menor que el monto de la comisión ('+convert(varchar(20),@v_saldo_actual)+')';
   RAISERROR(@v_mensaje,16,1); 
	   return;
end;

if (@v_codigo_estado_cuota_auxiliar=1 or @v_codigo_estado_cuota_auxiliar=2)
 begin
  set @v_nro_contrato='';
  --set @v_codigo_estado_cuota_auxiliar=1;-- print 'Permite realizar refinanciamiento de cuota.'
 end
 else
 begin
       RAISERROR('El refinanciamiento de la cuota solo esta permitido en estado Pendiente de Pago y En Proceso de Pago.',16,1); 
	   return;
 end;
 /************************************************************/ 

 select 
  @v_nro_contrato=cpc.nro_contrato,
  @v_codigo_equivalencia_empresa=e.codigo_equivalencia
from detalle_cronograma dp inner join cronograma_pago_comision cpc
on dp.codigo_cronograma=cpc.codigo_cronograma
inner join empresa_sigeco e on cpc.codigo_empresa=e.codigo_empresa
where dp.codigo_detalle=@codigo_detalle_cronograma;
/**********************************************************
OBTENIENDO EL NRO DE CUOTA TOTAL DEL CONTRATO
***********************************************************/
select 
	@v_total_nro_cuota_contrato=isnull(max(cc.Num_Cuota),0)+1
from contrato_cuota cc 
where cc.NumAtCard=@v_nro_contrato and cc.Codigo_empresa=@v_codigo_equivalencia_empresa and UPPER(cc.Cod_Estado) in('C','P') ;

/**************************************************************/
if @numero_cuota>@v_total_nro_cuota_contrato
begin
 set @v_mensaje='El nro de cuota a refinanciar ('+convert(varchar(20),@numero_cuota)+'), supera al nro. de cuota del contrato('+convert(varchar(20),isnull(@v_total_nro_cuota_contrato,0))+')';	 
     RAISERROR(@v_mensaje,16,1); 
	 return;
end;

 if @numero_cuota>(isnull(@v_total_nro_cuota_contrato,0)-isnull(@v_numero_cuota_maxima,0))
 begin    
     set @v_mensaje='El nro de cuota a refinanciar ('+convert(varchar(20),@numero_cuota)+'), supera al nro. de cuotas libres del contrato('+convert(varchar(20),isnull(@v_total_nro_cuota_contrato,0)-isnull(@v_numero_cuota_maxima,0))+')';	 
     RAISERROR(@v_mensaje,16,1); 
	 return;
 end;
 



/********************************************************
 OBTENIENDO EL PORCENTAJE DE IGV
*********************************************************/
select 
 @v_igv=valor 
from  parametro_sistema 
where codigo_parametro_sistema=9;
set @v_igv=@v_igv/100;

/*****************************************************
 actualizando operacio del pago
******************************************************/

 update operacion_cuota_comision 
 set estado_registro=0
 where codigo_detalle_cronograma=@codigo_detalle_cronograma and estado_registro=1;
 /**********************************************************************
   ANULANDO LA CUOTA POR REFINANCIAMIENTO
 ***********************************************************************/
 update detalle_cronograma set codigo_estado_cuota=5
 where codigo_detalle=@codigo_detalle_cronograma;
  /**********************************************************************
   INSERTANDO EL DETALLE DE LA OPERACION 
 ***********************************************************************/
 insert into operacion_cuota_comision(codigo_detalle_cronograma,codigo_tipo_operacion_cuota,motivo_movimiento,fecha_movimiento,usuario_registra,fecha_registra,estado_registro)
 values(@codigo_detalle_cronograma,5,'Anulando cuota por refinanciamiento',GETDATE(),@usuario_registra,GETDATE(),1);

/*******************************************************/
set @v_saldo_actual=@v_saldo_actual-@monto_financiar;

/****************************************
  CALCULANDO CUOTAS DEL MES
 *****************************************/

 set @v_cuota_mes=@monto_financiar/@numero_cuota;
 set @v_monto_igv=@v_cuota_mes*@v_igv;
 set @v_monto_sin_igv=@v_cuota_mes-@v_monto_igv;

/******************************************************
Si la cuota esta en estado en proceso de pago
debe modificar detalle planilla en estado abierto.
*******************************************************/

if @v_codigo_estado_cuota_auxiliar=2
begin
	select 
		@v_codigo_planilla=dp.codigo_planilla ,
		@v_codidgo_detalle_planilla_anterior=dp.codigo_detalle_planilla
	from detalle_planilla dp inner join planilla p
	on dp.codigo_planilla=p.codigo_planilla
	where dp.codigo_detalle_cronograma=@codigo_detalle_cronograma and p.codigo_estado_planilla=1;
	---------------------------------------------------------------------------------------------
	  

	  insert into detalle_cronograma(
	  codigo_cronograma,
	  codigo_articulo,
	  nro_cuota,
	  fecha_programada,
	  monto_bruto,
	  igv,
	  monto_neto,
	  codigo_tipo_cuota,
	  codigo_estado_cuota)
	  select 
	  codigo_cronograma,
	  codigo_articulo,
	  nro_cuota,
	  fecha_programada,
	 case when @v_saldo_actual=0 then @v_monto_sin_igv else @v_saldo_actual-@v_igv*@v_saldo_actual end,	
	 case when @v_saldo_actual=0 then @v_monto_igv else @v_igv*@v_saldo_actual end,	  
	 case when @v_saldo_actual=0 then @v_cuota_mes else @v_saldo_actual end,	  
	  codigo_tipo_cuota,
	  2
	  from 
	  detalle_cronograma 
	  where codigo_detalle=@codigo_detalle_cronograma;
	  ---------------------------------------------
	 
	 set @v_codigo_detalle_cronograma_actual=@@IDENTITY;
 --------------------------------------------------------------------------------------------
	  insert into detalle_planilla(
	  codigo_planilla,
	  codigo_articulo,
	  codigo_canal,
	  codigo_grupo,
	  codigo_empresa,
	  codigo_personal,
	  codigo_moneda,
	  nro_contrato,
	  codigo_tipo_venta,
	  codigo_tipo_pago,
	  codigo_cronograma,
	  codigo_detalle_cronograma,
	  excluido,
	  nro_cuota,
	  fecha_pago,
	  monto_bruto,
	  igv,
	  monto_neto,
	  estado_registro,
	  fecha_registra,
	  usuario_registra
	  )
      select 
	  codigo_planilla,
	  codigo_articulo,
	  codigo_canal,
	  codigo_grupo,
	  codigo_empresa,
	  codigo_personal,
	  codigo_moneda,
	  nro_contrato,
	  codigo_tipo_venta,
	  codigo_tipo_pago,
	  codigo_cronograma,
	  @v_codigo_detalle_cronograma_actual,
	  0,
	  nro_cuota,
	  fecha_pago,	 
	  case when @v_saldo_actual=0 then @v_monto_sin_igv else @v_saldo_actual-@v_igv*@v_saldo_actual end,	
	  case when @v_saldo_actual=0 then @v_monto_igv else @v_igv*@v_saldo_actual end,	  
	  case when @v_saldo_actual=0 then @v_cuota_mes else @v_saldo_actual end,	
	  1,
	  GETDATE(),
	  @usuario_registra	  
	  from detalle_planilla where codigo_detalle_planilla=@v_codidgo_detalle_planilla_anterior;
 
      insert into operacion_cuota_comision(codigo_detalle_cronograma,codigo_tipo_operacion_cuota,motivo_movimiento,fecha_movimiento,usuario_registra,fecha_registra,estado_registro)
      values(@v_codigo_detalle_cronograma_actual,2,'Agregando cuota a planilla por refinanciamiento',GETDATE(),@usuario_registra,GETDATE(),1);
	  

	 
	  if @v_saldo_actual=0 and @v_count_cuota>0
	   begin
	     set  @v_count_cuota=@v_count_cuota+1;
	   end
	   else
	   begin
	      set @v_count_cuota=@v_count_cuota+case when @v_count_cuota=1 then 0 else 1 end;
	   end;
	  --set @v_count_cuota=@v_count_cuota+case when @v_saldo_actual=0 then 1 else 1 end;
end;
if @v_codigo_estado_cuota_auxiliar=1 and @v_saldo_actual>0
 begin
 	  insert into detalle_cronograma(
	  codigo_cronograma,
	  codigo_articulo,
	  nro_cuota,
	  fecha_programada,
	  monto_bruto,
	  igv,
	  monto_neto,
	  codigo_tipo_cuota,
	  codigo_estado_cuota)
	  select 
	  codigo_cronograma,
	  codigo_articulo,
	  nro_cuota,
	  fecha_programada,
	  @v_saldo_actual-@v_igv*@v_saldo_actual ,	
	  @v_igv*@v_saldo_actual ,	  
	  @v_saldo_actual ,	  
	  codigo_tipo_cuota,
	  1
	  from 
	  detalle_cronograma 
	  where codigo_detalle=@codigo_detalle_cronograma;
	  ---------------------------------------------	 
	 set @v_codigo_detalle_cronograma_actual=@@IDENTITY;
 --------------------------------------------------------------------------------------------  
 
      insert into operacion_cuota_comision(codigo_detalle_cronograma,codigo_tipo_operacion_cuota,motivo_movimiento,fecha_movimiento,usuario_registra,fecha_registra,estado_registro)
      values(@v_codigo_detalle_cronograma_actual,1,'Agregando nueva cuota por refinanciamiento',GETDATE(),@usuario_registra,GETDATE(),1);
	  
	  set @v_count_cuota=@v_count_cuota+case when @v_count_cuota=1 then 0 else 1 end;
 end;
 

while @v_count_cuota<=@numero_cuota
begin
--set @v_fecha_pago=DateAdd(month,1,@v_fecha_pago);
set @v_numero_cuota_maxima=@v_numero_cuota_maxima+1;
insert into detalle_cronograma(
codigo_cronograma,
codigo_articulo,
nro_cuota,
fecha_programada,
monto_bruto,
igv,
monto_neto,
codigo_tipo_cuota,
codigo_estado_cuota)
values(
@v_codigo_cronograma,
@v_codigo_articulo,
@v_numero_cuota_maxima,
@v_fecha_pago,
@v_monto_sin_igv,
@v_monto_igv,
@v_cuota_mes,
@v_codigo_tipo_cuota,
1
);
set @v_count_cuota =@v_count_cuota+ 1;
end;

end;