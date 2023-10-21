CREATE PROCEDURE dbo.up_operacion_cuota_comision_insertar
(
	@p_codigo_detalle_cronograma	INT
	,@p_codigo_tipo_operacion_cuota	INT
	,@p_motivo_movimiento			VARCHAR(200)
	,@p_usuario_registra			VARCHAR(50)
)
AS
BEGIN
	SET NOCOUNT ON

	/**************************************************
	DESACTIVANDO REGISTRO ACTUAL COMO HISTORICO
	****************************************************/
	UPDATE 
		dbo.operacion_cuota_comision 
	SET 
		estado_registro=0 
	WHERE 
		codigo_detalle_cronograma = @p_codigo_detalle_cronograma 
		and estado_registro = 1;

	/**************************************************
	INSERTANDO COMO NUEVO REGISTRO
	****************************************************/
	INSERT INTO dbo.operacion_cuota_comision(
		codigo_detalle_cronograma,
		codigo_tipo_operacion_cuota,
		motivo_movimiento,
		fecha_movimiento,
		estado_registro,
		usuario_registra,
		fecha_registra
	)
	VALUES(
		@p_codigo_detalle_cronograma,
		@p_codigo_tipo_operacion_cuota,
		@p_motivo_movimiento,
		GETDATE(),
		1,
		@p_usuario_registra,
		GETDATE()
	);
	
	SET NOCOUNT OFF
END;