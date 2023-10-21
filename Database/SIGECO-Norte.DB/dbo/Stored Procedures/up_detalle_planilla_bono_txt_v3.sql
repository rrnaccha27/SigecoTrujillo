USE SIGECO
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[up_detalle_planilla_bono_txt_v3]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].up_detalle_planilla_bono_txt_v3
GO

CREATE PROCEDURE [dbo].up_detalle_planilla_bono_txt_v3
(
	  @p_codigo_checklist	INT
)
AS
BEGIN

	SET NOCOUNT ON

	SELECT
		det.codigo_planilla, chk.numero_checklist AS numero_planilla,rrhh.fecha_proceso,rrhh.codigo_empresa,rrhh.simbolo_moneda_cuenta_desembolso,rrhh.nombre_empresa,
		rrhh.numero_cuenta_desembolso,rrhh.tipo_cuenta_desembolso,rrhh.numero_cuenta_abono,rrhh.tipo_cuenta_abono,rrhh.simbolo_moneda_cuenta_abono,
		rrhh.nombre_tipo_documento,rrhh.nro_documento,rrhh.nombre_personal,rrhh.codigo_personal,rrhh.importe_abono_personal,rrhh.importe_desembolso_empresa,
		rrhh.calcular_detraccion,rrhh.[checksum], rrhh.codigo_grupo
	FROM 
		checklist_bono_detalle det 
	INNER JOIN sigeco_reporte_bono_rrhh rrhh
		ON rrhh.codigo_planilla = det.codigo_planilla and rrhh.codigo_empresa = det.codigo_empresa and rrhh.codigo_personal = det.codigo_personal 
	INNER JOIN checklist_bono chk
		ON chk.codigo_checklist = det.codigo_checklist
	WHERE 
		det.codigo_checklist = @p_codigo_checklist
		AND det.validado = 1 and rrhh.validado = 1
	ORDER BY 
		nombre_personal

	SET NOCOUNT OFF

END