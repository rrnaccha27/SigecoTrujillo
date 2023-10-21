USE SIGECO
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[up_personal_activar_validado]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].up_personal_activar_validado
GO

CREATE PROCEDURE dbO.up_personal_activar_validado
(
	@p_codigo_personal	INT
)
AS
BEGIN
	DECLARE
		 @c_PARAMETRO_CANAL	INT = 32
		,@c_NO				BIT = 0
		,@c_ACTIVO			BIT = 1

	DECLARE
		@v_codigo_canal	INT

	SET @v_codigo_canal = CONVERT(INT, ISNULL((SELECT valor FROM dbo.parametro_sistema WHERE codigo_parametro_sistema = @c_PARAMETRO_CANAL), '2'))

	IF EXISTS(SELECT * FROM dbo.vw_personal WHERE codigo_canal = @v_codigo_canal AND es_supervisor_canal = @c_NO AND es_supervisor_grupo = @c_NO AND estado_persona = @c_ACTIVO AND estado_canal_grupo = @c_ACTIVO AND codigo_personal = @p_codigo_personal)
	BEGIN
		UPDATE
			dbo.personal
		SET
			validado = NULL
		WHERE
			codigo_personal = @p_codigo_personal
	END
END