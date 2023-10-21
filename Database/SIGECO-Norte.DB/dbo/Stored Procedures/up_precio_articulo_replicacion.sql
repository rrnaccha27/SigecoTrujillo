CREATE PROCEDURE dbo.up_precio_articulo_replicacion
(
	@p_codigo_precio	INT
)
AS
BEGIN
	
	SELECT
		a.codigo_sku
		,a.nombre
		,a.abreviatura
		,c.codigo_equivalencia AS codigo_categoria
		,un.codigo_equivalencia AS codigo_unidad_negocio
		,CASE WHEN a.genera_comision = 1 THEN 'Y' ELSE 'N' END AS genera_comision
		,CASE WHEN a.genera_bono = 1 THEN 'Y' ELSE 'N' END AS genera_bono
		,CASE WHEN a.genera_bolsa_bono = 1 THEN 'Y' ELSE 'N' END AS suma_bolsa_bono
		,es.codigo_equivalencia AS codigo_empresa
		,tv.codigo_equivalencia AS codigo_tipo_venta
		,m.codigo_equivalencia AS codigo_moneda
		,pa.precio
		,pa.igv
		,pa.precio_total
		,pa.vigencia_inicio
		,pa.vigencia_fin
	FROM
		dbo.precio_articulo pa
	INNER JOIN dbo.articulo a
		ON pa.codigo_articulo = a.codigo_articulo
	INNER JOIN dbo.unidad_negocio un
		ON un.codigo_unidad_negocio = a.codigo_unidad_negocio
	INNER JOIN dbo.categoria c
		ON c.codigo_categoria = a.codigo_categoria
	INNER JOIN dbo.empresa_sigeco es
		ON es.codigo_empresa = pa.codigo_empresa
	INNER JOIN dbo.tipo_venta tv
		ON tv.codigo_tipo_venta = pa.codigo_tipo_venta
	INNER JOIN dbo.moneda m
		ON m.codigo_moneda = pa.codigo_moneda
	WHERE
		pa.codigo_precio = @p_codigo_precio

END