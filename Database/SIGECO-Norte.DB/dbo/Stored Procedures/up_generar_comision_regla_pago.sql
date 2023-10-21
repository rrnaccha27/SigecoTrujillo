CREATE PROCEDURE [dbo].[up_generar_comision_regla_pago]
(
	@p_nro_contrato						varchar(100)
	,@p_codigo_empresa_o				varchar(4)
	,@p_codigo_camposanto				int
	,@p_codigo_empresa					int
	,@p_codigo_canal_grupo				int
	,@p_codigo_tipo_venta				int
	,@p_codigo_tipo_pago				int
	,@p_codigo_articulo					int
	,@p_retorno							int output
	,@p_codigo_regla_pago				int output
	,@p_observacion						varchar(200) output
)
AS
BEGIN

	SET @p_retorno = 0

	DECLARE 
		@v_fecha_proceso	DATETIME

	SET @v_fecha_proceso = (SELECT TOP 1 CreateDate FROM dbo.cabecera_contrato WHERE NumAtCard = @p_nro_contrato AND Codigo_empresa = @p_codigo_empresa_o )

	SET @p_codigo_regla_pago = NULL

	--SELECT 
	--	*,
	--	dbo.fn_generar_cronograma_comision_eval_tipo_articulo(@p_nro_contrato, @p_codigo_empresa_o, codigo_tipo_articulo) AS EVAL_TIPO_ARTICULO,
	--	dbo.fn_generar_cronograma_comision_eval_plan_integral(@p_nro_contrato, @p_codigo_empresa_o, codigo_campo_santo, @v_fecha_proceso) AS EVAL_PLAN_INTEGRAL
	--FROM
	--	dbo.pcc_regla_pago_comision
	--WHERE 
	--	(codigo_campo_santo = @p_codigo_camposanto or codigo_campo_santo is null)
	--	and (codigo_empresa = @p_codigo_empresa or codigo_empresa is null)
	--	and (codigo_canal_grupo = @p_codigo_canal_grupo or codigo_canal_grupo is null)
	--	and (codigo_tipo_venta = @p_codigo_tipo_venta or codigo_tipo_venta is null)
	--	and (codigo_tipo_pago = @p_codigo_tipo_pago or codigo_tipo_pago is null)
	--	and (codigo_tipo_pago = @p_codigo_tipo_pago or codigo_tipo_pago is null)
	--	and @v_fecha_proceso between vigencia_inicio and vigencia_fin 
	--ORDER BY
	--	orden DESC
	
	SELECT TOP 1
		@p_codigo_regla_pago = codigo_regla_pago
		--,@v_codigo_tipo_articulo = codigo_tipo_articulo
		--,@v_evaluar_plan_integral = evaluar_plan_integral
		--,@v_evaluar_anexado = evaluar_anexado
		--,@v_codigo_tipo_articulo_anexado = codigo_tipo_articulo_anexado
	FROM
		dbo.pcc_regla_pago_comision
	WHERE 
		(codigo_campo_santo = @p_codigo_camposanto or codigo_campo_santo is null)
		and (codigo_empresa = @p_codigo_empresa or codigo_empresa is null)
		and (codigo_canal_grupo = @p_codigo_canal_grupo or codigo_canal_grupo is null)
		and (codigo_tipo_venta = @p_codigo_tipo_venta or codigo_tipo_venta is null)
		and (codigo_tipo_pago = @p_codigo_tipo_pago or codigo_tipo_pago is null)
		and (codigo_tipo_pago = @p_codigo_tipo_pago or codigo_tipo_pago is null)
		AND
		(
			(
				(codigo_tipo_articulo > 0 AND evaluar_plan_integral = 1)
				AND
				(
					dbo.fn_generar_cronograma_comision_eval_tipo_articulo(@p_nro_contrato, @p_codigo_empresa_o, codigo_tipo_articulo) = 1
					OR
					dbo.fn_generar_cronograma_comision_eval_plan_integral(@p_nro_contrato, @p_codigo_empresa_o, codigo_campo_santo, @v_fecha_proceso) = 1
				)
			)
			OR
			(
				(codigo_tipo_articulo = 0 OR (codigo_tipo_articulo > 0 AND dbo.fn_generar_cronograma_comision_eval_tipo_articulo(@p_nro_contrato, @p_codigo_empresa_o, codigo_tipo_articulo) = 1) )
				AND
				(evaluar_plan_integral = 0 OR (evaluar_plan_integral = 1 AND dbo.fn_generar_cronograma_comision_eval_plan_integral(@p_nro_contrato, @p_codigo_empresa_o, codigo_campo_santo, @v_fecha_proceso) = 1) )
			)
		)
		AND (evaluar_anexado = 0 or (evaluar_anexado = 1 AND evaluar_anexado = dbo.fn_generar_cronograma_comision_eval_anexado(@p_nro_contrato, @p_codigo_empresa_o, codigo_tipo_articulo_anexado)) )		
		--and codigo_articulo = @p_codigo_articulo
		and @v_fecha_proceso between vigencia_inicio and vigencia_fin 
	ORDER BY
		orden DESC
	
	IF (@p_codigo_regla_pago IS NULL)
	BEGIN
		SET @p_retorno = 0
		SET @p_observacion = 'No existe regla de pago.'
		SET NOEXEC ON;
	END

	SET @p_retorno = 1

END