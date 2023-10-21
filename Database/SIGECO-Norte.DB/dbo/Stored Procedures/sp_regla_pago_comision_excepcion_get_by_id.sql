CREATE PROC dbo.sp_regla_pago_comision_excepcion_get_by_id
(
	@codigo_regla int
)
AS
BEGIN
	SELECT r.*, e.nombre nombre_empresa, cg.nombre nombre_canal_grupo, c.nombre nombre_campo_santo, a.nombre nombre_articulo
	FROM regla_pago_comision_excepcion r, empresa_sigeco e, canal_grupo cg, campo_santo_sigeco c, articulo a
	WHERE r.codigo_empresa = e.codigo_empresa AND r.codigo_canal_grupo = cg.codigo_canal_grupo
	AND r.codigo_campo_santo = c.codigo_campo_santo AND r.codigo_regla = @codigo_regla
	AND r.estado_registro = 1 AND r.codigo_articulo = a.codigo_articulo
END