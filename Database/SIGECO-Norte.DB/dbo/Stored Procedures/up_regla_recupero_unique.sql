CREATE PROCEDURE dbo.up_regla_recupero_unique
(
	 @p_codigo_regla_recupero	INT
)
AS
BEGIN

	SELECT
		 TOP 1
		 codigo_regla_recupero
		,nombre
		,nro_cuota
		,estado_registro
		,CONVERT(VARCHAR(10), vigencia_inicio, 103) as vigencia_inicio
		,CONVERT(VARCHAR(10), vigencia_fin, 103) as vigencia_fin
	FROM
		dbo.regla_recupero
	WHERE
		codigo_regla_recupero = @p_codigo_regla_recupero

END