CREATE OR ALTER PROC INS_META_REGLA_COMISION
(
@codigo_regla_comision INT,
@tope_unidad decimal(10, 2),
@tope_unidad_comisionable decimal(10, 2),
@tope_unidad_fin  decimal(10, 2),
@codigo_meta_regla_comision  INT OUT
)
AS





INSERT INTO meta_regla_comision(codigo_regla_comision,tope_unidad ,tope_unidad_comisionable,tope_unidad_fin,estado_registro)
VALUES(@codigo_regla_comision,@tope_unidad,@tope_unidad_comisionable,@tope_unidad_fin,1)

SET @codigo_meta_regla_comision=@@IDENTITY


