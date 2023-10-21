CREATE PROC dbo.sp_regla_pago_comision_listado
(
	@codigo_empresa int,
	@codigo_canal_grupo int,
	@codigo_tipo_venta int,
	@codigo_tipo_pago int
)
AS
BEGIN
	SELECT 
		r.*, e.nombre nombre_empresa, cg.nombre nombre_canal_grupo, tp.nombre nombre_tipo_pago,
		tv.nombre nombre_tipo_venta, c.nombre nombre_campo_santo, a.nombre AS nombre_articulo,
		CASE 
			WHEN GETDATE() BETWEEN r.fecha_inicio AND CONVERT(DATETIME, CONVERT(VARCHAR(10), r.fecha_fin, 112) + ' 23:59:59') THEN 'Vigente'
			WHEN GETDATE() < r.fecha_inicio THEN 'Pendiente'
			WHEN GETDATE() > CONVERT(DATETIME, CONVERT(VARCHAR(10), r.fecha_fin, 112) + ' 23:59:59') THEN 'Vencida'
		END AS vigencia,
		CASE WHEN r.estado_registro = 1 THEN 'Activo' ELSE 'Inactivo' END AS estado_registro_nombre
	FROM 
		dbo.regla_pago_comision r
	LEFT JOIN dbo.empresa_sigeco e ON e.codigo_empresa = r.codigo_empresa 
	LEFT JOIN dbo.canal_grupo cg ON cg.codigo_canal_grupo = r.codigo_canal_grupo 
	LEFT JOIN dbo.tipo_venta tv ON tv.codigo_tipo_venta = r.codigo_tipo_venta 
	LEFT JOIN dbo.tipo_pago tp ON tp.codigo_tipo_pago = r.codigo_tipo_pago 
	LEFT JOIN dbo.campo_santo_sigeco c ON c.codigo_campo_santo = r.codigo_campo_santo
	LEFT JOIN dbo.articulo a ON a.codigo_articulo = r.codigo_articulo
	WHERE
	--r.estado_registro = 1 AND 
	(@codigo_empresa IS NULL OR (r.codigo_empresa = @codigo_empresa))
	AND (@codigo_canal_grupo IS NULL OR (r.codigo_canal_grupo = @codigo_canal_grupo))
	AND (@codigo_tipo_venta IS NULL OR (r.codigo_tipo_venta = @codigo_tipo_venta))
	AND (@codigo_tipo_pago IS NULL OR (r.codigo_tipo_pago = @codigo_tipo_pago))
	ORDER BY r.nombre_regla_pago, e.nombre, cg.nombre, tv.nombre
END