CREATE PROCEDURE dbo.up_detalle_cronograma_insertar
(
	@p_codigo_empresa			INT
	,@p_nro_contrato			VARCHAR(100)
	,@p_codigo_tipo_planilla	INT
	,@p_codigo_articulo			INT
	,@p_monto_neto				DECIMAL(10, 2)
	,@p_usuario_registra		VARCHAR(50)
	,@p_motivo					VARCHAR(200)
	,@p_nro_cuota				INT OUTPUT
)
AS
BEGIN
	SET NOCOUNT ON
		
	DECLARE
		@v_codigo_cronograma			INT
		,@v_monto_bruto					DECIMAL(10, 2)
		,@v_monto_igv					DECIMAL(10, 2)
		,@v_esHR						BIT = dbo.fn_generar_cronograma_comision_eval_hoja_resumen(@p_nro_contrato)
		,@c_IGV							DECIMAL(10, 2)
		,@v_codigo_detalle_cronograma	INT
		,@v_es_transferencia			BIT = 0

	SELECT TOP 1 
		@v_codigo_cronograma = codigo_cronograma 
		,@v_es_transferencia = tiene_transferencia
	FROM 
		dbo.cronograma_pago_comision 
	WHERE 
		CASE WHEN @v_esHR=0 THEN nro_contrato ELSE nro_contrato_adicional END = @p_nro_contrato 
		AND codigo_empresa = @p_codigo_empresa AND codigo_tipo_planilla = @p_codigo_tipo_planilla;

	SET @p_nro_cuota = (SELECT MAX(nro_cuota) + 1 FROM dbo.detalle_cronograma WHERE codigo_cronograma = @v_codigo_cronograma AND codigo_articulo = @p_codigo_articulo)
	SET @c_IGV = (SELECT TOP 1 (1 + CONVERT(NUMERIC, valor)/100) FROM parametro_sistema WHERE codigo_parametro_sistema = 9);
	SET	@v_monto_bruto = ROUND((@p_monto_neto / @c_IGV), 2)
	SET	@v_monto_igv = ROUND((@p_monto_neto - @v_monto_bruto), 2)

	INSERT INTO
		dbo.detalle_cronograma(
			codigo_cronograma
			,codigo_articulo
			,nro_cuota
			,fecha_programada
			,monto_bruto
			,igv
			,monto_neto
			,codigo_tipo_cuota
			,codigo_estado_cuota
			,estado_registro
			,es_registro_manual_comision
			,es_transferencia
		)
	VALUES(
		@v_codigo_cronograma
		,@p_codigo_articulo
		,@p_nro_cuota
		,NULL
		,@v_monto_bruto
		,@v_monto_igv
		,@p_monto_neto
		,2
		,1
		,1
		,0
		,@v_es_transferencia
	)

	SET @v_codigo_detalle_cronograma = @@IDENTITY;

	EXEC dbo.up_operacion_cuota_comision_insertar @v_codigo_detalle_cronograma, 1, @p_motivo, @p_usuario_registra

	SET NOCOUNT OFF
END;