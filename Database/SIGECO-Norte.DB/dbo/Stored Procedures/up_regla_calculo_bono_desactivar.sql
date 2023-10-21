CREATE PROCEDURE dbo.up_regla_calculo_bono_desactivar
(
	 @p_codigo_regla_calculo_bono	INT
	,@p_usuario_modifica			VARCHAR(50)
)
AS
BEGIN
	
	UPDATE
		dbo.regla_calculo_bono
	SET
		estado_registro = 0
		,fecha_modifica = GETDATE() 
		,usuario_modifica = @p_usuario_modifica
	WHERE
		codigo_regla_calculo_bono = @p_codigo_regla_calculo_bono

END;