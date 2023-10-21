CREATE PROCEDURE dbo.up_regla_calculo_bono_detalle
(
	 @p_codigo_regla_calculo_bono	INT
)
AS
BEGIN

	SELECT
		 TOP 1
		 codigo_regla_calculo_bono
		,tp.nombre as nombre_tipo_planilla
		,ISNULL(c.nombre, '') AS nombre_canal
		,ISNULL(g.nombre, '') AS nombre_grupo
		,CONVERT(VARCHAR(10), vigencia_inicio, 103) as vigencia_inicio
		,CONVERT(VARCHAR(10), vigencia_fin, 103) as vigencia_fin
		,monto_meta
		,porcentaje_pago
		,monto_tope
		,cantidad_ventas
		,calcular_igv
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
		--rcb.estado_registro = 1
		--AND 
		codigo_regla_calculo_bono = @p_codigo_regla_calculo_bono

END;