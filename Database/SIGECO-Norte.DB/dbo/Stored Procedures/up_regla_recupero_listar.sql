CREATE PROCEDURE dbo.up_regla_recupero_listar
(
	 @p_estado_registro	INT
)
AS
BEGIN

	SELECT
		 codigo_regla_recupero
		,nombre
		,nro_cuota
		,estado_registro
		,CASE WHEN estado_registro = 1 THEN 'Activo' ELSE 'Inactivo' END AS estado_nombre
		,CONVERT(VARCHAR(10), vigencia_inicio, 103) as vigencia_inicio
		,CONVERT(VARCHAR(10), vigencia_fin, 103) as vigencia_fin
	FROM
		dbo.regla_recupero
	WHERE
		( (@p_estado_registro = -1) OR (@p_estado_registro <> -1 AND estado_registro = @p_estado_registro) )

END