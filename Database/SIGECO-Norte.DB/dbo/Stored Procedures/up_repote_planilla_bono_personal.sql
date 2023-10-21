CREATE PROCEDURE [dbo].[up_repote_planilla_bono_personal]
 (
	@p_codigo_planilla int
)
as
begin

	declare 
		@monto_limite_detraccion decimal(10,2),
		@porcentaje_detraccion decimal(10,2),
		@nombre_canal varchar(100),
		@fecha_inicio varchar(20),
		@codigo_estado_planilla int,
		@codigo_tipo_planilla int,
		@codigo_canal int,
		@fecha_fin varchar(20),
		@v_igv decimal(10, 2);

	select @fecha_inicio=CONVERT(NVARCHAR(10), fecha_inicio, 103),
		@fecha_fin=CONVERT(NVARCHAR(10), fecha_fin, 103) ,
		@nombre_canal=upper(cg.nombre),
		@codigo_estado_planilla=codigo_estado_planilla,
		@codigo_tipo_planilla=codigo_tipo_planilla,
		@codigo_canal=codigo_canal
	from planilla_bono pb inner join canal_grupo cg on pb.codigo_canal=cg.codigo_canal_grupo where codigo_planilla=@p_codigo_planilla;

 
	select @monto_limite_detraccion=Convert(numeric,valor) from parametro_sistema where codigo_parametro_sistema=16;
	select @porcentaje_detraccion=Convert(numeric,valor)/100 from parametro_sistema where codigo_parametro_sistema=15;
	select @v_igv=1 + convert(numeric,valor)/100 from parametro_sistema where codigo_parametro_sistema=9;

	declare @resumen_grupo table(
		codigo_grupo int,
		codigo_personal int, 
		monto_bruto_grupo decimal(10,2),
		monto_igv_grupo decimal(10,2),
		monto_neto_grupo decimal(10,2) ,
		monto_contrato_grupo decimal(10,2), 
		monto_ingresado_grupo decimal(10,2), 
		meta_logrado decimal(10,2), 
		porcentaje_pago decimal(10,2) 
	);

	insert into @resumen_grupo
	select 
		dpl.codigo_grupo,
		dpl.codigo_personal,
		sum(dpl.monto_bruto) monto_bruto,
		sum(dpl.monto_igv) monto_igv,
		sum(dpl.monto_neto) monto_neto,
		sum(dpl.monto_contratado) monto_contratado,
		sum(dpl.monto_ingresado) monto_ingresado,

		max(rpb.meta_lograda) meta_lograda,
		max(rpb.porcentaje_pago) porcentaje_pago
	from detalle_planilla_bono dpl
	inner join resumen_planilla_bono rpb on dpl.codigo_planilla=rpb.codigo_planilla and dpl.codigo_personal=rpb.codigo_personal
	where dpl.codigo_planilla=@p_codigo_planilla
	group by dpl.codigo_grupo,dpl.codigo_personal;


	declare @resumen_empresa table(
		codigo_grupo int,
		codigo_personal int,
		codigo_empresa int,
		monto_bruto_empresa decimal(10,2),
		monto_igv_empresa decimal(10,2),
		monto_neto_empresa decimal(10,2) ,
		monto_ingresado_empresa decimal(10,2), 
		detraccion_empresa decimal(10,2), 
		monto_neto_bono_empresa decimal(10,2) ,
		calcular_detraccion bit
	);

	insert into @resumen_empresa
	select 
		dpl.codigo_grupo,dpl.codigo_personal,dpl.codigo_empresa,
		sum(dpl.monto_bruto) monto_bruto_empresa,
		sum(dpl.monto_igv) monto_igv_empresa,
		sum(dpl.monto_neto) monto_neto_empresa,
		sum(dpl.monto_ingresado) monto_ingresado_empresa,
		0,
		0,
		dbo.fn_canal_grupo_percibe_factura(isnull(dpl.codigo_grupo,@codigo_canal),dpl.codigo_empresa,case when @codigo_tipo_planilla = 1 then 0 else 1 end,1) aplicac_detraccion
	from  detalle_planilla_bono dpl where codigo_planilla=@p_codigo_planilla
	group by dpl.codigo_grupo,dpl.codigo_personal,dpl.codigo_empresa;


	update 
		@resumen_empresa 
	set
		detraccion_empresa=(case when @monto_limite_detraccion<monto_neto_empresa and calcular_detraccion=1
		then ROUND(monto_neto_empresa*@porcentaje_detraccion, 0) else 0 end)

	--update @resumen_empresa set detraccion_empresa=case when monto_neto_empresa>@monto_limite_detraccion then @porcentaje_detraccion*monto_neto_empresa else 0 end;

	update @resumen_empresa set monto_neto_bono_empresa=monto_neto_empresa-detraccion_empresa;

	-------------------------------------------------------------------------------

	declare @detraccion_grupo table(
		codigo_personal int,
		codigo_empresa int,
		codigo_grupo  int,
		monto_neto_empresa decimal(10,2),
		monto_detraccion_empresa decimal(10,2),
		monto_neto_bono_empresa decimal(10,2),
		calcular_detraccion bit
	);

	insert into @detraccion_grupo(codigo_personal,codigo_empresa,codigo_grupo,monto_neto_empresa,calcular_detraccion)
	select 
		dpl.codigo_personal,
		dpl.codigo_empresa,
		dpl.codigo_grupo,
		sum(dpl.monto_neto) ,
		dbo.fn_canal_grupo_percibe_factura(isnull(dpl.codigo_grupo,max(dpl.codigo_canal)),dpl.codigo_empresa,case when @codigo_tipo_planilla = 1 then 0 else 1 end,1) aplicac_detraccion
	from detalle_planilla_bono dpl 
	where dpl.codigo_planilla=@p_codigo_planilla
	group by 
		dpl.codigo_personal,
		dpl.codigo_empresa,
		dpl.codigo_grupo

	update 
		@detraccion_grupo 
	set
		monto_detraccion_empresa=(case when @monto_limite_detraccion<monto_neto_empresa and calcular_detraccion=1
		then ROUND(monto_neto_empresa*@porcentaje_detraccion, 0) else 0 end)

	--update @detraccion_grupo set monto_detraccion_empresa=case when monto_neto_empresa>@monto_limite_detraccion then @porcentaje_detraccion*monto_neto_empresa else 0 end;
	update @detraccion_grupo set monto_neto_bono_empresa=monto_neto_empresa-monto_detraccion_empresa;
	---------------------------------------------------------------------------------



	select 
		@p_codigo_planilla as codigo_planilla,
		@fecha_inicio as fecha_inicio,
		@fecha_fin as fecha_fin,
		@nombre_canal as nombre_canal,
		@codigo_estado_planilla as codigo_estado_planilla, 
		----------------
		g.codigo_grupo,
		c.nombre as nombre_grupo,
		p.codigo_personal,
		p.nombre+' '+p.apellido_paterno+' '+p.apellido_paterno as nombres_apellidos,
		emp.codigo_empresa,
		emp.nombre as nombre_empresa,
		e.monto_bruto_empresa,
		e.monto_igv_empresa ,
		e.monto_neto_empresa ,
		e.monto_ingresado_empresa, 
		e.detraccion_empresa, 
		e.monto_neto_bono_empresa,
		g.monto_bruto_grupo,
		g.monto_igv_grupo,
		--g.monto_neto_grupo ,
		(
			--(select
			--SUM(dg.monto_neto_bono_empresa)
			--from @detraccion_grupo dg where dg.codigo_grupo=g.codigo_grupo and dg.codigo_personal=g.codigo_personal)
			g.monto_neto_grupo
		) monto_neto_grupo,
		g.monto_contrato_grupo , 
		g.monto_ingresado_grupo, 
		g.meta_logrado , 
		g.porcentaje_pago 

	from 
	@resumen_grupo g 
	inner join @resumen_empresa e on g.codigo_personal=e.codigo_personal and g.codigo_grupo=e.codigo_grupo
	inner join personal p on g.codigo_personal=p.codigo_personal
	inner join canal_grupo c on g.codigo_grupo=c.codigo_canal_grupo
	inner join empresa_sigeco emp on emp.codigo_empresa=e.codigo_empresa
	where 
		g.monto_neto_grupo > 0
	order by nombre_grupo,nombres_apellidos;

end;