CREATE PROCEDURE [dbo].[up_planilla_insertar_inclusion_myj](
	@p_usuario_registra				varchar(30),
	@p_nro_contrato					varchar(100),
	@p_codigo_planilla				int,
	@p_tipo_ejecucion				bit,-- 0 listar, 1 guardar
	@p_total_registro_procesado		int out
)
AS
BEGIN

	DECLARE 
		@n int,
		@i int,
		--@v_existe_planilla int,
		@codigo_tipo_planilla int,
		--@numero_planilla varchar(50),
		@cantidad_registro_procesar int,
		--@fecha_apertura date,
		@codigo_estado_cuota int,
		--@codigo_estado_planilla int,
		@cantidad_planilla_mes int;

	set @codigo_estado_cuota=2;--INDICA QUE LA CUOTA SE ENCUENTRA EN PROCESO DE PAGO

	DECLARE
		@p_codigo_regla_tipo_planilla		int
		,@p_fecha_fin						datetime
		,@v_maximo_codigo_detalle_actual	int
		,@v_maximo_codigo_detalle_nuevo		int
		,@v_codigo_estado_cuota_pagada		int = 3--PAGADA
 
	SELECT TOP 1 
		@p_codigo_regla_tipo_planilla = codigo_regla_tipo_planilla
		,@p_fecha_fin = fecha_fin
	FROM dbo.planilla
	WHERE
		codigo_planilla = @p_codigo_planilla

	/**********************************************
	 obteniendo tipo planilla
	***********************************************/
	select top 1
	  @codigo_tipo_planilla=codigo_tipo_planilla 
	from regla_tipo_planilla where codigo_regla_tipo_planilla=@p_codigo_regla_tipo_planilla;
	/*************************************************/

	--TODO: VALIDAR SI SE REQUIRA ESTO, PORQUE SI SE INCLUYE PUDIESE HABERSE ACTUALIZADO EL CONTRATO
	--EXEC up_habilitar_detalle_cronograma @codigo_tipo_planilla, @p_fecha_fin

	/*******************************************************/
	declare @table table(
	nro int,
	codigo_canal int,
	codigo_empresa nvarchar(500),
	codigo_tipo_venta nvarchar(500),
	codigo_campo_santo nvarchar(500)
	);
	insert into @table
	select  
		row_number() over(order by codigo_detalle_regla_tipo_planilla ) nro,
		codigo_canal,
		codigo_empresa,
		codigo_tipo_venta,
		codigo_campo_santo
	from detalle_regla_tipo_planilla 
	where codigo_regla_tipo_planilla=@p_codigo_regla_tipo_planilla and
		  estado_registro=1;

	select @n=max(nro) from @table;
	set @i=1;
	/******************************************/
		declare 
			@v_codigo_canal int,
			@v_codigo_empresa nvarchar(500),
			@v_codigo_tipo_venta nvarchar(500),
			@v_codigo_campo_santo nvarchar(500);
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
		 es_emitido bit not null default 0
	 
	);

	DECLARE @t_listar TABLE
	(
		codigo_detalle	INT
	)

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
		 0
	from cronograma_pago_comision cpc
		 inner join empresa_sigeco ee on ee.codigo_empresa=cpc.codigo_empresa
		 inner join personal_canal_grupo pcg on cpc.codigo_personal_canal_grupo=pcg.codigo_registro	 
		 inner join articulo_cronograma ac on ac.codigo_cronograma=cpc.codigo_cronograma
		 inner join detalle_cronograma dc on (ac.codigo_cronograma=dc.codigo_cronograma and ac.codigo_articulo=dc.codigo_articulo)	
	where 
	cpc.nro_contrato = @p_nro_contrato
	and cpc.codigo_tipo_planilla=@codigo_tipo_planilla 
	--and dbo.fn_validar_contrato_documentacion_completa(ee.codigo_equivalencia, cpc.nro_contrato) = 1
	and cpc.estado_registro=1 
	and ac.estado_registro=1 
	and dc.estado_registro=1 
	and dc.codigo_estado_cuota=1
	and dc.nro_cuota =1
	--and dc.fecha_programada  between DATEADD(month, -3, @p_fecha_fin) and @p_fecha_fin;

	SET @v_maximo_codigo_detalle_actual = (SELECT MAX(codigo_detalle_planilla) FROM dbo.detalle_planilla WHERE codigo_planilla = @p_codigo_planilla)

	/*************************************************************************/

	 while @i<=@n
	 begin
		 select 
			 @v_codigo_canal=codigo_canal,
			 @v_codigo_empresa=codigo_empresa,
			 @v_codigo_tipo_venta=codigo_tipo_venta,
			 @v_codigo_campo_santo=codigo_campo_santo
		 from @table where nro=@i;

		IF (@p_tipo_ejecucion = 0)
		BEGIN
			insert into @t_listar
			select 
				dp.codigo_detalle
			from @detalle_planilla dp 
				   inner join dbo.fn_SplitReglaTipoPlanilla(@v_codigo_empresa) ee on ee.codigo=dp.codigo_empresa
				   inner join dbo.fn_SplitReglaTipoPlanilla(@v_codigo_tipo_venta) tv on tv.codigo=dp.codigo_tipo_venta
				   inner join dbo.fn_SplitReglaTipoPlanilla(@v_codigo_campo_santo) cs on cs.codigo=dp.codigo_campo_santo
			where dp.codigo_canal=@v_codigo_canal and dp.es_emitido=0;
		END
		ELSE
		BEGIN

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
			usuario_registra)

			select 
			@p_codigo_planilla,
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
			@p_usuario_registra	
			from @detalle_planilla dp 
				   inner join dbo.fn_SplitReglaTipoPlanilla(@v_codigo_empresa) ee on ee.codigo=dp.codigo_empresa
				   inner join dbo.fn_SplitReglaTipoPlanilla(@v_codigo_tipo_venta) tv on tv.codigo=dp.codigo_tipo_venta
				   inner join dbo.fn_SplitReglaTipoPlanilla(@v_codigo_campo_santo) cs on cs.codigo=dp.codigo_campo_santo
			where dp.codigo_canal=@v_codigo_canal and dp.es_emitido=0;
			----------------------------------------------
			update dc
			 set dc.es_emitido =1
			from detalle_planilla as dp
			inner join @detalle_planilla dc on dp.codigo_detalle_cronograma=dc.codigo_detalle
			where dp.codigo_planilla= @p_codigo_planilla and  dc.es_emitido=0;
		END

	/*******************************************************************************/
	 set @i=@i+1;
	end;
	/****************************************************************************/
	
	IF (@p_tipo_ejecucion = 1)
	BEGIN
		SET @v_maximo_codigo_detalle_nuevo = (SELECT MAX(codigo_detalle_planilla) FROM dbo.detalle_planilla WHERE codigo_planilla = @p_codigo_planilla)
	
		IF (@v_maximo_codigo_detalle_nuevo > @v_maximo_codigo_detalle_actual)
			SET @cantidad_registro_procesar = (SELECT COUNT(codigo_detalle_planilla) FROM dbo.detalle_planilla WHERE codigo_planilla = @p_codigo_planilla AND codigo_detalle_planilla BETWEEN (@v_maximo_codigo_detalle_actual + 1) AND @v_maximo_codigo_detalle_nuevo)
		ELSE
			SET @cantidad_registro_procesar = 0

		if(@cantidad_registro_procesar<=0)
		begin
			RAISERROR('No existen pagos habilitados para incluir.',16,1); 
			return;
		end;
	END
	ELSE
	BEGIN

		SELECT 
		dp.codigo_cronograma,
		@p_codigo_planilla as codigo_planilla,--dp.codigo_planilla,
		'' as observacion,--dp.observacion,
		0 as codigo_detalle_planilla, --dp.codigo_detalle_planilla,
		dp.codigo_detalle as codigo_detalle_cronograma,--dp.codigo_detalle_cronograma,
		a.nombre as nombre_articulo,
		dp.fecha_programada as fecha_pago,--dp.fecha_pago,
		dp.nro_cuota,
		dp.monto_bruto,
		dp.igv,
		dp.monto_neto,
		dp.nro_contrato,
		tv.codigo_tipo_venta,
		tv.nombre as nombre_tipo_venta,
		tp.codigo_tipo_pago,
		tp.nombre as nombre_tipo_pago,
		p.apellido_paterno,
		p.apellido_materno,
		p.nombre as nombre_persona,
		isnull(cg.nombre,' ') as nombre_grupo_canal,
		c.nombre as nombre_canal,
		emp.codigo_empresa,
		emp.nombre as nombre_empresa,
		(case when dc.es_registro_manual_comision=0 then dc.codigo_estado_cuota else @v_codigo_estado_cuota_pagada end) as codigo_estado_cuota,
		(case when dc.es_registro_manual_comision=0 then ec.nombre else 'Pagada' end) as nombre_estado_cuota,
		dc.estado_registro,
		dc.es_registro_manual_comision
		from 
		@detalle_planilla dp 
		inner join empresa_sigeco emp on emp.codigo_empresa=dp.codigo_empresa
		inner join tipo_venta tv on tv.codigo_tipo_venta=dp.codigo_tipo_venta
		inner join tipo_pago tp on tp.codigo_tipo_pago=dp.codigo_tipo_pago
		inner join articulo a on dp.codigo_articulo=a.codigo_articulo
		inner join personal p on p.codigo_personal=dp.codigo_personal
		inner join canal_grupo c on c.codigo_canal_grupo=dp.codigo_canal
		inner join detalle_cronograma dc on dp.codigo_detalle = dc.codigo_detalle
		inner join estado_cuota ec on dc.codigo_estado_cuota=ec.codigo_estado_cuota
		inner join canal_grupo cg on cg.codigo_canal_grupo=dp.codigo_canal_grupo
		where dp.es_emitido=0 
		AND dc.codigo_detalle in (select codigo_detalle from @t_listar)
		ORDER BY dp.codigo_empresa, dp.codigo_articulo, DP.nro_cuota;
		--return;

		SET @p_total_registro_procesado = (SELECT COUNT(codigo_detalle) FROM @t_listar)
		--if(@p_total_registro_procesado <= 0)
		--begin
		--	RAISERROR('No existen pagos habilitados para incluir.',16,1); 
		--	return;
		--end;
		return;
	END

	/*************************************************************************************/
	set @p_total_registro_procesado=@cantidad_registro_procesar;

	/******************************************
	ACTUALIZANDO CUOTAS CON ESTADO EN PROCESO DE PAGO 
	*******************************************/
	update dc
	set dc.codigo_estado_cuota = @codigo_estado_cuota ,
		dc.fecha_programada=isnull(dc.fecha_programada,dp.fecha_pago)
	from detalle_planilla as dp
	inner join detalle_cronograma as dc 
		   on dp.codigo_detalle_cronograma = dc.codigo_detalle 
	where dp.codigo_planilla= @p_codigo_planilla AND dp.codigo_detalle_planilla BETWEEN (@v_maximo_codigo_detalle_actual + 1) AND @v_maximo_codigo_detalle_nuevo;

	/******************************************
	DESACTIVANDO REGISTROS ANTIGUOS 
	*******************************************/
	update occ
	set occ.estado_registro =0
	from detalle_planilla as dp
	inner join detalle_cronograma dc on dp.codigo_detalle_cronograma=dc.codigo_detalle
	inner join operacion_cuota_comision as occ 
		   on dp.codigo_detalle_cronograma = occ.codigo_detalle_cronograma 
	where dp.codigo_planilla= @p_codigo_planilla and  occ.estado_registro=1 AND dp.codigo_detalle_planilla BETWEEN (@v_maximo_codigo_detalle_actual + 1) AND @v_maximo_codigo_detalle_nuevo;


	/******************************************
	INSERTANDO REGISTROS NUEVOS 
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
	/*registro*/
	select 
	codigo_detalle_cronograma,
	2,
	'Generacion de planilla',
	getdate(),
	1,
	@p_usuario_registra,
	getdate()
	from detalle_planilla where  codigo_planilla= @p_codigo_planilla AND codigo_detalle_planilla BETWEEN (@v_maximo_codigo_detalle_actual + 1) AND @v_maximo_codigo_detalle_nuevo;

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
			dp.codigo_planilla=@p_codigo_planilla and dc.es_registro_manual_comision=1 and dc.estado_registro=1 AND dp.codigo_detalle_planilla BETWEEN (@v_maximo_codigo_detalle_actual + 1) AND @v_maximo_codigo_detalle_nuevo
	   ) sc on (cm.codigo_detalle_cronograma=sc.codigo_detalle and cm.en_planilla=0 )
	   when matched then update set cm.codigo_planilla=@p_codigo_planilla,cm.en_planilla=1;

END;