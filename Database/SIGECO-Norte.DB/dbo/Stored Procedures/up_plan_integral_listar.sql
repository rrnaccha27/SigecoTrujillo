CREATE PROCEDURE dbo.up_plan_integral_listar
(
	 @p_estado_registro	INT
)
AS
BEGIN

	SELECT
		 pint.codigo_plan_integral
		,pint.nombre
		,pint.estado_registro
		,CASE WHEN pint.estado_registro = 1 THEN 'Activo' ELSE 'Inactivo' END AS estado_nombre
		,CONVERT(VARCHAR(10), pint.vigencia_inicio, 103) as vigencia_inicio
		,CONVERT(VARCHAR(10), pint.vigencia_fin, 103) as vigencia_fin
	FROM
		dbo.plan_integral pint
	WHERE
		( (@p_estado_registro = -1) OR (@p_estado_registro <> -1 AND pint.estado_registro = @p_estado_registro) )

END