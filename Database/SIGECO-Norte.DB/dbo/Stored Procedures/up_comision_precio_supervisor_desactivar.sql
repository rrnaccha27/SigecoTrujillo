CREATE PROCEDURE dbo.up_comision_precio_supervisor_desactivar
(
	@p_codigo_comision		INT
	,@p_usuario_modifica	VARCHAR(50)
)
AS
BEGIN
	SET NOCOUNT ON

	UPDATE
		dbo.comision_precio_supervisor
	SET
		estado_registro = 0
		,fecha_modifica = GETDATE()
		,usuario_modifica = @p_usuario_modifica
	WHERE 
		codigo_comision = @p_codigo_comision

	SET NOCOUNT OFF
END;