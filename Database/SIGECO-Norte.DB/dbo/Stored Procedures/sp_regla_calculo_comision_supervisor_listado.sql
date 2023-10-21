create PROC dbo.sp_regla_calculo_comision_supervisor_listado
(
@codigo_campo_santo int,
@codigo_empresa int,
@codigo_canal_grupo int
)
AS
BEGIN
	SELECT 
		r.*, e.nombre nombre_empresa, cg.nombre nombre_canal_grupo, c.nombre nombre_campo_santo
		,CASE WHEN r.estado_registro = 1 THEN 'Activo' ELSE 'Inactivo' END AS estado_registro_str
		,CASE WHEN r.incluye_igv = 1 THEN 'Si' ELSE 'No' END AS incluye_igv_str
	FROM 
		regla_calculo_comision_supervisor r
	LEFT JOIN empresa_sigeco e
		ON r.codigo_empresa = e.codigo_empresa  
	LEFT JOIN canal_grupo cg
		ON r.codigo_canal_grupo = cg.codigo_canal_grupo
	LEFT JOIN campo_santo_sigeco c
		ON r.codigo_campo_santo = c.codigo_campo_santo
	WHERE 
		--r.estado_registro = 1 AND 
		(@codigo_campo_santo = 0 OR (r.codigo_campo_santo = @codigo_campo_santo))
		AND (@codigo_empresa = 0 OR (r.codigo_empresa = @codigo_empresa))
		AND (@codigo_canal_grupo = 0 OR (r.codigo_canal_grupo = @codigo_canal_grupo))
	ORDER BY r.nombre, c.nombre, e.nombre, cg.nombre
END