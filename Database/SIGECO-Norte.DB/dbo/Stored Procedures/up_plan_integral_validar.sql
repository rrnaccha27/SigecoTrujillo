CREATE PROCEDURE dbo.up_plan_integral_validar
(
	@p_codigo_plan_integral	int
	,@p_resultado			int output
)
AS
BEGIN

	DECLARE @t_plan_detalle TABLE
	(
		indice					int identity(1, 1) not null,
		codigo_campo_santo		int,
		codigo_tipo_articulo	int, 
		codigo_tipo_articulo_2	int
	)

	DECLARE @t_plan_detalle_original TABLE
	(
		indice					int identity(1, 1) not null,
		codigo_campo_santo		int,
		codigo_tipo_articulo	int, 
		codigo_tipo_articulo_2	int
	)

	DECLARE @t_plan table
	(
		codigo_plan_integral	int
	)

	DECLARE 
		@v_indice int
		,@v_total int
		,@v_indice_original int
		,@v_total_original int
		,@v_vigencia_inicio datetime
		,@v_vigencia_fin datetime
		,@v_codigo_campo_santo		int
		,@v_codigo_tipo_articulo	int 
		,@v_codigo_tipo_articulo_2	int
		,@v_codigo_campo_santo_o	int
		,@v_codigo_tipo_articulo_o		int 
		,@v_codigo_tipo_articulo_2_o	int

	SET NOEXEC OFF

	SET @p_resultado = 0

	SELECT 
		@v_vigencia_inicio = vigencia_inicio
		,@v_vigencia_fin  = vigencia_fin 
	FROM dbo.plan_integral
	WHERE
		codigo_plan_integral = @p_codigo_plan_integral

	INSERT INTO @t_plan(codigo_plan_integral)
	SELECT codigo_plan_integral
	FROM dbo.plan_integral
	WHERE 
		estado_registro = 1
		AND codigo_plan_integral <> @p_codigo_plan_integral
		AND (
			@v_vigencia_inicio between vigencia_inicio AND vigencia_fin
			OR @v_vigencia_fin between vigencia_inicio AND vigencia_fin
			OR vigencia_inicio between @v_vigencia_inicio AND @v_vigencia_fin
			OR vigencia_fin between @v_vigencia_inicio AND @v_vigencia_fin)

	SET @v_total = ISNULL((SELECT COUNT(codigo_plan_integral) FROM @t_plan), 0)

	IF (@v_total = 0)
	BEGIN
		SET @p_resultado = 1
		SET NOEXEC ON	
	END

	INSERT INTO @t_plan_detalle (codigo_campo_santo, codigo_tipo_articulo, codigo_tipo_articulo_2)
	SELECT codigo_campo_santo, codigo_tipo_articulo, codigo_tipo_articulo_2
	FROM dbo.plan_integral_detalle
	WHERE estado_registro = 1
		AND codigo_plan_integral IN (SELECT codigo_plan_integral FROM @t_plan)

	INSERT INTO @t_plan_detalle_original (codigo_campo_santo, codigo_tipo_articulo, codigo_tipo_articulo_2)
	SELECT codigo_campo_santo, codigo_tipo_articulo, codigo_tipo_articulo_2
	FROM dbo.plan_integral_detalle
	WHERE codigo_plan_integral = @p_codigo_plan_integral

	SET @v_total = ISNULL((SELECT COUNT(codigo_campo_santo) FROM @t_plan_detalle), 0)
	SET @v_total_original = ISNULL((SELECT COUNT(codigo_campo_santo) FROM @t_plan_detalle_original), 0)

	IF (@v_total = 0)
	BEGIN
		SET @p_resultado = 1
		SET NOEXEC ON	
	END

	SET @v_indice = 1
	WHILE (@v_indice <= @v_total)
	BEGIN
	
		SELECT
			@v_codigo_campo_santo = codigo_campo_santo
			,@v_codigo_tipo_articulo = codigo_tipo_articulo
			,@v_codigo_tipo_articulo_2 = codigo_tipo_articulo_2
		FROM
			@t_plan_detalle
		WHERE
			indice = @v_indice

		SET @v_indice_original = 1
		WHILE (@v_indice_original <= @v_total_original)
		BEGIN
	
			SELECT
				@v_codigo_campo_santo_o = codigo_campo_santo
				,@v_codigo_tipo_articulo_o = codigo_tipo_articulo
				,@v_codigo_tipo_articulo_2_o = codigo_tipo_articulo_2
			FROM
				@t_plan_detalle_original
			WHERE
				indice = @v_indice_original

			IF ( 
				(@v_codigo_campo_santo = @v_codigo_campo_santo_o)
				AND
				( (@v_codigo_tipo_articulo = @v_codigo_tipo_articulo_o and @v_codigo_tipo_articulo_2 = @v_codigo_tipo_articulo_2_o) 
				or
				(@v_codigo_tipo_articulo = @v_codigo_tipo_articulo_2_o and @v_codigo_tipo_articulo_2 = @v_codigo_tipo_articulo_o)  )
			)
			BEGIN
				SET NOEXEC ON
			END

			SET @v_indice_original = @v_indice_original + 1
		END

		SET @v_indice = @v_indice + 1
	END

	SET @p_resultado = 1

	SET NOEXEC OFF
END