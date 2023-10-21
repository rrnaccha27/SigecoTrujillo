CREATE PROCEDURE dbo.up_reporte_cuotas_iniciales
(
	@p_fecha_inicial	VARCHAR(10)
	,@p_fecha_final		VARCHAR(10)
)
AS
BEGIN
	SET NOCOUNT ON

	DECLARE
		@v_fecha_inicio	DATETIME = CONVERT(DATETIME, @p_fecha_inicial)
		,@v_fecha_fin	DATETIME = CONVERT(DATETIME, @p_fecha_final + ' 23:59:59')

	select 
		cab.docentry
		,e.nombre as nombre_empresa
		,cab.numatcard as nro_contrato
		,upper(tv.abreviatura) as nombre_tipo_venta
		,cab.Cod_FormaPago as nombre_tipo_pago
		,convert(varchar, cab.createdate, 103) as fecha_contrato
		--,(select 'Cuota Inicial sin pago' from contrato_cuota cuo where cuo.Codigo_empresa = cab.codigo_empresa and cuo.NumAtCard = cab.NumAtCard and Num_Cuota = 0 and (Cod_Estado = 'P' or Fec_Pago is null))
		,cuo.num_importe_total as dinero_ingresado
		,cab.doctotal as monto_contratado
		,case when exists(
			select * from cronograma_pago_comision cpc
			inner join detalle_cronograma dc on dc.codigo_cronograma = cpc.codigo_cronograma 
			inner join empresa_sigeco e on e.codigo_equivalencia = cab.Codigo_empresa
			where cpc.nro_contrato = cab.NumAtCard and cpc.codigo_empresa = e.codigo_empresa
				and dc.nro_cuota = 1
				and dc.fecha_programada is not null
		) then 'SI' else 'NO' end comision_habilitada
		,isnull(ep.nombre,'') as estado_proceso
		,isnull(mi.Observacion, '') as observacion_proceso
	from cabecera_contrato cab
	inner join contrato_cuota cuo on cuo.Codigo_empresa = cab.codigo_empresa and cuo.NumAtCard = cab.NumAtCard and Num_Cuota = 0 and (Cod_Estado = 'P' or Fec_Pago is null)
	inner join empresa_sigeco e on e.codigo_equivalencia = cab.codigo_empresa
	inner join tipo_venta tv on tv.codigo_equivalencia = cab.cod_tipo_venta 
	left join contrato_migrado mi on mi.Codigo_empresa = cab.Codigo_empresa and mi.NumAtCard = cab.NumAtCard
	left join estado_proceso ep on ep.codigo_estado_proceso = mi.codigo_estado_proceso
	where 
		cab.CreateDate between @v_fecha_inicio and @v_fecha_fin
	order by cab.createdate, cab.numatcard, cab.codigo_empresa

	SET NOCOUNT OFF
END;