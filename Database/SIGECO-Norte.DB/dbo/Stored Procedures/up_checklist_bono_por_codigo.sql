USE SIGECO
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[up_checklist_bono_por_codigo]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].up_checklist_bono_por_codigo
GO

CREATE PROCEDURE dbo.up_checklist_bono_por_codigo
(
	@p_codigo_checklist	INT
)
AS
BEGIN
	SET NOCOUNT ON

	SELECT TOP 1
		c.codigo_checklist
		,c.numero_checklist
		,c.codigo_estado_checklist
		,ep.nombre as nombre_estado_checklist
		,CONVERT(VARCHAR, c.fecha_registra, 103) + ' ' + CONVERT(VARCHAR, c.fecha_registra, 108) as fecha_apertura
		,dbo.fn_obtener_nombre_usuario(c.usuario_registra) as usuario_apertura
		,ISNULL(CONVERT(VARCHAR, c.fecha_cierre, 103) + ' ' + CONVERT(VARCHAR, c.fecha_cierre, 108), '') as fecha_cierre
		,ISNULL(dbo.fn_obtener_nombre_usuario(c.usuario_cierre), '') as usuario_cierre
		,ISNULL(CONVERT(VARCHAR, c.fecha_anulacion, 103) + ' ' + CONVERT(VARCHAR, c.fecha_anulacion, 108), '') as fecha_anulacion
		,ISNULL(dbo.fn_obtener_nombre_usuario(c.usuario_anulacion), '') as usuario_anulacion
	FROM
		dbo.checklist_bono c
	INNER JOIN dbo.estado_planilla ep
		ON	ep.codigo_estado_planilla = c.codigo_estado_checklist
	WHERE
		c.codigo_checklist = @p_codigo_checklist
	ORDER BY
		c.fecha_registra DESC

	SET NOCOUNT OFF
END