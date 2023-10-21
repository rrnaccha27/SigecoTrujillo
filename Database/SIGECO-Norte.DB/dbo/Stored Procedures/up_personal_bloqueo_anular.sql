USE SIGECO
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[up_personal_bloqueo_anular]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].up_personal_bloqueo_anular
GO

CREATE PROCEDURE dbo.up_personal_bloqueo_anular
(
	@p_codigo_planilla					INT
	,@p_codigo_tipo_bloqueo_personal	INT
	,@p_usuario_modifica				VARCHAR(25)
)
AS
BEGIN

	DECLARE
		@c_estado_inactivo			BIT = 0
		,@c_fecha_proceso			DATETIME = GETDATE()

	UPDATE
		dbo.personal_bloqueo
	SET
		estado_registro = @c_estado_inactivo
		,fecha_modifica = @c_fecha_proceso
		,usuario_modifica = @p_usuario_modifica
	WHERE
		codigo_planilla = @p_codigo_planilla
		AND codigo_tipo_bloqueo = @p_codigo_tipo_bloqueo_personal

END