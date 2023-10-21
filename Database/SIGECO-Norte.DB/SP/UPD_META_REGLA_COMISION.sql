CREATE OR ALTER PROC UPD_META_REGLA_COMISION
(
@codigo_meta_regla_comision INT,
@tope_unidad decimal(10, 2),
@tope_unidad_comisionable decimal(10, 2),
@tope_unidad_fin  decimal(10, 2)
)
AS


UPDATE meta_regla_comision
SET tope_unidad=@tope_unidad,

tope_unidad_comisionable=@tope_unidad_comisionable,
tope_unidad_fin=@tope_unidad_fin
WHERE codigo_meta_regla_comision=@codigo_meta_regla_comision;




