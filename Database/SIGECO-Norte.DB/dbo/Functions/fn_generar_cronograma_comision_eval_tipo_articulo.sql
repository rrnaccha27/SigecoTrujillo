CREATE FUNCTION dbo.fn_generar_cronograma_comision_eval_tipo_articulo
(
	@p_nro_contrato				VARCHAR(100)
	,@p_codigo_empresa_o		VARCHAR(4)
	,@v_codigo_tipo_articulo	INT
)
RETURNS BIT
AS
BEGIN
	DECLARE
		@v_retorno				BIT = 0,
		@v_cantidad_registros	INT

	SELECT @v_cantidad_registros = COUNT(dc.ItemCode)
	FROM dbo.detalle_contrato dc
	INNER JOIN dbo.articulo a 
		on dc.ItemCode = a.codigo_sku
	WHERE
		dc.NumAtCard = @p_nro_contrato
		AND dc.Codigo_empresa = @p_codigo_empresa_o
		AND a.codigo_tipo_articulo = @v_codigo_tipo_articulo
			
	SET @v_retorno = CASE WHEN ISNULL(@v_cantidad_registros, 0) = 0 THEN 0 ELSE 1 END

	RETURN @v_retorno
END