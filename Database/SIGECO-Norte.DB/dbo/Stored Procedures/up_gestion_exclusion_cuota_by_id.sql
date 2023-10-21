CREATE PROCEDURE up_gestion_exclusion_cuota_by_id
(
	@codigo_exclusion int
)
AS
BEGIN

	select top 1
		dc.codigo_detalle,
		e.nombre as nombre_empresa,
		c.nombre as nombre_canal,
		g.nombre as nombre_grupo,
		a.nombre as nombre_articulo,
		tv.nombre as nombre_tipo_venta,
		tp.nombre as nombre_tipo_pago,
		dp.nro_cuota,
		dp.monto_bruto,
		dp.monto_neto,
		dp.igv,
		dp.nro_contrato,	
		isnull(ec.codigo_estado_cuota, 0) as codigo_estado_cuota,
		isnull(ec.nombre, '') as nombre_estado_cuota,
		(case when ex.estado_exclusion=1 then 'Activo' else 'Inactivo' end) nombre_estado_exclusion ,
		ex.codigo_exclusion,
		dbo.fn_obtener_nombre_usuario(ex.usuario_exclusion) usuario_exclusion,	
		ex.fecha_exclusion,
		ex.motivo_exclusion,	
		dbo.fn_obtener_nombre_usuario(ex.usuario_habilita) usuario_habilita,
		ex.fecha_habilita,
		ex.motivo_habilita
	from 
		exclusion_cuota_planilla ex 
	inner join detalle_planilla dp on ex.codigo_detalle_planilla=dp.codigo_detalle_planilla
	inner join canal_grupo c on dp.codigo_canal=c.codigo_canal_grupo
	inner join empresa_sigeco e on dp.codigo_empresa=e.codigo_empresa
	left join canal_grupo g on dp.codigo_grupo=g.codigo_canal_grupo
	inner join articulo a on dp.codigo_articulo=a.codigo_articulo
	inner join tipo_venta tv on tv.codigo_tipo_venta=dp.codigo_tipo_venta
	inner join tipo_pago tp on tp.codigo_tipo_pago=dp.codigo_tipo_pago
	left join detalle_cronograma dc on dc.codigo_detalle=ex.codigo_detalle_cronograma
	left join estado_cuota ec on ec.codigo_estado_cuota=dc.codigo_estado_cuota
	where 
		ex.codigo_exclusion=@codigo_exclusion;

END;