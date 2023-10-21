CREATE PROC dbo.sp_regla_pago_comision_excepcion_actualizar
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
	@fecha_modifica datetime,
	@usuario_modifica varchar(50),
	@codigo_regla int
)
AS
BEGIN
	UPDATE dbo.regla_pago_comision_excepcion
	SET 
	nombre = @nombre, codigo_campo_santo = @codigo_campo_santo, codigo_empresa = @codigo_empresa,
	codigo_canal_grupo = @codigo_canal_grupo, codigo_articulo = @codigo_articulo,
	valor_promocion = @valor_promocion,
	cuotas = @cuotas, vigencia_inicio = @vigencia_inicio,
	vigencia_fin = @vigencia_fin, estado_registro = @estado_registro, fecha_modifica = @fecha_modifica,
	usuario_modifica = @usuario_modifica
	WHERE codigo_regla = @codigo_regla
END