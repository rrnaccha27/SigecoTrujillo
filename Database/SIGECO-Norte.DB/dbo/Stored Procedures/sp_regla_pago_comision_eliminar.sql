CREATE PROC dbo.sp_regla_pago_comision_eliminar
(
@estado_registro bit,
@fecha_modifica datetime,
@usuario_modifica varchar(50),
@codigo_regla_pago int
)
AS
BEGIN
UPDATE regla_pago_comision
SET 
estado_registro = @estado_registro, fecha_modifica = @fecha_modifica,
usuario_modifica = @usuario_modifica
WHERE codigo_regla_pago = @codigo_regla_pago

END