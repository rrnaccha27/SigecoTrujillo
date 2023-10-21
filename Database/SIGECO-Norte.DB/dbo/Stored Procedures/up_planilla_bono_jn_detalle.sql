CREATE PROCEDURE [dbo].[up_planilla_bono_jn_detalle]
(
	@p_codigo_planilla	INT
)
AS
BEGIN
	SET NOCOUNT ON

	SELECT
		@p_codigo_planilla as codigo_planilla,
		codigo_canal_grupo,
		nombre_grupo,
		codigo_personal,
		vendedor,
		codigo_empresa,
		nombre_empresa,	
		codigo_canal,
		nombre_canal,	
		nombre_moneda
		,nro_contrato
		,0 as codigo_articulo
		,'' nombre_articulo
		,fecha_contrato
		,sum(dinero_ingresado) as dinero_ingresado
	FROM(
		SELECT 
			g.codigo_canal_grupo,
			g.nombre as nombre_grupo,
			p.codigo_personal,
			p.nombre + ISNULL(' ' + p.apellido_paterno,'') + ISNULL(' ' + p.apellido_materno,'') as vendedor,
			e.codigo_empresa,
			e.nombre as nombre_empresa,	
			dpb.codigo_canal,
			c.nombre as nombre_canal,	
			m.nombre as nombre_moneda
			,cpb.numero_contrato as nro_contrato
			,apb.codigo_articulo
			,a.nombre as nombre_articulo
			,apb.dinero_ingresado
			,convert(varchar, cpb.fecha_contrato, 103) as fecha_contrato
		FROM 
			detalle_planilla_bono dpb
		inner join dbo.resumen_planilla_bono rpb on rpb.codigo_planilla = @p_codigo_planilla and rpb.codigo_personal = dpb.codigo_personal
		inner join dbo.contrato_planilla_bono cpb on cpb.codigo_planilla = @p_codigo_planilla and cpb.codigo_supervisor = dpb.codigo_personal and cpb.codigo_empresa = dpb.codigo_empresa and cpb.codigo_grupo = dpb.codigo_grupo
		inner join dbo.articulo_planilla_bono apb on apb.codigo_planilla_bono = @p_codigo_planilla and apb.codigo_empresa = cpb.codigo_empresa and apb.nro_contrato = cpb.numero_contrato
		inner join dbo.articulo a on a.codigo_articulo = apb.codigo_articulo
		inner join personal p on cpb.codigo_personal=p.codigo_personal
		inner join canal_grupo c on c.codigo_canal_grupo=dpb.codigo_canal
		inner join canal_grupo g on g.codigo_canal_grupo=dpb.codigo_grupo
		inner join empresa_sigeco e on e.codigo_empresa=dpb.codigo_empresa
		inner join moneda m on m.codigo_moneda=dpb.codigo_moneda
		WHERE 
			dpb.codigo_planilla = @p_codigo_planilla
			AND ((apb.dinero_ingresado * rpb.porcentaje_pago) / 100) > 0
			AND apb.excluido = 0
		) X
		
	GROUP BY
		codigo_canal_grupo,
		nombre_grupo,
		codigo_personal,
		vendedor,
		codigo_empresa,
		nombre_empresa,	
		codigo_canal,
		nombre_canal,	
		nombre_moneda
		,nro_contrato
		,fecha_contrato
	ORDER BY
		codigo_empresa desc, fecha_contrato, nro_contrato

	SET NOCOUNT ON
END;