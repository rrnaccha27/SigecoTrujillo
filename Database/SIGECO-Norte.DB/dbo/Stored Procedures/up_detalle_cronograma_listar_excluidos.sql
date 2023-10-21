create procedure up_detalle_cronograma_listar_excluidos
as
begin
select 
dc.codigo_detalle,
dc.nro_cuota,
dc.fecha_programada,
dc.monto_bruto,
dc.monto_neto,
dc.igv,
art.nombre as nombre_articulo,
cpc.nro_contrato,
tv.codigo_tipo_venta,
tv.nombre as nombre_tipo_venta,
tp.codigo_tipo_pago,
tp.nombre as nombre_tipo_pago,
p.apellido_materno,
p.apellido_paterno,
p.nombre as nombre_persona,
c.nombre as nombre_canal,
isnull(g.nombre,' ') as nombre_grupo

from detalle_cronograma dc 
inner join articulo art on dc.codigo_articulo=art.codigo_articulo
inner join cronograma_pago_comision cpc on cpc.codigo_cronograma=dc.codigo_cronograma
inner join personal_canal_grupo pcg on pcg.codigo_registro=cpc.codigo_personal_canal_grupo
inner join canal_grupo c on c.codigo_canal_grupo=pcg.codigo_canal
left join canal_grupo g on pcg.codigo_canal_grupo=g.codigo_canal_grupo 
inner join personal p on p.codigo_personal=pcg.codigo_personal
inner join tipo_venta tv on tv.codigo_tipo_venta=cpc.codigo_tipo_venta
inner join tipo_pago tp on tp.codigo_tipo_pago=cpc.codigo_tipo_pago
where dc.codigo_estado_cuota=4

end;