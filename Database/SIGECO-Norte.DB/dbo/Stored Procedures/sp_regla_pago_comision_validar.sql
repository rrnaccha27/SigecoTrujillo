create PROC dbo.sp_regla_pago_comision_validar
	@codigo_regla_pago int,
	@codigo_campo_santo int,
	@codigo_empresa int,
	@codigo_canal_grupo int,
	@codigo_tipo_pago int,
	@codigo_tipo_venta int,
	@codigo_articulo int,
	@evaluar_plan_integral int,
	@evaluar_anexado int,

	@fecha_inicio datetime,
	@fecha_fin datetime,
	@cantidad int output
AS
BEGIN
	
	SELECT 
		@cantidad = count(r.codigo_regla_pago) 
	FROM 
		dbo.regla_pago_comision r
	WHERE 
		r.estado_registro = 1 
		AND ((@codigo_campo_santo IS NULL AND r.codigo_campo_santo IS NULL) or (@codigo_campo_santo IS NOT NULL AND r.codigo_campo_santo = @codigo_campo_santo))
		AND ((@codigo_empresa IS NULL AND r.codigo_empresa IS NULL) or (@codigo_empresa IS NOT NULL AND r.codigo_empresa = @codigo_empresa))
		AND ((@codigo_canal_grupo IS NULL AND r.codigo_canal_grupo IS NULL) or (@codigo_canal_grupo IS NOT NULL AND r.codigo_canal_grupo = @codigo_canal_grupo))
		AND ((@codigo_tipo_pago IS NULL AND r.codigo_tipo_pago IS NULL) or (@codigo_tipo_pago IS NOT NULL AND r.codigo_tipo_pago = @codigo_tipo_pago))
		AND ((@codigo_tipo_venta IS NULL AND r.codigo_tipo_venta IS NULL) or (@codigo_tipo_venta IS NOT NULL AND r.codigo_tipo_venta = @codigo_tipo_venta))
		AND ((@codigo_articulo IS NULL AND r.codigo_articulo IS NULL) or (@codigo_articulo IS NOT NULL AND r.codigo_articulo = @codigo_articulo))

		AND ((@evaluar_plan_integral IS NULL AND r.evaluar_plan_integral IS NULL) or (@evaluar_plan_integral IS NOT NULL AND r.evaluar_plan_integral = @evaluar_plan_integral))

		AND ((@evaluar_anexado IS NULL AND r.evaluar_anexado IS NULL) or (@evaluar_anexado IS NOT NULL AND r.evaluar_anexado = @evaluar_anexado))


		AND (@codigo_regla_pago = 0 OR (r.codigo_regla_pago <> @codigo_regla_pago))
		AND (
			@fecha_inicio between r.fecha_inicio AND r.fecha_fin
			OR @fecha_fin between r.fecha_inicio AND r.fecha_fin
			OR r.fecha_inicio between @fecha_inicio AND @fecha_fin
			OR r.fecha_fin between @fecha_inicio AND @fecha_fin)

	RETURN;
END