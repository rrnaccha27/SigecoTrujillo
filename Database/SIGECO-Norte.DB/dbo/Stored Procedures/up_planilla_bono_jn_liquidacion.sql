USE SIGECO
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[up_planilla_bono_jn_liquidacion]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].up_planilla_bono_jn_liquidacion
GO

CREATE PROCEDURE [dbo].up_planilla_bono_jn_liquidacion
(
	 @p_codigo_planilla	INT
)
AS
BEGIN
	SET NOCOUNT ON

	--declare @p_codigo_planilla int =116

	DECLARE 
		@v_valor					DECIMAL(10, 4)
		,@v_codigo_supervisor		INT
		,@v_codigo_grupo			INT
		,@v_nombre_personal			VARCHAR(250)
		,@v_codigo_personal			INT
		,@v_nombre_grupo			VARCHAR(250)
		,@v_documento_personal		VARCHAR(250)
		,@v_estado_planilla			INT
		,@v_concepto_liquidacion	VARCHAR(250)
	
	DECLARE
		@c_OFSA		INT = 1
		,@c_FUNJAR	INT = 2
		,@c_IGV		DECIMAL(10, 2)
		,@c_BONO	BIT = 0
		,@c_TIPO_S	BIT = 1

	DECLARE @t_resumen AS TABLE(
		codigo_planilla		INT
		,codigo_empresa		INT
		,dinero_ingresado	DECIMAL(10, 2)
		,con_igv			DECIMAL(10, 2)
		,sin_igv			DECIMAL(10, 2)
		,igv				DECIMAL(10, 2)
	)

	SET @v_valor = (SELECT ROUND((porcentaje_pago /100), 4) FROM dbo.resumen_planilla_bono WHERE codigo_planilla = @p_codigo_planilla)
	SET @v_codigo_supervisor = (SELECT TOP 1 codigo_personal FROM dbo.detalle_planilla_bono WHERE codigo_planilla = @p_codigo_planilla)
	SET @c_IGV = (SELECT (CONVERT(DECIMAL(10,2), valor)/100) + 1 FROM dbo.parametro_sistema WHERE codigo_parametro_sistema = 9)

	SELECT TOP 1 
		@v_estado_planilla = codigo_estado_planilla 
		,@v_concepto_liquidacion = 'CORRESPONDIENTE AL PERIODO DEL ' + dbo.fn_formatear_fecha(fecha_inicio) + ' AL ' + dbo.fn_formatear_fecha(fecha_fin)
	FROM 
		dbo.planilla_bono 
	WHERE 
		codigo_planilla = @p_codigo_planilla
	
	SELECT TOP 1 
		@v_codigo_grupo = codigo_grupo 
		,@v_nombre_personal = nombre_personal
		,@v_codigo_personal = codigo_personal
		,@v_nombre_grupo = nombre_grupo
	FROM 
		dbo.vw_personal 
	WHERE 
		codigo_personal = @v_codigo_supervisor
	
	SELECT TOP 1
		@v_documento_personal = td.nombre_tipo_documento + ': ' + p.nro_ruc
	FROM
		dbo.personal p
	INNER JOIN dbo.tipo_documento td
		ON td.codigo_tipo_documento = p.codigo_tipo_documento
	WHERE
		p.codigo_personal =  @v_codigo_supervisor

	INSERT INTO @t_resumen
	SELECT
		@p_codigo_planilla as codigo_planilla
		,MIN(codigo_empresa) as codigo_empresa
		,ROUND(SUM(TOTAL), 2) as TOTAL
		,ROUND(ROUND(SUM(TOTAL) * @v_valor, 2, 1) * @c_IGV, 2, 1) as con_igv
		,ROUND(SUM(TOTAL) * @v_valor, 2) as sin_igv
		,ROUND(ROUND(ROUND(SUM(TOTAL) * @v_valor, 2, 1) * @c_IGV, 2, 1) - ROUND(SUM(TOTAL) * @v_valor, 2, 1), 2) as igv
	FROM 
		(SELECT 
			dpb.codigo_empresa
			,ROUND(SUM(apb.dinero_ingresado), 2, 1) TOTAL
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
			AND dbo.fn_canal_grupo_percibe_factura(@v_codigo_grupo, cpb.codigo_empresa, @c_TIPO_S, @c_BONO) = 1
		GROUP BY
			dpb.codigo_canal, c.nombre, dpb.codigo_empresa) sub

	SELECT
		@p_codigo_planilla as codigo_planilla
		,pl.codigo_empresa as codigo_empresa
		,ee.nombre as nombre_empresa
		,ee.nombre_largo as nombre_empresa_largo
		,direccion_fiscal as direccion_fiscal_empresa
		,ruc as ruc_empresa
		,@v_nombre_grupo as nombre_grupo
		,@v_codigo_supervisor as codigo_personal
		,@v_nombre_personal as nombre_personal
		,@v_documento_personal as documento_personal
		,con_igv as monto_bono
		,sin_igv as  monto_sin_igv
		,igv as monto_igv
		,dbo.fn_GetLetrasPrecio(con_igv, 1) as  monto_bono_letras
		,@v_concepto_liquidacion AS concepto_liquidacion
		,@v_estado_planilla as codigo_estado_planilla
		,dinero_ingresado
	FROM
		@t_resumen pl
	INNER JOIN dbo.empresa_sigeco ee
		ON ee.codigo_empresa = pl.codigo_empresa 
	WHERE
		codigo_planilla = @p_codigo_planilla
	ORDER BY
		ee.nombre ASC

	SET NOCOUNT OFF
END;

