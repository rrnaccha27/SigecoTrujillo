create  proc [dbo].[up_reporte_planilla_resumen_empresa]
(
@codigo_planilla int
)
as
begin

declare 
	@v_limite_detraccion decimal(10,2),
	@v_porcentaje_detraccion decimal(10,2);
select @v_limite_detraccion=valor from parametro_sistema where codigo_parametro_sistema=16;
select @v_porcentaje_detraccion=valor from parametro_sistema where codigo_parametro_sistema=15;
set @v_porcentaje_detraccion=(@v_porcentaje_detraccion/100);

declare @tb_descuento table(
codigo_empresa int,
monto_descuento decimal(10,2)
);

insert into @tb_descuento
select 
  codigo_empresa , 
  sum(monto)
 from descuento where codigo_planilla=@codigo_planilla and estado_registro=1
 group by codigo_empresa;

declare @tb_resumen table(
  codigo_empresa int,
  monto_bruto decimal(10,2),
  monto_neto decimal(10,2),
  igv decimal(10,2)
);

/*************************************************************************/
insert into @tb_resumen
select 
	 dp.codigo_empresa,
	 sum(dp.monto_bruto)monto_bruto,
	 sum(dp.monto_neto)monto_neto,
	 sum(isnull(dp.igv,0))igv
from detalle_planilla dp inner join detalle_cronograma dc
on (dp.codigo_detalle_cronograma=dc.codigo_detalle and dc.estado_registro=1 )
where dp.excluido=0 and dp.estado_registro=1 and dp.codigo_planilla=@codigo_planilla and  dc.codigo_estado_cuota in(2,3)
group by dp.codigo_empresa;

/*************************************************************************/
select 
     res.codigo_empresa,
	 res.nombre,
	 res.monto_bruto,
	 res.igv, -- (-)
	 ----------------
	 res.monto_neto,
	 res.monto_descuento,-- (-),
	 ------------------
	 res.sub_total,
	 res.detraccion,
	 res.sub_total-res.detraccion as total_empresa
   
from (
select  
     r.codigo_empresa,
	 e.nombre,
	 r.monto_bruto,
	 r.igv, -- (-)
	 ----------------
	 r.monto_neto,
	 isnull(d.monto_descuento,0)monto_descuento, --(-)
	 -------------------------------
	 r.monto_neto-isnull(d.monto_descuento,0) as sub_total,	 

     (case when @v_limite_detraccion>r.monto_neto-isnull(d.monto_descuento,0) then 0 else 
	  (r.monto_neto-isnull(d.monto_descuento,0))*@v_porcentaje_detraccion
	  end) detraccion	 
from @tb_resumen r inner join empresa_sigeco e on r.codigo_empresa=e.codigo_empresa
 left join @tb_descuento d on r.codigo_empresa=d.codigo_empresa
 ) res
 
end;