USE SIGECO
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[up_planilla_insertar]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].up_planilla_insertar
GO

CREATE PROCEDURE [dbo].[up_planilla_insertar]
(
	@codigo_regla_tipo_planilla int,
	@fecha_inicio datetime,
	@fecha_fin datetime,
	@usuario_registra varchar(30),
	@codigo_planilla int out,
	@total_registro_procesado int out
)
AS
BEGIN

	DECLARE 
		@n int,
		@i int,
		@v_existe_planilla int,
		@codigo_tipo_planilla int,
		@numero_planilla varchar(50),
		@cantidad_registro_procesar int,
		@fecha_apertura datetime,
		@codigo_estado_cuota int,
		@codigo_estado_planilla int,
		@cantidad_planilla_mes int,
		@v_afecto_doc_completa bit,
		@v_meses_retroactivos int,
		@c_codigo_tipo_bloqueo_comision int,
		@c_VALIDADO	BIT;

	set @fecha_apertura=GETDATE();
	set @codigo_estado_planilla=1;
	set @codigo_estado_cuota=2;--INDICA QUE LA CUOTA SE ENCUENTRA EN PROCESO DE PAGO
	set @v_meses_retroactivos = (0 - isnull((select top 1 convert(int, valor) from parametro_sistema where codigo_parametro_sistema = 29), 6))
	set @c_codigo_tipo_bloqueo_comision = 1 /* COMISION */
	set @c_VALIDADO = 1 /* VALIDADO */

	select @numero_planilla=cast(YEAR(GETDATE()) AS VARCHAR)+'-'+REPLACE(STR(MONTH(GETDATE()), 2), SPACE(1), '0') 

	select 
		@cantidad_planilla_mes=isnull(count(1),0)+1 
	from 
		planilla
	where 
		year(fecha_registra)=YEAR(GETDATE()) 
		and month(fecha_registra)=month(GETDATE());

	set @numero_planilla=@numero_planilla+'-'+CAST(@cantidad_planilla_mes as varchar);

	/**********************************************
	 obteniendo tipo planilla
	***********************************************/
	select 
		@codigo_tipo_planilla=codigo_tipo_planilla
		,@v_afecto_doc_completa = afecto_doc_completa
	from 
		regla_tipo_planilla 
	where 
		codigo_regla_tipo_planilla=@codigo_regla_tipo_planilla;
	/*************************************************/
	select 
	   @v_existe_planilla=count(*) 
	from 
		planilla 
	where 
		codigo_tipo_planilla=@codigo_tipo_planilla 
		and fecha_inicio>=@fecha_inicio 
		and fecha_fin<=@fecha_fin 
		and codigo_estado_planilla in(1,2) ;
	/*
	if(@v_existe_planilla>0)
	begin
		RAISERROR('En el rango de fecha ingresado ya se encuentra una planilla generada.',16,1); 
		return;
	end;
	*/
	 
	--EXEC up_habilitar_detalle_cronograma @codigo_tipo_planilla, @fecha_fin
	/*******************************************************/
	declare @table table(
		nro int,
		codigo_canal int,
		codigo_empresa varchar(500),
		codigo_tipo_venta varchar(500),
		codigo_campo_santo varchar(500)
	);
	insert into @table
	select  
		row_number() over(order by codigo_detalle_regla_tipo_planilla ) nro,
		codigo_canal,
		codigo_empresa,
		codigo_tipo_venta,
		codigo_campo_santo
	from detalle_regla_tipo_planilla 
	where 
		codigo_regla_tipo_planilla=@codigo_regla_tipo_planilla 
		and estado_registro=1;

	select @n=max(nro) from @table;
	set @i=1;
	/******************************************/
	declare 
		@v_codigo_canal int,
		@v_codigo_empresa varchar(500),
		@v_codigo_tipo_venta varchar(500),
		@v_codigo_campo_santo varchar(500);
	/******************************************/
	declare @detalle_planilla table(
		 codigo_cronograma int,
		 codigo_empresa int,
		 codigo_moneda int,
		 codigo_tipo_pago int,
		 codigo_tipo_venta int,
		 codigo_tipo_planilla int,
		 --------------------------
		 codigo_articulo int,
		 codigo_campo_santo int,
		 ---------------------
		 codigo_canal int,
		 codigo_canal_grupo int,
		 codigo_personal int,
		 codigo_personal_referencial int,
		 ----------------------------
		 nro_contrato nvarchar(200),
		 codigo_detalle int,
		 nro_cuota int,
		 fecha_programada datetime,
		 monto_bruto decimal(10,2),
		 igv decimal(10,2),
		 monto_neto decimal(10,2),
		 es_emitido bit not null default 0,
		 es_transferencia bit default 0
	);

	insert into @detalle_planilla
	select distinct
		 cpc.codigo_cronograma,
		 cpc.codigo_empresa,
		 cpc.codigo_moneda,
		 cpc.codigo_tipo_pago,
		 cpc.codigo_tipo_venta,
		 cpc.codigo_tipo_planilla,
		 --------------------------
		 ac.codigo_articulo,
		 ac.codigo_campo_santo,
		 ---------------------
		 pcg.codigo_canal,
		 pcg.codigo_canal_grupo,
	 
		 pcg.codigo_personal,
		 dbo.fn_obtener_personal_supervisor(cpc.nro_contrato,ee.codigo_equivalencia,@codigo_tipo_planilla), 
		 cpc.nro_contrato,
		 ----------------------------
		 dc.codigo_detalle,
		 dc.nro_cuota,
		 dc.fecha_programada,
		 dc.monto_bruto,
		 dc.igv,
		 dc.monto_neto,
		 0,
		 dc.es_transferencia
	from 
		cronograma_pago_comision cpc
	inner join empresa_sigeco ee on ee.codigo_empresa=cpc.codigo_empresa
	inner join personal_canal_grupo pcg on cpc.codigo_personal_canal_grupo=pcg.codigo_registro
	inner join personal p on pcg.codigo_personal = p.codigo_personal
	inner join articulo_cronograma ac on ac.codigo_cronograma=cpc.codigo_cronograma
	inner join detalle_cronograma dc on (ac.codigo_cronograma=dc.codigo_cronograma and ac.codigo_articulo=dc.codigo_articulo)	
	where 
		cpc.codigo_tipo_planilla=@codigo_tipo_planilla 
		and ( (@v_afecto_doc_completa = 0) OR (@v_afecto_doc_completa = 1 and dbo.fn_validar_contrato_documentacion_completa(ee.codigo_equivalencia, cpc.nro_contrato) = 1) )
		and cpc.estado_registro=1 
		and ac.estado_registro=1 
		and dc.estado_registro=1 
		and dc.codigo_estado_cuota=1
		and dc.fecha_programada  between DATEADD(month, @v_meses_retroactivos, @fecha_fin) and @fecha_fin
		and dc.monto_neto > 0
		and dbo.fn_validar_contrato_estado(ee.codigo_equivalencia, cpc.nro_contrato) = 1;

	insert into planilla(
		numero_planilla,
		fecha_inicio,
		fecha_fin,
		fecha_apertura,
		usuario_apertura,
		codigo_regla_tipo_planilla,
		codigo_tipo_planilla,
		codigo_estado_planilla,
		estado_registro,
		fecha_registra,
		usuario_registra
	)
	values(
		@numero_planilla,
		@fecha_inicio,
		@fecha_fin,
		@fecha_apertura,
		@usuario_registra,
		@codigo_regla_tipo_planilla,
		@codigo_tipo_planilla,
		@codigo_estado_planilla,
		1,
		GETDATE(),
		@usuario_registra
	);

	set @codigo_planilla=@@IDENTITY;
	/*************************************************************************/

	while @i<=@n
	begin
		select 
			@v_codigo_canal=codigo_canal,
			@v_codigo_empresa=codigo_empresa,
			@v_codigo_tipo_venta=codigo_tipo_venta,
			@v_codigo_campo_santo=codigo_campo_santo
		from @table where nro=@i;

		insert into detalle_planilla(
			codigo_planilla,
			codigo_cronograma,
			codigo_moneda,
			codigo_canal,
			codigo_grupo,
			codigo_empresa,
			codigo_personal,
			codigo_personal_referencial,
			codigo_articulo,
			nro_contrato,
			codigo_tipo_venta,
			codigo_tipo_pago,

			codigo_detalle_cronograma,
			excluido,
			nro_cuota,
			fecha_pago,
			monto_bruto,
			igv,
			monto_neto,
			estado_registro,
			fecha_registra,
			usuario_registra,
			es_transferencia
		)
		select 
			@codigo_planilla,
			dp.codigo_cronograma,
			dp.codigo_moneda,
			dp.codigo_canal,
			dp.codigo_canal_grupo,
			dp.codigo_empresa,
			dp.codigo_personal,
			dp.codigo_personal_referencial,
			dp.codigo_articulo,
			dp.nro_contrato,
			dp.codigo_tipo_venta,
			dp.codigo_tipo_pago,
			dp.codigo_detalle,
			0 as excluido,
			dp.nro_cuota,
			dp.fecha_programada,
			dp.monto_bruto,
			dp.igv,
			dp.monto_neto,
			1 as estado_registro,
			GETDATE() as fecha_registra,
			@usuario_registra,
			dp.es_transferencia
		from 
			@detalle_planilla dp 
		inner join dbo.fn_SplitReglaTipoPlanilla(@v_codigo_empresa) ee on ee.codigo=dp.codigo_empresa
		inner join dbo.fn_SplitReglaTipoPlanilla(@v_codigo_tipo_venta) tv on tv.codigo=dp.codigo_tipo_venta
		inner join dbo.fn_SplitReglaTipoPlanilla(@v_codigo_campo_santo) cs on cs.codigo=dp.codigo_campo_santo
		where 
			dp.codigo_canal=@v_codigo_canal and dp.es_emitido=0;
		----------------------------------------------
		update dc
		set 
			dc.es_emitido =1
		from 
			detalle_planilla as dp
		inner join @detalle_planilla dc on dp.codigo_detalle_cronograma=dc.codigo_detalle
		where 
			dp.codigo_planilla= @codigo_planilla 
			and dc.es_emitido=0;

		set @i=@i+1;
	end;
	
	/****************************************************************************/
	select @cantidad_registro_procesar=count(*) from detalle_planilla where codigo_planilla= @codigo_planilla;
	if(@cantidad_registro_procesar<=0)
	begin
		RAISERROR('Para el rango de fechas establecido no existe pagos habilitados.',16,1); 
		return;
	end;
	/*************************************************************************************/
	
	set @total_registro_procesado=@cantidad_registro_procesar;

	/******************************************
	ACTUALIZANDO CUOTAS CON ESTADO EN PROCESO DE PAGO 
	*******************************************/
	update dc
	set 
		dc.codigo_estado_cuota = @codigo_estado_cuota ,
		dc.fecha_programada=isnull(dc.fecha_programada,dp.fecha_pago)
	from 
		detalle_planilla as dp
	inner join detalle_cronograma as dc 
		on dp.codigo_detalle_cronograma = dc.codigo_detalle 
	where 
		dp.codigo_planilla= @codigo_planilla ;

	/******************************************
	DESACTIVANDO REGISTROS ANTIGUOS DE OPERACION
	*******************************************/
	update occ
	set 
		occ.estado_registro =0
	from 
		detalle_planilla as dp
	inner join detalle_cronograma dc on dp.codigo_detalle_cronograma=dc.codigo_detalle
	inner join operacion_cuota_comision as occ on dp.codigo_detalle_cronograma = occ.codigo_detalle_cronograma 
	where 
		dp.codigo_planilla= @codigo_planilla 
		and occ.estado_registro=1;
	
	/******************************************
	INSERTANDO REGISTROS NUEVOS DE OPERACION
	*******************************************/
	insert into operacion_cuota_comision(
		codigo_detalle_cronograma,
		codigo_tipo_operacion_cuota,
		motivo_movimiento,
		fecha_movimiento,
		estado_registro,
		usuario_registra,
		fecha_registra
	)
	select 
		codigo_detalle_cronograma,
		2, 	/*registro*/
		'POR GENERACION DE PLANILLA ' + @numero_planilla,
		getdate(),
		1,
		@usuario_registra,
		getdate()
	from 
		detalle_planilla 
	where
		codigo_planilla= @codigo_planilla;

	-----------------------------------------------------------------------------------------
	--  SECCIÓN REGISTRO MANUAL DE COMISIONES
	-----------------------------------------------------------------------------------------
	merge into comision_manual cm
		using(
		select 
			dc.codigo_detalle 
		from 
		detalle_planilla dp 
			inner join detalle_cronograma dc on dp.codigo_detalle_cronograma=dc.codigo_detalle
			where 
			dp.codigo_planilla=@codigo_planilla and dc.es_registro_manual_comision=1 and dc.estado_registro=1 
		) sc on (cm.codigo_detalle_cronograma=sc.codigo_detalle and cm.en_planilla=0 )
	when matched then 
		update set cm.codigo_planilla=@codigo_planilla,cm.en_planilla=1;

	/*
	BLOQUEO DE VENDEDORES
	*/
	EXEC DBO.up_personal_bloqueo_registrar @codigo_planilla, @c_codigo_tipo_bloqueo_comision, @usuario_registra

END;