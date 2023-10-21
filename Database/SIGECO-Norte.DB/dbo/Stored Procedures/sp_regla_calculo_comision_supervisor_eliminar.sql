CREATE PROC dbo.sp_regla_calculo_comision_supervisor_eliminar
(
@estado_registro bit,
@fecha_modifica datetime,
@usuario_modifica varchar(50),
@codigo_regla int
)
AS
BEGIN
UPDATE regla_calculo_comision_supervisor
SET 
estado_registro = @estado_registro, fecha_modifica = @fecha_modifica,
usuario_modifica = @usuario_modifica
WHERE codigo_regla = @codigo_regla

END