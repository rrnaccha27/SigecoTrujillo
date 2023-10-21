CREATE PROCEDURE dbo.up_personal_validacion_ruc
(
	@p_codigo_personal	INT
	,@p_nro_ruc			VARCHAR(11)
)
AS
BEGIN

	SELECT TOP 1
		codigo_personal
	FROM
		dbo.personal
	WHERE
		codigo_personal <> @p_codigo_personal
		AND nro_ruc = @p_nro_ruc
		AND estado_registro = 1
END