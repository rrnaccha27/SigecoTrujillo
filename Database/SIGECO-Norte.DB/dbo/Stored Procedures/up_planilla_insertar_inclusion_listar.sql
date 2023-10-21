USE SIGECO
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[up_planilla_insertar_inclusion_listar]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].up_planilla_insertar_inclusion_listar
GO

CREATE PROCEDURE [dbo].up_planilla_insertar_inclusion_listar
(
	@p_nro_contrato				varchar(100)
	,@p_codigo_planilla			int
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
		,@c_validado						bit = 1
 
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
	SELECT DISTINCT
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
	FROM 
		cronograma_pago_comision cpc
	inner join empresa_sigeco ee on ee.codigo_empresa=cpc.codigo_empresa
	inner join personal_canal_grupo pcg on cpc.codigo_personal_canal_grupo=pcg.codigo_registro
	inner join personal p on pcg.codigo_personal = p.codigo_personal
	inner join articulo_cronograma ac on ac.codigo_cronograma=cpc.codigo_cronograma
	inner join detalle_cronograma dc on (ac.codigo_cronograma=dc.codigo_cronograma and ac.codigo_articulo=dc.codigo_articulo)	
	WHERE 
		cpc.nro_contrato = @p_nro_contrato
		and cpc.codigo_tipo_planilla=@codigo_tipo_planilla 
		--and dbo.fn_validar_contrato_documentacion_completa(ee.codigo_equivalencia, cpc.nro_contrato) = 1
		and cpc.estado_registro=1 
		and ac.estado_registro=1 
		and dc.estado_registro=1 
		and dc.codigo_estado_cuota=1
		and dc.monto_neto > 0;
		--and dc.fecha_programada  between DATEADD(month, -3, @p_fecha_fin) and @p_fecha_fin;

	SET @v_maximo_codigo_detalle_actual = (SELECT MAX(codigo_detalle_planilla) FROM dbo.detalle_planilla WHERE codigo_planilla = @p_codigo_planilla)

	/*************************************************************************/
	WHILE @i<=@n
	BEGIN
		select 
			@v_codigo_canal=codigo_canal,
			@v_codigo_empresa=codigo_empresa,
			@v_codigo_tipo_venta=codigo_tipo_venta,
			@v_codigo_campo_santo=codigo_campo_santo
		from @table where nro=@i;

		INSERT INTO @t_listar
		SELECT 
			dp.codigo_detalle
		FROM 
			@detalle_planilla dp 
		inner join dbo.fn_SplitReglaTipoPlanilla(@v_codigo_empresa) ee on ee.codigo=dp.codigo_empresa
		inner join dbo.fn_SplitReglaTipoPlanilla(@v_codigo_tipo_venta) tv on tv.codigo=dp.codigo_tipo_venta
		inner join dbo.fn_SplitReglaTipoPlanilla(@v_codigo_campo_santo) cs on cs.codigo=dp.codigo_campo_santo
		WHERE
			dp.codigo_canal=@v_codigo_canal and dp.es_emitido=0;

		SET @i=@i+1;
	END;
	/****************************************************************************/
	
	SELECT 
		dp.codigo_cronograma,
		@p_codigo_planilla as codigo_planilla,--dp.codigo_planilla,
		'' as observacion,--dp.observacion,
		0 as codigo_detalle_planilla, --dp.codigo_detalle_planilla,
		dp.codigo_detalle as codigo_detalle_cronograma,--dp.codigo_detalle_cronograma,
		a.nombre as nombre_articulo,
		isnull(convert(varchar, dp.fecha_programada, 103),'') as fecha_pago,--dp.fecha_pago,
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
	FROM
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
	WHERE 
		dp.es_emitido=0 
		AND dc.codigo_detalle in (select codigo_detalle from @t_listar)
	ORDER BY 
		dp.codigo_empresa, dp.codigo_articulo, DP.nro_cuota;

	--SET @p_total_registro_procesado = (SELECT COUNT(codigo_detalle) FROM @t_listar)
END;