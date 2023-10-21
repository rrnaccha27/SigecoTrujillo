CREATE PROCEDURE dbo.up_personal_validacion_documento
(
	@p_codigo_personal			INT
	,@p_codigo_tipo_documento	INT
	,@p_nro_documento			VARCHAR(15)
)
AS
BEGIN

	SELECT TOP 1
		codigo_personal
	FROM
		dbo.personal
	WHERE
		codigo_personal <> @p_codigo_personal
		AND codigo_tipo_documento = @p_codigo_tipo_documento
		AND nro_documento = @p_nro_documento
		AND estado_registro = 1
END