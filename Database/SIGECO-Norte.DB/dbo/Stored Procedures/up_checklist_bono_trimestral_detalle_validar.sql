USE SIGECO
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[up_checklist_bono_trimestral_detalle_validar]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].up_checklist_bono_trimestral_detalle_validar
GO

CREATE PROCEDURE dbo.up_checklist_bono_trimestral_detalle_validar
(
	@p_array_checklist_bono_detalle array_checklist_comision_detalle_type readonly,
	@p_usuario_modifica varchar(25)
)
AS
BEGIN
	SET NOCOUNT ON

	UPDATE ch
	SET
		ch.validado = 1
		,usuario_modifica = @p_usuario_modifica
		,fecha_modifica = GETDATE()
	FROM
		dbo.checklist_bono_trimestral_detalle ch
	INNER JOIN @p_array_checklist_bono_detalle d
		ON d.codigo_checklist_detalle = ch.codigo_checklist_detalle

	SET NOCOUNT OFF
END
