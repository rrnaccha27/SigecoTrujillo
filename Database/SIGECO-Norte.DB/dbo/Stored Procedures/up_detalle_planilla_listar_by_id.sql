create procedure [dbo].[up_detalle_planilla_listar_by_id](
@codigo_planilla int
)
as
begin
 
select 
dp.codigo_cronograma,
dp.codigo_planilla,
dp.observacion,
dp.codigo_detalle_planilla,
dp.codigo_detalle_cronograma,
a.nombre as nombre_articulo,
dp.fecha_pago,
dp.nro_cuota,
dp.monto_bruto,
dp.igv,
dp.monto_neto,
dp.nro_contrato,
tv.codigo_tipo_venta,
tv.nombre as nombre_tipo_venta,
tp.codigo_tipo_pago,
tp.nombre as nombre_tipo_pago,
p.apellido_paterno,
p.apellido_materno,
p.nombre as nombre_persona,
isnull(cg.nombre,' ') as nombre_grupo_canal,
c.nombre as nombre_canal,
emp.codigo_empresa,
emp.nombre as nombre_empresa,
dc.codigo_estado_cuota,
ec.nombre as nombre_estado_cuota,
dc.estado_registro
from 
detalle_planilla dp 
inner join detalle_cronograma dc on dp.codigo_detalle_cronograma=dc.codigo_detalle
inner join estado_cuota ec on dc.codigo_estado_cuota=ec.codigo_estado_cuota
inner join empresa_sigeco emp on emp.codigo_empresa=dp.codigo_empresa
inner join tipo_venta tv on tv.codigo_tipo_venta=dp.codigo_tipo_pago
inner join tipo_pago tp on tp.codigo_tipo_pago=dp.codigo_tipo_pago
inner join articulo a on dp.codigo_articulo=a.codigo_articulo
inner join personal p on p.codigo_personal=dp.codigo_personal
inner join canal_grupo c on c.codigo_canal_grupo=dp.codigo_canal
left join canal_grupo cg on cg.codigo_canal_grupo=dp.codigo_grupo
where dp.codigo_planilla=@codigo_planilla and dp.excluido=0 
end;