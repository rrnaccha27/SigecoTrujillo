USE SIGECO
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[up_reporte_liquidacion_detalle]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].up_reporte_liquidacion_detalle
GO

CREATE PROC [dbo].[up_reporte_liquidacion_detalle]
(
	@codigo_planilla int
)
AS
BEGIN
	SET NOCOUNT ON
    declare 
		@v_limite_detraccion decimal(10,2),
		@v_codigo_tipo_planilla int,
		@v_porcentaje_detraccion decimal(10,2),
		@v_numero_planilla nvarchar(30),
		@v_codigo_estado_planilla int,
		@v_fecha_inicio varchar(10),
		@v_fecha_fin varchar(10),
		@v_tipo_reporte varchar(10),
		@v_detraccion_por_contrato bit = 0;

	select 
		@v_numero_planilla=p.numero_planilla,
		@v_codigo_estado_planilla=p.codigo_estado_planilla,
		@v_fecha_inicio=convert(varchar(10),p.fecha_inicio,103),
		@v_fecha_fin=convert(varchar(10),p.fecha_fin,103),
		@v_codigo_tipo_planilla=p.codigo_tipo_planilla,
		@v_tipo_reporte = rtp.tipo_reporte,
		@v_detraccion_por_contrato = ISNULL(rtp.detraccion_por_contrato, 0)
	from planilla p
	inner join regla_tipo_planilla rtp on rtp.codigo_regla_tipo_planilla = p.codigo_regla_tipo_planilla
	where p.codigo_planilla=@codigo_planilla;
	-----------------------------------------------

	select 
		@v_limite_detraccion=valor 
	from 
		parametro_sistema 
	where 
		codigo_parametro_sistema=16;

	select 
		@v_porcentaje_detraccion=valor 
	from 
		parametro_sistema 
	where codigo_parametro_sistema=15;

	set @v_porcentaje_detraccion=(@v_porcentaje_detraccion/100);

	declare @planilla table(
		codigo_articulo int,
		codigo_moneda int,
		nro_contrato nvarchar(200),  
		nro_cuota int,
		codigo_tipo_venta int,
		codigo_tipo_pago int,
		codigo_empresa int,  
		codigo_canal  int,
		codigo_grupo int,
		codigo_personal int,
		codigo_personal_referencial int,
		-----------------------------------
		monto_bruto decimal(10,2),
		igv decimal(10,2),
		monto_neto decimal(10,2),  
		--------------------------------------------------------------------------------------------------------------------------
		monto_bruto_personal decimal(10,2),
		igv_personal decimal(10,2),
		monto_neto_personal decimal(10,2) ,
		--------------------------------------------------------------------------------------------------------------------------
		monto_descuento decimal(10,2) ,
		monto_neto_personal_con_descuento decimal(10,2),
		monto_detraccion_personal decimal(10,2),
		monto_neto_pagar_personal decimal(10,2),
		calcular_detraccion bit
	);

	declare @descuento table(
		codigo_empresa int,
		codigo_personal int,
		monto decimal(10,2)
	);

	insert into @descuento
	select 
		d.codigo_empresa,
		d.codigo_personal,
		sum(isnull(d.monto,0))
	from descuento d 
	where d.codigo_planilla=@codigo_planilla and estado_registro=1
	group by d.codigo_empresa,d.codigo_personal
	---------------------------------------------

	insert into @planilla
	select   
		dp.codigo_articulo,
		dp.codigo_moneda,
		dp.nro_contrato,
		dp.nro_cuota,
		dp.codigo_tipo_venta,
		dp.codigo_tipo_pago,
		dp.codigo_empresa,  
		dp.codigo_canal,
		dp.codigo_grupo,
		dp.codigo_personal,
		dp.codigo_personal_referencial,
		------------------
		dp.monto_bruto,
		dp.igv,
		dp.monto_neto,
		--------------------------------------------------
		case when row_number() over(partition by dp.codigo_empresa,dp.codigo_canal,dp.codigo_grupo,dp.codigo_personal, dp.codigo_moneda, case when @v_detraccion_por_contrato = 1 then dp.nro_contrato else dp.codigo_empresa end order by dp.codigo_personal )=1    


		then 
			sum(dp.monto_bruto)over(partition by dp.codigo_empresa,dp.codigo_canal,dp.codigo_grupo,dp.codigo_personal, dp.codigo_moneda, case when @v_detraccion_por_contrato = 1 then dp.nro_contrato else dp.codigo_empresa end) 
		else null end monto_bruto_personal,
  
		case when row_number() over(partition by dp.codigo_empresa,dp.codigo_canal,dp.codigo_grupo,dp.codigo_personal, dp.codigo_moneda, case when @v_detraccion_por_contrato = 1 then dp.nro_contrato else dp.codigo_empresa end order by dp.codigo_personal )=1    


		then 
		sum(dp.igv)over(partition by dp.codigo_empresa,dp.codigo_canal,dp.codigo_grupo,dp.codigo_personal, dp.codigo_moneda, case when @v_detraccion_por_contrato = 1 then dp.nro_contrato else dp.codigo_empresa end)   
		else null end igv_personal,  

		case when row_number() over(partition by dp.codigo_empresa,dp.codigo_canal,dp.codigo_grupo,dp.codigo_personal, dp.codigo_moneda, case when @v_detraccion_por_contrato = 1 then dp.nro_contrato else dp.codigo_empresa end order by dp.codigo_personal )=1    


		then 
		sum(dp.monto_neto)over(partition by dp.codigo_empresa,dp.codigo_canal,dp.codigo_grupo,dp.codigo_personal, dp.codigo_moneda, case when @v_detraccion_por_contrato = 1 then dp.nro_contrato else dp.codigo_empresa end)
		else null end monto_neto_personal ,
		null,
		null,
		null,
		null,
		dbo.fn_canal_grupo_percibe_factura(isnull(dp.codigo_grupo,dp.codigo_canal),dp.codigo_empresa,case when @v_codigo_tipo_planilla = 1 then 0 else 1 end,1) aplicac_detraccion
 
	from  
		detalle_planilla dp  
	inner join 
		detalle_cronograma dc on dp.codigo_detalle_cronograma=dc.codigo_detalle and dc.estado_registro=1  
	where 
		dp.codigo_planilla=@codigo_planilla
		and dp.estado_registro=1  
		and dp.excluido=0
		and dc.codigo_estado_cuota in(2,3)
		and dc.es_registro_manual_comision = 0

	-----------------------------------------------------------------------------------------------------------------
	merge into @planilla  p
	using(
		select codigo_empresa ,
		codigo_personal ,
		monto from @descuento d
	 ) sc on(p.codigo_empresa=sc.codigo_empresa and p.codigo_personal=sc.codigo_personal and p.monto_bruto_personal>0)
	 when 
		matched then update 
		set p.monto_descuento=sc.monto ;
	------------------------------------------------------------------------------------------------------------------
	
	------------------------------------------------------------
	--APLICANDO DESCUENTO A PERSONAL
	------------------------------------------------------------
	update @planilla set monto_neto_personal_con_descuento=isnull(monto_neto_personal,0)-isnull(monto_descuento,0)
	where monto_bruto_personal>0 --and calcular_detraccion = 1;

	update @planilla set monto_neto_personal_con_descuento=isnull(monto_bruto_personal,0)-isnull(monto_descuento,0)
	where monto_bruto_personal>0 and calcular_detraccion = 0;

	------------------------------------------------------------
	--CALCULANDO DETRACCION A PERSONAL
	------------------------------------------------------------
	update 
		@planilla 
	set
		monto_detraccion_personal=0
	where monto_neto_personal_con_descuento>0;
	 
	------------------------------------------------------------
	--APLICANDO DETRACCION A PERSONAL
	------------------------------------------------------------
	update 
		@planilla
	set 
		monto_neto_pagar_personal=isnull(monto_neto_personal_con_descuento,0)-isnull(monto_detraccion_personal,0)
	where monto_neto_personal_con_descuento>0;

	declare @t_final as table(
		fecha_inicio varchar(10),
		numero_planilla varchar(20),
		fecha_fin varchar(10),
		codigo_tipo_planilla int,
		codigo_estado_planilla int,
		codigo_empresa int,
		nombre_empresa  varchar(20),
		nombre_empresa_largo  varchar(100),
		ruc  varchar(20),
		direccion_fiscal varchar(100),
		codigo_personal int,
		nombre_tipo_documento varchar(25),
		nombre_personal varchar(250),
		nro_documento  varchar(25),
		email_personal  varchar(50),
		codigo_personal_referencial int,
		nombre_personal_referencial  varchar(250),
		email_personal_referencial  varchar(50),
		nombre_envio_correo  varchar(250),
		apellido_envio_correo  varchar(250),
		nro_contrato  varchar(100),
		nro_cuota int,
		nombre_articulo  varchar(250),
		nombre_tipo_venta  varchar(50),
		nombre_tipo_pago varchar(50),
		igv decimal(12,2), 
		monto_bruto decimal (12,2),
		monto_neto decimal(12,2),
		igv_empresa decimal(12,2),
		bruto_empresa decimal(12,2),
		neto_empresa  decimal(12,2),
		descuento_empresa  decimal(12,2),
		canal_grupo_nombre  varchar(50),
		descuento_motivo  varchar(250),
		codigo_jardines varchar(25),
		codigo_moneda int,
		detraccion_empresa decimal(12,2),
		neto_pagar_empresa decimal(12,2),
		neto_pagar_empresa_letra  varchar(250),
		tipo_reporte  varchar(25),
		calcular_detraccion bit,
		codigo_canal int
	)

	insert into @t_final
	select
		fecha_inicio,
		numero_planilla,
		fecha_fin,
		codigo_tipo_planilla,
		codigo_estado_planilla,
		codigo_empresa,
		nombre_empresa ,
		nombre_empresa_largo,
		ruc,
		direccion_fiscal,
		codigo_personal,
		nombre_tipo_documento,
		nombre_personal,
		nro_documento,
		email_personal,
		codigo_personal_referencial,
		nombre_personal_referencial,
		email_personal_referencial,
		nombre_envio_correo,
		apellido_envio_correo,
		nro_contrato,
		nro_cuota,
		nombre_articulo,
		nombre_tipo_venta,
		nombre_tipo_pago,
		igv,
		monto_bruto,
		monto_neto, 
		igv_empresa,
		bruto_empresa,
		neto_empresa,
		descuento_empresa,
		canal_grupo_nombre,
		descuento_motivo,
		codigo_jardines,
		codigo_moneda,
		detraccion_empresa=(case when @v_limite_detraccion<neto_pagar_empresa and calcular_detraccion=1
				   then round(neto_pagar_empresa*@v_porcentaje_detraccion, 0) else 0 end), 
		case when @v_tipo_reporte = '' then
			neto_pagar_empresa - (case when @v_limite_detraccion<neto_pagar_empresa and calcular_detraccion=1
					   then round(neto_pagar_empresa*@v_porcentaje_detraccion, 0) else 0 end) 
		else
			case when @v_tipo_reporte = '_c' /*and codigo_empresa = 1*/ then
				neto_pagar_empresa - (case when @v_limite_detraccion<neto_pagar_empresa and calcular_detraccion=1
						   then round(neto_pagar_empresa*@v_porcentaje_detraccion, 0) else 0 end) 
			else
				bruto_empresa
			end
		end
		as neto_pagar_empresa,
		case when @v_tipo_reporte = '' then
			dbo.fn_GetLetrasPrecio(neto_pagar_empresa - (case when @v_limite_detraccion<neto_pagar_empresa and calcular_detraccion=1
					   then round(neto_pagar_empresa*@v_porcentaje_detraccion, 0) else 0 end), codigo_moneda) 
		else
			case when @v_tipo_reporte = '_c' /*and codigo_empresa = 1*/ then
				dbo.fn_GetLetrasPrecio(neto_pagar_empresa - (case when @v_limite_detraccion<neto_pagar_empresa and calcular_detraccion=1
						   then round(neto_pagar_empresa*@v_porcentaje_detraccion, 0) else 0 end), codigo_moneda) 
			else
				dbo.fn_GetLetrasPrecio(bruto_empresa, codigo_moneda) 
			end
		end
		as neto_pagar_empresa_letra,
		@v_tipo_reporte as tipo_reporte,
		calcular_detraccion,
		codigo_canal
	from
	(
	select 
		@v_fecha_inicio as fecha_inicio,
		@v_numero_planilla as numero_planilla,
		@v_fecha_fin as fecha_fin,
		@v_codigo_tipo_planilla as codigo_tipo_planilla,
		@v_codigo_estado_planilla as codigo_estado_planilla,
		---------------------------
		e.codigo_empresa,
		e.nombre as nombre_empresa ,
		e.nombre_largo as nombre_empresa_largo,
		e.ruc,
		e.direccion_fiscal,
		------------------------------------------------
		p.codigo_personal,
		(select top 1 nombre_tipo_documento from dbo.tipo_documento td where td.codigo_tipo_documento = case when isnull(p.nro_ruc, '') = '' then 1 else 2 end) 
		as nombre_tipo_documento,
		isnull(p.nombre,' ')+' '+isnull(p.apellido_paterno,'')+' '+isnull(p.apellido_materno,' ') as nombre_personal,
		case when isnull(p.nro_ruc, '') = '' then p.nro_documento else p.nro_ruc end as nro_documento,
		isnull(p.correo_electronico,'') as email_personal,

		pl.codigo_personal_referencial,
		isnull(s.nombre,' ')+' '+isnull(s.apellido_paterno,'')+' '+isnull(s.apellido_materno,' ') as nombre_personal_referencial,
		isnull(s.correo_electronico,'') as email_personal_referencial,

		case when @v_codigo_tipo_planilla=1 then isnull(s.nombre,' ') else isnull(p.nombre,' ') end nombre_envio_correo,
		case when @v_codigo_tipo_planilla=1 then isnull(s.apellido_paterno,'') else isnull(p.apellido_paterno,'') end apellido_envio_correo,
		-----------------------------------------------
		pl.nro_contrato,
		pl.nro_cuota,
		art.nombre as nombre_articulo,
		tv.abreviatura as nombre_tipo_venta,
		tp.nombre as nombre_tipo_pago,
		--------------------------------
		pl.igv,
		pl.monto_bruto,
		pl.monto_neto,    
		---------------------------------
		sum(isnull(pl.igv_personal,0)) over(partition by pl.codigo_empresa,pl.codigo_personal, pl.codigo_moneda, case when @v_detraccion_por_contrato = 1 then pl.nro_contrato else pl.codigo_empresa end) igv_empresa,
		sum(isnull(pl.monto_bruto_personal,0)) over(partition by pl.codigo_empresa,pl.codigo_personal, pl.codigo_moneda, case when @v_detraccion_por_contrato = 1 then pl.nro_contrato else pl.codigo_empresa end) bruto_empresa,
		sum(isnull(pl.monto_neto_personal,0)) over(partition by pl.codigo_empresa,pl.codigo_personal, pl.codigo_moneda, case when @v_detraccion_por_contrato = 1 then pl.nro_contrato else pl.codigo_empresa end) neto_empresa,

		sum(isnull(pl.monto_descuento,0)) over(partition by pl.codigo_empresa,pl.codigo_personal, pl.codigo_moneda, case when @v_detraccion_por_contrato = 1 then pl.nro_contrato else pl.codigo_empresa end) descuento_empresa,
		--detraccion_empresa=(case when @v_limite_detraccion<(sum(isnull(pl.monto_detraccion_personal,0)) over(partition by pl.codigo_empresa,pl.codigo_personal)) and calcular_detraccion=1
		--	       then round((sum(isnull(pl.monto_detraccion_personal,0)) over(partition by pl.codigo_empresa,pl.codigo_personal))*@v_porcentaje_detraccion, 0) else null end),
		sum(isnull(pl.monto_neto_pagar_personal,0)) over(partition by pl.codigo_empresa,pl.codigo_personal, pl.codigo_moneda, case when @v_detraccion_por_contrato = 1 then pl.nro_contrato else pl.codigo_empresa end) neto_pagar_empresa,

		isnull(cg.nombre,dbo.fn_obtener_canal_grupo_vigente(pl.codigo_personal)) as canal_grupo_nombre,
		isnull((select top 1 ds.motivo from dbo.descuento ds where ds.codigo_empresa = pl.codigo_empresa and ds.codigo_planilla = @codigo_planilla and ds.codigo_personal = pl.codigo_personal and ds.estado_registro = 1), '') as descuento_motivo,
		p.codigo_equivalencia as codigo_jardines,
		pl.codigo_canal as codigo_canal_grupo,
		calcular_detraccion,
		pl.codigo_moneda,
		isnull(cg.codigo_padre, cg.codigo_canal_grupo)as codigo_canal
	from @planilla  pl 
	inner join
		personal p on pl.codigo_personal=p.codigo_personal
	inner join
		personal s on pl.codigo_personal_referencial=s.codigo_personal
	--inner join
	--tipo_documento td on p.codigo_tipo_documento=td.codigo_tipo_documento
	inner join 
		articulo art on art.codigo_articulo=pl.codigo_articulo
	inner join 
		empresa_sigeco e on pl.codigo_empresa=e.codigo_empresa
	inner join 
		tipo_venta tv on pl.codigo_tipo_venta=tv.codigo_tipo_venta
	inner join 
		moneda m on m.codigo_moneda=pl.codigo_moneda
	inner join 
		tipo_pago tp on pl.codigo_tipo_pago=tp.codigo_tipo_pago
	left join 
		canal_grupo cg on cg.codigo_canal_grupo = pl.codigo_grupo
	)temp
	order by
		codigo_empresa asc, codigo_canal_grupo desc, canal_grupo_nombre asc, nombre_personal asc, nro_contrato, nombre_articulo, nro_cuota;

	--select * from @t_final where codigo_empresa = 2 and codigo_personal = 1673 
	
	if (@v_detraccion_por_contrato = 1)
	begin
		update a
		set 
			a.igv_empresa = b.igv_empresa,
			a.bruto_empresa = b.bruto_empresa,
			a.neto_empresa = b.neto_empresa,
			a.descuento_empresa = b.descuento_empresa,
			a.detraccion_empresa = b.detraccion_empresa,
			a.neto_pagar_empresa = b.neto_pagar_empresa
		from @t_final a
		inner join
		(select
			codigo_empresa, 
			codigo_personal,
			igv_empresa = sum(igv_empresa),
			bruto_empresa = sum(bruto_empresa),
			neto_empresa = sum(neto_empresa),
			descuento_empresa = sum(descuento_empresa),
			detraccion_empresa = sum(detraccion_empresa),
			neto_pagar_empresa = sum(neto_pagar_empresa)
		from @t_final
		group by 
			codigo_empresa, codigo_personal)b on a.codigo_empresa = b.codigo_empresa and b.codigo_personal = a.codigo_personal
	end

	select 
		@codigo_planilla as codigo_planilla,
		fecha_inicio,
		numero_planilla,
		fecha_fin,
		codigo_tipo_planilla,
		codigo_estado_planilla,
		codigo_empresa,
		nombre_empresa,
		nombre_empresa_largo,
		ruc,
		direccion_fiscal,
		codigo_personal,
		nombre_tipo_documento,
		nombre_personal,
		nro_documento,
		email_personal,
		codigo_personal_referencial,
		nombre_personal_referencial,
		email_personal_referencial,
		nombre_envio_correo,
		apellido_envio_correo,
		nro_contrato,
		nro_cuota,
		nombre_articulo,
		nombre_tipo_venta,
		nombre_tipo_pago,
		igv,
		monto_bruto,
		monto_neto,
		igv_empresa,
		bruto_empresa,
		neto_empresa,
		descuento_empresa,
		canal_grupo_nombre,
		descuento_motivo,
		codigo_jardines,
		codigo_moneda,
		detraccion_empresa,
		neto_pagar_empresa,
		case when @v_tipo_reporte = '' then
			dbo.fn_GetLetrasPrecio(neto_pagar_empresa, codigo_moneda) 
		else
			case when @v_tipo_reporte = '_c' /*and codigo_empresa = 1*/ then
				dbo.fn_GetLetrasPrecio(neto_pagar_empresa , codigo_moneda) 
			else
				dbo.fn_GetLetrasPrecio(bruto_empresa, codigo_moneda) 
			end
		end
		as neto_pagar_empresa_letra,
		tipo_reporte,
		codigo_canal
	from @t_final	
	order by codigo_empresa asc, canal_grupo_nombre asc, nombre_personal asc, nro_contrato, nombre_articulo, nro_cuota;

	SET NOCOUNT OFF
END;