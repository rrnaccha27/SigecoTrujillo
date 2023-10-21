CREATE PROCEDURE [dbo].[up_reclamo_ValidaContrato]
	@codigo_empresa		INT
	,@nro_contrato		VARCHAR(20)
	,@p_codigo_personal	INT
AS
BEGIN
	DECLARE 
		@codigo_equivalencia_empresa	NVARCHAR(4)
		,@v_codigo_vendedor	NVARCHAR(10)

	SELECT @codigo_equivalencia_empresa = codigo_equivalencia FROM dbo.empresa_sigeco WHERE codigo_empresa = @codigo_empresa
	SET @v_codigo_vendedor = (SELECT TOP 1 codigo_equivalencia FROM dbo.personal WHERE codigo_personal = @p_codigo_personal)

	IF EXISTS(SELECT * FROM cabecera_contrato 
	WHERE Codigo_empresa=@codigo_equivalencia_empresa AND NumAtCard = @nro_contrato AND Cod_Vendedor = @v_codigo_vendedor)
	BEGIN
		SELECT 1
	END
	ELSE
	BEGIN
		SELECT 0
	END
END