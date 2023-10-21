USE SIGECO
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[up_checklist_bono_trimestral_cerrar]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].up_checklist_bono_trimestral_cerrar
GO

CREATE PROCEDURE dbo.up_checklist_bono_trimestral_cerrar
(
	@p_codigo_checklist		INT
	,@p_usuario_modifica	VARCHAR(25)
)
AS
BEGIN
	SET NOCOUNT ON

	DECLARE
		@c_codigo_estado_checklist_cerrado	INT = 2
		,@c_validado						BIT = 1

	UPDATE dbo.checklist_bono_trimestral
	SET
		codigo_estado_checklist = @c_codigo_estado_checklist_cerrado
		,fecha_modifica = GETDATE()
		,fecha_cierre = GETDATE()
		,usuario_modifica = @p_usuario_modifica
		,usuario_cierre = @p_usuario_modifica
	WHERE
		codigo_checklist = @p_codigo_checklist

	UPDATE rh
	SET
		rh.validado = @c_validado
	FROM
		dbo.sigeco_reporte_bono_trimestral_rrhh rh
	INNER JOIN dbo.checklist_bono_trimestral_detalle ch_d
		ON rh.importe_abono_personal IS NOT NULL AND ch_d.codigo_planilla = rh.codigo_planilla AND ch_d.codigo_empresa = rh.codigo_empresa AND ch_d.codigo_personal = rh.codigo_personal
	WHERE
		ch_d.codigo_checklist = @p_codigo_checklist
		AND ch_d.validado = @c_validado


	SET NOCOUNT OFF
END