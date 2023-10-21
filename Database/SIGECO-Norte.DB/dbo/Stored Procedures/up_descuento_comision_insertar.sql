CREATE PROCEDURE dbo.up_descuento_comision_insertar
(
	@p_codigo_personal				INT
	,@p_codigo_empresa				INT
	,@p_monto						INT
	,@p_motivo						VARCHAR(300)
	,@p_usuario_registra			VARCHAR(50)
	,@p_codigo_descuento_comision	INT OUTPUT
)
AS
BEGIN
	SET NOCOUNT ON

	INSERT INTO dbo.descuento_comision(
		codigo_personal,
		codigo_empresa,
		monto,
		saldo,
		motivo,
		estado_registro,
		usuario_registra,
		fecha_registra
	)
	VALUES(
		@p_codigo_personal,
		@p_codigo_empresa,
		@p_monto,
		@p_monto,
		@p_motivo,
		1,
		@p_usuario_registra,
		GETDATE()
	);
	
	SET @p_codigo_descuento_comision = @@IDENTITY
	SET NOCOUNT OFF
END;