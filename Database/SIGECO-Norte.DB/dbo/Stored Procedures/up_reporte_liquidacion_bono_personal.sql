CREATE PROC [dbo].[up_reporte_liquidacion_bono_personal](
	@p_codigo_planilla int
)
AS
BEGIN

	declare @p_codigo_personal int = null;

	declare
		@codigo_tipo_planilla int,
		@codigo_estado_planilla int,
		@monto_limite_detraccion decimal(10,2),
		@porcentaje_detraccion decimal(10,2),
		@nombre_canal varchar(100),
		@fecha_inicio varchar(20),
		@fecha_fin varchar(20),
		@v_igv decimal(10, 2);

	select @monto_limite_detraccion = convert(numeric,valor) from parametro_sistema where codigo_parametro_sistema=16;
	select @porcentaje_detraccion = convert(numeric,valor)/100 from parametro_sistema where codigo_parametro_sistema=15;
	select @v_igv=1 + convert(numeric,valor)/100 from parametro_sistema where codigo_parametro_sistema=9;

	select @fecha_inicio=CONVERT(NVARCHAR(10), fecha_inicio, 103),
		@fecha_fin=CONVERT(NVARCHAR(10), fecha_fin, 103) ,
		@nombre_canal=upper(cg.nombre),
		@codigo_estado_planilla=codigo_estado_planilla,
		@codigo_tipo_planilla=codigo_tipo_planilla
	from planilla_bono pb inner join canal_grupo cg on pb.codigo_canal=cg.codigo_canal_grupo where codigo_planilla=@p_codigo_planilla;
 
	declare @resumen_empresa table(
		codigo_grupo int,
		codigo_personal int,
		apellidos_nombres varchar(200),
		codigo_empresa int,
		nombre_empresa varchar(200),
		nombre_empresa_largo varchar(200),
		monto_contratado_empresa decimal(10,2),
		monto_ingresado_empresa decimal(10,2),
		monto_bruto_empresa decimal(10,2),
		monto_igv_empresa decimal(10,2),
		detraccion_empresa decimal(10,2),
		monto_neto_empresa decimal(10,2) ,
		monto_bono_total_persona decimal(10,2) ,
		calcular_detraccion bit
	);

	insert into @resumen_empresa
	select 
		dp.codigo_grupo,
		dp.codigo_personal,
		max(isnull(p.nombre,' ')+' '+ISNULL(p.apellido_paterno,'') +' '+ISNULL(p.apellido_materno, '')),
		dp.codigo_empresa,
		max(e.nombre),
		max(e.nombre_largo),
 
		sum(dp.monto_contratado),
		sum(dp.monto_ingresado),
		sum(dp.monto_bruto),
		sum(dp.monto_igv), 
		0,
		sum(dp.monto_neto),
		0,
		dbo.fn_canal_grupo_percibe_factura(isnull(dp.codigo_grupo,max(dp.codigo_canal)),dp.codigo_empresa,case when @codigo_tipo_planilla = 1 then 0 else 1 end,1) aplicac_detraccion
	from detalle_planilla_bono dp
	inner join personal p on dp.codigo_personal=p.codigo_personal
	inner join empresa_sigeco e on dp.codigo_empresa=e.codigo_empresa
	where dp.codigo_planilla=@p_codigo_planilla and dp.codigo_personal=isnull(@p_codigo_personal,dp.codigo_personal)
	group by dp.codigo_personal,dp.codigo_empresa,dp.codigo_grupo;

	--select * from @resumen_empresa 

	/************************************************************************************************
	CALCULANDO EL DETRACCION BONO POR EMPRESA
	*************************************************************************************************/
	update 
		@resumen_empresa 
	set
		detraccion_empresa=(case when @monto_limite_detraccion<monto_neto_empresa and calcular_detraccion=1
		then ROUND(monto_neto_empresa*@porcentaje_detraccion, 0) else 0 end)
 
	/************************************************************************************************
	CALCULANDO EL BONO TOTAL MENOS DETRACCION  POR EMPRESA
	*************************************************************************************************/
	update @resumen_empresa set monto_neto_empresa=monto_neto_empresa-detraccion_empresa;

	/************************************************************************************************
	CALCULANDO EL BONO TOTAL POR PERSONA
	*************************************************************************************************/
	merge into @resumen_empresa ds
	using(
		select codigo_personal,sum(monto_neto_empresa) monto_neto_empresa from @resumen_empresa group by codigo_personal
	) sc on(ds.codigo_personal=sc.codigo_personal)
	when matched then 
		update set ds.monto_bono_total_persona=sc.monto_neto_empresa;
 
	select 
		@p_codigo_planilla as codigo_planilla,
		@fecha_inicio as fecha_inicio,
		@fecha_fin as fecha_fin,
		@nombre_canal as nombre_canal,
		@codigo_estado_planilla as codigo_estado_planilla,
		------------------------------------------
		t.codigo_personal,
		r.apellidos_nombres,
		t.monto_contratado_personal,
		t.monto_ingresado_personal,
		r.monto_bono_total_persona,
   		-----------------------------------------------
		r.codigo_empresa,
		r.nombre_empresa,
		r.nombre_empresa_largo,
		r.monto_contratado_empresa,
		r.monto_ingresado_empresa,
		r.monto_bruto_empresa,
		r.monto_igv_empresa,
		r.detraccion_empresa,
		r.monto_neto_empresa,
		-------------------------------------------------------
		t.codigo_grupo,
		t.nombre_grupo,
		t.numero_contrato,
		t.nombre_tipo_venta,
		t.monto_contratado,
		t.monto_ingresado,
		t.porcentaje_pago,
		t.importe_bono_detalle,
		t.fecha_contrato,
		t.tipo_pago,
		t.num_ventas
	from 
		(SELECT 
			----------------------------------------
			d.codigo_personal,
			rp.monto_contratado as monto_contratado_personal,
			rp.monto_ingresado as monto_ingresado_personal,
			rp.meta_lograda,
			rp.porcentaje_pago,
			-----------------------------------------
			d.codigo_empresa,
			d.codigo_grupo, 
			b.numero_contrato,
			b.codigo_tipo_venta,
			tv.abreviatura as nombre_tipo_venta, 
			cg.nombre as nombre_grupo,
			b.monto_contratado,
			b.monto_ingresado, 
			b.monto_ingresado*(rp.porcentaje_pago/100) importe_bono_detalle,
			b.fecha_contrato,
			tp.nombre as tipo_pago,
			convert(int, cc.Num_Ventas) as num_ventas
			--,a.nombre as nombre_articulo
		FROM detalle_planilla_bono d
		inner join resumen_planilla_bono rp on d.codigo_personal=rp.codigo_personal  and d.codigo_planilla=rp.codigo_planilla  
		inner join contrato_planilla_bono b on d.codigo_empresa=b.codigo_empresa and 
			d.codigo_personal=b.codigo_personal and 
			d.codigo_planilla=b.codigo_planilla
		inner join tipo_venta tv on tv.codigo_tipo_venta=b.codigo_tipo_venta
		inner join empresa_sigeco es on es.codigo_empresa = b.codigo_empresa and es.estado_registro = 1
		inner join cabecera_contrato cc on cc.NumAtCard = b.numero_contrato and cc.Codigo_empresa = es.codigo_equivalencia
		inner join tipo_pago tp on cc.Cod_FormaPago = tp.codigo_equivalencia
		left join canal_grupo cg on cg.codigo_canal_grupo=d.codigo_grupo
		--left join articulo_planilla_bono apb on apb.codigo_planilla_bono = @p_codigo_planilla and b.codigo_empresa = apb.codigo_empresa and b.numero_contrato = apb.nro_contrato
		--left join articulo a on a.codigo_articulo = apb.codigo_articulo
		where d.codigo_planilla=@p_codigo_planilla and d.codigo_personal=isnull(@p_codigo_personal,d.codigo_personal)
		) t 
	inner join @resumen_empresa r on t.codigo_empresa=r.codigo_empresa and t.codigo_personal=r.codigo_personal
	order by
		t.nombre_grupo asc
		,apellidos_nombres
		,t.numero_contrato ;
 
END;