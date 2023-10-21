CREATE PROCEDURE dbo.up_personal_canal_grupo_desactivar
(
	@p_codigo_registro			INT
	,@p_usuario_modifica		VARCHAR(50)
)
AS
BEGIN
	
	UPDATE
		dbo.personal_canal_grupo
	SET
		estado_registro = 0
		,fecha_modifica = GETDATE() 
		,usuario_modifica = @p_usuario_modifica
	WHERE
		codigo_registro = @p_codigo_registro

END