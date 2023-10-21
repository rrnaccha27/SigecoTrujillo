CREATE PROCEDURE dbo.up_personal_canal_grupo_asignar_supervisor
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

			--IF (EXISTS(SELECT TOP 1 codigo_registro FROM dbo.personal_canal_grupo WHERE codigo_personal = @p_codigo_personal AND codigo_canal_grupo = @p_codigo_canal_grupo AND estado_registro = 1))
			--	UPDATE
			--		dbo.personal_canal_grupo
			--	SET
			--		es_supervisor_canal = CASE WHEN @p_es_canal_grupo = 1 THEN 1 ELSE 0 END
			--		,es_supervisor_grupo = CASE WHEN @p_es_canal_grupo = 0 THEN 1 ELSE 0 END
			--		,percibe_comision = @p_percibe_comision
			--		,percibe_bono = @p_percibe_bono
			--		,fecha_modifica = GETDATE()
			--		,usuario_modifica = @p_usuario_modifica
			--	WHERE
			--		codigo_personal = @p_codigo_personal
			--		AND codigo_canal_grupo = @p_codigo_canal_grupo
			--		AND estado_registro = 1
			--ELSE
			--BEGIN
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
					,CASE WHEN @p_es_canal_grupo = 1 THEN 1 ELSE 0 END
					,CASE WHEN @p_es_canal_grupo = 0 THEN 1 ELSE 0 END
					,@p_percibe_comision
					,@p_percibe_bono
					,1
					,GETDATE()
					,@p_usuario_modifica
				)
	
			--END
		COMMIT TRAN
	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT > 0
			ROLLBACK
	END CATCH
END