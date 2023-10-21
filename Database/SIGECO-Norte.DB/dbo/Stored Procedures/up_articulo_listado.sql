CREATE PROCEDURE [dbo].[up_articulo_listado]
(
	@p_nombre	VARCHAR(250)
)
AS
BEGIN
	SELECT 
	    DISTINCT 
		a.codigo_articulo,
		max(a.codigo_sku) as codigo_sku, 
		a.nombre, 
		a.abreviatura, 
		case when a.genera_comision = 1 then 'Si' else 'No' end as genera_comision, 
		case when a.genera_bono = 1 then 'Si' else 'No' end as genera_bono,  		
		case when a.genera_bolsa_bono = 1 then 'Si' else 'No' end as bolsa_bono,
		case when convert(bit,max(convert(int,a.estado_registro))) =1 then 'Activo' else 'Inactivo' end as estado_registro,
		(CASE WHEN(COUNT(p.codigo_precio) > 0 ) THEN 'Si' ELSE 'No' END) AS tiene_precio, 
		(CASE WHEN(COUNT(r.codigo_regla) > 0 ) THEN 'Si' ELSE 'No' END) AS tiene_comision
	FROM articulo a
	LEFT JOIN precio_articulo p ON a.estado_registro = 1 AND p.estado_registro = 1 AND p.codigo_articulo = a.codigo_articulo 
	LEFT JOIN regla_calculo_comision r ON r.estado_registro = 1 AND r.codigo_precio = p.codigo_precio 
	WHERE 
		--a.estado_registro = 1 AND 
		( LEN(@p_nombre) = 0 OR (LEN(@p_nombre) > 0 AND a.nombre LIKE + '%' + @p_nombre + '%') )
	GROUP BY a.codigo_articulo, a.nombre, a.abreviatura, a.genera_comision, a.genera_bono,a.genera_bolsa_bono
	ORDER BY a.nombre ASC
END;