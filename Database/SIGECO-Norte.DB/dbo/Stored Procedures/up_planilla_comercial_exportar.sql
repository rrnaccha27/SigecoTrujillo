create proc up_planilla_comercial_exportar
(
@p_codigo_planilla int
)
as
begin
declare @tb_descuento table
(
codigo_personal int,
codigo_empresa int,
descuento decimal(10,2)
);
declare @v_estado_planilla int,
        @v_limite_detraccion decimal(10,2),
		@v_porcentaje_detraccion decimal(10,2),
        @v_codigo_tipo_planilla int;

select 
	@v_estado_planilla=codigo_estado_planilla ,
	@v_codigo_tipo_planilla=codigo_tipo_planilla
from planilla where codigo_planilla=@p_codigo_planilla;

select 
	@v_limite_detraccion=valor 
from 
	parametro_sistema 
where 
	codigo_parametro_sistema=16;

select 
	@v_porcentaje_detraccion=valor 
from 
	parametro_sistema 
where codigo_parametro_sistema=15;

set @v_porcentaje_detraccion=(@v_porcentaje_detraccion/100);


insert into @tb_descuento
select 
   codigo_personal,
   codigo_empresa,
   sum(monto) 
from descuento
where codigo_planilla=@p_codigo_planilla and estado_registro=1
group by codigo_empresa,codigo_personal;


declare @resumen table
(
codigo_empresa int,
nombre_empresa varchar(200), 
 
codigo_canal int,
nombre_canal  varchar(200), 

codigo_grupo int ,
nombre_grupo  varchar(200), 

codigo_personal int,
personal  varchar(200),
monto_bruto decimal,
igv decimal,  
monto_neto decimal,
calcular_detraccion bit  
);

insert @resumen
select 
 dp.codigo_empresa,
 max(e.nombre) as nombre_empresa, 
 
 dp.codigo_canal,
 max(c.nombre) as nombre_canal,

 dp.codigo_grupo,
 max(cg.nombre) as nombre_grupo,

 p.codigo_personal,
 isnull(max(p.nombre),'')+' '+isnull(max(p.apellido_paterno),'')+' '+ isnull(max(p.apellido_materno),'') as apellido_materno,
 sum(dp.monto_bruto) as monto_bruto,
 sum(dp.igv) as igv,  
 sum(dp.monto_neto) as monto_neto,
 dbo.fn_canal_grupo_percibe_factura(isnull(dp.codigo_grupo,dp.codigo_canal),dp.codigo_empresa,case when @v_codigo_tipo_planilla = 1 then 0 else 1 end,1) aplicac_detraccion  
from detalle_planilla dp
inner join detalle_cronograma dc on dp.codigo_detalle_cronograma=dc.codigo_detalle
inner join empresa_sigeco e on e.codigo_empresa=dp.codigo_empresa
inner join personal p on p.codigo_personal=dp.codigo_personal
inner  join canal_grupo c on c.codigo_canal_grupo=dp.codigo_canal
left join canal_grupo cg on dp.codigo_grupo=cg.codigo_canal_grupo
where dp.codigo_planilla=@p_codigo_planilla and  dc.codigo_estado_cuota  in(2,3) and dc.estado_registro=1 and dp.excluido=0 and dp.estado_registro=1
group by dp.codigo_empresa,dp.codigo_canal, dp.codigo_grupo,p.codigo_personal;



select 
 r.codigo_empresa,
 r.nombre_empresa, 
 @v_estado_planilla as codigo_estado_planilla,
 r.codigo_canal,
 r.nombre_canal,

 r.codigo_grupo,
 r.nombre_grupo,

 r.codigo_personal,
 r.personal, 
 r.monto_bruto,
 r.igv,  
 --r.monto_neto,  
 isnull(d.descuento,0) +
 (case when @v_limite_detraccion<(r.monto_neto-isnull(d.descuento,0)) and calcular_detraccion=1
	       then (r.monto_neto-isnull(d.descuento,0))*@v_porcentaje_detraccion else 0 end) descuento,
r.monto_neto-(isnull(d.descuento,0) +
 (case when @v_limite_detraccion<(r.monto_neto-isnull(d.descuento,0)) and calcular_detraccion=1
	       then (r.monto_neto-isnull(d.descuento,0))*@v_porcentaje_detraccion else 0 end)) monto_neto

from @resumen r
left join @tb_descuento d
on r.codigo_empresa=d.codigo_empresa and r.codigo_personal=d.codigo_personal

end;