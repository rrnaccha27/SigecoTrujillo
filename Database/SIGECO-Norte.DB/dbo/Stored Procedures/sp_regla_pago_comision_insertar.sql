CREATE PROC dbo.sp_regla_pago_comision_insertar
(
	@nombre_regla_pago varchar(50),
	@codigo_empresa int,
	@codigo_campo_santo int,
	@codigo_canal_grupo int,
	@codigo_tipo_venta int,
	@codigo_tipo_pago int,
	@codigo_articulo int,
	@codigo_tipo_articulo int,
	@evaluar_plan_integral bit,
	@evaluar_anexado bit,
	@codigo_tipo_articulo_anexado int,
	@tipo_pago int,
	@valor_inicial_pago varchar(18),
	@valor_cuota_pago varchar(18),
	@fecha_inicio datetime,
	@fecha_fin datetime,
	@estado_registro bit,
	@fecha_registra datetime,
	@usuario_registra varchar(50),
	@p_codigo_regla_pago int out
)
AS
BEGIN
INSERT INTO regla_pago_comision
(nombre_regla_pago, codigo_empresa, codigo_campo_santo, codigo_canal_grupo, codigo_tipo_pago, codigo_tipo_venta, codigo_articulo,
 codigo_tipo_articulo, evaluar_plan_integral, evaluar_anexado, codigo_tipo_articulo_anexado,
 tipo_pago, valor_inicial_pago, valor_cuota_pago,
 fecha_inicio, fecha_fin, estado_registro, fecha_registra, usuario_registra)
VALUES
(@nombre_regla_pago, @codigo_empresa, @codigo_campo_santo, @codigo_canal_grupo, @codigo_tipo_pago, @codigo_tipo_venta, @codigo_articulo,
 @codigo_tipo_articulo, @evaluar_plan_integral, @evaluar_anexado, @codigo_tipo_articulo_anexado,
 @tipo_pago, @valor_inicial_pago, @valor_cuota_pago,
 @fecha_inicio, @fecha_fin, @estado_registro, @fecha_registra, @usuario_registra)
SET @p_codigo_regla_pago = @@IDENTITY

DECLARE 
	@v_statement varchar(1000)
	,@v_orden int
	,@v_maximo int

SET @v_statement = 'update regla_pago_comision set orden = '
SET @v_maximo = (SELECT MAX(orden) FROM dbo.regla_pago_comision_orden)
SET @v_orden = 1

WHILE (@v_orden <= @v_maximo)
BEGIN
	SELECT @v_statement = @v_statement + 'case when ' + nombre_campo + ' is not null then ' + case orden when 1 then '100000' when 2 then '10000' when 3 then '1000' when 4 then '100' when 5 then '10' when 6 then '1' end + ' else 0 end + '
	FROM
		dbo.regla_pago_comision_orden
	WHERE
		orden = @v_orden

	SET @v_orden = @v_orden + 1
END

SET @v_statement = @v_statement + '0 where codigo_regla_pago = ' + convert(varchar, @p_codigo_regla_pago)

EXEC (@v_statement)

RETURN;
END