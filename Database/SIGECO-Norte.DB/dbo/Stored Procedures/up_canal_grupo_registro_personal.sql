CREATE PROCEDURE dbo.up_canal_grupo_registro_personal
(
	@p_es_canal_grupo	INT
)
AS
BEGIN

	SELECT
		CASE WHEN cg.es_canal_grupo = 1 THEN 0 ELSE cg.codigo_padre END AS codigo_padre
		--,ISNULL(cg.codigo_padre, 0) as codigo_padre
		,cg.codigo_canal_grupo
		,cg.nombre
		,CONVERT(BIT, ISNULL((SELECT TOP 1 c.percibe FROM dbo.configuracion_canal_grupo c WHERE c.codigo_canal_grupo = cg.codigo_canal_grupo and c.supervisor_personal = 1 and c.comision_bono = 1), 0)) AS supervisor_percibe_comision
		,CONVERT(BIT, ISNULL((SELECT TOP 1 c.percibe FROM dbo.configuracion_canal_grupo c WHERE c.codigo_canal_grupo = cg.codigo_canal_grupo and c.supervisor_personal = 1 and c.comision_bono = 0), 0)) AS supervisor_percibe_bono
		,CONVERT(BIT, ISNULL((SELECT TOP 1 c.percibe FROM dbo.configuracion_canal_grupo c WHERE c.codigo_canal_grupo = cg.codigo_canal_grupo and c.supervisor_personal = 0 and c.comision_bono = 1), 0)) AS personal_percibe_comision
		,CONVERT(BIT, ISNULL((SELECT TOP 1 c.percibe FROM dbo.configuracion_canal_grupo c WHERE c.codigo_canal_grupo = cg.codigo_canal_grupo and c.supervisor_personal = 0 and c.comision_bono = 0), 0)) AS personal_percibe_bono
	FROM
		dbo.canal_grupo cg
	WHERE
		( cg.es_canal_grupo = @p_es_canal_grupo )
		AND cg.estado_registro = 1
END