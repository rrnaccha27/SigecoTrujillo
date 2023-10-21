USE SIGECO
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[up_checklist_bono_trimestral_listar]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].up_checklist_bono_trimestral_listar
GO

CREATE PROCEDURE dbo.up_checklist_bono_trimestral_listar
AS
BEGIN
	SET NOCOUNT ON

	DECLARE
		@c_estado_planilla_cerrada	INT = 2;

	SELECT
		c.codigo_checklist
		,c.numero_checklist
		,c.codigo_estado_checklist
		,ep.nombre as nombre_estado_checklist
		,CONVERT(VARCHAR, c.fecha_registra, 103) + ' ' + CONVERT(VARCHAR, c.fecha_registra, 108) as fecha_apertura
		,c.usuario_registra as usuario_apertura
		,ISNULL(CONVERT(VARCHAR, c.fecha_cierre, 103) + ' ' + CONVERT(VARCHAR, c.fecha_cierre, 108), '') as fecha_cierre
		,ISNULL(c.usuario_cierre, '') as usuario_cierre
		,ISNULL(CONVERT(VARCHAR, c.fecha_anulacion, 103) + ' ' + CONVERT(VARCHAR, c.fecha_anulacion, 108), '') as fecha_anulacion
		,ISNULL(c.usuario_anulacion, '') as usuario_anulacion
		,p.valor as estilo  
		,r.descripcion as nombre_regla
	FROM
		dbo.checklist_bono_trimestral c
	INNER JOIN dbo.estado_planilla ep
		ON	ep.codigo_estado_planilla = c.codigo_estado_checklist
	INNER JOIN dbo.planilla_bono_trimestral pbt
		ON pbt.codigo_planilla = c.codigo_planilla and pbt.codigo_estado_planilla = @c_estado_planilla_cerrada
	INNER JOIN dbo.regla_bono_trimestral r
		ON r.codigo_regla = pbt.codigo_regla
	INNER JOIN dbo.fn_split_parametro_sistema('21,22,23') p 
		ON c.codigo_estado_checklist = p.codigo  
	ORDER BY
		c.fecha_registra DESC

	SET NOCOUNT OFF
END