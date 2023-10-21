CREATE PROCEDURE up_detalle_planilla_pago_habilitados_by_id
(
	@p_codigo_planilla int
)
AS
BEGIN

	declare @tb_descuento table
	(
		codigo_personal int,
		descuento decimal(10,2)
	);
	insert into @tb_descuento
	select 
	   codigo_personal,
	   sum(monto) 
	from descuento
	where codigo_planilla=@p_codigo_planilla and estado_registro=1
	group by codigo_personal;


	declare @tb_resumen table(
		codigo_canal_grupo int,
		nombre_grupo varchar(150),
		codigo_personal int,
		apellido_materno varchar(150),
		apellido_paterno  varchar(150),
		nombre_persona  varchar(150),
		monto_bruto decimal(10,2),
		igv  decimal(10,2),  
		monto_neto   decimal(10,2)  
	);

	insert into @tb_resumen
	select 
		dp.codigo_grupo as codigo_canal_grupo,
		max(cg.nombre) as nombre_grupo,
		p.codigo_personal,
		max(p.apellido_materno) as apellido_materno,
		max(p.apellido_paterno) as apellido_paterno,
		max(p.nombre) as nombre_persona,
		sum(dp.monto_bruto) as monto_bruto,
		sum(dp.igv) as igv,  
		sum(dp.monto_neto) as monto_neto  
	from detalle_planilla dp
	--inner join detalle_cronograma dc on dp.codigo_detalle_cronograma=dc.codigo_detalle
	inner join personal p on p.codigo_personal=dp.codigo_personal
	left join canal_grupo cg on cg.codigo_canal_grupo=dp.codigo_grupo
	left join @tb_descuento td on td.codigo_personal=p.codigo_personal
	where dp.codigo_planilla=@p_codigo_planilla --and  dc.codigo_estado_cuota  in(2,3) and dc.estado_registro=1
	group by dp.codigo_grupo,p.codigo_personal

	select 
		r.codigo_canal_grupo,
		r.nombre_grupo,
		r.codigo_personal,
		r.apellido_materno,
		r.apellido_paterno,
		r.nombre_persona,
		r.monto_bruto,
		r.igv,  
		r.monto_neto,
		isnull(d.descuento,0) descuento,
		r.monto_bruto+r.igv-isnull(d.descuento,0) as comision_total,
		r.monto_neto
	from @tb_resumen r
	left join @tb_descuento d on r.codigo_personal=d.codigo_personal

END;