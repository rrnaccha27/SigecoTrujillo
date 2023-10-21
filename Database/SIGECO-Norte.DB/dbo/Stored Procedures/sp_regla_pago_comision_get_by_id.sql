CREATE PROC dbo.sp_regla_pago_comision_get_by_id
(
	@codigo_regla_pago int
)
AS
BEGIN
	SELECT 
		r.*, 
		ISNULL(e.nombre, '') AS nombre_empresa, ISNULL(cg.nombre, '') AS  nombre_canal_grupo, ISNULL(tp.nombre, '') AS nombre_tipo_pago,
		ISNULL(tv.nombre, '') AS nombre_tipo_venta, ISNULL(c.nombre, '') AS nombre_campo_santo, ISNULL(a.nombre, '') AS nombre_articulo,
		ISNULL(ta.nombre, '') AS nombre_tipo_articulo, ISNULL(ta2.nombre, '') AS nombre_tipo_articulo_anexado,
		CONVERT(INT, r.evaluar_plan_integral) AS evaluar_plan_integral_b, CONVERT(INT, r.evaluar_anexado) AS evaluar_anexado_b
	FROM 
		dbo.regla_pago_comision r
	LEFT JOIN dbo.empresa_sigeco e ON e.codigo_empresa = r.codigo_empresa 
	LEFT JOIN dbo.canal_grupo cg ON cg.codigo_canal_grupo = r.codigo_canal_grupo 
	LEFT JOIN dbo.tipo_venta tv ON tv.codigo_tipo_venta = r.codigo_tipo_venta 
	LEFT JOIN dbo.tipo_pago tp ON tp.codigo_tipo_pago = r.codigo_tipo_pago 
	LEFT JOIN dbo.campo_santo_sigeco c ON c.codigo_campo_santo = r.codigo_campo_santo
	LEFT JOIN dbo.articulo a ON a.codigo_articulo = r.codigo_articulo
	LEFT JOIN dbo.tipo_articulo ta ON ta.codigo_tipo_articulo = r.codigo_tipo_articulo
	LEFT JOIN dbo.tipo_articulo ta2 ON ta2.codigo_tipo_articulo = r.codigo_tipo_articulo_anexado
	WHERE 
		r.codigo_regla_pago = @codigo_regla_pago-- AND r.estado_registro = 1 
END