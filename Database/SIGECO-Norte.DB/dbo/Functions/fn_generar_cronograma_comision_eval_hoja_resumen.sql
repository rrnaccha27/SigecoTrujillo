CREATE FUNCTION [dbo].[fn_generar_cronograma_comision_eval_hoja_resumen]
(
	@p_nro_contrato VARCHAR(100)
)
RETURNS BIT
AS
BEGIN

	DECLARE
		@c_ParametroHR	INT = 19 -- VALOR DE PARAMETRO
	
	DECLARE
		@c_HojaResumen	VARCHAR(10) = (SELECT TOP 1 valor from parametro_sistema where codigo_parametro_sistema = @c_ParametroHR)

	DECLARE
		@v_esContratoAdicional	BIT = 0

	SET @v_esContratoAdicional = CASE WHEN LEFT(@p_nro_contrato, LEN(@c_HojaResumen)) = @c_HojaResumen THEN 1 ELSE 0 END

	RETURN @v_esContratoAdicional
END