USE SIGECO
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[up_planilla_bono_jn_resumen_titulos]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].up_planilla_bono_jn_resumen_titulos
GO

CREATE PROCEDURE [dbo].up_planilla_bono_jn_resumen_titulos
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

	DECLARE @t_empresas as table(
		codigo_empresa int
		,nombre varchar(250)
	)

	DECLARE @t_titulo as table(
		codigo_planilla int
		,titulo_ofsa varchar(250)
		,titulo_funjar varchar(250)
	)

	SET @v_valor = (SELECT ROUND((porcentaje_pago /100), 4) FROM dbo.resumen_planilla_bono WHERE codigo_planilla = @p_codigo_planilla)
	SET @v_codigo_supervisor = (SELECT TOP 1 codigo_personal FROM dbo.detalle_planilla_bono WHERE codigo_planilla = @p_codigo_planilla)
	SET @v_codigo_grupo = (SELECT TOP 1 codigo_grupo FROM dbo.vw_personal WHERE codigo_personal = @v_codigo_supervisor)


	INSERT INTO 
		@t_empresas
	SELECT 
		dpb.codigo_empresa
		,MIN(es.nombre) as empresa
	FROM 
		detalle_planilla_bono dpb
	inner join dbo.resumen_planilla_bono rpb on rpb.codigo_planilla = @p_codigo_planilla and rpb.codigo_personal = dpb.codigo_personal
	inner join dbo.contrato_planilla_bono cpb on cpb.codigo_planilla = @p_codigo_planilla and cpb.codigo_supervisor = dpb.codigo_personal and cpb.codigo_empresa = dpb.codigo_empresa and cpb.codigo_grupo = dpb.codigo_grupo
	inner join dbo.articulo_planilla_bono apb on apb.codigo_planilla_bono = @p_codigo_planilla and apb.codigo_empresa = cpb.codigo_empresa and apb.nro_contrato = cpb.numero_contrato
	inner join dbo.articulo a on a.codigo_articulo = apb.codigo_articulo
	inner join canal_grupo c on c.codigo_canal_grupo=dpb.codigo_canal
	inner join empresa_sigeco es on dpb.codigo_empresa = es.codigo_empresa
	WHERE 
		dpb.codigo_planilla = @p_codigo_planilla
		AND ((apb.dinero_ingresado * rpb.porcentaje_pago) / 100) > 0
		AND apb.excluido = 0
		--rand dpb.codigo_empresa = 1
	GROUP BY
		dpb.codigo_empresa

	INSERT INTO @t_titulo VALUES(@p_codigo_planilla, '', '')
	

	SELECT
		@p_codigo_planilla as codigo_planilla
		,
			ISNULL((SELECT nombre FROM @t_empresas WHERE codigo_empresa = @c_FUNJAR) + ' ', '')
			+ CASE WHEN dbo.fn_canal_grupo_percibe_factura(@v_codigo_grupo, @c_FUNJAR, @c_TIPO_S, @c_BONO) = 1 THEN '(incl. IGV)' ELSE '' END as titulo_funjar
		,
			ISNULL((SELECT nombre FROM @t_empresas WHERE codigo_empresa = @c_OFSA) + ' ', '') 
			+ CASE WHEN dbo.fn_canal_grupo_percibe_factura(@v_codigo_grupo, @c_OFSA, @c_TIPO_S, @c_BONO) = 1 THEN '(incl. IGV)'ELSE '' END as titulo_ofsa


	SET NOCOUNT OFF
END;