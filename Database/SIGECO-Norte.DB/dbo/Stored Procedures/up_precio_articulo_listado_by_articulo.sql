CREATE PROCEDURE [dbo].[up_precio_articulo_listado_by_articulo]
(
	@p_codigo_articulo	INT
)
AS
BEGIN  
	SELECT 
		 pa.codigo_precio
		,pa.codigo_articulo
		,pa.codigo_empresa
		,e.nombre as nombre_empresa
		,pa.codigo_tipo_venta
		,tv.nombre as nombre_tipo_venta
		,pa.codigo_moneda
		,m.nombre as nombre_moneda
		,isnull(pa.cuota_inicial,0) as cuota_inicial
		,pa.precio
		,pa.precio_total
		,pa.igv
		,pa.vigencia_inicio
		,pa.vigencia_fin		
		,(SELECT COUNT(r.codigo_regla) FROM dbo.regla_calculo_comision r WHERE pa.estado_registro = 1 AND r.estado_registro = 1 AND r.codigo_precio = pa.codigo_precio)
		  AS comisiones
	FROM 
		dbo.precio_articulo pa
	INNER JOIN dbo.empresa_sigeco e 
		ON pa.estado_registro = 1 AND e.estado_registro = 1 AND pa.codigo_empresa = e.codigo_empresa
	INNER JOIN dbo.tipo_venta tv 
		ON pa.codigo_tipo_venta = tv.codigo_tipo_venta
	INNER JOIN dbo.moneda m 
		ON pa.codigo_moneda = m.codigo_moneda
	WHERE
		pa.estado_registro =1
		AND pa.codigo_articulo = @p_codigo_articulo
	ORDER BY
		pa.codigo_empresa ASC, pa.codigo_tipo_venta ASC, pa.vigencia_inicio ASC
END