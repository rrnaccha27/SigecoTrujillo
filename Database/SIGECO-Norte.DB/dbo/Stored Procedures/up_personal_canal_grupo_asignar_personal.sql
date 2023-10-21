CREATE PROCEDURE dbo.up_personal_canal_grupo_asignar_personal
(
	@p_codigo_personal		INT
	,@p_codigo_canal_grupo	INT
	,@p_es_canal_grupo		BIT
	,@p_percibe_comision	BIT
	,@p_percibe_bono		BIT
	,@p_usuario_modifica	VARCHAR(50)
)
AS
BEGIN
	BEGIN TRY
		BEGIN TRAN
		DECLARE @v_codigo_canal	INT

		IF (@p_es_canal_grupo = 0)
			SET @v_codigo_canal = (SELECT TOP 1 codigo_padre FROM dbo.canal_grupo WHERE codigo_canal_grupo = @p_codigo_canal_grupo)
		ELSE
			SET @v_codigo_canal = @p_codigo_canal_grupo

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
			,@v_codigo_canal
			,0
			,0
			,@p_percibe_comision
			,@p_percibe_bono
			,1
			,GETDATE()
			,@p_usuario_modifica
		)

		COMMIT TRAN
	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT > 0
			ROLLBACK
	END CATCH
END