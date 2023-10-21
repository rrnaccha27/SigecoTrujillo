CREATE PROCEDURE dbo.up_regla_calculo_bono_listar
(
	 @p_codigo_tipo_planilla	INT
)
AS
BEGIN

	SELECT
		 rcb.codigo_regla_calculo_bono
		,tp.nombre as nombre_tipo_planilla
		,ISNULL(c.nombre, '') AS canal_nombre
		,ISNULL(g.nombre, '') AS grupo_nombre
		,CONVERT(VARCHAR(10), vigencia_inicio, 103) as vigencia_inicio
		,CONVERT(VARCHAR(10), vigencia_fin, 103) as vigencia_fin
		,CASE WHEN rcb.estado_registro = 1 THEN 'Activo' ELSE 'Inactivo' END AS estado_registro
		,CASE WHEN rcb.calcular_igv = 1 THEN 'Si' ELSE 'No' END AS calcular_igv
	FROM
		dbo.regla_calculo_bono rcb
	INNER JOIN
		dbo.tipo_planilla tp
		ON tp.codigo_tipo_planilla = rcb.codigo_tipo_planilla
	INNER JOIN
		dbo.canal_grupo c
		ON rcb.codigo_canal = c.codigo_canal_grupo
	LEFT JOIN
		dbo.canal_grupo g
		ON rcb.codigo_grupo = g.codigo_canal_grupo
	WHERE
		--rcb.estado_registro = 1 AND 
		( (@p_codigo_tipo_planilla = -1) OR (@p_codigo_tipo_planilla <> -1 AND rcb.codigo_tipo_planilla = @p_codigo_tipo_planilla) )
	ORDER BY
		rcb.estado_registro desc, rcb.codigo_tipo_planilla asc

END;