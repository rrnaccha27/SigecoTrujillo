CREATE FUNCTION dbo.fn_generar_cronograma_comision_eval_anexado
(
	@p_nro_contrato						VARCHAR(100)
	,@p_codigo_empresa_o				VARCHAR(4)
	,@v_codigo_tipo_articulo_anexado	INT
)
RETURNS BIT
AS
BEGIN
	DECLARE
		@v_retorno					BIT = 0,
		@v_codigo_empresa_anexado	VARCHAR(4),
		@v_nro_contrato_anexado		VARCHAR(100),
		@v_cantidad_registros		INT = 0

	SELECT TOP 1
		@v_codigo_empresa_anexado = Cod_Empresa_Referencia
		,@v_nro_contrato_anexado = CASE WHEN LEN(Num_Contrato_Referencia) < 10 THEN RIGHT('0000000000' + Num_Contrato_Referencia, 10) ELSE Num_Contrato_Referencia END 
	FROM
		dbo.cabecera_contrato
	WHERE
		numatcard = @p_nro_contrato
		and codigo_empresa = @p_codigo_empresa_o

	IF (ISNULL(@v_codigo_empresa_anexado, '') = '' AND ISNULL(@v_nro_contrato_anexado, '') = '') 
		RETURN @v_retorno

	SELECT @v_cantidad_registros = COUNT(dc.ItemCode)
	FROM detalle_contrato dc
	INNER join articulo a 
		ON dc.ItemCode = a.codigo_sku
	WHERE
		dc.NumAtCard = @v_nro_contrato_anexado
		AND dc.Codigo_empresa = @v_codigo_empresa_anexado
		AND a.codigo_tipo_articulo = @v_codigo_tipo_articulo_anexado

	SET @v_retorno = CASE WHEN ISNULL(@v_cantidad_registros, 0) = 0 THEN 0 ELSE 1 END

	RETURN @v_retorno
END