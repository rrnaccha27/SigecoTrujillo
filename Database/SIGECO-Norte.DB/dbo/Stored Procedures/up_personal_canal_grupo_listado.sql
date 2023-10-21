CREATE PROCEDURE dbo.up_personal_canal_grupo_listado
(
	@p_codigo_personal	INT
)
AS
BEGIN
DECLARE 
	@v_estado_personal BIT
	
	SET @v_estado_personal = (SELECT TOP 1 estado_registro FROM dbo.personal WHERE codigo_personal = @p_codigo_personal)
	
	--Grupos
	SELECT
		pcg.codigo_registro
		,pcg.codigo_personal
		,CONVERT(VARCHAR(10), grupo.codigo_padre) AS codigo_canal
		,'' AS nombre_canal
		,CONVERT(VARCHAR(10), grupo.codigo_canal_grupo) AS codigo_grupo
		,grupo.nombre AS nombre_grupo
		,CASE WHEN pcg.es_supervisor_canal = 1 THEN 'true' ELSE 'false' END AS es_supervisor_canal
		,CASE WHEN pcg.es_supervisor_grupo = 1 THEN 'true' ELSE 'false' END AS es_supervisor_grupo
		,CASE WHEN pcg.percibe_comision = 1 THEN 'true' ELSE 'false' END AS percibe_comision
		,CASE WHEN pcg.percibe_bono = 1 THEN 'true' ELSE 'false' END AS percibe_bono
		,CASE WHEN pcg.estado_registro = 1 THEN 'true' ELSE 'false' END AS estado_registro
	FROM
		dbo.personal_canal_grupo pcg
	INNER JOIN dbo.canal_grupo grupo
		ON grupo.codigo_canal_grupo = pcg.codigo_canal_grupo and grupo.es_canal_grupo = 0
	--INNER JOIN dbo.canal_grupo canal 
	--	ON grupo.codigo_padre = canal.codigo_canal_grupo
	WHERE 
		--pcg.estado_registro = 1 AND 
		pcg.codigo_personal = @p_codigo_personal
		--AND ( (@v_estado_personal = 1) OR (@v_estado_personal = 0 AND pcg.estado_registro = 1)  )
	UNION

	--Canales
	SELECT
		pcg.codigo_registro
		,pcg.codigo_personal
		,CONVERT(VARCHAR(10), canal.codigo_canal_grupo) AS codigo_canal
		,canal.nombre AS nombre_canal
		,'' AS codigo_grupo
		,'' AS nombre_grupo
		,CASE WHEN pcg.es_supervisor_canal = 1 THEN 'true' ELSE 'false' END AS es_supervisor_canal
		,CASE WHEN pcg.es_supervisor_grupo = 1 THEN 'true' ELSE 'false' END AS es_supervisor_grupo
		,CASE WHEN pcg.percibe_comision = 1 THEN 'true' ELSE 'false' END AS percibe_comision
		,CASE WHEN pcg.percibe_bono = 1 THEN 'true' ELSE 'false' END AS percibe_bono
		,CASE WHEN pcg.estado_registro = 1 THEN 'true' ELSE 'false' END AS estado_registro
	FROM
		dbo.personal_canal_grupo pcg
	INNER JOIN dbo.canal_grupo canal
		ON canal.codigo_canal_grupo = pcg.codigo_canal_grupo and canal.es_canal_grupo = 1
	WHERE 
		--pcg.estado_registro = 1 AND 
		pcg.codigo_personal = @p_codigo_personal
		--AND ( (@v_estado_personal = 1) OR (@v_estado_personal = 0 AND pcg.estado_registro = 1)  )
	ORDER BY 
		pcg.codigo_registro asc
END