USE SIGECO
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[up_checklist_comision_detalle_listado]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].up_checklist_comision_detalle_listado
GO
CREATE PROCEDURE dbo.up_checklist_comision_detalle_listado
(
	@p_codigo_checklist	INT
	,@p_validado		BIT
)
AS
BEGIN
	SET NOCOUNT ON
	
	SELECT
		ch.codigo_checklist_detalle
		,ch.codigo_grupo
		,ch.nombre_grupo
		,ch.nombre_empresa
		,ISNULL(p.nro_ruc, '') AS ruc_personal
		,rh.nombre_personal
		,rh.importe_abono_personal
		,CONVERT(INT, ch.validado) AS validado
		,CASE ch.validado WHEN 0 THEN 'No' WHEN 1 THEN 'Si' ELSE '' END AS validado_texto
		,ch.numero_planilla
		,dbo.fn_obtener_nombre_usuario(ch.usuario_modifica) AS usuario_modifica
		,dbo.fn_formatear_fecha_hora(ch.fecha_modifica) AS fecha_modifica
	FROM
		dbo.checklist_comision_detalle ch
	INNER JOIN dbo.sigeco_reporte_comision_rrhh rh
		ON rh.codigo_planilla = ch.codigo_planilla AND rh.codigo_empresa = ch.codigo_empresa AND rh.codigo_personal = ch.codigo_personal
	INNER JOIN dbo.personal p 
		ON p.codigo_personal = rh.codigo_personal
	WHERE 
		ch.codigo_checklist = @p_codigo_checklist
		AND ch.validado = @p_validado
	ORDER BY
		ch.nombre_grupo ASC
		,ch.nombre_empresa DESC
		,rh.nombre_personal
	
	SET NOCOUNT Off
END