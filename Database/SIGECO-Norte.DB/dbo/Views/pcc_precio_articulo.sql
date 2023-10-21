CREATE VIEW dbo.pcc_precio_articulo
AS
	SELECT DISTINCT
		codigo_precio, 
		codigo_articulo,
		codigo_empresa,
		codigo_tipo_venta,
		codigo_moneda,
		precio_total,
		vigencia_inicio,
		convert(datetime, convert(varchar(8), vigencia_fin, 112) + ' 23:59:59') AS vigencia_fin
	FROM 
		dbo.precio_articulo 
	WHERE
		estado_registro = 1