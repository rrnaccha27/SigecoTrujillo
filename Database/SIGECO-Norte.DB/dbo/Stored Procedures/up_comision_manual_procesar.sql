CREATE PROCEDURE [dbo].[up_comision_manual_procesar]
(
	@p_codigo_comision_manual	INT,
	@p_resultado				VARCHAR(200) OUTPUT 
)
AS
BEGIN

	SET @p_resultado = ''

	DECLARE
		@v_nro_documento				VARCHAR(15)
		,@v_codigo_tipo_documento		INT
		,@v_codigo_tipo_documento_j		VARCHAR(4)
		,@v_codigo_empresa				INT
		,@v_codigo_empresa_j			VARCHAR(4)
		,@v_numatcard					VARCHAR(100)
		,@v_codigo_personal				INT
		,@v_codigo_canal				INT
		,@v_codigo_articulo				INT
		,@v_codigo_tipo_venta			INT
		,@v_codigo_tipo_pago			INT
		,@v_comision					DECIMAL(10, 2)

	DECLARE
		@v_contrato_valido				BIT = 0
		,@v_articulo_valido				BIT = 0
		,@v_codigo_cronograma			INT
		,@v_codigo_detalle_cronograma	INT

	SELECT TOP 1
		@v_nro_documento = nro_documento
		,@v_codigo_tipo_documento = codigo_tipo_documento
		,@v_codigo_empresa = codigo_empresa
		,@v_numatcard = nro_contrato
		,@v_codigo_empresa = codigo_empresa
		,@v_codigo_personal = codigo_personal
		,@v_codigo_canal = codigo_canal
		,@v_codigo_articulo = codigo_articulo
		,@v_codigo_tipo_venta = codigo_tipo_venta
		,@v_codigo_tipo_pago = codigo_tipo_pago
		,@v_comision = comision
	FROM
		dbo.comision_manual
	WHERE
		codigo_comision_manual = @p_codigo_comision_manual

	SET @v_codigo_tipo_documento_j = (SELECT TOP 1 codigo_equivalencia FROM dbo.tipo_documento WHERE codigo_tipo_documento = @v_codigo_tipo_documento and estado_registro = 1)
	SET @v_codigo_empresa_j = (SELECT TOP 1 codigo_equivalencia FROM dbo.empresa_sigeco WHERE codigo_empresa = @v_codigo_empresa)

	SELECT 
		@v_codigo_empresa_j = CODIGO_EMPRESA
		,@v_numatcard = NUMATCARD
	FROM 
		dbo.difunto_contrato 
	WHERE 
		NUM_DOC = @v_nro_documento
		AND CODIGO_DOC = @v_codigo_tipo_documento_j

	IF (LEN(ISNULL(@v_codigo_empresa_j, '')) = 0 OR LEN(ISNULL(@v_numatcard, '')) = 0)
	BEGIN
		IF LEN(ISNULL(@v_numatcard, '')) > 0
		BEGIN
			SELECT 
				@v_codigo_empresa_j = Codigo_empresa
				,@v_numatcard = NumAtCard
			FROM 
				dbo.cabecera_contrato
			WHERE 
				Codigo_empresa = @v_codigo_empresa_j
				AND NumAtCard = @v_numatcard
		END
	END

	IF (LEN(ISNULL(@v_codigo_empresa_j, '')) = 0 OR LEN(ISNULL(@v_numatcard, '')) = 0)
	BEGIN
		RETURN;
	END

	SELECT
		@v_contrato_valido = 1
	FROM
		dbo.cabecera_contrato cc
	--INNER JOIN dbo.personal p 
	--	ON p.codigo_equivalencia = cc.Cod_Vendedor
	INNER JOIN dbo.tipo_venta tv
		ON cc.Cod_Tipo_Venta = tv.codigo_equivalencia
	--INNER JOIN dbo.tipo_pago tp
	--	ON cc.Cod_FormaPago = tp.codigo_equivalencia
	WHERE
		cc.NumAtCard = @v_numatcard
		AND cc.Codigo_empresa = @v_codigo_empresa_j
		--AND	p.codigo_personal = @v_codigo_personal
		AND tv.codigo_tipo_venta = @v_codigo_tipo_venta
		--AND tp.codigo_tipo_pago = @v_codigo_tipo_pago

	IF (@v_contrato_valido = 0)
	BEGIN
		--SET @p_resultado = 'Los datos del registro no corresponden al contrato.'
		RETURN;
	END

	SELECT
		@v_codigo_cronograma = codigo_cronograma 
	FROM
		dbo.cronograma_pago_comision
	WHERE
		codigo_empresa = @v_codigo_empresa
		AND nro_contrato = @v_numatcard
		AND codigo_tipo_planilla = 1

	IF (ISNULL(@v_codigo_cronograma, 0) = 0)
	BEGIN
		SET @p_resultado = 'No tiene cronograma de comision el contrato.'
		RETURN;
	END

	SELECT 
		@v_articulo_valido = 1
	FROM 
		dbo.articulo_cronograma 
	WHERE 
		codigo_cronograma = @v_codigo_cronograma
		AND codigo_articulo = @v_codigo_articulo
		AND monto_comision = @v_comision

	IF ISNULL(@v_articulo_valido, 0) = 0
	BEGIN
		SET @p_resultado = 'No coincide el monto de comision del articulo con el contrato.'
		RETURN;
	END

	IF EXISTS(SELECT TOP 1 codigo_detalle FROM dbo.detalle_cronograma WHERE codigo_cronograma = @v_codigo_cronograma AND codigo_articulo = @v_codigo_articulo and codigo_estado_cuota = 3)
	BEGIN
		SET @p_resultado = 'La comision se encuentra ya pagada.'
		RETURN;
	END

	SELECT TOP 1
		@v_codigo_detalle_cronograma = codigo_detalle
	FROM 
		dbo.detalle_cronograma 
	WHERE 
		codigo_cronograma = @v_codigo_cronograma
		AND codigo_articulo = @v_codigo_articulo

	UPDATE 
		dbo.detalle_cronograma
	SET
		es_registro_manual_comision = 1
	WHERE
		codigo_cronograma = @v_codigo_cronograma
		AND codigo_articulo = @v_codigo_articulo

	UPDATE 
		dbo.comision_manual
	SET
		codigo_detalle_cronograma = @v_codigo_detalle_cronograma
		,nro_contrato = @v_numatcard
	WHERE
		codigo_comision_manual = @p_codigo_comision_manual
	
END;