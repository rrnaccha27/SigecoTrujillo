CREATE PROCEDURE dbo.up_generar_cronograma_validar_asociado
(
	@p_codigo_empresa	NVARCHAR(4)
	,@p_nro_contrato	NVARCHAR(200)
	,@p_codigo_articulo	INT
	,@p_genera_comision	BIT OUTPUT
)
AS
BEGIN
SET NOEXEC OFF;
DECLARE
	@v_tiene_asociado			BIT
	,@v_codigo_empresa_asociado	NVARCHAR(4)
	,@v_nro_contrato_asociado	NVARCHAR(200)
	,@v_anio_asociado			INT
	,@v_anio_validacion			INT
	
	SET @p_genera_comision = 1
	
	SET @v_anio_validacion = (SELECT TOP 1 anio_contrato_vinculante FROM dbo.articulo WHERE codigo_articulo = @p_codigo_articulo and tiene_contrato_vinculante = 1)
	
	IF (@v_anio_validacion IS NULL)
		SET NOEXEC ON;

	SELECT
		@v_tiene_asociado = 1
		,@v_codigo_empresa_asociado = Cod_Empresa_Referencia
		,@v_nro_contrato_asociado = Num_Contrato_Referencia
		--,@v_anio_asociado = DATEPART(YEAR, CreateDate)
	FROM
		dbo.cabecera_contrato
	WHERE
		Codigo_empresa = @p_codigo_empresa
		AND NumAtCard = @p_nro_contrato
	
	IF (LEN(ISNULL(@v_codigo_empresa_asociado,'')) = 0 OR LEN(ISNULL(@v_nro_contrato_asociado,'')) = 0)
		SET NOEXEC ON;
	
	IF (@v_tiene_asociado IS NULL)
		SET NOEXEC ON;
	
	WHILE (@v_tiene_asociado = 1)
	BEGIN
		SET @v_tiene_asociado = NULL
		
		SELECT
			@v_tiene_asociado = 1
			,@v_codigo_empresa_asociado = Cod_Empresa_Referencia
			,@v_nro_contrato_asociado = Num_Contrato_Referencia
			,@v_anio_asociado = DATEPART(YEAR, CreateDate)
		FROM
			dbo.cabecera_contrato
		WHERE
			Codigo_empresa = @v_codigo_empresa_asociado
			AND NumAtCard = @v_nro_contrato_asociado		

		IF (@v_tiene_asociado IS NULL)
			BREAK

		IF (LEN(ISNULL(@v_codigo_empresa_asociado,'')) = 0 OR LEN(ISNULL(@v_nro_contrato_asociado,'')) = 0)
			BREAK	
	END
	
	IF (@v_anio_asociado IS NOT NULL)
	BEGIN
		IF (@v_anio_asociado <= @v_anio_validacion)
			SET @p_genera_comision = 0	
	END
	
SET NOEXEC OFF;
END