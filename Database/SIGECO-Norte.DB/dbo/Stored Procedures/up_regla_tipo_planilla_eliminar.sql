CREATE PROCEDURE up_regla_tipo_planilla_eliminar
(
	@p_codigo_regla_tipo_planilla	INT,
	@p_usuario_registra				VARCHAR(50)
)
AS
BEGIN
	UPDATE 
		dbo.regla_tipo_planilla 
	SET
		estado_registro = 0,
		usuario_modifica = @p_usuario_registra,
		fecha_modifica=GETDATE()	
	 WHERE 
		codigo_regla_tipo_planilla = @p_codigo_regla_tipo_planilla;
END;