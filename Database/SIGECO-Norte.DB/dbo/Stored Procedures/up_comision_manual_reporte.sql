CREATE PROC dbo.up_comision_manual_reporte
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
	SET NOCOUNT ON
	SELECT 
		CONVERT(VARCHAR, cm.fecha_registra, 103) as fecha_registra
		,e.nombre as empresa
		,tv.abreviatura as tipo_venta
		,tp.nombre as tipo_pago
		,p.nombre +  ISNULL(' ' + p.apellido_paterno, ' ') + ISNULL(' ' + p.apellido_materno, ' ') as vendedor
		,cg.nombre as canal
		,a.nombre as articulo
		,cm.comision
		,cm.nro_contrato
		,cm.nro_factura_vendedor
		,UPPER(cm.usuario_registra) as usuario_registra
	FROM 
		dbo.comision_manual cm
	INNER JOIN dbo.empresa_sigeco e ON e.codigo_empresa = cm.codigo_empresa
	INNER JOIN dbo.tipo_venta tv ON tv.codigo_tipo_venta = cm.codigo_tipo_venta
	INNER JOIN dbo.tipo_pago tp ON tp.codigo_tipo_pago = cm.codigo_tipo_pago
	INNER JOIN dbo.personal p on p.codigo_personal = cm.codigo_personal
	INNER JOIN dbo.canal_grupo cg on cg.codigo_canal_grupo = cm.codigo_canal
	INNER JOIN dbo.articulo a ON a.codigo_articulo = cm.codigo_articulo
	WHERE
		( @p_codigo_empresa = 0 OR (@p_codigo_empresa > 0 AND cm.codigo_empresa = @p_codigo_empresa) )
		AND ( @p_codigo_canal_grupo = 0 OR (@p_codigo_canal_grupo > 0 AND cm.codigo_canal = @p_codigo_canal_grupo) )
		AND ( (@p_fecha_inicio IS NULL) OR (NOT @p_fecha_inicio IS NULL AND cm.fecha_registra BETWEEN CONVERT(DATETIME, @p_fecha_inicio) AND CONVERT(DATETIME, @p_fecha_fin + ' 23:59:59')) )
		AND (LEN(@p_usuario) = 0 OR (LEN(@p_usuario) > 0 and UPPER(cm.usuario_registra) = UPPER(@p_usuario)))
		AND (@p_estado_registro = -1 OR (@p_estado_registro <> -1 AND cm.estado_registro = CONVERT(BIT, @p_estado_registro)))
	ORDER BY
		fecha_registra ASC
		, empresa DESC
		, tipo_venta ASC
		, tipo_pago ASC
		, vendedor ASC
	SET NOCOUNT OFF
END