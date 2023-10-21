USE SIGECO
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[up_planilla_bono_jn_contabilidad]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].up_planilla_bono_jn_contabilidad
GO

CREATE PROCEDURE [dbo].up_planilla_bono_jn_contabilidad
(
	@p_codigo_planilla	INT
)
AS
BEGIN
	SET NOCOUNT ON
	
	DECLARE @t_1 TABLE(
		empresa				VARCHAR(100)
		,dinero_ingresado	DECIMAL(10, 2)
		,unidad				VARCHAR(100)
	)

	DECLARE
		@v_parametro_canales_jn	VARCHAR(10)
		,@v_porcentaje			DECIMAL(12, 4)

	SET @v_parametro_canales_jn = (SELECT TOP 1 valor FROM dbo.parametro_sistema WHERE codigo_parametro_sistema = 28) -- Canales de Planilla Bono JN

	SET @v_porcentaje = (SELECT ROUND((porcentaje_pago /100), 4) FROM dbo.resumen_planilla_bono WHERE codigo_planilla = @p_codigo_planilla)

	INSERT INTO @t_1
	SELECT 
		e.nombre,round(apb.dinero_ingresado * @v_porcentaje, 2),  un.nombre 
	FROM 
		articulo_planilla_bono apb 
	INNER JOIN articulo a 
		ON a.codigo_articulo = apb.codigo_articulo 
	INNER JOIN unidad_negocio un 
		ON un.codigo_unidad_negocio = a.codigo_unidad_negocio
	INNER JOIN empresa_sigeco e 
		ON e.codigo_empresa = apb.codigo_empresa
	WHERE 
		apb.codigo_planilla_bono = @p_codigo_planilla
		AND apb.excluido = 0

	SELECT 
		@p_codigo_planilla as codigo_planilla, empresa, sum(dinero_ingresado) bono, unidad as unidad_negocio
	FROM @t_1
	GROUP BY 
		empresa, unidad
	ORDER BY 
		empresa, unidad

	SET NOCOUNT OFF
END;