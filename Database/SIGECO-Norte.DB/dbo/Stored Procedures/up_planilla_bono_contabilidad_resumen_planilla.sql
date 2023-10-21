CREATE PROCEDURE [dbo].[up_planilla_bono_contabilidad_resumen_planilla]
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
		dbo.planilla_bono pb
	inner join dbo.detalle_planilla_bono dpb on pb.codigo_planilla = @p_codigo_planilla and dpb.codigo_planilla = pb.codigo_planilla
	inner join dbo.empresa_sigeco e on e.codigo_empresa = dpb.codigo_empresa
	inner join dbo.resumen_planilla_bono rpb on rpb.codigo_planilla = @p_codigo_planilla and rpb.codigo_personal = dpb.codigo_personal
	inner join dbo.contrato_planilla_bono cpb on cpb.codigo_planilla = @p_codigo_planilla and case when pb.codigo_tipo_planilla = 1 then cpb.codigo_personal else cpb.codigo_supervisor end = dpb.codigo_personal and cpb.codigo_empresa = dpb.codigo_empresa
	inner join dbo.articulo_planilla_bono apb on apb.codigo_planilla_bono = @p_codigo_planilla and apb.codigo_empresa = cpb.codigo_empresa and apb.nro_contrato = cpb.numero_contrato
	WHERE 
		pb.codigo_planilla = @p_codigo_planilla
		AND ((apb.dinero_ingresado * rpb.porcentaje_pago) / 100) > 0
		AND apb.excluido = 0
	GROUP BY
		dpb.codigo_empresa
		,e.nombre
	ORDER BY
		dpb.codigo_empresa

	SET NOCOUNT OFF
END;