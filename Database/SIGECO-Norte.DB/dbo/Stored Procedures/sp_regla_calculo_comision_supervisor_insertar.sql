CREATE PROC dbo.sp_regla_calculo_comision_supervisor_insertar
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
@fecha_registra datetime,
@usuario_registra varchar(50),
@p_codigo_regla int out
)
AS
BEGIN
INSERT INTO regla_calculo_comision_supervisor
(nombre, codigo_campo_santo, codigo_empresa, codigo_canal_grupo, tipo_supervisor, valor_pago, incluye_igv,
vigencia_inicio, vigencia_fin, estado_registro, fecha_registra, usuario_registra)
VALUES
(@nombre, @codigo_campo_santo, @codigo_empresa, @codigo_canal_grupo, @tipo_supervisor, @valor_pago, @incluye_igv,
@vigencia_inicio, @vigencia_fin, @estado_registro, @fecha_registra, @usuario_registra)
SET @p_codigo_regla = @@IDENTITY
RETURN;
END