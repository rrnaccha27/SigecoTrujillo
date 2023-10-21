USE SIGECO
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[up_planilla_contabilidad_resumen_planilla_v3]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].up_planilla_contabilidad_resumen_planilla_v3
GO

CREATE PROCEDURE [dbo].up_planilla_contabilidad_resumen_planilla_v3
(
	@p_codigo_checklist	INT
)
AS
BEGIN
	SET NOCOUNT ON

	SELECT
		chk.codigo_checklist, cont.codigo_empresa, cont.NOM_EMPRESA as nombre_empresa, det.numero_planilla, det.codigo_planilla, count(cont.codigo_empresa) as comisiones
	FROM 
		checklist_comision_detalle det 
	INNER JOIN sigeco_reporte_comision_rrhh rrhh
		ON rrhh.codigo_planilla = det.codigo_planilla and rrhh.codigo_empresa = det.codigo_empresa and rrhh.codigo_personal = det.codigo_personal 
	INNER JOIN personal p
		ON p.codigo_personal = det.codigo_personal
	INNER JOIN checklist_comision chk
		ON chk.codigo_checklist = det.codigo_checklist
	INNER JOIN sigeco_reporte_comision_contabilidad cont
		ON cont.ORDEN = 2 and cont.codigo_planilla = rrhh.codigo_planilla and cont.codigo_empresa = rrhh.codigo_empresa and cont.C_AGENTE = p.codigo_equivalencia
	WHERE 
		det.codigo_checklist = @p_codigo_checklist
		AND det.validado = 1 and rrhh.validado = 1
	GROUP BY
		chk.codigo_checklist, cont.codigo_empresa, cont.NOM_EMPRESA, det.numero_planilla, det.codigo_planilla
	ORDER BY 
		codigo_empresa


	SET NOCOUNT OFF
END;


