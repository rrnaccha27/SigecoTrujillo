CREATE PROC dbo.sp_regla_calculo_comision_supervisor_get_by_id
(
@codigo_regla int
)
AS
BEGIN
	SELECT 
		r.*, e.nombre nombre_empresa, cg.nombre nombre_canal_grupo, c.nombre nombre_campo_santo
	FROM regla_calculo_comision_supervisor r	
	INNER JOIN empresa_sigeco e
			ON r.codigo_empresa = e.codigo_empresa  
	INNER JOIN canal_grupo cg
		ON r.codigo_canal_grupo = cg.codigo_canal_grupo
	LEFT JOIN campo_santo_sigeco c
		ON r.codigo_campo_santo = c.codigo_campo_santo
	WHERE 
		r.codigo_regla = @codigo_regla
		AND r.estado_registro = 1 
END