CREATE PROCEDURE dbo.up_regla_calculo_bono_validar
(
	 @p_codigo_tipo_planilla		INT
	,@p_codigo_canal				INT
	,@p_codigo_grupo				INT
	,@p_vigencia_inicio				VARCHAR(8)
	,@p_vigencia_fin				VARCHAR(8)
	,@p_codigo_regla_calculo_bono	INT OUTPUT
)
AS
BEGIN

	DECLARE
		@v_vigencia_inicio	DATETIME = CONVERT(DATETIME, @p_vigencia_inicio)
		,@v_vigencia_fin	DATETIME = CONVERT(DATETIME, @p_vigencia_fin)

	SELECT TOP 1
		@p_codigo_regla_calculo_bono = codigo_regla_calculo_bono
	FROM
		dbo.regla_calculo_bono r
	WHERE
		estado_registro = 1
		AND codigo_tipo_planilla = @p_codigo_tipo_planilla
		AND codigo_canal = @p_codigo_canal
		AND codigo_grupo = CASE WHEN @p_codigo_grupo = 0 THEN NULL ELSE @p_codigo_grupo END
		AND (
			@v_vigencia_inicio BETWEEN r.vigencia_inicio AND r.vigencia_fin
			OR @v_vigencia_fin BETWEEN r.vigencia_inicio AND r.vigencia_fin
			OR r.vigencia_inicio BETWEEN @v_vigencia_inicio AND @v_vigencia_fin
			OR r.vigencia_fin BETWEEN @v_vigencia_inicio AND @v_vigencia_fin)
	
	SET @p_codigo_regla_calculo_bono = ISNULL(@p_codigo_regla_calculo_bono, 0)
END;