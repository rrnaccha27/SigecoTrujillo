CREATE FUNCTION [dbo].[fn_formatear_fecha_hora]
(
	@p_fecha	DATETIME
)
RETURNS VARCHAR(25) WITH SCHEMABINDING
BEGIN
	DECLARE		
		@v_retorno VARCHAR(25) = ''

	IF NOT (@p_fecha IS NULL)
		SET @v_retorno = CONVERT(VARCHAR, @p_fecha, 103) + ' ' + CONVERT(VARCHAR, @p_fecha, 108)

	RETURN @v_retorno;
END;