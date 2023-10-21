CREATE PROCEDURE dbo.up_comision_manual_detalle
(
	@p_codigo_comision_manual	INT
)
AS
BEGIN
	SELECT
		cm.codigo_comision_manual
		,cm.codigo_tipo_documento
		,td.nombre_tipo_documento
		,cm.nro_documento
		,cm.nombre
		,cm.apellido_paterno
		,cm.apellido_materno
		,cm.codigo_personal
		,cm.nombre + ISNULL(' ' + cm.apellido_paterno, '') + ISNULL(' ' + cm.apellido_materno, '') AS nombre_completo_fallecido
		,p.nombre + ISNULL(' ' + p.apellido_paterno, '') + ISNULL(' ' + p.apellido_materno, '') AS nombre_personal
		,cm.codigo_canal
		,cg.nombre AS nombre_canal
		,cm.codigo_empresa
		,e.nombre AS nombre_empresa
		,cm.nro_contrato
		,cm.codigo_articulo
		,a.nombre AS nombre_articulo
		,cm.comentario
		,cm.comision_sin_igv
		,cm.igv
		,cm.comision
		,cm.codigo_tipo_pago
		,tp.nombre as nombre_tipo_pago
		,cm.codigo_tipo_venta
		,tv.nombre as nombre_tipo_venta
		,cm.nro_factura_vendedor
		,ep.nombre as nombre_estado_proceso
		,ISNULL(pl.numero_planilla, '') + ' ' + ISNULL(rtp.nombre, '') AS nombre_planilla
	FROM
		dbo.comision_manual cm
	INNER JOIN dbo.personal p
		ON cm.codigo_comision_manual = @p_codigo_comision_manual AND p.codigo_personal= cm.codigo_personal
	INNER JOIN dbo.canal_grupo cg
		ON cg.codigo_canal_grupo = cm.codigo_canal
	INNER JOIN dbo.articulo a
		ON a.codigo_articulo = cm.codigo_articulo
	INNER JOIN tipo_pago tp 
		ON cm.codigo_tipo_pago=tp.codigo_tipo_pago
	INNER JOIN tipo_venta tv 
		ON cm.codigo_tipo_venta=tv.codigo_tipo_venta
	INNER JOIN dbo.empresa_sigeco e
		ON e.codigo_empresa = cm.codigo_empresa
	INNER JOIN dbo.tipo_documento td
		ON td.codigo_tipo_documento = cm.codigo_tipo_documento
	INNER JOIN dbo.estado_proceso ep
		ON ep.codigo_estado_proceso = cm.codigo_estado_proceso
	LEFT JOIN dbo.planilla pl
		ON pl.codigo_planilla = cm.codigo_planilla
	LEFT JOIN dbo.regla_tipo_planilla rtp
		ON rtp.codigo_regla_tipo_planilla = pl.codigo_regla_tipo_planilla
		
	WHERE
		cm.codigo_comision_manual = @p_codigo_comision_manual
END;