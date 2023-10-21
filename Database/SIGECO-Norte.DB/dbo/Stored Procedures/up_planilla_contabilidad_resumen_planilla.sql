CREATE PROCEDURE [dbo].[up_planilla_contabilidad_resumen_planilla]
(
	@p_codigo_planilla	INT
)
AS
BEGIN
	SET NOCOUNT ON

	SELECT 
		dp.codigo_planilla
		,dp.codigo_empresa
		,e.nombre as nombre_empresa
		,count(e.codigo_empresa) as comisiones
	FROM
		dbo.detalle_planilla dp
	INNER JOIN dbo.empresa_sigeco e
		ON e.codigo_empresa = dp.codigo_empresa
	INNER JOIN dbo.detalle_cronograma dc
		ON dc.codigo_detalle = dp.codigo_detalle_cronograma and dc.codigo_estado_cuota = 3
	WHERE
		dp.codigo_planilla = @p_codigo_planilla
		and dc.es_registro_manual_comision = 0
		and dp.excluido = 0
		and dc.codigo_estado_cuota = 3
	GROUP BY
		dp.codigo_planilla
		,dp.codigo_empresa
		,e.nombre
	ORDER BY
		dp.codigo_empresa

	SET NOCOUNT OFF
END;