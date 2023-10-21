create proc [dbo].[up_contrato_analisis_detalle_cuota_bkp]
(
@codigo_articulo int,
@codigo_empresa int,
@codigo_moneda int,
@nro_contrato nvarchar(200)

)
as
begin

DECLARE @v_nro_contrato VARCHAR(100)

SELECT 
	@v_nro_contrato = NumAtCard
FROM dbo.cabecera_contrato cc
INNER JOIN dbo.empresa_sigeco e 
	ON e.codigo_empresa = @codigo_empresa
WHERE 
	(ISNULL(REPLICATE('0', 10 - LEN(cc.NumAtCard)), '') + cc.NumAtCard) = (ISNULL(REPLICATE('0', 10 - LEN(@nro_contrato)), '') + @nro_contrato)
	AND cc.Codigo_empresa = e.codigo_equivalencia

select
dc.es_registro_manual_comision,
dc.codigo_detalle as codigo_detalle_cronograma,
dc.codigo_estado_cuota,
(case when dc.codigo_estado_cuota=3 then (
select 
   max(fecha_movimiento) 
from operacion_cuota_comision oc where  oc.codigo_detalle_cronograma=dc.codigo_detalle and oc.codigo_tipo_operacion_cuota=3
)  else null end) as fecha_cierre,

(case when dc.codigo_estado_cuota=5 then (
select 
   max(fecha_movimiento) 
from operacion_cuota_comision oc where  oc.codigo_detalle_cronograma=dc.codigo_detalle and oc.codigo_tipo_operacion_cuota=5
)  else null end) as fecha_anulado,

(case when dc.codigo_estado_cuota=4 then (
select 
   max(fecha_movimiento) 
from operacion_cuota_comision oc where  oc.codigo_detalle_cronograma=dc.codigo_detalle and oc.codigo_tipo_operacion_cuota=4
)  else null end) as fecha_exclusion,

(case when dc.codigo_estado_cuota=5 then (
select top 1
   oc.motivo_movimiento 
from operacion_cuota_comision oc where  oc.codigo_detalle_cronograma=dc.codigo_detalle and oc.codigo_tipo_operacion_cuota=5
order by oc.codigo_operacion_cuota_comision desc
)  else ' ' end) as observacion,
dc.nro_cuota,
dc.igv,
dc.monto_bruto,
dc.monto_neto,
dc.fecha_programada,
tp.nombre as nombre_tipo_planilla,
(case when dc.es_registro_manual_comision=1 then 'Pagada' else ec.nombre end) as nombre_estado_cuota

,(
	CASE WHEN dc.codigo_estado_cuota=3 THEN
	(
		SELECT TOP 1
			p.numero_planilla
		FROM dbo.planilla p
		INNER JOIN detalle_planilla dp 
			on p.codigo_planilla = dp.codigo_planilla
		WHERE
			dp.codigo_detalle_cronograma = dc.codigo_detalle
			and dp.nro_cuota = dc.nro_cuota
	)
	ELSE '' END
) AS numero_planilla
from cronograma_pago_comision cpc
inner join detalle_cronograma dc on cpc.codigo_cronograma=dc.codigo_cronograma
inner join tipo_planilla tp on cpc.codigo_tipo_planilla=tp.codigo_tipo_planilla
inner join estado_cuota ec on ec.codigo_estado_cuota=dc.codigo_estado_cuota
where 
dc.codigo_articulo=@codigo_articulo and 
cpc.codigo_empresa=@codigo_empresa and 
codigo_moneda=@codigo_moneda and 
cpc.nro_contrato=@v_nro_contrato and 
cpc.codigo_tipo_planilla=2

union all

select
dc.es_registro_manual_comision,
dc.codigo_detalle as codigo_detalle_cronograma,
dc.codigo_estado_cuota,
(case when dc.codigo_estado_cuota=3 then (
select 
   max(fecha_movimiento) 
from operacion_cuota_comision oc where  oc.codigo_detalle_cronograma=dc.codigo_detalle and oc.codigo_tipo_operacion_cuota=3
)  else null end) as fecha_cierre,

(case when dc.codigo_estado_cuota=5 then (
select 
   max(fecha_movimiento) 
from operacion_cuota_comision oc where  oc.codigo_detalle_cronograma=dc.codigo_detalle and oc.codigo_tipo_operacion_cuota=5
)  else null end) as fecha_anulado,

(case when dc.codigo_estado_cuota=4 then (
select 
   max(fecha_movimiento) 
from operacion_cuota_comision oc where  oc.codigo_detalle_cronograma=dc.codigo_detalle and oc.codigo_tipo_operacion_cuota=4
)  else null end) as fecha_exclusion,

(case when dc.codigo_estado_cuota=5 then (
select top 1
   oc.motivo_movimiento 
from operacion_cuota_comision oc where  oc.codigo_detalle_cronograma=dc.codigo_detalle and oc.codigo_tipo_operacion_cuota=5
order by oc.codigo_operacion_cuota_comision desc
)  else ' ' end) as observacion,

dc.nro_cuota,
dc.igv,
dc.monto_bruto,
dc.monto_neto,
dc.fecha_programada,
tp.nombre as nombre_tipo_planilla,
(case when dc.es_registro_manual_comision=1 then 'Pagada' else ec.nombre end) as nombre_estado_cuota
,(
	CASE WHEN dc.codigo_estado_cuota=3 THEN
	(
		SELECT TOP 1
			p.numero_planilla
		FROM dbo.planilla p
		INNER JOIN detalle_planilla dp 
			on p.codigo_planilla = dp.codigo_planilla
		WHERE
			dp.codigo_detalle_cronograma = dc.codigo_detalle
			and dp.nro_cuota = dc.nro_cuota
	)
	ELSE '' END
) AS numero_planilla
from cronograma_pago_comision cpc
inner join detalle_cronograma dc on cpc.codigo_cronograma=dc.codigo_cronograma
inner join tipo_planilla tp on cpc.codigo_tipo_planilla=tp.codigo_tipo_planilla
inner join estado_cuota ec on ec.codigo_estado_cuota=dc.codigo_estado_cuota
where 
dc.codigo_articulo=@codigo_articulo and 
cpc.codigo_empresa=@codigo_empresa and 
codigo_moneda=@codigo_moneda and 
cpc.nro_contrato=@v_nro_contrato and 
cpc.codigo_tipo_planilla=1
order by nombre_tipo_planilla asc,nro_cuota asc;

end;