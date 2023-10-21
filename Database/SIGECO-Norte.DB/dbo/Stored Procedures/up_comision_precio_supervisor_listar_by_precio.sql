CREATE PROCEDURE [dbo].up_comision_precio_supervisor_listar_by_precio
(
	@p_codigo_precio int
)
AS
BEGIN

	SELECT
		cps.codigo_comision,
		cps.codigo_precio , 
		cps.codigo_canal_grupo,
		cps.codigo_tipo_pago,  
		cps.codigo_tipo_comision_supervisor ,
		cps.valor ,
		cps.vigencia_inicio ,
		cps.vigencia_fin ,
		cps.estado_registro,
		cps.fecha_registra,
		cps.usuario_registra,
		tv.abreviatura as nombre_tipo_venta
	FROM 
		dbo.comision_precio_supervisor cps 
	INNER JOIN dbo.precio_articulo pa 
		ON cps.codigo_precio=pa.codigo_precio
	INNER JOIN dbo.tipo_venta tv 
		ON pa.codigo_tipo_venta=tv.codigo_tipo_venta
	WHERE
		cps.codigo_precio=@p_codigo_precio 
		AND cps.estado_registro=1
	ORDER BY
		cps.codigo_canal_grupo asc
		,cps.codigo_tipo_pago asc
		,cps.codigo_tipo_comision_supervisor asc
		,cps.vigencia_inicio asc;
END;