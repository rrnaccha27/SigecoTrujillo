USE SIGECO
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[up_checklist_bono_trimestral_anular]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].up_checklist_bono_trimestral_anular
GO

CREATE PROCEDURE dbo.up_checklist_bono_trimestral_anular
(
	@p_codigo_checklist		INT
	,@p_usuario_modifica	VARCHAR(25)
)
AS
BEGIN
	SET NOCOUNT ON

	UPDATE dbo.checklist_bono_trimestral
	SET
		codigo_estado_checklist = 3
		,fecha_modifica = GETDATE()
		,fecha_anulacion = GETDATE()
		,usuario_modifica = @p_usuario_modifica
		,usuario_anulacion = @p_usuario_modifica
	WHERE
		codigo_checklist = @p_codigo_checklist

	SET NOCOUNT OFF
END