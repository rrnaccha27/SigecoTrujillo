USE SIGECO
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[up_personal_validar]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].up_personal_validar
GO

CREATE PROCEDURE dbo.up_personal_validar
(
	@p_array_personal	ARRAY_PERSONAL_TYPE readonly,
	@p_usuario_valida	VARCHAR(25)
)
AS
BEGIN
	SET NOCOUNT ON

	DECLARE
		@c_fecha_proceso	DATETIME = GETDATE()
		,@c_estado_activo	BIT = 1
		,@c_validado		BIT = 1

	UPDATE p
	SET
		p.validado = @c_validado
		,p.usuario_validado = @p_usuario_valida
		,p.fecha_validado = @c_fecha_proceso
	FROM
		dbo.personal p
	INNER JOIN @p_array_personal array
		ON array.codigo_personal = p.codigo_personal

	INSERT INTO dbo.personal_historial_validacion
	(
		codigo_personal
		,estado_registro
		,fecha_registra
		,usuario_registra
	)
	SELECT
		codigo_personal
		,@c_estado_activo
		,@c_fecha_proceso
		,@p_usuario_valida
	FROM
		@p_array_personal

	SET NOCOUNT OFF
END
