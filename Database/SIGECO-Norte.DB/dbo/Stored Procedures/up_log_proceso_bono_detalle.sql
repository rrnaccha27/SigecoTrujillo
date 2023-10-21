USE SIGECO
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[up_log_proceso_bono_detalle]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].up_log_proceso_bono_detalle
GO

CREATE PROCEDURE dbo.up_log_proceso_bono_detalle
(
	 @p_codigo_planilla	INT
)
AS
BEGIN
	SELECT
		l.nro_contrato
		,ISNULL(e.nombre, '') as empresa
		,ISNULL(c.nombre, '') + ISNULL(' - ' + g.nombre, '') as canal_grupo
		,ISNULL(ep.nombre, '') as estado_nombre
		,ISNULL(l.observacion, '') as observacion
		,ISNULL(l.codigo_estado, 2) as codigo_estado
		,ISNULL(cpb.monto_ingresado, 0) as monto_ingresado
	FROM
		dbo.log_proceso_bono l
	LEFT JOIN 
		dbo.empresa_sigeco e
		ON e.codigo_empresa = l.codigo_empresa
	LEFT JOIN
		dbo.estado_proceso ep
		ON ep.codigo_estado_proceso = l.codigo_estado
	LEFT JOIN dbo.canal_grupo c
		ON c.codigo_canal_grupo = l.codigo_canal and c.es_canal_grupo = 1
	LEFT JOIN dbo.canal_grupo g
		ON g.codigo_canal_grupo = l.codigo_grupo and g.es_canal_grupo = 0
	LEFT JOIN contrato_planilla_bono cpb ON 
		cpb.codigo_planilla = @p_codigo_planilla and cpb.numero_contrato = l.nro_contrato and cpb.codigo_empresa = l.codigo_empresa
	WHERE
		l.codigo_planilla = @p_codigo_planilla
	ORDER BY
		l.nro_contrato, l.codigo_empresa
END