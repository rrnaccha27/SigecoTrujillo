CREATE FUNCTION dbo.fn_EsContratoDoble
(
	@p_nro_contrato		VARCHAR(100)
	,@p_codigo_empresa	VARCHAR(4)
)
RETURNS BIT
AS
BEGIN
	DECLARE
		@v_EsContratoDoble	BIT = 0
		,@c_OFSA			CHAR(4) = '0002'
		,@c_FUNJAR			CHAR(4) = '0939'


	SET @v_EsContratoDoble = CASE WHEN EXISTS(SELECT TOP 1 NumAtCard FROM dbo.cabecera_contrato WHERE NumAtCard = @p_nro_contrato AND Codigo_empresa = CASE WHEN @p_codigo_empresa	= @c_OFSA THEN @c_FUNJAR ELSE @c_OFSA END ) THEN 1 ELSE 0 END
		
	RETURN @v_EsContratoDoble

END