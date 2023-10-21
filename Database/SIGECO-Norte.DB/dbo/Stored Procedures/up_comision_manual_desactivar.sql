CREATE PROC dbo.up_comision_manual_desactivar
(
	@p_codigo_comision_manual	int ,
	@p_usuario_modifica			varchar(50)
)
AS
BEGIN
	SET NOCOUNT ON

	DECLARE @v_mensaje	VARCHAR(250)
	
	EXEC up_comision_manual_validar_referencia @p_codigo_comision_manual, @v_mensaje OUTPUT
	
	IF LEN(ISNULL(@v_mensaje, '')) <> 0 
	BEGIN
		SELECT @v_mensaje AS mensaje
		RETURN
	END

	UPDATE
		dbo.comision_manual
	SET
		estado_registro = 0,
		fecha_modifica = GETDATE(),
		usuario_modifica = @p_usuario_modifica
	WHERE
		codigo_comision_manual = @p_codigo_comision_manual
	
	SET NOCOUNT OFF
END