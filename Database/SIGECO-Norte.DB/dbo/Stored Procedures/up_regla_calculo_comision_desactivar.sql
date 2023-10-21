CREATE PROCEDURE dbo.up_regla_calculo_comision_desactivar
(
	@p_codigo_regla			INT
	,@p_usuario_modifica	VARCHAR(50)
)
AS
BEGIN
	SET NOCOUNT ON

	UPDATE
		dbo.regla_calculo_comision
	SET
		estado_registro = 0
		,fecha_modifica = GETDATE()
		,usuario_modifica = @p_usuario_modifica
	WHERE 
		codigo_regla = @p_codigo_regla

	SET NOCOUNT OFF
END;