CREATE PROC dbo.sp_regla_calculo_comision_supervisor_actualizar
(
@nombre varchar(50),
@codigo_empresa int,
@codigo_campo_santo int,
@codigo_canal_grupo int,
@tipo_supervisor int,
@valor_pago DECIMAL(10, 2),
@incluye_igv bit,
@vigencia_inicio datetime,
@vigencia_fin datetime,
@estado_registro bit,
@fecha_modifica datetime,
@usuario_modifica varchar(50),
@codigo_regla int
)
AS
BEGIN
UPDATE regla_calculo_comision_supervisor
SET 
nombre = @nombre, codigo_campo_santo = @codigo_campo_santo, codigo_empresa = @codigo_empresa,
codigo_canal_grupo = @codigo_canal_grupo, tipo_supervisor = @tipo_supervisor,
valor_pago = @valor_pago, incluye_igv = @incluye_igv, vigencia_inicio = @vigencia_inicio,
vigencia_fin = @vigencia_fin, estado_registro = @estado_registro, fecha_modifica = @fecha_modifica,
usuario_modifica = @usuario_modifica
WHERE codigo_regla = @codigo_regla

END