CREATE PROCEDURE dbo.up_personal_activar
	@p_codigo_personal		INT
	,@p_usuario_modifica	VARCHAR(50)
AS
BEGIN
	SET NOCOUNT ON
	
	DECLARE 
		@v_codigo_registro	INT

	UPDATE
		dbo.personal
	SET
		estado_registro = 1
		,usuario_modifica = @p_usuario_modifica
		,fecha_modifica = GETDATE()
	WHERE
		codigo_personal = @p_codigo_personal
		AND estado_registro = 0

	SELECT TOP 1
		@v_codigo_registro = codigo_registro
	FROM
		dbo.personal_canal_grupo
	WHERE
		codigo_personal = @p_codigo_personal
		AND estado_registro = 0
	ORDER BY 
		fecha_registra DESC

	UPDATE
		dbo.personal_canal_grupo
	SET
		estado_registro = 1
		,usuario_modifica = @p_usuario_modifica
		,fecha_modifica = GETDATE()
	WHERE
		codigo_registro = @v_codigo_registro

	SET NOCOUNT OFF
END;