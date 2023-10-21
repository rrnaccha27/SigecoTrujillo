CREATE PROCEDURE dbo.up_personal_canal_grupo_desasignar_supervisor
(
	@p_codigo_personal		INT
	,@p_codigo_canal_grupo	INT
	,@p_usuario_modifica	VARCHAR(50)
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
		codigo_personal = @p_codigo_personal
		AND codigo_canal_grupo = @p_codigo_canal_grupo
END