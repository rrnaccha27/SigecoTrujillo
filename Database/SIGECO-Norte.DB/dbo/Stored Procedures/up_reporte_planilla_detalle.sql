CREATE PROCEDURE [dbo].[up_reporte_planilla_detalle]
(
	@codigo_planilla	int
	,@p_codigo_personal	int
)
AS
BEGIN

	SET NOCOUNT ON

	declare 
		@v_limite_detraccion decimal(10,2),
		@v_codigo_tipo_planilla int,
		@v_nombre_tipo_planilla nvarchar(60),
		@v_porcentaje_detraccion decimal(10,2),
		@v_numero_planilla nvarchar(30),
		@v_codigo_estado_planilla int,
		@v_fecha_inicio varchar(10),
		@v_fecha_fin varchar(10),
		@v_tipo_reporte varchar(10),
		@v_detraccion_por_contrato bit = 0;

	----------------------------------------------
	select 
		@v_numero_planilla=pl.numero_planilla,
		@v_codigo_estado_planilla=pl.codigo_estado_planilla,
		@v_fecha_inicio=convert(varchar(10),pl.fecha_inicio,103),
		@v_fecha_fin=convert(varchar(10),pl.fecha_fin,103),
		@v_codigo_tipo_planilla=pl.codigo_tipo_planilla,
		@v_nombre_tipo_planilla=rtp.nombre,
		@v_tipo_reporte = rtp.tipo_reporte,
		@v_detraccion_por_contrato = isnull(rtp.detraccion_por_contrato, 0)
	from 
		planilla pl 
	inner join regla_tipo_planilla rtp 
		on pl.codigo_regla_tipo_planilla=rtp.codigo_regla_tipo_planilla
	where 
		pl.codigo_planilla=@codigo_planilla;
	-----------------------------------------------
	--print @v_codigo_tipo_planilla;
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
	where 
		codigo_parametro_sistema=15;

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
		monto_bruto decimal(10,2),
		-------------------------------------------------------------------------------------------------------------------------
		igv decimal(10,2),
		monto_neto decimal(10,2),  
		monto_bruto_empresa decimal(10,2),
		igv_empresa  decimal(10,2),
		monto_neto_empresa  decimal(10,2),
		--------------------------------------------------------------------------------------------------------------------------
		monto_bruto_canal  decimal(10,2),
		igv_canal  decimal(10,2),
		monto_neto_canal  decimal(10,2),
		--------------------------------------------------------------------------------------------------------------------------
		monto_bruto_grupo  decimal(10,2),
		igv_grupo  decimal(10,2),
		monto_neto_grupo  decimal(10,2),
		--------------------------------------------------------------------------------------------------------------------------
		monto_bruto_personal decimal(10,2),
		igv_personal decimal(10,2),
		monto_neto_personal decimal(10,2) ,
		monto_descuento decimal(10,2) ,
		monto_neto_personal_con_descuento decimal(10,2),
		monto_detraccion_personal decimal(10,2),
		monto_neto_pagar_personal decimal(10,2),
		calcular_detraccion bit,
		es_comision_manual bit default(0),
		fecha_contrato	varchar(10),
		usuario_cm varchar(50)
	);

	declare @descuento table(
		codigo_empresa int,
		codigo_personal int,
		monto decimal(10,2)
	);

	declare @t_detraccion table
	(
		codigo_empresa int, 
		codigo_canal int, 
		codigo_personal int, 
		codigo_moneda int, 
		calcular_detraccion bit, 
		monto_neto_personal decimal(10, 2),
		nro_contrato varchar(100)
	);

	insert into @descuento
	select 
		d.codigo_empresa,
		d.codigo_personal,
		sum(isnull(d.monto,0))
	from 
		descuento d 
	where 
		d.codigo_planilla=@codigo_planilla and estado_registro=1
	group by 
		d.codigo_empresa,d.codigo_personal


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
		dp.monto_bruto,
		dp.igv,
		dp.monto_neto,
		case when row_number() over(partition by dp.codigo_empresa, dp.codigo_moneda order by dp.codigo_empresa )=1    
		then 
			sum(dp.monto_bruto)over(partition by dp.codigo_empresa, dp.codigo_moneda) 
		else null end monto_bruto_empresa,

		case when row_number() over(partition by dp.codigo_empresa, dp.codigo_moneda order by dp.codigo_empresa )=1    
		then 
			sum(dp.igv)over(partition by dp.codigo_empresa, dp.codigo_moneda)   
		else null end igv_empresa,

		case when row_number() over(partition by dp.codigo_empresa, dp.codigo_moneda order by dp.codigo_empresa )=1    
		then 
			sum(dp.monto_neto)over(partition by dp.codigo_empresa, dp.codigo_moneda)
		else null end monto_neto_empresa,
		--------------------------------------------------------------------------------------------------------------------------
		case when row_number() over(partition by dp.codigo_empresa,dp.codigo_canal, dp.codigo_moneda order by dp.codigo_canal )=1    
		then 
			sum(dp.monto_bruto)over(partition by dp.codigo_empresa,dp.codigo_canal, dp.codigo_moneda) 
		else null end monto_bruto_canal,
  
		case when row_number() over(partition by dp.codigo_empresa,dp.codigo_canal, dp.codigo_moneda order by dp.codigo_canal )=1    
		then 
			sum(dp.igv)over(partition by dp.codigo_empresa,dp.codigo_canal, dp.codigo_moneda)   
		else null end igv_canal,

		case when row_number() over(partition by dp.codigo_empresa,dp.codigo_canal, dp.codigo_moneda order by dp.codigo_canal )=1    
		then 
			sum(dp.monto_neto)over(partition by dp.codigo_empresa,dp.codigo_canal, dp.codigo_moneda)
		else null end monto_neto_canal,
		--------------------------------------------------------------------------------------------------------------------------
		case when row_number() over(partition by dp.codigo_empresa,dp.codigo_canal,dp.codigo_grupo, dp.codigo_moneda order by dp.codigo_grupo )=1    
		then 
			sum(dp.monto_bruto)over(partition by dp.codigo_empresa,dp.codigo_canal,dp.codigo_grupo, dp.codigo_moneda) 
		else null end monto_bruto_grupo,
  
		case when row_number() over(partition by dp.codigo_empresa,dp.codigo_canal,dp.codigo_grupo, dp.codigo_moneda order by dp.codigo_grupo )=1    
		then 
			sum(dp.igv)over(partition by dp.codigo_empresa,dp.codigo_canal,dp.codigo_grupo, dp.codigo_moneda)   
		else null end igv_grupo,

		case when row_number() over(partition by dp.codigo_empresa,dp.codigo_canal,dp.codigo_grupo, dp.codigo_moneda order by dp.codigo_grupo )=1    
		then 
			sum(dp.monto_neto)over(partition by dp.codigo_empresa,dp.codigo_canal,dp.codigo_grupo, dp.codigo_moneda)
		else null end monto_neto_grupo,
		--------------------------------------------------------------------------------------------------------------------------
		case when row_number() over(partition by dp.codigo_empresa,dp.codigo_canal,dp.codigo_grupo, dp.codigo_personal, dp.codigo_moneda, case when @v_detraccion_por_contrato = 1 then dp.nro_contrato else dp.codigo_empresa end order by  dp.codigo_personal  )=1 

   
		then 
			sum(dp.monto_bruto)over(partition by dp.codigo_empresa,dp.codigo_canal,dp.codigo_grupo, dp.codigo_personal, dp.codigo_moneda, case when @v_detraccion_por_contrato = 1 then dp.nro_contrato else dp.codigo_empresa end  ) 
		else null end monto_bruto_personal,
  
		case when row_number() over(partition by dp.codigo_empresa,dp.codigo_canal,dp.codigo_grupo, dp.codigo_personal, dp.codigo_moneda, case when @v_detraccion_por_contrato = 1 then dp.nro_contrato else dp.codigo_empresa end order by  dp.codigo_personal  )=1 

   
		then 
			sum(dp.igv)over(partition by dp.codigo_empresa,dp.codigo_canal,dp.codigo_grupo, dp.codigo_personal, dp.codigo_moneda, case when @v_detraccion_por_contrato = 1 then dp.nro_contrato else dp.codigo_empresa end )   
		else null end igv_personal,

		case when row_number() over(partition by dp.codigo_empresa,dp.codigo_canal,dp.codigo_grupo, dp.codigo_personal, dp.codigo_moneda, case when @v_detraccion_por_contrato = 1 then dp.nro_contrato else dp.codigo_empresa end order by  dp.codigo_personal  )=1 

   
		then 
			sum(dp.monto_neto)over(partition by dp.codigo_empresa,dp.codigo_canal,dp.codigo_grupo, dp.codigo_personal, dp.codigo_moneda, case when @v_detraccion_por_contrato = 1 then dp.nro_contrato else dp.codigo_empresa end )
		else null end monto_neto_personal ,
		null,
		null,
		null,
		null,
		dbo.fn_canal_grupo_percibe_factura(isnull(dp.codigo_grupo,dp.codigo_canal),dp.codigo_empresa,case when @v_codigo_tipo_planilla = 1 then 0 else 1 end,1) aplicac_detraccion,
		dc.es_registro_manual_comision,
		null as fecha_contrato,
		ISNULL(LOWER(cm.usuario_registra), '') as usuario_cm
	from  
		detalle_planilla dp  
	inner join detalle_cronograma dc 
		on dp.codigo_detalle_cronograma=dc.codigo_detalle and dc.estado_registro=1
	left join comision_manual cm
		on dc.es_registro_manual_comision = 1 and cm.codigo_detalle_cronograma = dc.codigo_detalle
	where 
		dp.codigo_planilla=@codigo_planilla 
		and ( (@p_codigo_personal = 0) or (@p_codigo_personal <> 0 and dp.codigo_personal = @p_codigo_personal) )
		and dp.estado_registro=1  
		and dp.excluido=0
		and dc.codigo_estado_cuota in(2,3)
	order by 
		codigo_empresa,codigo_canal,codigo_grupo asc;


	------------------------------------------------------------
	--OBTENIENDO LA FECHA DEL CONTRATO
	------------------------------------------------------------
	merge into @planilla  p
	using(
		select 
			convert(varchar, cc.CreateDate, 103) as fecha_contrato
			,e.codigo_empresa
			,cc.numatcard as nro_contrato
		from 
			cabecera_contrato cc 
		inner join empresa_sigeco e 
			on e.codigo_equivalencia = cc.Codigo_empresa
	) sc on(p.codigo_empresa=sc.codigo_empresa and p.nro_contrato = sc.nro_contrato)
	when matched then 
		update 
			set 
				p.fecha_contrato=sc.fecha_contrato;

	------------------------------------------
	---DESCUENTO PLANILLA DE TIPO VENDEDOR
	-------------------------------------------
	merge into @planilla  p
	using(
		select codigo_empresa ,
		codigo_personal ,
		monto from @descuento d
	) sc on(p.codigo_empresa=sc.codigo_empresa and p.codigo_personal=sc.codigo_personal and p.monto_bruto_personal>0)
	when matched then 
		update 
			set 
				p.monto_descuento=sc.monto ;

	------------------------------------------------------------
	--APLICANDO DESCUENTO A PERSONAL
	------------------------------------------------------------
	update @planilla set monto_neto_personal_con_descuento=isnull(monto_neto_personal,0)-isnull(monto_descuento,0)
	where monto_bruto_personal>0 and calcular_detraccion = 1;

	update @planilla set monto_bruto_personal=isnull(monto_bruto_personal,0)-isnull(monto_descuento,0)
	where monto_bruto_personal>0 and calcular_detraccion = 0;

	------------------------------------------------------------
	--CALCULANDO DETRACCION A PERSONAL
	------------------------------------------------------------
	insert into @t_detraccion
	select codigo_empresa, codigo_canal, codigo_personal, codigo_moneda, calcular_detraccion, sum(monto_neto_personal), case when @v_detraccion_por_contrato = 1 then nro_contrato else codigo_empresa end from @planilla
	group by codigo_empresa, codigo_canal, codigo_personal, codigo_moneda, calcular_detraccion, case when @v_detraccion_por_contrato = 1 then nro_contrato else codigo_empresa end

	update @t_detraccion
	set calcular_detraccion = case when @v_limite_detraccion < monto_neto_personal and calcular_detraccion = 1 then 1 else 0 end 
	
	update @t_detraccion set nro_contrato = right('0000000000' + nro_contrato, 10)
		
	update @planilla 
		set calcular_detraccion = d.calcular_detraccion
	from @planilla p
	left join @t_detraccion d
		on d.codigo_empresa = p.codigo_empresa and d.codigo_canal = p.codigo_canal and d.codigo_personal = p.codigo_personal and d.codigo_moneda = p.codigo_moneda
		and d.nro_contrato = case when @v_detraccion_por_contrato = 1 then p.nro_contrato else d.nro_contrato end

	update 
		@planilla 
	set
		monto_detraccion_personal=(case when calcular_detraccion=1
			then round(monto_neto_personal_con_descuento*@v_porcentaje_detraccion, 0) else null end)
	where 
		monto_neto_personal_con_descuento>0;

	------------------------------------------------------------
	--APLICANDO DETRACCION A PERSONAL
	------------------------------------------------------------
	update 
		@planilla
	set 
		monto_neto_pagar_personal=isnull(monto_neto_personal_con_descuento,0)-isnull(monto_detraccion_personal,0)
	where 
		monto_neto_personal_con_descuento>0;

	select 
		@codigo_planilla as codigo_planilla,
		@v_codigo_tipo_planilla as codigo_tipo_planilla,
		@v_nombre_tipo_planilla as nombre_tipo_planilla,
		@v_numero_planilla as numero_planilla,
		@v_codigo_estado_planilla as codigo_estado_planilla,
		@v_fecha_inicio as fecha_inicio,
		@v_fecha_fin as fecha_fin,
	
		m.codigo_moneda,
		m.nombre as moneda,
		art.nombre as articulo,
		pl.nro_contrato,  
		pl.nro_cuota ,
	
		tv.abreviatura as tipo_venta,	
		tp.nombre as tipo_pago,
		pl.codigo_empresa ,  
		e.nombre as empresa,	
		--e.ruc,
		pl.codigo_canal  ,
		c.nombre as canal,
		pl.codigo_grupo ,
		g.nombre as grupo,
		pl.codigo_personal,		
		isnull(p.nombre,'') +' '+isnull(p.apellido_paterno,'')+' '+isnull(p.apellido_materno,'') as personal,
		isnull(s.nombre,'') +' '+isnull(s.apellido_paterno,'')+' '+isnull(s.apellido_materno,'') as personal_referencial,

		pl.monto_bruto ,	
		pl.igv,
		pl.monto_neto,  
		--------------------------------------------------------------------------------------------------------------------------
		isnull(pl.monto_bruto_empresa,0) monto_bruto_empresa,
		isnull(pl.igv_empresa,0) igv_empresa,
		isnull(pl.monto_neto_empresa,0) monto_neto_empresa,
		--------------------------------------------------------------------------------------------------------------------------
		isnull(pl.monto_bruto_canal,0) monto_bruto_canal,
		isnull(pl.igv_canal,0) igv_canal,
		isnull(pl.monto_neto_canal,0) monto_neto_canal,
		--------------------------------------------------------------------------------------------------------------------------
		isnull(pl.monto_bruto_grupo,0) monto_bruto_grupo,
		isnull(pl.igv_grupo,0) igv_grupo,
		isnull(pl.monto_neto_grupo,0) monto_neto_grupo,
		--------------------------------------------------------------------------------------------------------------------------
		isnull(pl.monto_bruto_personal,0) monto_bruto_personal,
		isnull(pl.igv_personal,0) igv_personal,
		isnull(pl.monto_neto_personal,0) monto_neto_personal,
		isnull(pl.monto_descuento,0) monto_descuento,

		isnull(pl.monto_neto_personal_con_descuento,0) monto_neto_personal_con_descuento,
		isnull(pl.monto_detraccion_personal,0) monto_detraccion_personal,
		isnull(pl.monto_neto_pagar_personal,0) monto_neto_pagar_personal,
		es_comision_manual,
		fecha_contrato,
		usuario_cm,
		@v_tipo_reporte as tipo_reporte
	from 
		@planilla pl 
	inner join
		 personal p on pl.codigo_personal=p.codigo_personal
	inner join
		 personal s on pl.codigo_personal_referencial=s.codigo_personal
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
	inner join 
		canal_grupo c on c.codigo_canal_grupo=pl.codigo_canal
	left join 
		canal_grupo g on g.codigo_canal_grupo=pl.codigo_grupo
	order by 
		pl.codigo_empresa,pl.nro_contrato,art.nombre,pl.nro_cuota;

	SET NOCOUNT OFF
END;