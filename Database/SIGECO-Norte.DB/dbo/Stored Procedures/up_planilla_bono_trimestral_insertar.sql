USE [SIGECO]
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[up_planilla_bono_trimestral_insertar]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].up_planilla_bono_trimestral_insertar
GO

CREATE PROCEDURE [dbo].[up_planilla_bono_trimestral_insertar]
(
	@p_codigo_regla int,
	@p_codigo_periodo int,
	@p_usuario_registra varchar(30),
	@p_codigo_planilla int out,
	@p_total_registro_procesado int out
)
AS
BEGIN
	SET NOCOUNT ON

	DECLARE 
		@n int,
		@i int,
		@v_existe_planilla int,
		@v_numero_planilla varchar(50),
		@v_cantidad_procesados int,
		@v_anio_proceso int,
		@v_cantidad_planilla int,
		@v_codigo_tipo_bono int,
		@v_periodo_rango varchar(250),
		@v_IGV decimal(12, 2),
		@v_concepto_liquidacion	varchar(250);

	DECLARE
		@c_fecha_proceso						datetime = GETDATE(),
		@c_codigo_estado_planilla_abierto		int = 1,
		@c_codigo_estado_planilla_cerrado		int = 2,
		@c_periodo_final						int = 4,
		@c_codigo_tipo_documento_DNI			int = 1,
		@c_parametro_IGV						int = 9,
		@c_codigo_tipo_bloqueo_bono_trimestral	int = 3,
		@c_OFSA									int = 1;

	SET @v_anio_proceso = YEAR(GETDATE()) + CASE WHEN @p_codigo_periodo = @c_periodo_final THEN -1 ELSE 0 END 
	SET @v_IGV = (SElECT 1 + CONVERT(DECIMAL(12, 2),(CONVERT(FLOAT, valor)/100)) FROM dbo.parametro_sistema WHERE codigo_parametro_sistema = @c_parametro_IGV)

	select top 1
		@v_codigo_tipo_bono = codigo_tipo_bono
	from
		dbo.regla_bono_trimestral
	where 
		codigo_regla = @p_codigo_regla

	select 
		@v_cantidad_planilla=isnull(count(1),0)+1 
	from 
		dbo.planilla_bono_trimestral
	where
		anio_periodo = @v_anio_proceso

	set @v_numero_planilla = CONVERT(varchar, @v_anio_proceso) + '-' + RIGHT('000' + CAST(@v_cantidad_planilla as varchar), 3);

	select 
	   @v_existe_planilla=count(*) 
	from 
		dbo.planilla_bono_trimestral
	where 
		codigo_periodo = @p_codigo_periodo
		AND anio_periodo = @v_anio_proceso
		AND codigo_tipo_bono = @v_codigo_tipo_bono
		AND codigo_estado_planilla IN (@c_codigo_estado_planilla_abierto, @c_codigo_estado_planilla_cerrado);

	SET @v_periodo_rango = (SELECT TOP 1 rango FROM periodo_trimestral WHERE codigo_periodo = @p_codigo_periodo)
	
	IF(@v_existe_planilla>0)
	BEGIN
		RAISERROR('El periodo seleccionado ya cuenta con una planilla similar.',16,1); 
		RETURN;
	END;
	 
	DECLARE @t_configuracion TABLE(
		nro int,
		codigo_canal int,
		codigo_empresa varchar(500),
		codigo_tipo_venta varchar(500)
	);

	INSERT INTO @t_configuracion
	SELECT  
		row_number() over(order by codigo_regla_detalle) nro,
		codigo_canal,
		codigo_empresa,
		codigo_tipo_venta
	FROM 
		dbo.regla_bono_trimestral_detalle
	WHERE 
		codigo_regla = @p_codigo_regla 
		and estado_registro = 1;

	SELECT @n = max(nro), @i = 1 from @t_configuracion;

	DECLARE 
		@v_codigo_canal int,
		@v_codigo_empresa varchar(500),
		@v_codigo_tipo_venta varchar(500)

	DECLARE @t_detalle_planilla table(
		id					INT IDENTITY(1, 1),
		numero_contrato		VARCHAR(100),
		codigo_empresa		INT,
		codigo_empresa_j	VARCHAR(4),
		monto_contratado	DECIMAL(12, 2),
		unidad_venta		INT,
		flag_trasferencia	BIT,
		monto_trasferencia	DECIMAL(12, 2),
		codigo_canal		INT,
		nombre_canal		VARCHAR(250),
		codigo_grupo		INT,
		nombre_grupo		VARCHAR(250),
		codigo_canal_grupo	INT,
		codigo_vendedor		INT,
		nombre_vendedor		VARCHAR(250),
		codigo_supervisor	INT,
		nombre_supervisor	VARCHAR(250),
		correo_supervisor	VARCHAR(250),
		estado_registro		BIT DEFAULT(1),
		observacion			VARCHAR(250)
	)

	DECLARE @t_sumarizado table
	(
		codigo_personal		INT
		,codigo_empresa		INT
		,codigo_canal_grupo	INT
		,codigo_canal		INT
		,nombre_canal		VARCHAR(250)
		,codigo_grupo		INT
		,nombre_grupo		VARCHAR(250)
		,nombre_personal	VARCHAR(250)
		,codigo_supervisor	INT
		,correo_supervisor	VARCHAR(250)
		,nombre_supervisor	VARCHAR(250)
		,monto_contratado	DECIMAL(12, 2)
		,unidad_venta		INT
	)

	DECLARE @t_resumen table
	(
		rango				INT IDENTITY(1, 1)
		,codigo_personal	INT
		,monto_bono			DECIMAL(12, 2)
	)

	/*************************************************************************/

	WHILE (@i <= @n)
	BEGIN
		SELECT TOP 1
			@v_codigo_canal=codigo_canal,
			@v_codigo_empresa=codigo_empresa,
			@v_codigo_tipo_venta=codigo_tipo_venta
		FROM @t_configuracion 
		WHERE nro=@i;

		--SELECT TOP 1
		--	codigo_canal,
		--	codigo_empresa,
		--	codigo_tipo_venta,
		--	@v_anio_proceso,
		--	@v_periodo_rango
		--FROM @t_configuracion 
		--WHERE nro=@i;

		INSERT INTO @t_detalle_planilla(
			numero_contrato,
			codigo_empresa,
			codigo_empresa_j,
			monto_contratado,
			unidad_venta,
			flag_trasferencia,
			monto_trasferencia,
			codigo_canal,
			nombre_canal,
			codigo_grupo,
			nombre_grupo,
			codigo_canal_grupo,
			codigo_vendedor,
			nombre_vendedor,
			codigo_supervisor,
			nombre_supervisor,
			correo_supervisor
		)
		SELECT DISTINCT
			cab.NumAtCard
			,ee.codigo_empresa
			,cab.Codigo_empresa
			,cab.DocTotal
			,CONVERT(INT, ISNULL(cab.Num_Ventas, 0)) as unidad_venta
			,cab.Flg_Transferencia
			,cab.MontoTransferencia
			,pcg.codigo_canal
			,canal.nombre as nombre_canal
			,cg.codigo_canal_grupo as codigo_grupo
			,cg.nombre as nombre_grupo
			,pcg.codigo_registro
			,v.codigo_personal as codigo_vendedor
			,RTRIM(v.nombre + ISNULL(' ' + v.apellido_paterno, '') + ISNULL(' ' + v.apellido_materno, ''))
			,s.codigo_personal as codigo_supervisor
			,RTRIM(s.nombre + ISNULL(' ' + s.apellido_paterno, '') + ISNULL(' ' + s.apellido_materno, ''))
			,ISNULL(s.correo_electronico, '')
		FROM 
			cabecera_contrato cab
		inner join empresa_sigeco ee on ee.codigo_equivalencia = cab.codigo_empresa
		inner join canal_grupo cg on cg.codigo_equivalencia = cab.codigo_grupo and cg.es_canal_grupo = 0
		inner join personal v on v.codigo_equivalencia = cab.Cod_Vendedor and  v.estado_registro = 1
		inner join personal_canal_grupo pcg on pcg.codigo_personal = v.codigo_personal and pcg.estado_registro = 1 and pcg.es_supervisor_grupo = 0
		inner join canal_grupo canal on pcg.codigo_canal = canal.codigo_canal_grupo and canal.es_canal_grupo = 1
		inner join personal s on s.codigo_equivalencia = cab.Cod_Supervisor and  s.estado_registro = 1
		inner join tipo_venta tv on tv.codigo_equivalencia = cab.Cod_Tipo_Venta
		inner join dbo.fn_SplitReglaTipoPlanilla(@v_periodo_rango) pe on pe.codigo = MONTH(cab.CreateDate)
		inner join dbo.fn_SplitReglaTipoPlanilla(@v_codigo_empresa) xe on xe.codigo = ee.codigo_empresa
		inner join dbo.fn_SplitReglaTipoPlanilla(@v_codigo_tipo_venta) xtv on xtv.codigo = tv.codigo_tipo_venta
		WHERE 
			YEAR(cab.CreateDate) = @v_anio_proceso
			AND ISNULL(cab.Cod_Estado_Contrato, '') = ''
			AND pcg.codigo_canal = @v_codigo_canal

		SET @i = @i + 1;
	END;

	SELECT @i = MIN(id), @n = MAX(id) FROM @t_detalle_planilla

	DECLARE
		@r_numero_contrato		VARCHAR(100),
		@r_codigo_empresa		INT,
		@r_codigo_empresa_j		VARCHAR(4),
		@r_monto_contratado		DECIMAL(12, 2),
		@r_unidad_venta			INT,
		@r_flag_trasferencia	BIT,
		@r_monto_trasferencia	DECIMAL(12, 2),
		@r_codigo_canal			INT,
		@r_nombre_canal			VARCHAR(250),
		@r_codigo_grupo			INT,
		@r_nombre_grupo			VARCHAR(250),
		@r_codigo_canal_grupo	VARCHAR(250),
		@r_codigo_vendedor		INT,
		@r_nombre_vendedor		VARCHAR(250),
		@r_codigo_supervisor	INT,
		@r_nombre_supervisor	VARCHAR(250),
		@r_correo_supervisor	VARCHAR(250),
		@r_estado_registro		BIT

	DECLARE
		@v_monto_no_bolsa	INT

	UPDATE 
		d
	SET
		estado_registro = 0,
		observacion = 'No tiene cuotas.'
	FROM
		@t_detalle_planilla d
	WHERE
		NOT EXISTS(SELECT Num_Cuota FROM dbo.contrato_cuota cu WHERE cu.NumAtCard = d.numero_contrato and cu.Codigo_empresa = d.codigo_empresa_j AND cu.Cod_Estado IN ('C', 'P'))
	
	UPDATE 
		d
	SET
		estado_registro = 0,
		observacion = 'No tiene articulos.'
	FROM
		@t_detalle_planilla d
	WHERE
		NOT EXISTS(SELECT ItemCode FROM dbo.detalle_contrato dc WHERE dc.NumAtCard = d.numero_contrato AND dc.Codigo_empresa = d.codigo_empresa_j)

	WHILE (@i <= @n)
	BEGIN
		SELECT TOP 1
			@r_numero_contrato		= numero_contrato,
			@r_codigo_empresa		= codigo_empresa,
			@r_codigo_empresa_j		= @r_codigo_empresa_j,
			@r_monto_contratado		= monto_contratado,
			@r_unidad_venta			= unidad_venta,
			@r_flag_trasferencia	= flag_trasferencia,
			@r_monto_trasferencia	= monto_trasferencia,
			@r_codigo_canal			= codigo_canal,
			@r_nombre_canal			= nombre_canal,
			@r_codigo_grupo			= codigo_grupo,
			@r_nombre_grupo			= nombre_grupo,
			@r_codigo_canal_grupo	= codigo_canal_grupo,
			@r_codigo_vendedor		= codigo_vendedor,
			@r_nombre_vendedor		= nombre_vendedor,
			@r_codigo_supervisor	= codigo_supervisor,
			@r_nombre_supervisor	= nombre_supervisor,
			@r_correo_supervisor	= correo_supervisor,
			@r_estado_registro		= estado_registro
		FROM
			@t_detalle_planilla
		WHERE
			id = @i

		IF @r_estado_registro = 1
		BEGIN

			-- Aquellos articulos que NO suman a la bolsa (monto contratado)
			SET @v_monto_no_bolsa = NULL
			SELECT 
				@v_monto_no_bolsa = SUM(dc.Price * dc.Quantity)
			FROM 
				dbo.detalle_contrato dc 
			INNER JOIN dbo.articulo a
				on dc.ItemCode = a.codigo_sku and a.genera_bolsa_bono = 0
			WHERE 
				dc.Codigo_empresa =  @r_codigo_empresa_j
				AND dc.NumAtCard = @r_numero_contrato

			SET @v_monto_no_bolsa = ISNULL(@v_monto_no_bolsa, 0)
			SET @r_monto_contratado = ISNULL(@r_monto_contratado, 0) - ISNULL(@v_monto_no_bolsa, 0)
			SET @r_monto_contratado = @r_monto_contratado - CASE WHEN @r_flag_trasferencia = 1 THEN @r_monto_trasferencia ELSE 0 END

			IF @r_monto_contratado <= 0
			BEGIN
				UPDATE 
					@t_detalle_planilla
				SET
					estado_registro = 0,
					observacion = 'No tiene monto contratado.'
				WHERE
					id = @i
			END
			ELSE
			BEGIN
				UPDATE 
					@t_detalle_planilla
				SET
					monto_contratado = @r_monto_contratado
				WHERE
					id = @i
			
				IF (EXISTS(SELECT codigo_personal FROM @t_sumarizado WHERE codigo_personal = @r_codigo_vendedor))
				BEGIN
					UPDATE @t_sumarizado
					SET
						monto_contratado = monto_contratado + @r_monto_contratado
						,unidad_venta =  unidad_venta + @r_unidad_venta
					WHERE
						codigo_personal = @r_codigo_vendedor --and codigo_empresa = @r_codigo_empresa
				END
				ELSE
				BEGIN
					INSERT INTO @t_sumarizado
					(
						codigo_personal
						,codigo_empresa
						,codigo_canal_grupo
						,codigo_canal
						,nombre_canal
						,codigo_grupo
						,nombre_grupo
						,nombre_personal
						,codigo_supervisor
						,correo_supervisor
						,nombre_supervisor
						,monto_contratado
						,unidad_venta
					)
					VALUES
					(
						@r_codigo_vendedor
						,@r_codigo_empresa
						,@r_codigo_canal_grupo
						,@r_codigo_canal
						,@r_nombre_canal
						,@r_codigo_grupo
						,@r_nombre_grupo
						,@r_nombre_vendedor
						,@r_codigo_supervisor
						,@r_correo_supervisor
						,@r_nombre_supervisor
						,@r_monto_contratado
						,@r_unidad_venta
					)
				END
			END

		END

		SET @i = @i + 1
	END

	INSERT INTO 
		@t_resumen (codigo_personal)
	SELECT
		codigo_personal
	FROM
		@t_sumarizado
	ORDER BY
		monto_contratado DESC, unidad_venta DESC

	UPDATE 
		r
	SET
		monto_bono = meta.monto
	FROM
		@t_resumen r
	INNER JOIN dbo.regla_bono_trimestral_meta meta 
		ON meta.codigo_regla = @p_codigo_regla AND meta.estado_registro = 1 and r.rango between meta.rango_inicio and meta.rango_fin

	UPDATE 
		d
	SET
		observacion = CASE WHEN r.monto_bono IS NULL THEN 'No llego al bono. Rango:' + CONVERT(varchar,r.rango) ELSE 'LLego al bono. Rango: '+ CONVERT(varchar,r.rango) + ' - Monto:' + CONVERT(varchar,r.monto_bono) END
	FROM
		@t_detalle_planilla d
	LEFT JOIN @t_resumen r 
		ON d.codigo_vendedor = r.codigo_personal

	--/****************************************************************************/
	SET @v_cantidad_procesados = (SELECT COUNT(*) FROM @t_resumen WHERE monto_bono IS NOT NULL)

	IF (@v_cantidad_procesados <= 0)
	BEGIN
		RAISERROR('No hubieron resultados para la planilla.',16,1); 
		RETURN;
	END;
	--/*************************************************************************************/
	
	SET @p_total_registro_procesado = @v_cantidad_procesados;

	--PASAR A LAS TABLAS OFICIALES
	INSERT INTO dbo.planilla_bono_trimestral(
		numero_planilla,
		codigo_regla,
		codigo_tipo_bono,
		codigo_periodo,
		anio_periodo,
		codigo_estado_planilla,
		fecha_apertura,
		usuario_apertura,
		fecha_registra,
		usuario_registra
	)
	values(
		@v_numero_planilla,
		@p_codigo_regla,
		@v_codigo_tipo_bono,
		@p_codigo_periodo,
		@v_anio_proceso,
		@c_codigo_estado_planilla_abierto,
		@c_fecha_proceso,
		@p_usuario_registra,
		@c_fecha_proceso,
		@p_usuario_registra
	);

	SET @p_codigo_planilla = SCOPE_IDENTITY();

	SELECT TOP 1
		--'CORRESPONDIENTE A ' + UPPER(tb.nombre) + ' AL PERIODO ' + CONVERT(VARCHAR, anio_periodo) + ' ' +  pe.nombre,
		@v_concepto_liquidacion = 'CORRESPONDIENTE A ' + UPPER(rb.descripcion) + ' DEL PERIODO ' + CONVERT(VARCHAR, anio_periodo) + ' ' +  pe.nombre
	FROM 
		planilla_bono_trimestral pl
	INNER JOIN periodo_trimestral pe 
		on pe.codigo_periodo = pl.codigo_periodo
	INNER JOIN tipo_bono_trimestral tb
		on tb.codigo_tipo_bono = pl.codigo_tipo_bono
	INNER JOIN regla_bono_trimestral rb
		on rb.codigo_regla = pl.codigo_regla
	WHERE
		pl.codigo_planilla = @p_codigo_planilla

	INSERT INTO dbo.planilla_bono_trimestral_detalle
	(
		codigo_planilla
		,codigo_empresa
		,nombre_empresa
		,nombre_empresa_largo
		,direccion_fiscal_empresa
		,ruc_empresa
		,codigo_canal_grupo
		,codigo_canal
		,nombre_canal
		,codigo_grupo
		,nombre_grupo
		,codigo_personal
		,nombre_personal
		,documento_personal
		,codigo_personal_j
		,codigo_supervisor
		,correo_supervisor
		,nombre_supervisor
		,monto_contratado
		,unidad_venta
		,rango
		,monto_bono
		,monto_sin_igv
		,monto_igv
		,monto_bono_letras
		,concepto_liquidacion
		,monto_bono_grupo
		,monto_bono_canal
		,monto_bono_empresa
	)
	SELECT
		@p_codigo_planilla
		,ee.codigo_empresa
		,ee.nombre
		,ee.nombre_largo
		,ee.direccion_fiscal
		,ee.ruc
		,s.codigo_canal_grupo
		,s.codigo_canal
		,s.nombre_canal
		,s.codigo_grupo
		,s.nombre_grupo
		,s.codigo_personal
		,s.nombre_personal
		,td.nombre_tipo_documento + ' : ' + CASE WHEN pe.codigo_tipo_documento = @c_codigo_tipo_documento_DNI THEN pe.nro_documento ELSE pe.nro_ruc END
		,pe.codigo_equivalencia
		,s.codigo_supervisor
		,s.correo_supervisor
		,s.nombre_supervisor
		,s.monto_contratado
		,s.unidad_venta
		,r.rango
		,r.monto_bono
		,ROUND(r.monto_bono / @v_IGV, 2)
		,r.monto_bono - ROUND(r.monto_bono / @v_IGV, 2)
		,CASE WHEN ISNULL(r.monto_bono, 0) > 0 THEN dbo.fn_GetLetrasPrecio(r.monto_bono, 1) ELSE '' END
		,CASE WHEN ISNULL(r.monto_bono, 0) > 0 THEN @v_concepto_liquidacion ELSE '' END
		,SUM(monto_bono) over(partition by ee.codigo_empresa, s.codigo_canal, s.codigo_grupo)
		,SUM(monto_bono) over(partition by ee.codigo_empresa, s.codigo_canal)
		,SUM(monto_bono) over(partition by ee.codigo_empresa)
	FROM 
		@t_sumarizado s
	INNER JOIN @t_resumen r 
		ON s.codigo_personal = r.codigo_personal 
	INNER JOIN dbo.empresa_sigeco ee
		ON ee.codigo_empresa = @c_OFSA--s.codigo_empresa
	INNER JOIN dbo.personal pe
		ON pe.codigo_personal = s.codigo_personal
	INNER JOIN dbo.tipo_documento td
		ON pe.codigo_tipo_documento = td.codigo_tipo_documento
	ORDER BY 
		r.rango ASC

	UPDATE det
	SET
		monto_bono_grupo_sin_igv = ROUND(monto_bono_grupo / @v_IGV, 2)
		,monto_bono_canal_sin_igv = ROUND(monto_bono_canal / @v_IGV, 2)
		,monto_bono_empresa_sin_igv = ROUND(monto_bono_empresa / @v_IGV, 2)
		,monto_bono_grupo_igv = monto_bono_grupo - ROUND(monto_bono_grupo / @v_IGV, 2)
		,monto_bono_canal_igv = monto_bono_canal - ROUND(monto_bono_canal / @v_IGV, 2)
		,monto_bono_empresa_igv = monto_bono_empresa - ROUND(monto_bono_empresa / @v_IGV, 2)
	FROM 
		dbo.planilla_bono_trimestral_detalle det 
	WHERE
		det.monto_bono IS NOT NULL
		AND det.codigo_planilla = @p_codigo_planilla

	INSERT INTO dbo.planilla_bono_trimestral_contratos
	(
		codigo_planilla
		,codigo_personal
		,codigo_empresa
		,numero_contrato
		,monto_contratado
		,estado_registro
		,observacion
	)
	SELECT 
		@p_codigo_planilla
		,codigo_vendedor
		,codigo_empresa
		,numero_contrato
		,monto_contratado
		,estado_registro
		,observacion
	FROM 
		@t_detalle_planilla

	--select * from @t_detalle_planilla
	--select * from @t_sumarizado s
	--inner join @t_resumen r on s.codigo_personal = r.codigo_personal 
	--order by r.rango ASC

	/*
	BLOQUEO DE VENDEDORES
	*/
	EXEC dbo.up_personal_bloqueo_registrar @p_codigo_planilla, @c_codigo_tipo_bloqueo_bono_trimestral, @p_usuario_registra

	SET NOCOUNT OFF
END;

