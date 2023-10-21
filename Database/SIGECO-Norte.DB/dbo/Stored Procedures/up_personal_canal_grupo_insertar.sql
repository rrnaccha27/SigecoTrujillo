CREATE PROCEDURE dbo.up_personal_canal_grupo_insertar
(
	@p_codigo_personal			INT
	,@p_codigo_canal_grupo		INT
	,@p_codigo_canal			INT
	,@p_es_supervisor_canal		BIT
	,@p_es_supervisor_grupo		BIT
	,@p_percibe_comision		BIT
	,@p_percibe_bono			BIT
	,@p_usuario_registra		VARCHAR(50)
	,@p_codigo_registro			INT OUTPUT
)
AS
BEGIN
	
	INSERT INTO
		dbo.personal_canal_grupo
	(
		codigo_personal
		,codigo_canal_grupo
		,codigo_canal
		,es_supervisor_canal
		,es_supervisor_grupo
		,percibe_comision
		,percibe_bono
		,estado_registro
		,fecha_registra
		,usuario_registra
	)
	VALUES
	(
		@p_codigo_personal
		,@p_codigo_canal_grupo
		,@p_codigo_canal
		,@p_es_supervisor_canal
		,@p_es_supervisor_grupo
		,@p_percibe_comision
		,@p_percibe_bono
		,1
		,GETDATE()
		,@p_usuario_registra
	)
	SET @p_codigo_registro = @@IDENTITY

END