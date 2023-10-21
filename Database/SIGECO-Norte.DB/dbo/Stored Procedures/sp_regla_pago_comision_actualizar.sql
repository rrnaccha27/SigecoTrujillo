CREATE PROC dbo.sp_regla_pago_comision_actualizar
(
@nombre_regla_pago varchar(50),
@codigo_empresa int,
@codigo_campo_santo int,
@codigo_canal_grupo int,
@codigo_tipo_venta int,
@codigo_tipo_pago int,
@codigo_articulo int,
@tipo_pago int,
@valor_inicial_pago varchar(18),
@valor_cuota_pago varchar(18),
@fecha_inicio datetime,
@fecha_fin datetime,
@estado_registro bit,
@fecha_modifica datetime,
@usuario_modifica varchar(50),
@codigo_regla_pago int
)
AS
BEGIN
UPDATE regla_pago_comision
SET 
nombre_regla_pago = @nombre_regla_pago, codigo_empresa = @codigo_empresa, codigo_campo_santo = @codigo_campo_santo,
codigo_canal_grupo = @codigo_canal_grupo, codigo_tipo_pago = @codigo_tipo_pago,
codigo_tipo_venta = @codigo_tipo_venta, codigo_articulo = @codigo_articulo,
tipo_pago = @tipo_pago,
valor_inicial_pago = @valor_inicial_pago, valor_cuota_pago = @valor_cuota_pago,
fecha_inicio = @fecha_inicio,
fecha_fin = @fecha_fin, estado_registro = @estado_registro, fecha_modifica = @fecha_modifica,
usuario_modifica = @usuario_modifica
WHERE codigo_regla_pago = @codigo_regla_pago

END