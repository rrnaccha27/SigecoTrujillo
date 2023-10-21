CREATE PROC dbo.sp_regla_pago_comision_excepcion_validar
	@codigo_regla int,
	@codigo_campo_santo int,
	@codigo_empresa int,
	@codigo_canal_grupo int,
	@codigo_articulo int,
	@valor_promocion int,
	@vigencia_inicio datetime,
	@vigencia_fin datetime,
	@cantidad int output
AS
BEGIN
	
	SELECT @cantidad = count(r.codigo_regla) 
	FROM dbo.regla_pago_comision_excepcion r
	WHERE 
	r.estado_registro = 1 AND
	r.codigo_campo_santo = @codigo_campo_santo AND
	r.codigo_empresa = @codigo_empresa AND
	r.codigo_canal_grupo = @codigo_canal_grupo AND 
	r.codigo_articulo = @codigo_articulo AND
	--r.valor_promocion = @valor_promocion AND
	(@codigo_regla = 0 OR (r.codigo_regla <> @codigo_regla))
	AND (
		@vigencia_inicio between r.vigencia_inicio AND r.vigencia_fin
		OR @vigencia_fin between r.vigencia_inicio AND r.vigencia_fin
		OR r.vigencia_inicio between @vigencia_inicio AND @vigencia_fin
		OR r.vigencia_fin between @vigencia_inicio AND @vigencia_fin)


	RETURN;
END