CREATE PROCEDURE dbo.up_comision_manual_listado
(
	@p_codigo_empresa		INT
	,@p_codigo_canal_grupo	INT
	,@p_fecha_inicio		VARCHAR(10)
	,@p_fecha_fin			VARCHAR(10)
	,@p_usuario				VARCHAR(20)
	,@p_estado_registro		INT
)
AS
BEGIN
	SELECT
		cm.codigo_comision_manual
		,p.nombre + ISNULL(' ' + p.apellido_paterno, '') + ISNULL(' ' + p.apellido_materno, '') AS nombre_personal
		,cm.nombre + ' ' + cm.apellido_paterno + ' ' + cm.apellido_materno AS nombre_fallecido
		,ISNULL(cm.nro_contrato, '') as nro_contrato
		,ISNULL(e.nombre, '') as nombre_empresa
		,CONVERT(VARCHAR(10), cm.fecha_registra, 103) + ' ' + CONVERT(VARCHAR(10), cm.fecha_registra, 108) as fecha_registra
		,cm.estado_registro
		,CASE WHEN cm.estado_registro = 1 THEN 'Activo' ELSE 'Inactivo' END AS nombre_estado_registro
		,cm.codigo_estado_cuota
		,ec.nombre as nombre_estado_cuota
		,ep.nombre as nombre_estado_proceso
		,ISNULL(cm.nro_factura_vendedor, '') AS nro_factura_vendedor
		,UPPER(cm.usuario_registra) as usuario_registra
	FROM
		dbo.comision_manual cm
	INNER JOIN dbo.personal p
		ON p.codigo_personal = cm.codigo_personal
	INNER JOIN dbo.estado_cuota ec
		ON ec.codigo_estado_cuota = cm.codigo_estado_cuota
	INNER JOIN dbo.estado_proceso ep
		ON ep.codigo_estado_proceso = cm.codigo_estado_proceso
	LEFT JOIN dbo.empresa_sigeco e
		ON e.codigo_empresa = cm.codigo_empresa
	WHERE
		( @p_codigo_empresa = 0 OR (@p_codigo_empresa > 0 AND cm.codigo_empresa = @p_codigo_empresa) )
		AND ( @p_codigo_canal_grupo = 0 OR (@p_codigo_canal_grupo > 0 AND cm.codigo_canal = @p_codigo_canal_grupo) )
		AND ( (@p_fecha_inicio IS NULL) OR (NOT @p_fecha_inicio IS NULL AND cm.fecha_registra BETWEEN CONVERT(DATETIME, @p_fecha_inicio) AND CONVERT(DATETIME, @p_fecha_fin + ' 23:59:59')) )
		AND (LEN(@p_usuario) = 0 OR (LEN(@p_usuario) > 0 and UPPER(cm.usuario_registra) = UPPER(@p_usuario)))
		AND (@p_estado_registro = -1 OR (@p_estado_registro <> -1 AND cm.estado_registro = CONVERT(BIT, @p_estado_registro)))
	ORDER BY
		cm.fecha_registra
		,nombre_personal

END