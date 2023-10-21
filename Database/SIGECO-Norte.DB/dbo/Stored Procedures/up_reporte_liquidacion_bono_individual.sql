USE SIGECO
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[up_reporte_liquidacion_bono_individual]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].up_reporte_liquidacion_bono_individual
GO

CREATE PROCEDURE [dbo].[up_reporte_liquidacion_bono_individual]
(
	@v_codigo_planilla int
)
AS
BEGIN
	SET NOCOUNT ON

	declare 
		 @nombre_canal varchar(100),
		 @fecha_inicio varchar(20),
		 @codigo_estado_planilla int,
		 @codigo_tipo_planilla int,
		 @fecha_fin varchar(20);

	 declare   
		 @v_limite_detraccion decimal(10,2),
		-- @v_porcentaje_igv decimal(10,2),
		 @v_porcentaje_detraccion decimal(10,2);

	 select @v_porcentaje_detraccion=convert(decimal(10,2),valor)  from parametro_sistema where codigo_parametro_sistema=15;
	 select @v_limite_detraccion=convert(decimal(10,2),valor)  from parametro_sistema where codigo_parametro_sistema=16;
	 --select @v_porcentaje_igv=convert(decimal(10,2),valor)  from parametro_sistema where codigo_parametro_sistema=9;
 
	-- set @v_porcentaje_igv=@v_porcentaje_igv/100;
	 set @v_porcentaje_detraccion=(@v_porcentaje_detraccion/100);

	select @fecha_inicio=CONVERT(NVARCHAR(10), fecha_inicio, 103),
		@fecha_fin=CONVERT(NVARCHAR(10), fecha_fin, 103) ,
		@nombre_canal=upper(cg.nombre),
		@codigo_estado_planilla=codigo_estado_planilla,
		@codigo_tipo_planilla=codigo_tipo_planilla
	from planilla_bono pb inner join canal_grupo cg on pb.codigo_canal=cg.codigo_canal_grupo where codigo_planilla=@v_codigo_planilla;

	declare @cabecera table(
		codigo_supervisor int,
		apellidos_nombres_supervisor nvarchar(200),
		codigo_empresa int,
		nombre_empresa  nvarchar(200),
		codigo_grupo int,
		nombre_grupo  nvarchar(200),
		monto_bruto_empresa_supervisor decimal(10,2),
		monto_igv_empresa_supervisor decimal(10,2),
		monto_neto_empresa_supervisor decimal(10,2),
		monto_detraccion_empresa_supervisor decimal(10,2),
		monto_neto_pagar_empresa_supervisor decimal(10,2),
		calcular_detraccion bit,
		codigo_canal int,
		nombre_supervisor varchar(250), 
		email_supervisor varchar(250)
	);

	insert into @cabecera(
		codigo_supervisor,apellidos_nombres_supervisor,codigo_empresa,nombre_empresa,codigo_grupo,nombre_grupo,monto_neto_empresa_supervisor,
		monto_igv_empresa_supervisor,monto_bruto_empresa_supervisor,calcular_detraccion, codigo_canal, nombre_supervisor, email_supervisor
	)
	select 
		dpb.codigo_personal,
		max(p.nombre+' '+p.apellido_paterno+' '+p.apellido_materno),
		dpb.codigo_empresa,
		max(e.nombre),
		dpb.codigo_grupo,
		max(c.nombre),  
   
		sum(dpb.monto_neto) ,
		sum(dpb.monto_igv) ,
		sum(dpb.monto_bruto) ,	
		dbo.fn_canal_grupo_percibe_factura(isnull(dpb.codigo_grupo,max(dpb.codigo_canal)),dpb.codigo_empresa,case when @codigo_tipo_planilla = 1 then 0 else 1 end,1) aplicac_detraccion,
		ISNULL(max(c.codigo_padre), dpb.codigo_grupo),
		max(dpb.nombre_supervisor),
		max(dpb.email_supervisor)
	from detalle_planilla_bono dpb inner join resumen_planilla_bono rpb on 
	dpb.codigo_planilla=rpb.codigo_planilla and rpb.codigo_personal=dpb.codigo_personal
	inner join personal p on dpb.codigo_personal=p.codigo_personal
	inner join empresa_sigeco e on dpb.codigo_empresa=e.codigo_empresa
	inner join canal_grupo c on dpb.codigo_grupo=c.codigo_canal_grupo
	where dpb.codigo_planilla=@v_codigo_planilla
	group by dpb.codigo_empresa, dpb.codigo_personal,dpb.codigo_grupo

	update 
		@cabecera 
	set
		monto_detraccion_empresa_supervisor=(case when @v_limite_detraccion<monto_neto_empresa_supervisor and calcular_detraccion=1
		then ROUND(monto_neto_empresa_supervisor*@v_porcentaje_detraccion, 0) else 0 end)
	where monto_neto_empresa_supervisor>0;
	 
	update @cabecera set monto_neto_pagar_empresa_supervisor  =isnull(monto_neto_empresa_supervisor,0)-isnull(monto_detraccion_empresa_supervisor,0);

	select
		@v_codigo_planilla as codigo_planilla,
	     'CORRESPONDIENTE AL PERIODO DEL '+ @fecha_inicio+' AL '+@fecha_fin as concepto,
	    @v_porcentaje_detraccion*100 as porcentaje_detraccion,
		@fecha_inicio as fecha_inicio,
		@fecha_fin as fecha_fin,
		@nombre_canal as nombre_canal,
		@codigo_estado_planilla as codigo_estado_planilla, 
		e.nombre_largo as nombre_empresa_largo,
		--e.nombre as nombre_empresa,
		e.direccion_fiscal,
		e.ruc,
		p.nro_documento,
	    td.nombre_tipo_documento,
		-------------------------------------------------------
		codigo_supervisor,
		apellidos_nombres_supervisor,
		e.codigo_empresa,
		nombre_empresa,
		codigo_grupo,
		nombre_grupo,
		monto_bruto_empresa_supervisor,
		monto_igv_empresa_supervisor ,
		monto_neto_empresa_supervisor ,

		monto_detraccion_empresa_supervisor ,
		monto_neto_pagar_empresa_supervisor ,
		dbo.fn_GetLetrasPrecio(isnull(monto_neto_pagar_empresa_supervisor,0), 1) neto_pagar_empresa_supervisor_letra,
		codigo_canal,
		nombre_supervisor,
		email_supervisor
	from @cabecera c inner join empresa_sigeco e on c.codigo_empresa=e.codigo_empresa
	inner join personal p on c.codigo_supervisor=p.codigo_personal
	inner join tipo_documento td on td.codigo_tipo_documento=p.codigo_tipo_documento
	where monto_neto_pagar_empresa_supervisor > 0
	order by codigo_supervisor;

	SET NOCOUNT OFF
END;