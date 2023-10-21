CREATE PROCEDURE dbo.up_articulo_para_regla_pago_comision
(
	@p_nombre	VARCHAR(50)
)
AS
BEGIN

	SELECT
		TOP 50
		codigo_articulo
		,codigo_sku
		,nombre
		,genera_comision
	FROM
		dbo.articulo
	WHERE
		estado_registro = 1
		AND nombre like '%' + @p_nombre + '%'
END