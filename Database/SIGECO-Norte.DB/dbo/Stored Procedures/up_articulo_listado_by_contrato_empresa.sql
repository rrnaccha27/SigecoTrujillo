create proc [dbo].[up_articulo_listado_by_contrato_empresa]
(
	 @codigo_empresa		VARCHAR(20)
	,@nro_contrato		VARCHAR(20)
	,@nombre			VARCHAR(250)
)
as
begin

	select 
		e.codigo_empresa,
		e.codigo_equivalencia as codigo_equivalencia_empresa,
		e.nombre as nombre_empresa,
		dc.NumAtCard as numero_contrato,
		cs.codigo_campo_santo,
		cs.nombre as nombre_campo_santo,
		art.codigo_articulo,
		art.abreviatura,
		art.codigo_sku,
		art.nombre as nombre_articulo
	from detalle_contrato dc
		inner join empresa_sigeco e on e.codigo_equivalencia=dc.Codigo_empresa
		inner join campo_santo_sigeco cs on dc.Codigo_Camposanto=cs.codigo_equivalencia
		inner join articulo art on art.codigo_sku=dc.ItemCode
	where upper(dc.NumAtCard)=upper(@nro_contrato) and e.codigo_empresa=@codigo_empresa and 
			art.nombre like '%'+isnull(@nombre,art.nombre)+'%';
  
end;

SET ANSI_NULLS ON