USE SIGECO
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[up_planilla_bono_trimestral_contabilidad_resumen]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].up_planilla_bono_trimestral_contabilidad_resumen
GO

CREATE PROCEDURE [dbo].up_planilla_bono_trimestral_contabilidad_resumen
(
	@p_codigo_planilla	INT
)
AS
BEGIN
	SET NOCOUNT ON

	SELECT 
		@p_codigo_planilla as codigo_planilla
		,dpb.codigo_empresa
		,e.nombre as nombre_empresa
		,count(e.codigo_empresa) as bonos
	FROM 
		dbo.planilla_bono_trimestral pb
	inner join dbo.planilla_bono_trimestral_detalle dpb on pb.codigo_planilla = @p_codigo_planilla and dpb.codigo_planilla = pb.codigo_planilla
	inner join dbo.empresa_sigeco e on e.codigo_empresa = dpb.codigo_empresa
	--inner join dbo.resumen_planilla_bono rpb on rpb.codigo_planilla = @p_codigo_planilla and rpb.codigo_personal = dpb.codigo_personal
	--inner join dbo.contrato_planilla_bono cpb on cpb.codigo_planilla = @p_codigo_planilla and case when pb.codigo_tipo_planilla = 1 then cpb.codigo_personal else cpb.codigo_supervisor end = dpb.codigo_personal and cpb.codigo_empresa = dpb.codigo_empresa
	--inner join dbo.articulo_planilla_bono apb on apb.codigo_planilla_bono = @p_codigo_planilla and apb.codigo_empresa = cpb.codigo_empresa and apb.nro_contrato = cpb.numero_contrato
	WHERE 
		pb.codigo_planilla = @p_codigo_planilla
		AND dpb.monto_bono IS NOT NULL
	GROUP BY
		dpb.codigo_empresa
		,e.nombre
	ORDER BY
		dpb.codigo_empresa

	SET NOCOUNT OFF
END;

