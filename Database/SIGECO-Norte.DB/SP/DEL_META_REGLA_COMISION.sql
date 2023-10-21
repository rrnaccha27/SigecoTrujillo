CREATE OR ALTER PROC DEL_META_REGLA_COMISION
(
@estado_registro  bit,
@usuario_modifica NVARCHAR(50),
@codigo_meta_regla_comision INT

)
AS


UPDATE meta_regla_comision
SET estado_registro=@estado_registro
WHERE codigo_meta_regla_comision=@codigo_meta_regla_comision;




