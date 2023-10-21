CREATE PROCEDURE [dbo].up_detalle_planilla_excluido_by_id
(
	@codigo_planilla int
)
AS
BEGIN
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
		dbo.fn_obtener_nombre_usuario(ex.usuario_exclusion) as usuario_exclusion,
		ex.fecha_exclusion,
		ex.motivo_exclusion,
		dbo.fn_obtener_nombre_usuario(ex.usuario_habilita) as usuario_habilita,
		ex.motivo_habilita,
		(case when ex.estado_exclusion=1 then 'Activo' else 'Inactivo' end) nombre_estado_exclusion
		,CONVERT(INT, dp.es_transferencia) AS es_transferencia
	from 
		exclusion_cuota_planilla ex inner join
	detalle_planilla dp  on (ex.codigo_detalle_planilla=dp.codigo_detalle_planilla and ex.codigo_planilla=@codigo_planilla)
	inner join empresa_sigeco emp on dp.codigo_empresa= emp.codigo_empresa
	inner join tipo_venta tv on dp.codigo_tipo_pago=tv.codigo_tipo_venta
	inner join tipo_pago tp on  dp.codigo_tipo_pago=tp.codigo_tipo_pago
	inner join articulo a on    dp.codigo_articulo=a.codigo_articulo
	inner join personal p on    dp.codigo_personal=p.codigo_personal
	inner join canal_grupo c on dp.codigo_canal=c.codigo_canal_grupo
	left join canal_grupo cg on dp.codigo_grupo=cg.codigo_canal_grupo
	where 
		dp.codigo_planilla=@codigo_planilla 
		AND dp.excluido=1; 

END;