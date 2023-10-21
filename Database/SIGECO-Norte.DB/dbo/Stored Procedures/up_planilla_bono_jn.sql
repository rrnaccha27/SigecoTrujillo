USE SIGECO
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[up_planilla_bono_jn]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].up_planilla_bono_jn
GO

CREATE PROCEDURE [dbo].[up_planilla_bono_jn]
(
	@p_codigo_planilla int
)
AS
BEGIN
	SET NOCOUNT ON

	DECLARE @t_planilla_resumen TABLE(
		codigo_planilla INT,
		nombre		VARCHAR(100),
		funjar		DECIMAL(12, 2),
		b_funjar	DECIMAL(12, 2),
		fosa		DECIMAL(12, 2),
		b_ofsa		DECIMAL(12, 2),
		total		DECIMAL(12, 2),
		b_total		DECIMAL(12, 2)
	)

	DECLARE @t_planilla TABLE(
		codigo_planilla			INT
		,numero_planilla		VARCHAR(20)
		,codigo_tipo_planilla	INT
		,codigo_estado_planilla	INT
		,fecha_inicio			VARCHAR(10)
		,fecha_fin				VARCHAR(10)
		,fecha_registra			DATETIME
		,dinero_ingresado		DECIMAL(12, 2)
		,bono					DECIMAL(12, 2)
	)

	DECLARE
		@v_total						DECIMAL(12, 2)
		,@v_bono						DECIMAL(12, 2)
		,@v_porcentaje					VARCHAR(4)
		,@v_meta_90						DECIMAL(12, 2)
		,@v_meta_100					DECIMAL(12, 2)
		,@v_parametro_canales_jn		VARCHAR(10)
		,@v_codigo_regla_calculo_bono	INT

	SET @v_parametro_canales_jn = (SELECT TOP 1 valor FROM dbo.parametro_sistema WHERE codigo_parametro_sistema = 28) -- Canales de Planilla Bono JN

	INSERT INTO @t_planilla_resumen
	SELECT 
		codigo_planilla,nombre_canal,FUNJAR,B_FUNJAR,OFSA,B_OFSA,TOTAL,B_TOTAL
	FROM 
		dbo.sigeco_reporte_bono_jn_resumen
	WHERE 
		codigo_planilla = @p_codigo_planilla
	ORDER BY 
		nombre_canal
	--EXEC up_planilla_bono_jn_resumen @p_codigo_planilla

	SELECT @v_total = sum(total), @v_bono = sum(b_total) FROM @t_planilla_resumen

	INSERT INTO @t_planilla
	SELECT TOP 1
		pl.codigo_planilla
		,pl.numero_planilla
		,pl.codigo_tipo_planilla
		,pl.codigo_estado_planilla
		,convert(varchar, pl.fecha_inicio, 103) as fecha_inicio
		,convert(varchar, pl.fecha_fin, 103) as fecha_fin
		,fecha_registra
		,@v_total as dinero_ingresado
		,@v_bono as bono
	FROM 
		planilla_bono pl 
	WHERE 
		pl.codigo_planilla = @p_codigo_planilla;

	SELECT TOP 1
		@v_meta_90 = monto_meta
		,@v_meta_100 = ROUND(monto_meta*100/90, 2)
		,@v_porcentaje = LEFT(CONVERT(VARCHAR, ROUND(porcentaje_pago * 100, 1)), 3) + '%'
		,@v_codigo_regla_calculo_bono = codigo_regla_calculo_bono
	FROM 
		dbo.pcb_regla_calculo_bono
	INNER JOIN dbo.fn_SplitReglaTipoPlanilla(@v_parametro_canales_jn) x on pcb_regla_calculo_bono.codigo_canal = x.codigo -- /*funes y gestores*/
	WHERE 
		es_jn = 1
		and (SELECT fecha_registra FROM @t_planilla) between vigencia_inicio and vigencia_fin
	ORDER BY codigo_canal, codigo_grupo desc

	IF (@v_total NOT BETWEEN @v_meta_90 AND @v_meta_100)
	BEGIN

		SELECT TOP 1
			@v_meta_90 = monto_meta
			--,@v_meta_100 = ROUND(monto_meta*100/90, 2)
			,@v_porcentaje = LEFT(CONVERT(VARCHAR, ROUND(porcentaje_pago * 100, 1)), 3) + '%'
		FROM 
			dbo.pcb_regla_calculo_bono_matriz
		WHERE
			codigo_regla_calculo_bono = @v_codigo_regla_calculo_bono	
	END

	SELECT TOP 1
		codigo_planilla			
		,numero_planilla		
		,codigo_tipo_planilla	
		,codigo_estado_planilla	
		,fecha_inicio			
		,fecha_fin				
		,fecha_registra			
		,dinero_ingresado		
		,bono					
		,@v_porcentaje as porcentaje				
		,@v_meta_100 as meta_100
		,@v_meta_90 as meta_90
	FROM
		@t_planilla

	SET NOCOUNT OFF
END;