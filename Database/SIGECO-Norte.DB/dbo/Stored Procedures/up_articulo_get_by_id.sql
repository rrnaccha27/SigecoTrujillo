CREATE PROCEDURE [dbo].[up_articulo_get_by_id]
(
	@codigo_articulo int
)
AS
BEGIN
	select top 1
		codigo_articulo,
		codigo_unidad_negocio,
		codigo_categoria,
		codigo_tipo_articulo,
		codigo_sku,
		nombre,
		abreviatura,
		genera_bono,
		genera_comision,
		genera_bolsa_bono,
		tiene_contrato_vinculante,
		anio_contrato_vinculante,
		estado_registro,
		fecha_registra,
		usuario_registra,
		cantidad_unica
	from 
		articulo 
	where 
		codigo_articulo=@codigo_articulo;
END;