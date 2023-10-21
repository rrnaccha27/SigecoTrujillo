create procedure up_reporte_liquidacion_bono_supervisor
 (
 @v_codigo_planilla int,
 @v_codigo_personal int
 )
 as
 begin
 
declare  
--@v_codigo_canal int, 
 @v_limite_detraccion decimal(10,2),
 @v_porcentaje_igv decimal(10,2),
 @v_porcentaje_detraccion decimal(10,2);

  select @v_porcentaje_detraccion=convert(decimal(10,2),valor)  from parametro_sistema where codigo_parametro_sistema=15;
 select @v_limite_detraccion=convert(decimal(10,2),valor)  from parametro_sistema where codigo_parametro_sistema=16;
 select @v_porcentaje_igv=convert(decimal(10,2),valor)  from parametro_sistema where codigo_parametro_sistema=9;

 
 set @v_porcentaje_igv=@v_porcentaje_igv/100;
 set @v_porcentaje_detraccion=(@v_porcentaje_detraccion/100);


 declare @t_contrato_planilla table( 
 codigo_planila int,
 codigo_personal int, 
 codigo_empresa int, 
 monto_contratado decimal(10,2),
 monto_ingresado decimal(10,2),  
 porcentaje_meta decimal(10,2),
 monto_bono decimal(10,2) ,
 codigo_supervisor int
 );

insert into @t_contrato_planilla
select 
r.codigo_planilla,
r.codigo_personal,
r.codigo_empresa,
r.monto_contratado,
r.monto_ingresado,
 
 (select top 1 rpb.porcentaje_pago from resumen_planilla_bono rpb where rpb.codigo_personal=codigo_personal) porcentaje_pago,
 0,codigo_supervisor
from contrato_planilla_bono r where r.codigo_planilla=@v_codigo_planilla;


 update @t_contrato_planilla set
 monto_bono=(porcentaje_meta/100)*monto_ingresado;
 
 /*********************************************************/
  declare @t_resumen table(
 codigo_supervisor int,
 codigo_empresa int,
 monto_total decimal(10,2),
 monto_detraccion  decimal(10,2),
 monto_valor_venta  decimal(10,2),
 monto_igv decimal(10,2),
 monto_neto_pagar decimal(10,2)
 );

 insert into @t_resumen
 select 
     codigo_supervisor,
	 codigo_empresa,
	sum(monto_bono) bono_empresa,
	0,
	0,
	0,
	0	
 from @t_contrato_planilla 
group by codigo_supervisor,codigo_empresa;
/*******************************************************************************/
update @t_resumen set monto_valor_venta=monto_total/(1+@v_porcentaje_igv);
update @t_resumen set monto_igv=monto_valor_venta*@v_porcentaje_igv;
update @t_resumen set monto_detraccion=(case when monto_valor_venta>=@v_limite_detraccion then monto_total*@v_porcentaje_detraccion else 0 end);
update @t_resumen set monto_neto_pagar=monto_valor_venta+monto_igv-isnull(monto_detraccion,0);


select 
    dp.codigo_personal,
	dp.codigo_empresa,	
	(isnull(s.nombre,' ')+' '+isnull(s.apellido_paterno,' ')+' '+isnull(s.apellido_materno,' ')) as datos_supervisor,
	s.nro_documento,
	td.nombre_tipo_documento,
	em.ruc,
	em.nombre_largo as nombre_empresa_largo,
	em.direccion_fiscal,
	em.nombre as nombre_empresa,
	g.nombre as nombre_grupo,
	----------------------
	r.monto_valor_venta,
	r.monto_igv,
	r.monto_detraccion,
	r.monto_neto_pagar

from detalle_planilla_bono dp inner join @t_resumen r on dp.codigo_personal=r.codigo_supervisor and dp.codigo_empresa=r.codigo_empresa
inner join  personal s on dp.codigo_personal=s.codigo_personal
inner join empresa_sigeco em on em.codigo_empresa=dp.codigo_empresa
inner join tipo_documento td on td.codigo_tipo_documento=s.codigo_tipo_documento
left join canal_grupo g on dp.codigo_grupo=g.codigo_canal_grupo;

end;