CREATE PROCEDURE [dbo].[up_contrato_analisis_detalle_cuota]
(
	@codigo_articulo int,
	@codigo_empresa int,
	@codigo_moneda int,
	@nro_contrato nvarchar(200)
)
AS
BEGIN

	DECLARE 
		@v_nro_contrato VARCHAR(100)
		,@c_esHR		BIT = 0

	SELECT 
		@v_nro_contrato = NumAtCard
	FROM dbo.cabecera_contrato cc
	INNER JOIN dbo.empresa_sigeco e 
		ON e.codigo_empresa = @codigo_empresa
	WHERE 
		(ISNULL(REPLICATE('0', 10 - LEN(cc.NumAtCard)), '') + cc.NumAtCard) = (ISNULL(REPLICATE('0', 10 - LEN(@nro_contrato)), '') + @nro_contrato)
		AND cc.Codigo_empresa = e.codigo_equivalencia

	SET @c_esHR = (SELECT dbo.fn_generar_cronograma_comision_eval_hoja_resumen(@v_nro_contrato))

	select
	dc.es_registro_manual_comision,
	dc.codigo_detalle as codigo_detalle_cronograma,
	dc.codigo_estado_cuota,
	(
		CASE WHEN dc.codigo_estado_cuota IN (2,3) THEN
		(
			SELECT TOP 1
				p.fecha_fin
			FROM dbo.planilla p
			INNER JOIN detalle_planilla dp 
				on p.codigo_planilla = dp.codigo_planilla
			WHERE
				dp.codigo_detalle_cronograma = dc.codigo_detalle
				and dp.nro_cuota = dc.nro_cuota
				and p.codigo_estado_planilla in (1, 2)
		)
		ELSE NULL END
	) AS fecha_cierre,

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

	(
	case 
		when dc.codigo_estado_cuota=1 then (
			select top 1
			   oc.motivo_movimiento 
			from operacion_cuota_comision oc where  oc.codigo_detalle_cronograma=dc.codigo_detalle and oc.codigo_tipo_operacion_cuota=6 and oc.estado_registro = 1
			/*order by oc.codigo_operacion_cuota_comision desc*/
		)  
		when dc.codigo_estado_cuota=5 then (
			select top 1
			   oc.motivo_movimiento 
			from operacion_cuota_comision oc where  oc.codigo_detalle_cronograma=dc.codigo_detalle and oc.codigo_tipo_operacion_cuota=5
			order by oc.codigo_operacion_cuota_comision desc
		)  
		when dc.codigo_estado_cuota=4 then (
			select top 1
			   e.motivo_exclusion
			from exclusion_cuota_planilla e where  e.codigo_detalle_cronograma=dc.codigo_detalle and e.estado_exclusion=1
		)  
		else ' ' end
	) as observacion,
	dc.nro_cuota,
	dc.igv,
	dc.monto_bruto,
	dc.monto_neto,
	dc.fecha_programada,
	tp.nombre as nombre_tipo_planilla,
	(case when dc.es_registro_manual_comision=1 then 'Pagada' else ec.nombre end) as nombre_estado_cuota

	,(
		CASE WHEN dc.codigo_estado_cuota IN (2, 3) THEN
		(
			SELECT TOP 1
				p.numero_planilla
			FROM dbo.planilla p
			INNER JOIN detalle_planilla dp 
				on p.codigo_planilla = dp.codigo_planilla
			WHERE
				dp.codigo_detalle_cronograma = dc.codigo_detalle
				and dp.nro_cuota = dc.nro_cuota
				and p.codigo_estado_planilla in (1, 2)
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
		CASE WHEN @c_esHR = 0 THEN cpc.nro_contrato ELSE cpc.nro_contrato_adicional END = @v_nro_contrato and 
		cpc.codigo_tipo_planilla=2

	UNION ALL

	select
	dc.es_registro_manual_comision,
	dc.codigo_detalle as codigo_detalle_cronograma,
	dc.codigo_estado_cuota,
	(
		CASE WHEN dc.codigo_estado_cuota IN (2,3) THEN
		(
			SELECT TOP 1
				p.fecha_fin
			FROM dbo.planilla p
			INNER JOIN detalle_planilla dp 
				on p.codigo_planilla = dp.codigo_planilla
			WHERE
				dp.codigo_detalle_cronograma = dc.codigo_detalle
				and dp.nro_cuota = dc.nro_cuota
				and p.codigo_estado_planilla in (1, 2)
		)
		ELSE NULL END
	) AS fecha_cierre,
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

	(
	case 
		when dc.codigo_estado_cuota=1 then (
			select top 1
			   oc.motivo_movimiento 
			from operacion_cuota_comision oc where  oc.codigo_detalle_cronograma=dc.codigo_detalle and oc.codigo_tipo_operacion_cuota=6 and oc.estado_registro = 1
			/*order by oc.codigo_operacion_cuota_comision desc*/
		)  
		when dc.codigo_estado_cuota=5 then (
			select top 1
			   oc.motivo_movimiento 
			from operacion_cuota_comision oc where  oc.codigo_detalle_cronograma=dc.codigo_detalle and oc.codigo_tipo_operacion_cuota=5
			order by oc.codigo_operacion_cuota_comision desc
		)  
		when dc.codigo_estado_cuota=4 then (
			select top 1
			   e.motivo_exclusion
			from exclusion_cuota_planilla e where  e.codigo_detalle_cronograma=dc.codigo_detalle and e.estado_exclusion=1
		)  
		else ' ' end
	) as observacion,
	dc.nro_cuota,
	dc.igv,
	dc.monto_bruto,
	dc.monto_neto,
	dc.fecha_programada,
	tp.nombre as nombre_tipo_planilla,
	(case when dc.es_registro_manual_comision=1 then 'Pagada' else ec.nombre end) as nombre_estado_cuota
	,(
		CASE WHEN dc.codigo_estado_cuota IN (2,3) THEN
		(
			SELECT TOP 1
				p.numero_planilla
			FROM dbo.planilla p
			INNER JOIN detalle_planilla dp 
				on p.codigo_planilla = dp.codigo_planilla
			WHERE
				dp.codigo_detalle_cronograma = dc.codigo_detalle
				and dp.nro_cuota = dc.nro_cuota
				and p.codigo_estado_planilla in (1, 2)
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
		CASE WHEN @c_esHR = 0 THEN cpc.nro_contrato ELSE cpc.nro_contrato_adicional END = @v_nro_contrato and 
		cpc.codigo_tipo_planilla=1
	order by nombre_tipo_planilla asc,nro_cuota asc,codigo_estado_cuota asc;

END;