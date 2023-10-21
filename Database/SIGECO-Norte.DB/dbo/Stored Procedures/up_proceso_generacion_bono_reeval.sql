CREATE PROCEDURE dbo.up_proceso_generacion_bono_reeval
(
	@p_codigo_planilla INT
	,@p_codigo_personal		INT
)
AS
BEGIN
	SET NOCOUNT ON

	--declare @p_codigo_planilla INT = 41
	--,@p_codigo_personal		INT = 1282

	DECLARE
		@v_codigo_tipo_planilla	INT
		,@v_codigo_canal		INT
		,@c_igv					DECIMAL(12, 4)
		,@v_codigo_regla_bono INT
		,@v_monto_meta_rango_inicio DECIMAL(12, 4)
		,@v_porcentaje_pago DECIMAL(12, 4)
		,@v_monto_tope DECIMAL(12, 4)
		,@v_cantidad_ventas INT
		,@v_calcular_igv BIT
		,@v_monto_meta_rango_fin DECIMAL(12, 4)
		,@v_meta_lograda DECIMAL(12, 4)
		,@v_ventas INT = 10
		,@v_monto_contrato DECIMAL(12, 4)

	SELECT
		@v_monto_meta_rango_inicio = NULL
		,@v_monto_meta_rango_fin = 9999999.99
		,@v_porcentaje_pago = NULL
		,@v_monto_tope = NULL
		,@v_cantidad_ventas = NULL
		,@v_meta_lograda = NULL

	SELECT TOP 1 
		@v_codigo_tipo_planilla = codigo_tipo_planilla 
		,@v_codigo_canal = codigo_canal
	FROM 
		dbo.planilla_bono 
	WHERE 
		codigo_planilla = @p_codigo_planilla

	SET @c_igv = (SELECT TOP 1 ROUND(CONVERT(DECIMAL(12, 4), valor)/100, 4) + 1 FROM dbo.parametro_sistema WHERE codigo_parametro_sistema = 9)-- Confirmar el codigo_parametro_sistema

	SELECT TOP 1
		@v_codigo_regla_bono = codigo_regla_calculo_bono
		,@v_monto_meta_rango_inicio = monto_meta
		,@v_porcentaje_pago = porcentaje_pago
		,@v_monto_tope = monto_tope
		,@v_cantidad_ventas = cantidad_ventas
		,@v_calcular_igv = calcular_igv
	FROM 
		dbo.pcb_regla_calculo_bono
	WHERE 
		codigo_canal = @v_codigo_canal
		and codigo_tipo_planilla = @v_codigo_tipo_planilla
		and GETDATE() between vigencia_inicio and vigencia_fin
		--and ((codigo_grupo IS NULL) OR (codigo_grupo IS NOT NULL AND codigo_grupo = @v_codigo_grupo)) /* T0D0 obtener el grupo */
	ORDER BY codigo_canal, codigo_grupo DESC

	SELECT 
		@v_monto_contrato = monto_contratado
	FROM
		dbo.resumen_planilla_bono 
	WHERE 
		codigo_planilla = @p_codigo_planilla 
		AND codigo_personal = @p_codigo_personal

	IF (@v_monto_meta_rango_inicio IS NOT NULL)
	BEGIN
		IF (@v_ventas >= @v_cantidad_ventas)
		BEGIN				
			--print '@v_monto_contrato [' + convert(varchar(20), @v_monto_contrato) + ']  @v_monto_meta_rango_inicio [' + convert(varchar(20), @v_monto_meta_rango_inicio) + '] @v_monto_meta_rango_fin [' + convert(varchar(20), @v_monto_meta_rango_fin) + ']'
			SET @v_meta_lograda = ROUND((@v_monto_contrato *100) / @v_monto_meta_rango_inicio, 4)
			--print '@v_monto_contrato [' + convert(varchar(20), @v_monto_contrato) + ']  @v_monto_meta_rango_inicio [' + convert(varchar(20), @v_monto_meta_rango_inicio) + '] @v_monto_meta_rango_fin [' + convert(varchar(20), @v_monto_meta_rango_fin) + ']'
			
			-- Procedemos con la validacion de la regla a nivel 100%
			IF (@v_monto_contrato NOT BETWEEN @v_monto_meta_rango_inicio and @v_monto_meta_rango_fin)
			BEGIN
				--print 'rango alternativo'					
				SET @v_monto_meta_rango_fin = @v_monto_meta_rango_inicio
				SET @v_porcentaje_pago = NULL
				--Obtenemos segun la matriz de posibles valores alternos
				select top 1 
					@v_porcentaje_pago = ROUND(porcentaje_pago / 100, 4)
					,@v_monto_meta_rango_inicio = monto_meta 
				from dbo.regla_calculo_bono_matriz 
				where 
					codigo_regla_calculo_bono = @v_codigo_regla_bono
					AND @v_monto_contrato >= monto_meta order by monto_meta desc				
				
				--print '@v_monto_contrato [' + convert(varchar(20), @v_monto_contrato) + ']  @v_monto_meta_rango_inicio [' + convert(varchar(20), @v_monto_meta_rango_inicio) + '] @v_monto_meta_rango_fin [' + convert(varchar(20), @v_monto_meta_rango_fin) + '] @v_porcentaje_pago [' + convert(varchar(10), @v_porcentaje_pago) + ']'
			END
				
			-- Debe estar en el rango y tener porcentaje de pago				
			IF (@v_monto_contrato between @v_monto_meta_rango_inicio and @v_monto_meta_rango_fin AND @v_porcentaje_pago IS NOT NULL)
			BEGIN
				--print 'recalcular'

				UPDATE dbo.resumen_planilla_bono
				SET
					porcentaje_pago = (@v_porcentaje_pago * 100)
					,meta_lograda = @v_meta_lograda
				WHERE 
					codigo_planilla =  @p_codigo_planilla
					AND codigo_personal = @p_codigo_personal

				UPDATE dbo.detalle_planilla_bono
				SET
					monto_neto = (monto_ingresado * @v_porcentaje_pago)-- / 100
				WHERE 
					codigo_planilla =  @p_codigo_planilla
					AND codigo_personal = @p_codigo_personal

				UPDATE dbo.detalle_planilla_bono
				SET
					monto_bruto = (monto_neto / @c_igv) 
					,monto_neto = (monto_neto / @c_igv) 
				WHERE 
					codigo_planilla =  @p_codigo_planilla
					AND codigo_personal = @p_codigo_personal
					AND monto_igv = 0

				UPDATE dbo.detalle_planilla_bono
				SET
					monto_bruto = (monto_neto / @c_igv) 
					,monto_igv = monto_neto - (monto_neto / @c_igv) 
				WHERE 
					codigo_planilla =  @p_codigo_planilla
					AND codigo_personal = @p_codigo_personal
					AND monto_igv > 0
			END
			ELSE
			BEGIN	
				--print 'no ubico matriz de pago'
				UPDATE l
				SET 
					codigo_estado = 2
					,observacion = 'Por exclusion y reproceso, no se ubico matriz de pago'
				FROM 
					log_proceso_bono l
				INNER JOIN dbo.contrato_planilla_bono cpb 
					ON cpb.codigo_planilla = @p_codigo_planilla and cpb.codigo_planilla = l.codigo_planilla and l.codigo_empresa = cpb.codigo_empresa and l.nro_contrato = cpb.numero_contrato
				WHERE
					cpb.codigo_personal = @p_codigo_personal

				DELETE FROM contrato_planilla_bono where codigo_planilla = @p_codigo_planilla AND codigo_personal = @p_codigo_personal
				DELETE FROM resumen_planilla_bono where codigo_planilla = @p_codigo_planilla AND codigo_personal = @p_codigo_personal
				DELETE FROM detalle_planilla_bono where codigo_planilla = @p_codigo_planilla AND codigo_personal = @p_codigo_personal
			END

		END
		ELSE
		BEGIN
			--print 'no llego a cantidad ventas'
			UPDATE l
			SET 
				codigo_estado = 2
				,observacion = 'Por exclusion y reproceso, no llego a cantidad de ventas'
			FROM 
				log_proceso_bono l
			INNER JOIN dbo.contrato_planilla_bono cpb 
				ON cpb.codigo_planilla = @p_codigo_planilla and cpb.codigo_planilla = l.codigo_planilla and l.codigo_empresa = cpb.codigo_empresa and l.nro_contrato = cpb.numero_contrato
			WHERE
				cpb.codigo_personal = @p_codigo_personal

			DELETE FROM contrato_planilla_bono where codigo_planilla = @p_codigo_planilla AND codigo_personal = @p_codigo_personal
			DELETE FROM resumen_planilla_bono where codigo_planilla = @p_codigo_planilla AND codigo_personal = @p_codigo_personal
			DELETE FROM detalle_planilla_bono where codigo_planilla = @p_codigo_planilla AND codigo_personal = @p_codigo_personal
		END
	END
	ELSE
	BEGIN
		--print 'no tiene regla de pago'
		UPDATE l
		SET 
			codigo_estado = 2
			,observacion = 'Por exclusion y reproceso, no tiene regla de pago'
		FROM 
			log_proceso_bono l
		INNER JOIN dbo.contrato_planilla_bono cpb 
			ON cpb.codigo_planilla = @p_codigo_planilla and cpb.codigo_planilla = l.codigo_planilla and l.codigo_empresa = cpb.codigo_empresa and l.nro_contrato = cpb.numero_contrato
		WHERE
			cpb.codigo_personal = @p_codigo_personal

		DELETE FROM contrato_planilla_bono where codigo_planilla = @p_codigo_planilla AND codigo_personal = @p_codigo_personal
		DELETE FROM resumen_planilla_bono where codigo_planilla = @p_codigo_planilla AND codigo_personal = @p_codigo_personal
		DELETE FROM detalle_planilla_bono where codigo_planilla = @p_codigo_planilla AND codigo_personal = @p_codigo_personal
	END

	SET NOCOUNT OFF
END;