CREATE PROCEDURE dbo.up_descuento_comision_detalle_planilla_listar
(
	@p_codigo_descuento_comision	INT
)
AS
BEGIN
	SET NOCOUNT ON

	SELECT 
		p.numero_planilla AS nro_planilla
		,CONVERT(VARCHAR, p.fecha_inicio, 103) AS fecha_inicio
		,CONVERT(VARCHAR, p.fecha_fin, 103) AS fecha_fin
		,rtp.nombre as nombre_planilla
		,tp.nombre as nombre_tipo_planilla
		,ep.nombre as estado_planilla
		,d.monto
		,CONVERT(VARCHAR, d.fecha_registra, 103) AS fecha_registra
		,dbo.fn_obtener_nombre_usuario(d.usuario_registra) AS usuario_registra
	FROM 
		descuento_comision dc
	INNER JOIN dbo.descuento d 
		ON d.codigo_descuento_comision = dc.codigo_descuento_comision and d.estado_registro = 1
	INNER JOIN dbo.planilla p
		ON p.codigo_planilla = d.codigo_planilla
	INNER JOIN dbo.estado_planilla ep
		ON ep.codigo_estado_planilla = p.codigo_estado_planilla
	INNER JOIN dbo.regla_tipo_planilla rtp
		ON rtp.codigo_regla_tipo_planilla = p.codigo_regla_tipo_planilla
	INNER JOIN dbo.tipo_planilla tp
		ON tp.codigo_tipo_planilla = p.codigo_tipo_planilla
	WHERE
		dc.codigo_descuento_comision = @p_codigo_descuento_comision
	ORDER BY
		p.codigo_planilla DESC

	SET NOCOUNT OFF
END;