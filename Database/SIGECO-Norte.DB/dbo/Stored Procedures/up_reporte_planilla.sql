CREATE proc [dbo].[up_reporte_planilla]
(
@codigo_planilla int
)
as
begin

declare @tb_planilla table(
codigo_planilla int,
codigo_moneda int,
nombre_moneda varchar(50),
codigo_empresa int,
nombre_empresa varchar(50)
);


insert into @tb_planilla
select  
distinct
pl.codigo_planilla,
m.codigo_moneda,
m.nombre as nombre_moneda,
e.codigo_empresa,
e.nombre as nombre_empresa
from planilla pl
inner join detalle_planilla dpl on pl.codigo_planilla=dpl.codigo_planilla
inner join cronograma_pago_comision cp  on dpl.codigo_cronograma=cp.codigo_cronograma
inner join empresa_sigeco e on cp.codigo_empresa=e.codigo_empresa
inner join moneda m on m.codigo_moneda=cp.codigo_moneda
where pl.codigo_planilla=@codigo_planilla;

select 
pl.numero_planilla,
pl.fecha_inicio,
pl.fecha_fin,
tb_pl.codigo_moneda,
tb_pl.nombre_moneda,
tb_pl.codigo_empresa,
tb_pl.nombre_empresa
from planilla pl
--inner join canal_grupo cg on pl.codigo_canal=cg.codigo_canal_grupo
inner join @tb_planilla tb_pl on pl.codigo_planilla=tb_pl.codigo_planilla
where pl.codigo_planilla=@codigo_planilla;

end;