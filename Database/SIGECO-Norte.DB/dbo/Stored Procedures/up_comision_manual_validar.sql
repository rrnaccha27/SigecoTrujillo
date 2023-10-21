CREATE PROC dbo.up_comision_manual_validar
(
	@p_codigo_comision_manual	int,
	@p_codigo_tipo_documento	int ,
	@p_nro_documento			varchar(15) ,
	@p_codigo_empresa			int	,
	@p_nro_contrato				varchar(100) ,
	@p_codigo_articulo			int, 
	@p_codigo_personal			int 
)
AS
BEGIN
	SET NOCOUNT ON

	DECLARE
		@v_codigo_vendedor	VARCHAR(10)
		,@v_codigo_empresa	VARCHAR(4)

	IF EXISTS(
		SELECT TOP 1
			codigo_comision_manual
		FROM
			dbo.comision_manual
		WHERE
			estado_registro = 1
			AND codigo_tipo_documento = @p_codigo_tipo_documento
			AND nro_documento = @p_nro_documento
			AND codigo_articulo = @p_codigo_articulo
			AND codigo_empresa = @p_codigo_empresa
			AND codigo_comision_manual <> @p_codigo_comision_manual
			)
	BEGIN
		SELECT 'Existe un registro de comision para este articulo al fallecido ingresado.' AS mensaje
		RETURN
	END

	--IF (ISNULL(@p_codigo_empresa, 0) > 0 AND ISNULL(@p_nro_contrato, '') <> '')
	--BEGIN
	--	SET @v_codigo_vendedor = (SELECT TOP 1 codigo_equivalencia FROM dbo.personal WHERE codigo_personal = @p_codigo_personal)
	--	SET @v_codigo_empresa = (SELECT TOP 1 codigo_equivalencia FROM dbo.empresa_sigeco WHERE codigo_empresa = @p_codigo_empresa)

	--	IF NOT EXISTS(SELECT TOP 1 Codigo_empresa FROM dbo.cabecera_contrato WHERE NumAtCard = @p_nro_contrato AND Codigo_empresa = @v_codigo_empresa AND Cod_Vendedor = @v_codigo_vendedor)
	--	BEGIN
	--		SELECT 'El contrato no pertenece al Vendedor seleccionado.' AS mensaje
	--		RETURN
	--	END
	--END

	--IF (ISNULL(@p_codigo_empresa, 0) > 0 AND ISNULL(@p_nro_contrato, '') <> '')
	--BEGIN
	--	IF NOT EXISTS(SELECT TOP 1 codigo_cronograma FROM dbo.cronograma_pago_comision WHERE nro_contrato = @p_nro_contrato AND codigo_empresa = @p_codigo_empresa)
	--	BEGIN
	--		SELECT 'El contrato ya cuenta con un cronograma de pagos.' AS mensaje
	--		RETURN
	--	END
	--END

	SET NOCOUNT OFF
END