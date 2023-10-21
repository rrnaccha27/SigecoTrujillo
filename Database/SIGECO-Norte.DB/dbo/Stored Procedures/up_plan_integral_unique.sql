CREATE PROCEDURE dbo.up_plan_integral_unique
(
	 @p_codigo_plan_integral	INT
)
AS
BEGIN

	SELECT
		 TOP 1
		 codigo_plan_integral
		,nombre
		,estado_registro
		,CONVERT(VARCHAR(10), vigencia_inicio, 103) as vigencia_inicio
		,CONVERT(VARCHAR(10), vigencia_fin, 103) as vigencia_fin
	FROM
		dbo.plan_integral
	WHERE
		codigo_plan_integral = @p_codigo_plan_integral

END