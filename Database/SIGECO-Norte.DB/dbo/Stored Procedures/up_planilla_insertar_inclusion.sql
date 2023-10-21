USE SIGECO
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[up_planilla_insertar_inclusion]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].up_planilla_insertar_inclusion
GO

CREATE PROCEDURE [dbo].up_planilla_insertar_inclusion
(
	@p_codigo_planilla				int,
	@p_usuario_registra				varchar(30),
	@p_contratos					xml,
	@p_total_registro_procesado		int out
)
AS
BEGIN

	DECLARE 
		@codigo_tipo_planilla int,
		@cantidad_registro_procesar int,
		@codigo_estado_cuota int = 2--INDICA QUE LA CUOTA SE ENCUENTRA EN PROCESO DE PAGO

	DECLARE
		@p_codigo_regla_tipo_planilla		int
		,@p_fecha_fin						datetime
		,@v_maximo_codigo_detalle_actual	int
		,@v_maximo_codigo_detalle_nuevo		int
		,@v_codigo_estado_cuota_pagada		int = 3--PAGADA
		,@v_numero_planilla					varchar(100)
		,@c_validado						bit = 1
 
	SELECT TOP 1 
		@p_codigo_regla_tipo_planilla = codigo_regla_tipo_planilla
		,@p_fecha_fin = fecha_fin
		,@v_numero_planilla	 = numero_planilla
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

	;WITH ParsedXML AS
	(
	SELECT
		ParamValues.C.value('@codigo_detalle_cronograma', 'int') AS codigo_detalle_cronograma
	FROM 
		@p_contratos.nodes('//contratos/contrato') AS ParamValues(C)
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
		 isnull(dc.fecha_programada, getdate()),
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
	INNER JOIN ParsedXML tmp ON tmp.codigo_detalle_cronograma = dc.codigo_detalle
	where 
		cpc.codigo_tipo_planilla=@codigo_tipo_planilla 
		--and dbo.fn_validar_contrato_documentacion_completa(ee.codigo_equivalencia, cpc.nro_contrato) = 1
		and cpc.estado_registro=1 
		and ac.estado_registro=1 
		and dc.estado_registro=1 
		and dc.codigo_estado_cuota=1
		and dc.monto_neto > 0;
		--and dc.fecha_programada  between DATEADD(month, -3, @p_fecha_fin) and @p_fecha_fin;

	SET @v_maximo_codigo_detalle_actual = (SELECT MAX(codigo_detalle_planilla) FROM dbo.detalle_planilla WHERE codigo_planilla = @p_codigo_planilla)

	/*************************************************************************/

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
		es_transferencia)
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
		@p_usuario_registra,
		dp.es_transferencia
	from
		@detalle_planilla dp 
	where 
		dp.es_emitido=0;
	----------------------------------------------
	update dc
	set dc.es_emitido =1
	from 
		detalle_planilla as dp
	inner join @detalle_planilla dc 
		on dp.codigo_detalle_cronograma=dc.codigo_detalle
	where dp.codigo_planilla= @p_codigo_planilla and  dc.es_emitido=0;

	/****************************************************************************/
	
	SET @v_maximo_codigo_detalle_nuevo = (SELECT MAX(codigo_detalle_planilla) FROM dbo.detalle_planilla WHERE codigo_planilla = @p_codigo_planilla)
	
	IF (@v_maximo_codigo_detalle_nuevo > @v_maximo_codigo_detalle_actual)
		SET @cantidad_registro_procesar = (SELECT COUNT(codigo_detalle_planilla) FROM dbo.detalle_planilla WHERE codigo_planilla = @p_codigo_planilla AND codigo_detalle_planilla BETWEEN (@v_maximo_codigo_detalle_actual + 1) AND @v_maximo_codigo_detalle_nuevo)
	ELSE
		SET @cantidad_registro_procesar = 0

	IF(@cantidad_registro_procesar<=0)
	BEGIN
		RAISERROR('No existen pagos habilitados para incluir.',16,1); 
		RETURN;
	END;
	/*************************************************************************************/
	set @p_total_registro_procesado=@cantidad_registro_procesar;

	/******************************************
	ACTUALIZANDO CUOTAS CON ESTADO EN PROCESO DE PAGO 
	*******************************************/
	update dc
	set dc.codigo_estado_cuota = @codigo_estado_cuota
		--,dc.fecha_programada=isnull(dc.fecha_programada,dp.fecha_pago)
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
	inner join detalle_cronograma dc 
		on dp.codigo_detalle_cronograma=dc.codigo_detalle
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
		'INCLUSION MANUAL EN PLANILLA ' + @v_numero_planilla,
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

	/*
	BLOQUEO DE VENDEDORES
	*/
	EXEC dbo.up_personal_bloqueo_registrar_por_inclusion @p_codigo_planilla, @p_usuario_registra, @p_contratos

END;