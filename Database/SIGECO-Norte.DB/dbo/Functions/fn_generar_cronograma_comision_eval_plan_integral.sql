CREATE FUNCTION dbo.fn_generar_cronograma_comision_eval_plan_integral
(
	@p_nro_contrato				VARCHAR(100)
	,@p_codigo_empresa_o		VARCHAR(4)
	,@p_codigo_camposanto		INT
	,@v_fecha_proceso			DATETIME
)
RETURNS BIT
AS
BEGIN
	DECLARE
		@v_retorno				BIT = 0,
		@v_cantidad_registros	INT = 0

	DECLARE @t_articulo_integral TABLE
	(
		codigo_tipo_articulo	int
	)
	
	INSERT INTO @t_articulo_integral
	SELECT pidet.codigo_tipo_articulo
	FROM dbo.plan_integral_detalle pidet
	INNER JOIN plan_integral pint
		ON pint.estado_registro = 1 and pint.codigo_plan_integral = pidet.codigo_plan_integral
	WHERE
		pidet.estado_registro = 1
		AND pidet.codigo_campo_santo = @p_codigo_camposanto
		AND @v_fecha_proceso BETWEEN pint.vigencia_inicio AND CONVERT(DATETIME, CONVERT(VARCHAR(8), pint.vigencia_fin, 112) + ' 23:59:59')

	INSERT INTO @t_articulo_integral
	SELECT pidet.codigo_tipo_articulo_2
	FROM dbo.plan_integral_detalle pidet
	INNER JOIN plan_integral pint
		on pint.estado_registro = 1 and pint.codigo_plan_integral = pidet.codigo_plan_integral
	WHERE
		pidet.estado_registro = 1
		AND pidet.codigo_campo_santo = @p_codigo_camposanto
		AND @v_fecha_proceso BETWEEN pint.vigencia_inicio AND CONVERT(DATETIME, CONVERT(VARCHAR(8), pint.vigencia_fin, 112) + ' 23:59:59')

	SET @v_cantidad_registros = ISNULL((SELECT COUNT(codigo_tipo_articulo) FROM @t_articulo_integral), 0)

	IF (@v_cantidad_registros > 0)
	BEGIN
		SET @v_cantidad_registros = NULL

		SELECT @v_cantidad_registros = COUNT(dc.ItemCode)
		FROM detalle_contrato dc
		INNER JOIN articulo a 
			ON dc.ItemCode = a.codigo_sku
		INNER JOIN @t_articulo_integral ai
			ON ai.codigo_tipo_articulo = a.codigo_tipo_articulo
		WHERE
			dc.NumAtCard = @p_nro_contrato
			AND dc.Codigo_empresa = @p_codigo_empresa_o

		SET @v_cantidad_registros = ISNULL(@v_cantidad_registros, 0)

		IF (@v_cantidad_registros = 0)
		BEGIN
			SET @v_cantidad_registros = NULL
				
			SELECT @v_cantidad_registros = COUNT(dc.ItemCode)
			FROM detalle_contrato dc
			INNER JOIN articulo a 
				ON dc.ItemCode = a.codigo_sku
			INNER JOIN @t_articulo_integral ai
				ON ai.codigo_tipo_articulo = a.codigo_tipo_articulo
			WHERE
				dc.NumAtCard = @p_nro_contrato
				AND dc.Codigo_empresa = CASE WHEN @p_codigo_empresa_o = '0002' THEN '0939' ELSE '0002' END
		END
	END

	SET @v_retorno = CASE WHEN ISNULL(@v_cantidad_registros, 0) = 0 THEN 0 ELSE 1 END

	RETURN @v_retorno
END