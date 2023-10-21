CREATE PROC dbo.sp_canal_grupo_reporte
@fecha_inicio datetime,
@fecha_fin datetime,
@p_es_canal_grupo	BIT,
@p_codigo_padre	INT
AS
BEGIN
SELECT 
		cg.codigo_canal_grupo
		,cg.nombre
		,ISNULL(p.codigo_personal, 0) AS codigo_personal
		,ISNULL(p.nombre + ' ' +p.apellido_paterno + ' ' + p.apellido_materno, '') AS nombre_personal
		,ISNULL((SELECT TOP  1 CASE WHEN c.percibe = 1 THEN 'Si' ELSE 'No' END FROM dbo.configuracion_canal_grupo c WHERE cg.codigo_canal_grupo = c.codigo_canal_grupo AND c.supervisor_personal = 1 AND c.comision_bono = 1),'No') AS s_percibe_comision
		,ISNULL((SELECT TOP  1 CASE WHEN c.percibe = 1 THEN 'Si' ELSE 'No' END FROM dbo.configuracion_canal_grupo c WHERE cg.codigo_canal_grupo = c.codigo_canal_grupo AND c.supervisor_personal = 1 AND c.comision_bono = 0),'No') AS s_percibe_bono
		,ISNULL((SELECT TOP  1 CASE WHEN c.percibe = 1 THEN 'Si' ELSE 'No' END FROM dbo.configuracion_canal_grupo c WHERE cg.codigo_canal_grupo = c.codigo_canal_grupo AND c.supervisor_personal = 0 AND c.comision_bono = 1),'No') AS p_percibe_comision
		,ISNULL((SELECT TOP  1 CASE WHEN c.percibe = 1 THEN 'Si' ELSE 'No' END FROM dbo.configuracion_canal_grupo c WHERE cg.codigo_canal_grupo = c.codigo_canal_grupo AND c.supervisor_personal = 0 AND c.comision_bono = 0),'No') AS p_percibe_bono
		,CASE WHEN cg.administra_grupos = 1 THEN 'Si' ELSE 'No' END administra_grupos
	FROM 
		dbo.canal_grupo cg
	LEFT JOIN 
		dbo.personal_canal_grupo pcg
		on pcg.estado_registro = 1 AND pcg.codigo_canal_grupo = cg.codigo_canal_grupo AND ( (@p_es_canal_grupo = 1 AND pcg.es_supervisor_canal = 1) OR (@p_es_canal_grupo = 0 AND pcg.es_supervisor_grupo = 1) )
	LEFT JOIN 
		dbo.personal p
		on p.estado_registro = 1 AND p.codigo_personal = pcg.codigo_personal 
	WHERE 
		--cg.es_canal_grupo = 1
		--AND 
		cg.estado_registro = 1
		AND ( (@p_es_canal_grupo <> 0 AND cg.es_canal_grupo = @p_es_canal_grupo) OR (@p_es_canal_grupo = 0 AND (cg.codigo_padre = @p_codigo_padre)) )
		AND (cg.fecha_registra between @fecha_inicio AND @fecha_fin)
	ORDER BY 
		cg.nombre
END