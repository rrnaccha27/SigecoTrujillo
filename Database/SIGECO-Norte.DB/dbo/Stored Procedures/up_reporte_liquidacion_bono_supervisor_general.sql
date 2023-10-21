CREATE PROCEDURE [dbo].[up_reporte_liquidacion_bono_supervisor_general]
(
	@v_codigo_planilla int
)
AS
BEGIN

	declare 
		@nombre_canal varchar(100),
		@fecha_inicio varchar(20),
		@codigo_estado_planilla int,
		@codigo_tipo_planilla int,
		@fecha_fin varchar(20);

	declare  
		@v_limite_detraccion decimal(10,2),
		@v_porcentaje_igv decimal(10,2),
		@v_porcentaje_detraccion decimal(10,2);

	select @v_porcentaje_detraccion=convert(decimal(10,2),valor)  from parametro_sistema where codigo_parametro_sistema=15;
	select @v_limite_detraccion=convert(decimal(10,2),valor)  from parametro_sistema where codigo_parametro_sistema=16;
	select @v_porcentaje_igv = 1 + (convert(decimal(10,2),valor)/100)  from parametro_sistema where codigo_parametro_sistema=9;

	-- set @v_porcentaje_igv=@v_porcentaje_igv/100;
	set @v_porcentaje_detraccion=(@v_porcentaje_detraccion/100);

	select @fecha_inicio=CONVERT(NVARCHAR(10), fecha_inicio, 103),
		@fecha_fin=CONVERT(NVARCHAR(10), fecha_fin, 103) ,
		@nombre_canal=upper(cg.nombre),
		@codigo_estado_planilla=codigo_estado_planilla,
		@codigo_tipo_planilla=codigo_tipo_planilla
	from planilla_bono pb 
	inner join canal_grupo cg on pb.codigo_canal=cg.codigo_canal_grupo where codigo_planilla=@v_codigo_planilla;

	declare @cabecera table(
		codigo_supervisor int,
		apellidos_nombres_supervisor nvarchar(200),
		codigo_empresa int,
		nombre_empresa  nvarchar(200),
		codigo_grupo int,
		nombre_grupo  nvarchar(200),
		monto_igv_empresa decimal(10,2),
		monto_bruto_empresa decimal(10,2),
		monto_neto_empresa decimal(10,2),
		monto_detraccion_empresa decimal(10,2),
		monto_neto_bono_empresa decimal(10,2),
		monto_ingresado_empresa decimal(10,2),
		monto_neto_supervisor decimal(10,2),

		monto_contratado_supervisor decimal(10,2),
		monto_ingresado_supervisor decimal(10,2),
		calcular_detraccion bit
	);

	insert into @cabecera(codigo_supervisor,apellidos_nombres_supervisor,codigo_empresa,nombre_empresa,
	codigo_grupo,nombre_grupo,monto_bruto_empresa,monto_neto_empresa,monto_ingresado_empresa,monto_contratado_supervisor,monto_ingresado_supervisor,monto_igv_empresa,calcular_detraccion)
	select 
		dpb.codigo_personal,
		p.nombre+' '+p.apellido_paterno+' '+p.apellido_materno,
		dpb.codigo_empresa,
		e.nombre,
		dpb.codigo_grupo,
		c.nombre,
		sum(dpb.monto_bruto) over(partition by dpb.codigo_personal, dpb.codigo_empresa) as monto_bruto_empresa,
		sum(dpb.monto_neto) over(partition by dpb.codigo_personal, dpb.codigo_empresa) as monto_neto_empresa,
		sum(dpb.monto_ingresado) over(partition by dpb.codigo_personal, dpb.codigo_empresa) as monto_ingresado_empresa,
		rpb.monto_contratado as monto_contratado_supervisor,
		rpb.monto_ingresado as monto_ingresado_supervisor,
		sum(dpb.monto_igv) over(partition by dpb.codigo_personal, dpb.codigo_empresa) as monto_neto_empresa,
		dbo.fn_canal_grupo_percibe_factura(isnull(dpb.codigo_grupo,dpb.codigo_canal),dpb.codigo_empresa,case when @codigo_tipo_planilla = 1 then 0 else 1 end,1) aplicac_detraccion
	from detalle_planilla_bono dpb 
	inner join resumen_planilla_bono rpb on dpb.codigo_planilla=rpb.codigo_planilla and rpb.codigo_personal=dpb.codigo_personal
	inner join personal p on dpb.codigo_personal=p.codigo_personal
	inner join empresa_sigeco e on dpb.codigo_empresa=e.codigo_empresa
	inner join canal_grupo c on dpb.codigo_grupo=c.codigo_canal_grupo
	where dpb.codigo_planilla=@v_codigo_planilla

	update 
		@cabecera 
	set
		monto_detraccion_empresa=(case when @v_limite_detraccion<monto_neto_empresa and calcular_detraccion=1
		then ROUND(monto_neto_empresa*@v_porcentaje_detraccion, 0) else 0 end)
	 
	--update @cabecera set monto_detraccion_empresa=(case when monto_neto_empresa>=@v_limite_detraccion then monto_neto_empresa*@v_porcentaje_detraccion else 0 end);

	update @cabecera set monto_neto_bono_empresa=monto_neto_empresa-monto_detraccion_empresa;

	merge into @cabecera ds
	using(
		select codigo_supervisor,sum(monto_neto_bono_empresa) monto_neto_bono_supervisor from @cabecera group by codigo_supervisor
	) sc on(ds.codigo_supervisor=sc.codigo_supervisor)
	when matched then 
		update set ds.monto_neto_supervisor=sc.monto_neto_bono_supervisor;
	--------------------------------------------------------------------------------------------------------------------------
	declare @tabla_detalle table(
		codigo_supervisor int,
		codigo_personal int,
		codigo_empresa int,
		apellidos_nombres_personal nvarchar(200),
		numero_contrato  nvarchar(100),
		nombre_tipo_venta  nvarchar(100),
		monto_contratado decimal(10,2),
		monto_ingresado decimal(10,2),
		porcentaje_pago decimal(10,2),
		monto_bono decimal(10,2),
		fecha_contrato varchar(10)
	);

	insert into @tabla_detalle
	select 
		cpb.codigo_supervisor,
		cpb.codigo_personal,
		cpb.codigo_empresa,   
		p.nombre +' '+p.apellido_paterno+' '+p.apellido_materno as datos_personal,   
		cpb.numero_contrato,
		tv.abreviatura as nombre_tipo_venta,
		cpb.monto_contratado,
		cpb.monto_ingresado,
		rpb.porcentaje_pago,
		cpb.monto_ingresado*(rpb.porcentaje_pago/100) as monto_bono,
		cpb.fecha_contrato
	from contrato_planilla_bono cpb 
	inner join resumen_planilla_bono rpb on cpb.codigo_planilla=rpb.codigo_planilla and cpb.codigo_supervisor=rpb.codigo_personal
	inner join personal p on cpb.codigo_personal=p.codigo_personal
	inner join tipo_venta tv on cpb.codigo_tipo_venta=tv.codigo_tipo_venta
	where cpb.codigo_planilla=@v_codigo_planilla;

	select 
		@v_codigo_planilla as codigo_planilla,
		@fecha_inicio as fecha_inicio,
		@fecha_fin as fecha_fin,
		@nombre_canal as nombre_canal,
		@codigo_estado_planilla as codigo_estado_planilla, 

		a.codigo_supervisor ,
		a.apellidos_nombres_supervisor,
		a.codigo_empresa,
		a.nombre_empresa,
		a.codigo_grupo,
		a.nombre_grupo,

		a.monto_bruto_empresa ,
		a.monto_neto_empresa ,
		a.monto_detraccion_empresa ,
		a.monto_neto_bono_empresa ,
		a.monto_ingresado_empresa ,
		a.monto_igv_empresa,
		a.monto_neto_supervisor ,
		a.monto_contratado_supervisor ,
		a.monto_ingresado_supervisor ,
		------------------------------------- 
		b.codigo_personal,
		b.apellidos_nombres_personal ,
		b.numero_contrato  ,
		b.nombre_tipo_venta  ,
		b.monto_contratado ,
		b.monto_ingresado ,
		b.porcentaje_pago ,
		b.monto_bono ,
		b.fecha_contrato
	from @cabecera a 
	inner join @tabla_detalle b	on a.codigo_supervisor=b.codigo_supervisor and a.codigo_empresa=b.codigo_empresa
	order by
		a.apellidos_nombres_supervisor
		,b.apellidos_nombres_personal
		,b.numero_contrato;

END;


-------------------------------------------------------------------------------------------------------------------------