USE SIGECO
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[up_detalle_planilla_txt]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].up_detalle_planilla_txt
GO

CREATE PROCEDURE [dbo].[up_detalle_planilla_txt]
(
	@p_codigo_planilla int
)
AS
BEGIN

	declare 	
		@v_limite_detraccion decimal(10,2),
		@v_codigo_tipo_planilla int,
		@v_porcentaje_detraccion decimal(10,2),
		@v_numero_planilla varchar(20),
		@v_fecha_proceso varchar(10),
		@v_detraccion_por_contrato bit = 0;
	
	select   
	  @v_numero_planilla=p.numero_planilla,
	  @v_codigo_tipo_planilla=p.codigo_tipo_planilla,
	  @v_fecha_proceso=convert(varchar(10),p.fecha_apertura ,103),
	  @v_detraccion_por_contrato = ISNULL(rtp.detraccion_por_contrato, 0)
	from 
		planilla p
	inner join regla_tipo_planilla rtp on rtp.codigo_regla_tipo_planilla = p.codigo_regla_tipo_planilla
	where 
		p.codigo_planilla=@p_codigo_planilla;
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
		codigo_moneda int,	
		codigo_empresa int,  	
		codigo_personal int,	
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
		calcular_detraccion bit,
		codigo_grupo int
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
	where d.codigo_planilla=@p_codigo_planilla and estado_registro=1
	group by d.codigo_empresa,d.codigo_personal
	---------------------------------------------

	insert into @planilla
	select   
		dp.codigo_moneda,  
		dp.codigo_empresa,    
		dp.codigo_personal,  
		------------------
		dp.monto_bruto,
		dp.igv,
		dp.monto_neto,
		--------------------------------------------------
		case when row_number() over(partition by dp.codigo_empresa,dp.codigo_canal,dp.codigo_personal, dp.codigo_moneda,case when @v_detraccion_por_contrato = 1 then dp.nro_contrato else dp.codigo_empresa end  order by dp.codigo_personal )=1    
		then 
			sum(dp.monto_bruto)over(partition by dp.codigo_empresa,dp.codigo_canal,dp.codigo_personal, dp.codigo_moneda,case when @v_detraccion_por_contrato = 1 then dp.nro_contrato else dp.codigo_empresa end) 
		else null end monto_bruto_personal,
  
		case when row_number() over(partition by dp.codigo_empresa,dp.codigo_canal,dp.codigo_personal, dp.codigo_moneda, case when @v_detraccion_por_contrato = 1 then dp.nro_contrato else dp.codigo_empresa end order by dp.codigo_personal )=1    
		then 
			sum(dp.igv)over(partition by dp.codigo_empresa,dp.codigo_canal,dp.codigo_personal, dp.codigo_moneda, case when @v_detraccion_por_contrato = 1 then dp.nro_contrato else dp.codigo_empresa end)   
		else null end igv_personal,  

		case when row_number() over(partition by dp.codigo_empresa,dp.codigo_canal,dp.codigo_personal, dp.codigo_moneda, case when @v_detraccion_por_contrato = 1 then dp.nro_contrato else dp.codigo_empresa end order by dp.codigo_personal )=1    
		then 
			sum(dp.monto_neto)over(partition by dp.codigo_empresa,dp.codigo_canal,dp.codigo_personal, dp.codigo_moneda, case when @v_detraccion_por_contrato = 1 then dp.nro_contrato else dp.codigo_empresa end)
		else null end monto_neto_personal ,
		null,
		null,
		null,
		null,
		dbo.fn_canal_grupo_percibe_factura(isnull(dp.codigo_grupo,dp.codigo_canal),dp.codigo_empresa,case when p.codigo_tipo_planilla = 1 then 0 else 1 end,1) aplicac_detraccion
		,dp.codigo_grupo
	from  
		detalle_planilla dp  
	inner join 
		detalle_cronograma dc on dp.codigo_detalle_cronograma=dc.codigo_detalle and dc.estado_registro=1  
	inner join 
		planilla p on dp.codigo_planilla = p.codigo_planilla
	where 
		dp.codigo_planilla=@p_codigo_planilla
		and dp.estado_registro=1  
		and dp.excluido=0
		and dc.codigo_estado_cuota in(2,3)
		and dc.es_registro_manual_comision = 0;

	------------------------------------------
	---aplicando descuento
	-------------------------------------------
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
	update 
		@planilla 
	set 
		monto_neto_personal_con_descuento=isnull(monto_neto_personal,0)-isnull(monto_descuento,0)
	where
		monto_bruto_personal>0;

	------------------------------------------------------------
	--CALCULANDO DETRACCION A PERSONAL
	------------------------------------------------------------
	update 
		@planilla 
	set
		monto_detraccion_personal=(
			case when @v_limite_detraccion<monto_neto_personal_con_descuento and calcular_detraccion=1 then round(monto_neto_personal_con_descuento*@v_porcentaje_detraccion, 0) else null end
		)
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

	declare @resumen table(
		codigo_empresa int,
		codigo_personal int,
		codigo_moneda int,
		monto_neto_pagar_personal decimal(12,2),
		calcular_detraccion bit,
		codigo_grupo int
	);

	--select * from @planilla where codigo_personal = 1383

	insert into @resumen
	select 
		pl.codigo_empresa,
		pl.codigo_personal,
		pl.codigo_moneda,
		sum(pl.monto_neto_pagar_personal) monto_neto_pagar_personal,
		max(convert(int,pl.calcular_detraccion)) calcular_detraccion,
		max(codigo_grupo)
	from @planilla pl
	group by pl.codigo_empresa,pl.codigo_personal, pl.codigo_moneda

	DELETE FROM @resumen WHERE ISNULL(monto_neto_pagar_personal, 0) = 0

	declare
		@v_checkSum_FUNJAR_n decimal(20)
		,@v_checkSum_OFSA_n decimal(20)
		,@v_checkSum_FUNJAR varchar(20)
		,@v_checkSum_OFSA varchar(20)

	select 
		@v_checkSum_FUNJAR_n = sum(convert(decimal(20),case when len(replace(case when b.interbancario = 0 then p.nro_cuenta else p.codigo_interbancario end,'-',''))>0 then right(replace(case when b.interbancario = 0 then p.nro_cuenta else p.codigo_interbancario end,'-',''), case when tcpe.simbolo = 'A' then 11 else 10 end) else '0' end))
	from @resumen  pl 
	inner join
		 personal p on pl.codigo_personal=p.codigo_personal
	inner join
		 tipo_cuenta tcpe on p.codigo_tipo_cuenta=tcpe.codigo_tipo_cuenta
	inner join 
		banco b on b.codigo_banco = p.codigo_banco 
	where pl.codigo_empresa = 2

	select top 1
		@v_checkSum_FUNJAR_n = @v_checkSum_FUNJAR_n + convert(decimal(20),case when len(e.nro_cuenta)>0 then right(replace(e.nro_cuenta,'-',''), case when tcemp.simbolo = 'A' then 11 else 10 end) else '0' end)
	from
		empresa_sigeco e 
	inner join
		 tipo_cuenta tcemp on e.codigo_tipo_cuenta=tcemp.codigo_tipo_cuenta
	where e.codigo_empresa = 2

	select 
		@v_checkSum_OFSA_n = sum(convert(decimal(20),case when len(replace(case when b.interbancario = 0 then p.nro_cuenta else p.codigo_interbancario end,'-',''))>0 then right(replace(case when b.interbancario = 0 then p.nro_cuenta else p.codigo_interbancario end,'-',''), case when tcpe.simbolo = 'A' then 11 else 10 end) else '0' end))
	from @resumen  pl 
	inner join
		 personal p on pl.codigo_personal=p.codigo_personal
	inner join
		 tipo_cuenta tcpe on p.codigo_tipo_cuenta=tcpe.codigo_tipo_cuenta
	inner join 
		banco b on b.codigo_banco = p.codigo_banco 
	where pl.codigo_empresa = 1

	select top 1
		@v_checkSum_OFSA_n = @v_checkSum_OFSA_n + convert(decimal(20),case when len(e.nro_cuenta)>0 then right(replace(e.nro_cuenta,'-',''), case when tcemp.simbolo = 'A' then 11 else 10 end) else '0' end)
	from
		empresa_sigeco e 
	inner join
		 tipo_cuenta tcemp on e.codigo_tipo_cuenta=tcemp.codigo_tipo_cuenta
	where e.codigo_empresa = 1

	set @v_checkSum_FUNJAR = right('00000' + convert(varchar(20), @v_checkSum_FUNJAR_n), 15)
	set @v_checkSum_OFSA = right('00000' + convert(varchar(20), @v_checkSum_OFSA_n), 15)

	select 
		@p_codigo_planilla as codigo_planilla,
		@v_numero_planilla as numero_planilla,
		--m.simbolo as simbolo_moneda,
		@v_fecha_proceso as fecha_proceso,
		e.codigo_empresa,
		---------------------------------------------------------------
		mtc.simbolo as simbolo_moneda_cuenta_desembolso,
		e.nombre as nombre_empresa ,
		replace(e.nro_cuenta,'-','') as numero_cuenta_desembolso,
		isnull(tcemp.simbolo,'') as tipo_cuenta_desembolso,
		-------------------------------------------------------------------
		case when b.interbancario = 1 then
			left(replace(p.codigo_interbancario,'-',''), 20)
		else
			left(replace(p.nro_cuenta,'-',''), 14)
		end as numero_cuenta_abono,
		case when b.interbancario = 1 then
			'B'
		else
			isnull(tcpe.simbolo,'') 
		end as tipo_cuenta_abono,
		mca.simbolo as simbolo_moneda_cuenta_abono,
		td.nombre_tipo_documento,
		case when p.es_persona_juridica = 1 then
			left(p.nro_ruc, 12)
		else
			left(p.nro_documento, 12)
		end as nro_documento,
		replace(replace(isnull(p.nombre,'')+isnull(' '+p.apellido_paterno,'')+isnull(' '+p.apellido_materno,''), '.', ''), 'ñ', 'N') as nombre_personal,
		p.codigo_personal,  
		------------------------------------------------------------------
		isnull(pl.monto_neto_pagar_personal,0)  importe_abono_personal,
		sum(isnull(pl.monto_neto_pagar_personal,0)) over(partition by pl.codigo_empresa, pl.codigo_moneda) importe_desembolso_empresa,
		calcular_detraccion,
		[checksum] = case when pl.codigo_empresa = 1 then @v_checkSum_OFSA else @v_checkSum_FUNJAR end,
		e.nombre + '_' +  mpl.codigo_equivalencia as codigo_agrupador
		,pl.codigo_grupo
		,0 as validado
	from @resumen  pl 
	inner join
		personal p on pl.codigo_personal=p.codigo_personal
	inner join
		tipo_cuenta tcpe on p.codigo_tipo_cuenta=tcpe.codigo_tipo_cuenta
	inner join 
		banco b on b.codigo_banco = p.codigo_banco
	inner join
		tipo_documento td on p.codigo_tipo_documento=td.codigo_tipo_documento
	inner join   
		empresa_sigeco e on pl.codigo_empresa=e.codigo_empresa
	inner join
		tipo_cuenta tcemp on e.codigo_tipo_cuenta=tcemp.codigo_tipo_cuenta
	inner join 	 
		moneda mca on mca.codigo_moneda=p.codigo_cuenta_moneda
	left join 
		moneda mtc on mtc.codigo_moneda=e.codigo_cuenta_moneda
	left join moneda mpl
		on mpl.codigo_moneda = pl.codigo_moneda
	where 
		isnull(pl.monto_neto_pagar_personal,0) > 0
	order by 
		nombre_personal;

END;