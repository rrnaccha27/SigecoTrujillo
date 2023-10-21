USE SIGECO
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[up_planilla_bono_jn_resumen]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].up_planilla_bono_jn_resumen
GO

CREATE PROCEDURE [dbo].up_planilla_bono_jn_resumen
(
	@p_codigo_planilla	INT
)
AS
BEGIN
	SET NOCOUNT ON

	DECLARE 
		@v_valor DECIMAL(10, 4)
		,@v_codigo_supervisor	INT
		,@v_codigo_grupo		INT
	
	DECLARE
		@c_OFSA		INT = 1
		,@c_FUNJAR	INT = 2
		,@c_IGV		DECIMAL(10, 2)
		,@c_BONO	BIT = 0
		,@c_TIPO_S	BIT = 1

	SET @v_valor = (SELECT ROUND((porcentaje_pago /100), 4) FROM dbo.resumen_planilla_bono WHERE codigo_planilla = @p_codigo_planilla)
	SET @v_codigo_supervisor = (SELECT TOP 1 codigo_personal FROM dbo.detalle_planilla_bono WHERE codigo_planilla = @p_codigo_planilla)
	SET @v_codigo_grupo = (SELECT TOP 1 codigo_grupo FROM dbo.vw_personal WHERE codigo_personal = @v_codigo_supervisor)
	SET @c_IGV = (SELECT (CONVERT(DECIMAL(10,2), valor)/100) + 1 FROM dbo.parametro_sistema WHERE codigo_parametro_sistema = 9)

	SELECT
		@p_codigo_planilla as codigo_planilla
		,nombre as nombre_canal
		,ROUND(SUM(FUNJAR), 2) as FUNJAR
		,ROUND(SUM(FUNJAR) * @v_valor, 2) * CASE WHEN dbo.fn_canal_grupo_percibe_factura(@v_codigo_grupo, @c_FUNJAR, @c_TIPO_S, @c_BONO) = 1 THEN @c_IGV ELSE 1 END as B_FUNJAR
		,ROUND(SUM(OFSA), 2) as OFSA
		,ROUND(SUM(OFSA) * @v_valor, 2)  * CASE WHEN dbo.fn_canal_grupo_percibe_factura(@v_codigo_grupo, @c_OFSA, @c_TIPO_S, @c_BONO) = 1 THEN @c_IGV ELSE 1 END as B_OFSA
		,ROUND(SUM(TOTAL), 2) as TOTAL
		,
		ROUND(SUM(FUNJAR) * @v_valor, 2) * CASE WHEN dbo.fn_canal_grupo_percibe_factura(@v_codigo_grupo, @c_FUNJAR, @c_TIPO_S, @c_BONO) = 1 THEN @c_IGV ELSE 1 END
		+
		ROUND(SUM(OFSA) * @v_valor, 2)  * CASE WHEN dbo.fn_canal_grupo_percibe_factura(@v_codigo_grupo, @c_OFSA, @c_TIPO_S, @c_BONO) = 1 THEN @c_IGV ELSE 1 END
		--ROUND(SUM(TOTAL) * @v_valor, 2) 
		as B_TOTAL
	FROM 
		(SELECT 
			c.nombre
			,case when dpb.codigo_empresa = @c_OFSA THEN sum(apb.dinero_ingresado) else 0 end OFSA
			,case when dpb.codigo_empresa = @c_FUNJAR THEN sum(apb.dinero_ingresado) else 0 end FUNJAR
			,sum(apb.dinero_ingresado) TOTAL
		FROM 
			detalle_planilla_bono dpb
		inner join dbo.resumen_planilla_bono rpb on rpb.codigo_planilla = @p_codigo_planilla and rpb.codigo_personal = dpb.codigo_personal
		inner join dbo.contrato_planilla_bono cpb on cpb.codigo_planilla = @p_codigo_planilla and cpb.codigo_supervisor = dpb.codigo_personal and cpb.codigo_empresa = dpb.codigo_empresa and cpb.codigo_grupo = dpb.codigo_grupo
		inner join dbo.articulo_planilla_bono apb on apb.codigo_planilla_bono = @p_codigo_planilla and apb.codigo_empresa = cpb.codigo_empresa and apb.nro_contrato = cpb.numero_contrato
		inner join dbo.articulo a on a.codigo_articulo = apb.codigo_articulo
		inner join canal_grupo c on c.codigo_canal_grupo=dpb.codigo_canal
		WHERE 
			dpb.codigo_planilla = @p_codigo_planilla
			AND ((apb.dinero_ingresado * rpb.porcentaje_pago) / 100) > 0
			AND apb.excluido = 0
		GROUP BY
			dpb.codigo_canal, c.nombre, dpb.codigo_empresa) sub
	GROUP BY
		nombre;

	SET NOCOUNT OFF
END;