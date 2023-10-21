CREATE PROCEDURE [dbo].[up_regla_calculo_comision_listar_by_precio]
(
	@codigo_precio int
)
AS
BEGIN
	SET NOCOUNT ON
	SELECT
		rcc.codigo_regla,
		rcc.codigo_precio , 
		rcc.codigo_canal  ,
		rcc.codigo_tipo_pago,  
		rcc.codigo_tipo_comision ,
		rcc.valor ,
		rcc.vigencia_inicio ,
		rcc.vigencia_fin ,
	
		rcc.estado_registro,
		rcc.fecha_registra,
		rcc.usuario_registra,
	
		tv.abreviatura as nombre_tipo_venta
	FROM 
		regla_calculo_comision rcc 
	INNER JOIN precio_articulo pa on rcc.codigo_precio=pa.codigo_precio
	INNER JOIN tipo_venta tv on pa.codigo_tipo_venta=tv.codigo_tipo_venta
	WHERE 
		rcc.codigo_precio=@codigo_precio and rcc.estado_registro=1
	ORDER BY 
		rcc.codigo_canal asc, rcc.codigo_tipo_pago asc, rcc.vigencia_inicio;
	SET NOCOUNT Off
END;