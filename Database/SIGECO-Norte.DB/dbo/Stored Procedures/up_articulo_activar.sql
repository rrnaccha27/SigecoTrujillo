CREATE PROCEDURE dbo.up_articulo_activar
(
	@p_codigo_articulo	INT,
	@p_usuario			VARCHAR(20)
)
AS
BEGIN
	SET NOCOUNT ON
	
	IF EXISTS(SELECT TOP 1 codigo_articulo FROM dbo.articulo WHERE codigo_articulo = @p_codigo_articulo and estado_registro = 1)
	BEGIN
		PRINT 'El articulo se encuentra activo.'
		RETURN;
	END

	EXEC dbo.up_articulo_log_insertar @p_codigo_articulo

	UPDATE 
		dbo.articulo 
	SET 
		estado_registro = 1,
		usuario_modifica = @p_usuario,
		fecha_modifica = GETDATE()
	WHERE 
		codigo_articulo = @p_codigo_articulo 
		AND estado_registro = 0;
	
	SET NOCOUNT OFF
END;