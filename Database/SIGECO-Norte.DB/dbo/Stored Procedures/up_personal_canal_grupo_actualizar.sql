CREATE PROCEDURE dbo.up_personal_canal_grupo_actualizar
(
	@p_codigo_registro			INT
	,@p_codigo_personal			INT
	,@p_codigo_canal_grupo		INT
	,@p_codigo_canal			INT
	,@p_es_supervisor_canal		BIT
	,@p_es_supervisor_grupo		BIT
	,@p_percibe_comision		BIT
	,@p_percibe_bono			BIT
	,@p_usuario_modifica		VARCHAR(50)
	,@p_estado_registro			BIT
)
AS
BEGIN
	
	UPDATE
		dbo.personal_canal_grupo
	SET
		codigo_personal = @p_codigo_personal
		,codigo_canal_grupo = @p_codigo_canal_grupo
		,codigo_canal = @p_codigo_canal
		,es_supervisor_canal = @p_es_supervisor_canal
		,es_supervisor_grupo = @p_es_supervisor_grupo
		,percibe_comision = @p_percibe_comision
		,percibe_bono = @p_percibe_bono
		,estado_registro = @p_estado_registro
		,fecha_modifica = GETDATE() 
		,usuario_modifica = @p_usuario_modifica
	WHERE
		codigo_registro = @p_codigo_registro

END