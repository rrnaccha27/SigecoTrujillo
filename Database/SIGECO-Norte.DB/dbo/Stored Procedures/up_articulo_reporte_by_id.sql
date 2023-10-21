CREATE PROCEDURE [dbo].[up_articulo_reporte_by_id]
(
	@codigo_articulo int
)
AS
BEGIN
	declare @v_estado_registro bit;

	select @v_estado_registro=estado_registro from articulo where codigo_articulo=@codigo_articulo;

	select 
		a.codigo_articulo,
		a.nombre as nombre_articulo,
		a.abreviatura,
		a.codigo_sku,
		un.nombre as nombre_unidad_negocio,
		c.nombre as nombre_categoria,
		a.genera_bolsa_bono,
		a.genera_bono,
		a.genera_comision,
		-------------------------------
		pa.codigo_precio,
		e.nombre as nombre_empresa,
		tv.abreviatura as nombre_tipo_venta,
		m.nombre as nombre_moneda,
		pa.precio,
		pa.igv,
		pa.precio_total,
		pa.vigencia_inicio as vigencia_inicio_precio_articulo,
		pa.vigencia_fin as vigencia_fin_precio_articulo,
		-----------------------------------
		cg.nombre as nombre_canal,
		tp.nombre as nombre_tipo_pago,
		rcc.valor,
		rcc.vigencia_inicio,
		rcc.vigencia_fin,
		tc.codigo_tipo_comision,
		tc.nombre as nombre_tipo_comision
	from articulo a 
	inner join categoria c on a.codigo_categoria=c.codigo_categoria
	inner join unidad_negocio un on un.codigo_unidad_negocio=a.codigo_unidad_negocio
	-------------------------------------------------------------------------------
	left join precio_articulo pa on a.codigo_articulo=pa.codigo_articulo
	left join empresa_sigeco e on e.codigo_empresa=pa.codigo_empresa
	left join tipo_venta tv on tv.codigo_tipo_venta=pa.codigo_tipo_venta
	left join moneda m on m.codigo_moneda=pa.codigo_moneda
	--------------------------------------------------------------------------------
	left join regla_calculo_comision rcc on rcc.codigo_precio=pa.codigo_precio
	left join canal_grupo cg on cg.codigo_canal_grupo=rcc.codigo_canal
	left join tipo_pago tp on tp.codigo_tipo_pago=rcc.codigo_tipo_pago
	left join tipo_comision tc on tc.codigo_tipo_comision=rcc.codigo_tipo_comision
	where a.codigo_articulo=@codigo_articulo --and 
	--isnull(pa.estado_registro,case when @v_estado_registro=1 then 1 else 0 end)=@v_estado_registro and isnull(rcc.estado_registro,case when @v_estado_registro=1 then 1 else 0 end)=@v_estado_registro
END;