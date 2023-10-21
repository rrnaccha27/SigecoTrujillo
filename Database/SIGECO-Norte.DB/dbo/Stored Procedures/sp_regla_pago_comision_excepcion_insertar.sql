CREATE PROC dbo.sp_regla_pago_comision_excepcion_insertar
(
	@nombre varchar(50),
	@codigo_empresa int,
	@codigo_campo_santo int,
	@codigo_canal_grupo int,
	@codigo_articulo int,
	@valor_promocion int,
	@cuotas int,
	@vigencia_inicio datetime,
	@vigencia_fin datetime,
	@estado_registro bit,
	@fecha_registra datetime,
	@usuario_registra varchar(50),
	@p_codigo_regla int out
)
AS
BEGIN
	INSERT INTO dbo.regla_pago_comision_excepcion
	(nombre, codigo_campo_santo, codigo_empresa, codigo_canal_grupo, codigo_articulo, valor_promocion, cuotas,
	vigencia_inicio, vigencia_fin, estado_registro, fecha_registra, usuario_registra)
	VALUES
	(@nombre, @codigo_campo_santo, @codigo_empresa, @codigo_canal_grupo, @codigo_articulo, @valor_promocion, @cuotas,
	@vigencia_inicio, @vigencia_fin, @estado_registro, @fecha_registra, @usuario_registra)
	SET @p_codigo_regla = @@IDENTITY
	RETURN;
END