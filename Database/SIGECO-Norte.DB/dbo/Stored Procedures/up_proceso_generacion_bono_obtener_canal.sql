CREATE PROCEDURE dbo.up_proceso_generacion_bono_obtener_canal
(
	@p_es_planilla_jn BIT = 0
)
AS
BEGIN
	SET NOCOUNT ON
	SELECT 
		c.nombre AS nombre
		,c.codigo_canal_grupo AS codigo
	FROM
		dbo.canal_grupo c
	INNER JOIN
		(SELECT DISTINCT 
			codigo_canal
		FROM
			dbo.regla_calculo_bono
		WHERE estado_registro = 1 AND es_jn = @p_es_planilla_jn) r 
		ON r.codigo_canal = c.codigo_canal_grupo
	WHERE 
		c.es_canal_grupo = 1
		and c.estado_registro = 1
	SET NOCOUNT OFF
END