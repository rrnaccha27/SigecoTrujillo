create proc up_gestion_exclusion_listar_detalle_pago_comision
(
 @arrya_exclusion_id  dbo.arrya_exclusion_id_type readonly
)
as
begin

declare @tab_pago table(
 codigo_exclusion int,
 codigo_planilla int,
 codigo_detalle_planilla int,
 codigo_cronograma int,
 codigo_detalle_cronogrma int, 
 nro_cuota int,
 nro_contrato varchar(20),
 monto_bruto decimal(10,2),
 igv decimal(10,2),
 monto_neto decimal(10,2),
 codigo_tipo_pago int,
 codigo_tipo_venta int,
 codigo_tipo_planilla int,
 codigo_canal int,
 apellidos_nombres varchar(150),
 nombre_empresa varchar(100)
)
declare @tab_planilla table(
 codigo_planilla int,
 numero_planilla varchar(150),
 codigo_tipo_planilla int,
 fecha_inicio date,
 fecha_fin date,
 codigo_regla_tipo_planilla int,
 nombre_regla_tipo_planilla varchar(100)
);


insert into @tab_pago
select   
 ex.codigo_exclusion,
 (select max(pla.codigo_planilla) from planilla pla where pla.codigo_tipo_planilla=cpc.codigo_tipo_planilla and pla.codigo_estado_planilla = 1 and ex.codigo_regla_tipo_planilla = pla.codigo_regla_tipo_planilla),
  ex.codigo_detalle_planilla,
 dc.codigo_cronograma,
 dc.codigo_detalle,
 dc.nro_cuota,
 cpc.nro_contrato,
 dc.monto_bruto,
 dc.igv,
 dc.monto_neto,
 cpc.codigo_tipo_pago,
 cpc.codigo_tipo_venta,
 cpc.codigo_tipo_planilla,
 pcg.codigo_canal,
 p.nombre+' '+ISNULL(p.apellido_paterno, '')+' '+ISNULL(p.apellido_materno, '') as apellidos_nombres,
 e.nombre as nombre_empresa
from exclusion_cuota_planilla ex inner join @arrya_exclusion_id d on (d.codigo_exclusion=ex.codigo_exclusion and ex.estado_exclusion=1)
inner join detalle_cronograma dc on ex.codigo_detalle_cronograma=dc.codigo_detalle
inner join cronograma_pago_comision cpc on cpc.codigo_cronograma=dc.codigo_cronograma
inner join empresa_sigeco e on e.codigo_empresa = cpc.codigo_empresa
inner join personal_canal_grupo pcg on pcg.codigo_registro=cpc.codigo_personal_canal_grupo
inner join personal p on p.codigo_personal=pcg.codigo_personal
where dc.codigo_estado_cuota=4;


insert into @tab_planilla
select distinct 
p.codigo_planilla,
p.numero_planilla,
p.codigo_tipo_planilla,
p.fecha_inicio,
p.fecha_fin,
p.codigo_regla_tipo_planilla,
rtp.nombre as nombre_regla_tipo_planilla
from planilla p 
inner join regla_tipo_planilla rtp on rtp.codigo_regla_tipo_planilla = p.codigo_regla_tipo_planilla
where p.codigo_estado_planilla=1
and p.codigo_regla_tipo_planilla in (select ex.codigo_regla_tipo_planilla from exclusion_cuota_planilla ex where ex.estado_exclusion = 1)


select 
 pag.codigo_exclusion,
 pag.codigo_detalle_planilla,
 pag.codigo_cronograma,
 pag.codigo_detalle_cronogrma,
 pag.nro_cuota,
 pag.nro_contrato,
 pag.monto_bruto ,
 pag.codigo_canal,
 pag.igv,
 pag.monto_neto,
 pag.codigo_tipo_pago,
 pag.codigo_tipo_venta,
 pag.codigo_tipo_planilla,
 pag.codigo_canal,
 pag.apellidos_nombres,
 pag.nombre_empresa,
 ------------
isnull(pla.codigo_planilla,0) codigo_planilla,
pla.numero_planilla,
pla.fecha_inicio,
pla.fecha_fin,
pla.codigo_regla_tipo_planilla,
pla.nombre_regla_tipo_planilla
from @tab_pago pag left join @tab_planilla pla
on pag.codigo_planilla=pla.codigo_planilla

end;