create proc up_obtener_pago_comision_by_articulo_personal
(
   @p_codigo_articulo int,
   @p_codigo_personal int,
   @p_codigo_empresa int,
   @p_nro_contrato varchar(20)
)
as
begin

declare   
   -----------------------------
   @v_monto_comision_articulo decimal(10,2),
   @v_monto_bruto decimal(10,2),
   @v_monto_igv decimal(10,2),
   @v_monto_neto decimal(10,2),

   @v_monto_neto_pendiente decimal(10,2),
   @v_monto_neto_en_proceso_pago decimal(10,2),
   @v_monto_neto_pagado decimal(10,2),
   @v_monto_neto_excluido decimal(10,2),
   @v_monto_neto_anulado decimal(10,2),
    ----------------------------
	@total_registro_encontrado int,
	@v_codigo_cronogrma int;
	------------------------

select 
  @total_registro_encontrado=count(1),
  @v_codigo_cronogrma=min(cpc.codigo_cronograma)
from cronograma_pago_comision cpc inner join
personal_canal_grupo pcg on cpc.codigo_personal_canal_grupo=pcg.codigo_registro
inner join articulo_cronograma a on a.codigo_cronograma=cpc.codigo_cronograma
where cpc.codigo_empresa=@p_codigo_empresa and 
cpc.nro_contrato=@p_nro_contrato and 
a.codigo_articulo=@p_codigo_articulo and 
pcg.codigo_personal=@p_codigo_personal and 
pcg.estado_registro=1;


if @total_registro_encontrado=1
begin
	select 
	  @v_codigo_cronogrma=a.codigo_cronograma
	from cronograma_pago_comision cpc inner join
	personal_canal_grupo pcg on cpc.codigo_personal_canal_grupo=pcg.codigo_registro
	inner join articulo_cronograma a on a.codigo_cronograma=cpc.codigo_cronograma
	where cpc.codigo_empresa=@p_codigo_empresa and
	 cpc.nro_contrato=@p_nro_contrato and 
	 a.codigo_articulo=@p_codigo_articulo and
	 pcg.codigo_personal=@p_codigo_personal;   
end;
/*****************************************************
OBTENIENDO MONTO DE LA COMISION
******************************************************/
select 
  @v_monto_comision_articulo=monto_comision
from articulo_cronograma 
where codigo_articulo=@p_codigo_articulo and
      codigo_cronograma=@v_codigo_cronogrma;


select      	 
     @v_monto_bruto= sum(dc.monto_bruto),
	 @v_monto_igv=sum(dc.igv),
	 @v_monto_neto=sum(dc.monto_neto),
	 @v_monto_neto_pendiente=sum(case when dc.codigo_estado_cuota=1 then dc.monto_neto else 0 end),
	 @v_monto_neto_en_proceso_pago=sum(case when dc.codigo_estado_cuota=2 then dc.monto_neto else 0 end),
	 @v_monto_neto_pagado=sum(case when dc.codigo_estado_cuota=3 then dc.monto_neto else 0 end),
	 @v_monto_neto_excluido=sum(case when dc.codigo_estado_cuota=4 then dc.monto_neto else 0 end),
	 @v_monto_neto_anulado=sum(case when dc.codigo_estado_cuota=5 then dc.monto_neto else 0 end)
from detalle_cronograma dc 
where dc.codigo_cronograma=@v_codigo_cronogrma and dc.codigo_articulo=@p_codigo_articulo;


select 
    isnull(@v_codigo_cronogrma,0) as codigo_cronograma,
    isnull(@total_registro_encontrado,0) as total_registro_encontrado,
	isnull(@v_monto_comision_articulo,0) as monto_comision_articulo,
	isnull(@v_monto_bruto,0) as monto_bruto,
	isnull(@v_monto_igv,0) as monto_igv,
	isnull(@v_monto_neto,0) as monto_neto,
	isnull(@v_monto_neto_pendiente,0) as monto_neto_pendiente,
	isnull(@v_monto_neto_en_proceso_pago,0) as monto_neto_en_proceso_pago,
	isnull(@v_monto_neto_pagado,0) as monto_neto_pagado,
	isnull(@v_monto_neto_excluido,0) as monto_neto_excluido,
	isnull(@v_monto_neto_anulado,0) as  monto_neto_anulado;

end;