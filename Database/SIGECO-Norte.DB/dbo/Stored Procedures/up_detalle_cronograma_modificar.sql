CREATE PROCEDURE dbo.up_detalle_cronograma_modificar
(
	@p_codigo_detalle_cronograma	INT
	,@p_importe_comision			DECIMAL(10, 2)
	,@p_usuario_modifica			VARCHAR(50)
	,@p_motivo						VARCHAR(200)
)
AS
BEGIN
	SET NOCOUNT ON

	DECLARE
		@c_IGV			DECIMAL(10, 2)
		,@v_monto_bruto	DECIMAL(10, 2)
		,@v_monto_igv	DECIMAL(10, 2)

	SET @c_IGV = (SELECT TOP 1 (1 + CONVERT(NUMERIC, valor)/100) FROM parametro_sistema WHERE codigo_parametro_sistema = 9);

	SET	@v_monto_bruto = ROUND((@p_importe_comision / @c_IGV), 2)
	SET	@v_monto_igv = ROUND((@p_importe_comision - @v_monto_bruto), 2)

	/* SE GUARDA EN EL LOG LOS DATOS ANTES DE MODIFICARLOS */
	INSERT INTO detalle_cronograma_log(
		fecha_log
		,codigo_detalle
		,codigo_cronograma
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
	)
	SELECT TOP 1
		GETDATE()
		,codigo_detalle
		,codigo_cronograma
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
	FROM
		dbo.detalle_cronograma
	WHERE
		codigo_detalle = @p_codigo_detalle_cronograma
	
	/* SE MODIFICAN LOS DATOS */
	UPDATE
		dbo.detalle_cronograma
	SET
		monto_bruto = @v_monto_bruto
		,igv = @v_monto_igv
		,monto_neto = @p_importe_comision
		,codigo_tipo_cuota = 2
	WHERE
		codigo_detalle = @p_codigo_detalle_cronograma

	/* SE REGISTRA LA OPERACION DE MODIFICACION */
	EXEC dbo.up_operacion_cuota_comision_insertar @p_codigo_detalle_cronograma, 6, @p_motivo, @p_usuario_modifica

	SET NOCOUNT OFF
END;