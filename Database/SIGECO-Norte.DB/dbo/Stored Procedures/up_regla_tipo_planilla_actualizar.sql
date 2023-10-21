CREATE PROCEDURE up_regla_tipo_planilla_actualizar
(
	@p_codigo_regla_tipo_planilla	INT,
	@p_nombre						VARCHAR(200),
	@p_usuario_registra				VARCHAR(50)
)
AS
BEGIN
	UPDATE 
		regla_tipo_planilla 
	SET 
		nombre = @p_nombre
		,usuario_modifica = @p_usuario_registra
		,fecha_modifica = getdate() 
	WHERE 
		codigo_regla_tipo_planilla=@p_codigo_regla_tipo_planilla;
END;