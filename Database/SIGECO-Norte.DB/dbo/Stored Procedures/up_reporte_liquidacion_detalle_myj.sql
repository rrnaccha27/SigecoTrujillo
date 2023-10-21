CREATE PROC [dbo].[up_reporte_liquidacion_detalle_myj]
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
		@v_fecha_fin varchar(10);


	select 
		@v_numero_planilla=numero_planilla,
		@v_codigo_estado_planilla=codigo_estado_planilla,
		@v_fecha_inicio=convert(varchar(10),fecha_inicio,103),
		@v_fecha_fin=convert(varchar(10),fecha_fin,103),
		@v_codigo_tipo_planilla=codigo_tipo_planilla
	from planilla 
	where codigo_planilla=@codigo_planilla;
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
		case when row_number() over(partition by dp.codigo_empresa,dp.codigo_canal,dp.codigo_grupo,dp.codigo_personal, dp.codigo_moneda order by dp.codigo_personal )=1    
		then 
			sum(dp.monto_bruto)over(partition by dp.codigo_empresa,dp.codigo_canal,dp.codigo_grupo,dp.codigo_personal, dp.codigo_moneda) 
		else null end monto_bruto_personal,
  
		case when row_number() over(partition by dp.codigo_empresa,dp.codigo_canal,dp.codigo_grupo,dp.codigo_personal, dp.codigo_moneda order by dp.codigo_personal )=1    
		then 
		sum(dp.igv)over(partition by dp.codigo_empresa,dp.codigo_canal,dp.codigo_grupo,dp.codigo_personal, dp.codigo_moneda)   
		else null end igv_personal,  

		case when row_number() over(partition by dp.codigo_empresa,dp.codigo_canal,dp.codigo_grupo,dp.codigo_personal, dp.codigo_moneda order by dp.codigo_personal )=1    
		then 
		sum(dp.monto_neto)over(partition by dp.codigo_empresa,dp.codigo_canal,dp.codigo_grupo,dp.codigo_personal, dp.codigo_moneda)
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
		and dp.codigo_personal = 175

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
	where monto_bruto_personal>0;

	--select * from @planilla

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

	select * from @planilla

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
		neto_pagar_empresa - (case when @v_limite_detraccion<neto_pagar_empresa and calcular_detraccion=1
				   then round(neto_pagar_empresa*@v_porcentaje_detraccion, 0) else 0 end) as neto_pagar_empresa,
		dbo.fn_GetLetrasPrecio(neto_pagar_empresa - (case when @v_limite_detraccion<neto_pagar_empresa and calcular_detraccion=1
				   then round(neto_pagar_empresa*@v_porcentaje_detraccion, 0) else 0 end), codigo_moneda) neto_pagar_empresa_letra
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
		sum(isnull(pl.igv_personal,0)) over(partition by pl.codigo_empresa,pl.codigo_personal, pl.codigo_moneda) igv_empresa,
		sum(isnull(pl.monto_bruto_personal,0)) over(partition by pl.codigo_empresa,pl.codigo_personal, pl.codigo_moneda) bruto_empresa,
		sum(isnull(pl.monto_neto_personal,0)) over(partition by pl.codigo_empresa,pl.codigo_personal, pl.codigo_moneda) neto_empresa,

		sum(isnull(pl.monto_descuento,0)) over(partition by pl.codigo_empresa,pl.codigo_personal, pl.codigo_moneda) descuento_empresa,
		--detraccion_empresa=(case when @v_limite_detraccion<(sum(isnull(pl.monto_detraccion_personal,0)) over(partition by pl.codigo_empresa,pl.codigo_personal)) and calcular_detraccion=1
		--	       then round((sum(isnull(pl.monto_detraccion_personal,0)) over(partition by pl.codigo_empresa,pl.codigo_personal))*@v_porcentaje_detraccion, 0) else null end),
		sum(isnull(pl.monto_neto_pagar_personal,0)) over(partition by pl.codigo_empresa,pl.codigo_personal, pl.codigo_moneda) neto_pagar_empresa,

		dbo.fn_obtener_canal_grupo_vigente(pl.codigo_personal) as canal_grupo_nombre,
		isnull((select top 1 ds.motivo from dbo.descuento ds where ds.codigo_empresa = pl.codigo_empresa and ds.codigo_planilla = @codigo_planilla and ds.codigo_personal = pl.codigo_personal and ds.estado_registro = 1), '') as descuento_motivo,
		p.codigo_equivalencia as codigo_jardines,
		pl.codigo_canal as codigo_canal_grupo,
		calcular_detraccion,
		pl.codigo_moneda
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
	)temp
	order by
		codigo_empresa asc, codigo_canal_grupo desc, canal_grupo_nombre asc, nombre_personal asc, nro_contrato, nombre_articulo, nro_cuota;

	SET NOCOUNT OFF
END;