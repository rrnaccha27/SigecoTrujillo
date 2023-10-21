CREATE PROCEDURE dbo.up_descuento_comision_validar
(
	@p_codigo_personal	INT
	,@p_codigo_empresa	INT
	,@p_codigo_descuento_comision	INT OUTPUT
)
AS
BEGIN
	SET NOCOUNT ON

	SET @p_codigo_descuento_comision = 0

	SELECT TOP 1
		@p_codigo_descuento_comision = codigo_descuento_comision
	FROM
		dbo.descuento_comision
	WHERE
		estado_registro = 1
		AND codigo_personal = @p_codigo_personal
		AND codigo_empresa = @p_codigo_empresa
		AND saldo > 0

	SET NOCOUNT OFF
END;