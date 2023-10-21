USE SIGECO
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[up_proceso_generacion_bono_jn]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].up_proceso_generacion_bono_jn
GO

CREATE PROCEDURE [dbo].[up_proceso_generacion_bono_jn]
(
	 @p_codigo_planilla			int
	,@p_codigo_tipo_planilla	int
	,@p_codigo_canal			int
	,@p_fecha_inicio			datetime
	,@p_fecha_fin				datetime
	,@p_usuario					varchar(50)
	,@p_id_proceso				uniqueidentifier
)
AS
BEGIN
SET NOCOUNT ON
	DECLARE	
		@v_reglas_bono					int
		,@v_codigo_regla_bono			int
		,@v_nro_contrato				varchar(100)
		,@v_codigo_personal				int
		,@v_codigo_empresa_j			nvarchar(4)
		,@v_codigo_empresa				int
		,@v_monto_contrato				decimal(12, 4)
		,@v_ventas						int
		,@v_codigo_grupo				int
		,@v_codigo_grupo_j				varchar(10)
		,@v_monto_meta_rango_inicio		decimal(12, 4)
		,@v_monto_meta_rango_fin		decimal(12, 4)
		,@v_monto_meta_rango_fin_tmp	decimal(12, 4)
		,@v_porcentaje_pago				decimal(12, 4)
		,@v_monto_tope					decimal(12, 4)
		,@v_monto_ingresado				decimal(12, 4)
		,@v_monto_a_pagar				decimal(12, 4)
		,@v_igv							decimal(12, 4)
		,@v_monto_a_pagar_igv			decimal(12, 4)
		,@v_monto_a_pagar_bruto			decimal(12, 4)
		,@v_calcular_igv				bit
		,@v_cantidad_ventas				int
		,@v_nro_registro				int
		,@v_total_registros				int
		,@v_codigo_tipo_venta			int
		,@v_codigo_moneda				int
		,@v_monto_no_bolsa				decimal(12, 4)
		,@v_monto_no_bono				decimal(12, 4)
		,@v_meta_lograda				decimal(12, 4)
		,@v_nro_registro_pagos			int
		,@v_total_registros_pagos		int
		,@v_codigo_supervisor			int
		,@v_codigo_personal_original	int
		,@v_codigo_empresas				varchar(100)
		,@v_codigo_canal				int
		,@v_fecha_contrato				varchar(10)
		,@v_detraccion					bit

	declare
		@v_precio_igv				decimal(12, 4)
		,@v_factor					decimal(12, 4)
		,@v_monto_transferencia		decimal(12, 4)
		,@v_cuota_inicial			decimal(12, 4)
		,@v_total_articulos			decimal(12, 4)
		,@v_trf_articulos			decimal(12, 4)
		,@v_parametro_canales_jn	varchar(10)

	DECLARE
		@c_tipo_planilla_vendedor		int = 1
		,@c_tipo_planilla_supervisor	int = 2
		,@c_estado_no_procesado			int = 1
		,@c_estado_error				int = 2
		,@c_estado_procesado			int = 3
		,@c_planilla_estado_cerrada		int = 2
		,@c_tipo_venta_necesidad		int = 1 
	
	DECLARE @t_Contrato TABLE
	(
		id							int identity(1, 1)
		,codigo_personal			int 
		,codigo_personal_original	int
		,codigo_empresa				int 
		,codigo_canal				int 
		,codigo_grupo				int 
		,monto_contrato				decimal(12, 4)
		,ventas						int
		,monto_ingresado			decimal(12, 4)
		,codigo_moneda				int
	)

	DECLARE @t_Contrato_Pagos TABLE
	(
		id							int
		,codigo_personal			int 
		,codigo_empresa				int 
		,codigo_canal				int 
		,codigo_grupo				int 
		,monto_contrato				decimal(12, 4)
		,ventas						int
		,monto_ingresado			decimal(12, 4)
		,codigo_moneda				int
	)

	IF (SELECT CURSOR_STATUS('global','contrato_bono_cursor')) >= -1
	BEGIN
		IF (SELECT CURSOR_STATUS('global','contrato_bono_cursor')) > -1
		BEGIN
			CLOSE contrato_bono_cursor
		END
		DEALLOCATE contrato_bono_cursor
	END

	--Algunos valores que no cambiaran en todo el proceso
	SET @v_igv = (SELECT TOP 1 ROUND(CONVERT(DECIMAL(12, 4), valor)/100, 4) FROM dbo.parametro_sistema WHERE codigo_parametro_sistema = 9) 
	SET @v_parametro_canales_jn = (SELECT TOP 1 valor FROM dbo.parametro_sistema WHERE codigo_parametro_sistema = 28)

	declare @t_contratos table(
		id uniqueidentifier
		, codigo_empresa_o varchar(4)
		, codigo_empresa int
		, nro_contrato varchar(100)
		, codigo_tipo_venta int
		, codigo_canal int
	)

	INSERT INTO @t_contratos (id, codigo_empresa_o, codigo_empresa, nro_contrato, codigo_tipo_venta, codigo_canal)
	select 
		@p_id_proceso	
		,cc.Codigo_empresa
		,e.codigo_empresa
		,cc.NumAtCard
		,tv.codigo_tipo_venta
		,pcg.codigo_canal
	from 
		cabecera_contrato cc
	inner join vw_personal pcg on cc.Cod_Vendedor = pcg.codigo_equivalencia and cc.codigo_grupo = pcg.codigo_equivalencia_grupo and pcg.estado_canal_grupo = 1
	inner join empresa_sigeco e on e.codigo_equivalencia = cc.Codigo_empresa 
	inner join tipo_venta tv on cc.Cod_Tipo_Venta = tv.codigo_equivalencia
	inner join dbo.fn_SplitReglaTipoPlanilla(@v_parametro_canales_jn) x on pcg.codigo_canal = x.codigo -- /*funes y gestores*/
	where 
		LTRIM(ISNULL(cc.Cod_Estado_Contrato, '')) = ''
		AND cc.Cod_Tipo_Venta = 'NI'
		AND CONVERT(date, cc.CreateDate) BETWEEN convert(date, @p_fecha_inicio) AND convert(date, @p_fecha_fin)

	IF NOT EXISTS(SELECT TOP 1 nro_contrato FROM @t_contratos where id = @p_id_proceso)
		RETURN;

	DECLARE contrato_bono_cursor CURSOR FAST_FORWARD FORWARD_ONLY FOR 
	SELECT 
		cc.codigo_empresa
		,cc.NumAtCard
		,p.codigo_personal
		,pbc.codigo_empresa
		,(cc.DocTotal / (@v_igv + 1))
		,cc.Num_Ventas
		,pbc.codigo_tipo_venta--(SELECT TOP 1 tv.codigo_tipo_venta FROM dbo.tipo_venta tv WHERE tv.codigo_equivalencia = cc.Cod_Tipo_Venta) AS Cod_Tipo_Venta
		,(SELECT TOP 1 m.codigo_moneda FROM dbo.moneda m WHERE m.codigo_equivalencia = cc.DocCur) AS DocCur
		,pbc.codigo_canal
		,convert(varchar(10), cc.CreateDate, 103)
		,convert(decimal(12, 4), case when ISNULL(cc.Flg_Transferencia, 0) = 1 then ISNULL(MontoTransferencia/(@v_igv + 1), 0.00) else 0.00 end)
		,ISNULL(cc.codigo_grupo, '')
	FROM
		@t_contratos pbc
	INNER JOIN dbo.cabecera_contrato cc
		on pbc.id = @p_id_proceso and pbc.nro_contrato = cc.NumAtCard and pbc.codigo_empresa_o = cc.Codigo_empresa
	INNER JOIN dbo.personal p
		on p.codigo_equivalencia = cc.Cod_Vendedor
	WHERE
		--cc.Flg_Documentacion_Completa = '01' and 
		pbc.id = @p_id_proceso

	INSERT INTO dbo.log_proceso_bono_cabecera(id, codigo_planilla, codigo_canal, codigo_tipo_planilla, fecha_inicio, fecha_fin, usuario_registra, fecha_registra)
	SELECT 
		@p_id_proceso, codigo_planilla, codigo_canal, codigo_tipo_planilla, fecha_inicio, fecha_fin, @p_usuario, GETDATE()
	FROM
		dbo.planilla_bono
	WHERE
		codigo_planilla = @p_codigo_planilla

	INSERT INTO dbo.log_proceso_bono (codigo_planilla, codigo_tipo_planilla, nro_contrato, codigo_empresa, codigo_canal, codigo_estado, observacion)
	SELECT
		@p_codigo_planilla
		,@p_codigo_tipo_planilla
		,pbc.nro_contrato
		,pbc.codigo_empresa
		,pbc.codigo_canal
		,@c_estado_no_procesado
		,''
	FROM
		@t_contratos pbc
	INNER JOIN dbo.cabecera_contrato cc
		on pbc.id = @p_id_proceso and pbc.nro_contrato = cc.NumAtCard and pbc.codigo_empresa_o = cc.Codigo_empresa
	INNER JOIN dbo.personal p
		on p.codigo_equivalencia = cc.Cod_Vendedor
	WHERE
		--cc.Flg_Documentacion_Completa = '01' and 
		pbc.id = @p_id_proceso

	OPEN contrato_bono_cursor
	FETCH NEXT FROM contrato_bono_cursor
	INTO @v_codigo_empresa_j, @v_nro_contrato, @v_codigo_personal, @v_codigo_empresa, @v_monto_contrato, @v_ventas, @v_codigo_tipo_venta, @v_codigo_moneda, @v_codigo_canal, @v_fecha_contrato, @v_monto_transferencia, @v_codigo_grupo_j

	WHILE @@FETCH_STATUS = 0  
	BEGIN 
		--print ' analizando: ' + convert(varchar, @v_codigo_empresa_j) + ' - ' + convert(varchar, @v_nro_contrato) + ' - ' + convert(varchar, @v_codigo_personal)
		IF (NOT EXISTS(SELECT TOP 1 ItemCode FROM dbo.detalle_contrato WHERE NumAtCard = @v_nro_contrato and Codigo_empresa = @v_codigo_empresa_j))
		BEGIN
			--print convert(varchar(10), @v_codigo_empresa_j) + ' - ' + convert(varchar(10), @v_nro_contrato) + 'no tiene articulos'
			UPDATE dbo.log_proceso_bono SET codigo_estado = @c_estado_error, observacion = 'No tiene articulos' WHERE codigo_planilla = @p_codigo_planilla AND nro_contrato = @v_nro_contrato AND codigo_empresa = @v_codigo_empresa
			FETCH NEXT FROM contrato_bono_cursor   
			INTO @v_codigo_empresa_j, @v_nro_contrato, @v_codigo_personal, @v_codigo_empresa, @v_monto_contrato, @v_ventas, @v_codigo_tipo_venta, @v_codigo_moneda, @v_codigo_canal, @v_fecha_contrato, @v_monto_transferencia, @v_codigo_grupo_j

			CONTINUE
		END

		IF (NOT EXISTS(SELECT TOP 1 Num_Cuota FROM dbo.contrato_cuota WHERE NumAtCard = @v_nro_contrato and Codigo_empresa = @v_codigo_empresa_j AND Cod_Estado IN ('C', 'P')))
		BEGIN
			--print convert(varchar(10), @v_codigo_empresa_j) + ' - ' + convert(varchar(10), @v_nro_contrato) + 'no tiene cuotas'
			UPDATE dbo.log_proceso_bono SET codigo_estado = @c_estado_error, observacion = 'No tiene cuotas' WHERE codigo_planilla = @p_codigo_planilla AND nro_contrato = @v_nro_contrato and codigo_empresa = @v_codigo_empresa
			FETCH NEXT FROM contrato_bono_cursor
			INTO @v_codigo_empresa_j, @v_nro_contrato, @v_codigo_personal, @v_codigo_empresa, @v_monto_contrato, @v_ventas, @v_codigo_tipo_venta, @v_codigo_moneda, @v_codigo_canal, @v_fecha_contrato, @v_monto_transferencia, @v_codigo_grupo_j

			CONTINUE
		END

		IF (EXISTS(
				SELECT TOP 1 l.nro_contrato FROM dbo.log_proceso_bono l
				INNER JOIN dbo.log_proceso_bono_cabecera c on c.codigo_planilla = l.codigo_planilla
				INNER JOIN dbo.planilla_bono p on p.codigo_planilla = c.codigo_planilla and p.es_planilla_jn = 1
				WHERE 
					l.nro_contrato = @v_nro_contrato
					AND l.codigo_empresa = @v_codigo_empresa
					AND c.codigo_tipo_planilla = @p_codigo_tipo_planilla
					AND p.codigo_estado_planilla = @c_planilla_estado_cerrada
		))
		BEGIN
			--print convert(varchar(10), @v_codigo_empresa_j) + ' - ' + convert(varchar(10), @v_nro_contrato) + 'ya fue pagada'
			UPDATE dbo.log_proceso_bono SET codigo_estado = @c_estado_error, observacion = 'Ya fue pagada' WHERE codigo_planilla = @p_codigo_planilla AND nro_contrato = @v_nro_contrato and codigo_empresa = @v_codigo_empresa
			FETCH NEXT FROM contrato_bono_cursor
			INTO @v_codigo_empresa_j, @v_nro_contrato, @v_codigo_personal, @v_codigo_empresa, @v_monto_contrato, @v_ventas, @v_codigo_tipo_venta, @v_codigo_moneda, @v_codigo_canal, @v_fecha_contrato, @v_monto_transferencia, @v_codigo_grupo_j

			CONTINUE
		END

		--print convert(varchar(8), Fec_Pago, 112) + ' -- ' + convert(varchar(8), @p_fecha_fin, 112)
		-- Monto Ingresado
		set @v_monto_ingresado = 0
		select 
			@v_monto_ingresado = SUM(CONVERT(decimal(12, 4), Num_Importe_Total_SinIgv))
		from dbo.contrato_cuota
		where 
			Codigo_empresa = @v_codigo_empresa_j
			AND NumAtCard = @v_nro_contrato
			--AND convert(varchar(8), Fec_Pago, 112) <= convert(varchar(8), @p_fecha_fin, 112) -- Validar si la fecha de pago es limitante
			AND Cod_Estado IN ('C')
			AND Num_Cuota = 0 --Solo tomara las cuotas iniciales
			AND dbo.fn_proceso_generacion_bono_eval_recupero_essalud(@v_nro_contrato, @v_codigo_empresa_j) = 0

		SET @v_monto_ingresado = ISNULL(@v_monto_ingresado, 0)

		-- Detalle para Articulos
		insert into articulo_planilla_bono (codigo_planilla_bono, codigo_articulo, codigo_empresa, codigo_personal, nro_contrato, monto_contratado, dinero_ingresado)
		select
			@p_codigo_planilla
			,a.codigo_articulo
			,@v_codigo_empresa 
			,@v_codigo_personal
			,@v_nro_contrato
			,case when a.genera_bolsa_bono = 1 then dc.Price * dc.Quantity else 0 end
			,case when a.genera_bono = 1 then ROUND((ROUND((dc.Price * 100) / @v_monto_contrato, 4) / 100) * @v_monto_ingresado, 4) * dc.Quantity else 0 end
		from 
			dbo.detalle_contrato dc 
		inner join dbo.articulo a
			on dc.ItemCode = a.codigo_sku and (a.genera_bono = 1 or a.genera_bolsa_bono = 1) and a.estado_registro = 1
		where 
			dc.Codigo_empresa =  @v_codigo_empresa_j
			AND dc.NumAtCard = @v_nro_contrato

		-- Aquellos articulos que NO generan bono (monto ingresado)
		SET @v_monto_no_bono = 0

		select 
			@v_monto_no_bono = SUM(ROUND((ROUND((dc.Price * 100) / @v_monto_contrato, 4) / 100) * @v_monto_ingresado, 4) * dc.Quantity)
		from 
			dbo.detalle_contrato dc 
		inner join dbo.articulo a
			on dc.ItemCode = a.codigo_sku  and a.genera_bono = 0 and a.estado_registro = 1
		where 
			dc.Codigo_empresa =  @v_codigo_empresa_j
			AND dc.NumAtCard = @v_nro_contrato
			
		SET @v_monto_no_bono = ISNULL(@v_monto_no_bono, 0)
		SET @v_monto_ingresado = ISNULL(@v_monto_ingresado, 0) - ISNULL(@v_monto_no_bono, 0)

		-- Aquellos articulos que NO suman a la bolsa (monto contratado)
		SET @v_monto_no_bolsa = NULL
		select 
			@v_monto_no_bolsa = SUM(dc.Price * dc.Quantity)
		from 
			dbo.detalle_contrato dc 
		inner join dbo.articulo a
			on dc.ItemCode = a.codigo_sku and a.genera_bolsa_bono = 0
		where 
			dc.Codigo_empresa =  @v_codigo_empresa_j
			AND dc.NumAtCard = @v_nro_contrato

		SET @v_monto_no_bolsa = ISNULL(@v_monto_no_bolsa, 0)
		SET @v_monto_contrato = ISNULL(@v_monto_contrato, 0) - ISNULL(@v_monto_no_bolsa, 0)
			
		IF @v_monto_contrato <= 0
		BEGIN
			--print 'no tiene monto contrato'
			UPDATE dbo.log_proceso_bono SET codigo_estado = @c_estado_error, observacion = 'No tiene monto contratado' WHERE codigo_planilla = @p_codigo_planilla AND nro_contrato = @v_nro_contrato and codigo_empresa = @v_codigo_empresa
			FETCH NEXT FROM contrato_bono_cursor   
			INTO @v_codigo_empresa_j, @v_nro_contrato, @v_codigo_personal, @v_codigo_empresa, @v_monto_contrato, @v_ventas, @v_codigo_tipo_venta, @v_codigo_moneda, @v_codigo_canal, @v_fecha_contrato, @v_monto_transferencia, @v_codigo_grupo_j

			CONTINUE
		END
					
		-- Buscamos si tiene Grupo
		IF (@v_codigo_grupo_j = '')
		BEGIN
			SELECT 
				TOP 1 @v_codigo_grupo = CASE WHEN codigo_canal_grupo <> codigo_canal THEN codigo_canal_grupo ELSE 0 END
			FROM 
				dbo.personal_canal_grupo
			WHERE 
				codigo_personal = @v_codigo_personal
				AND codigo_canal = @v_codigo_canal
				AND estado_registro = 1
		END
		ELSE
		BEGIN
			SELECT 
				TOP 1 @v_codigo_grupo = codigo_canal_grupo
			FROM 
				dbo.canal_grupo
			WHERE 
				codigo_equivalencia = @v_codigo_grupo_j
				AND estado_registro = 1
		END

		IF (@v_codigo_grupo IS NULL)
			SET @v_codigo_grupo = 0

		--Supervisor Marco Perez
		SET @v_codigo_supervisor = 1676

		--TODO: TRANSFERENCIA
		IF (@v_monto_transferencia > 0)
		BEGIN
			select 
				@v_cuota_inicial = SUM(CONVERT(decimal(12, 4), Num_Importe_Total_SinIgv))
			from dbo.contrato_cuota
			where 
				Codigo_empresa = @v_codigo_empresa_j
				AND NumAtCard = @v_nro_contrato
				--AND convert(varchar(8), Fec_Pago, 112) <= convert(varchar(8), @p_fecha_fin, 112) -- Validar si la fecha de pago es limitante
				AND Cod_Estado IN ('C')
				AND Num_Cuota = 0 --Solo tomara las cuotas iniciales
				--AND dbo.fn_proceso_generacion_bono_eval_recupero_essalud(@v_nro_contrato, @v_codigo_empresa_j) = 0

			select 
				@v_precio_igv = SUM(ROUND((dc.Price * dc.Quantity), 4)) 
			from 
				dbo.detalle_contrato dc 
			inner join dbo.articulo a
				on dc.ItemCode = a.codigo_sku  and a.genera_bono = 1
			where 
				dc.Codigo_empresa =  @v_codigo_empresa_j
				AND dc.NumAtCard = @v_nro_contrato

			SET @v_factor = @v_precio_igv/@v_monto_contrato
			SET @v_monto_ingresado = (@v_cuota_inicial - @v_monto_transferencia) * @v_factor
		END

		--PRORATEO GENERAL
		SELECT 
			@v_total_articulos = SUM(dinero_ingresado)
		FROM
			dbo.articulo_planilla_bono
		WHERE
			codigo_planilla_bono = @p_codigo_planilla
			and codigo_empresa = @v_codigo_empresa 
			and codigo_personal = @v_codigo_personal
			and nro_contrato = @v_nro_contrato

		IF (@v_total_articulos > 0 AND @v_total_articulos > @v_monto_ingresado)
		BEGIN
			UPDATE
				dbo.articulo_planilla_bono
			SET
				dinero_ingresado = ROUND((((dinero_ingresado * 100) / @v_total_articulos) * @v_monto_ingresado) / 100, 2)
			WHERE
				codigo_planilla_bono = @p_codigo_planilla
				and codigo_empresa = @v_codigo_empresa 
				and codigo_personal = @v_codigo_personal
				and nro_contrato = @v_nro_contrato
		END
		--PRORATEO GENERAL

		--print @v_nro_contrato  + ' - ' + convert(varchar, @v_monto_ingresado)
		IF (@v_monto_ingresado > 0)
		BEGIN
			INSERT INTO dbo.contrato_planilla_bono (codigo_planilla, codigo_personal, numero_contrato, codigo_empresa, codigo_tipo_venta, monto_contratado, monto_ingresado, codigo_supervisor, fecha_contrato, codigo_grupo)
			VALUES(@p_codigo_planilla, @v_codigo_personal, @v_nro_contrato, @v_codigo_empresa, @v_codigo_tipo_venta, @v_monto_contrato, @v_monto_ingresado, @v_codigo_supervisor, @v_fecha_contrato, @v_codigo_grupo)

			SET @v_codigo_personal_original = @v_codigo_personal

			IF (@p_codigo_tipo_planilla = @c_tipo_planilla_supervisor)
				SET @v_codigo_personal = 0
				
			IF EXISTS(SELECT TOP 1 codigo_personal FROM @t_Contrato WHERE codigo_personal = @v_codigo_personal and codigo_empresa = @v_codigo_empresa and codigo_canal = @v_codigo_canal and codigo_grupo = @v_codigo_grupo and codigo_moneda = @v_codigo_moneda)
				UPDATE @t_Contrato
				SET
					monto_ingresado = monto_ingresado + @v_monto_ingresado
					,monto_contrato = monto_contrato + @v_monto_contrato
					,ventas = ventas + @v_ventas
				WHERE
					codigo_personal = @v_codigo_personal 
					and codigo_empresa = @v_codigo_empresa 
					and codigo_canal = @v_codigo_canal 
					and codigo_grupo = @v_codigo_grupo
					and codigo_moneda = @v_codigo_moneda
					
			ELSE
				INSERT INTO @t_Contrato (codigo_personal, codigo_empresa, codigo_canal, codigo_grupo, monto_contrato, ventas, monto_ingresado, codigo_personal_original, codigo_moneda)
				VALUES(@v_codigo_personal, @v_codigo_empresa, @v_codigo_canal, @v_codigo_grupo, @v_monto_contrato, @v_ventas, @v_monto_ingresado, @v_codigo_personal_original, @v_codigo_moneda)

			UPDATE dbo.log_proceso_bono SET codigo_estado = @c_estado_procesado, observacion = '', codigo_grupo = @v_codigo_grupo  WHERE codigo_planilla = @p_codigo_planilla AND nro_contrato = @v_nro_contrato and codigo_empresa = @v_codigo_empresa
		END
		ELSE
		BEGIN
			UPDATE dbo.log_proceso_bono SET codigo_estado = @c_estado_error, observacion = 'El monto de dinero ingresado es cero.', codigo_grupo = @v_codigo_grupo WHERE codigo_planilla = @p_codigo_planilla AND nro_contrato = @v_nro_contrato and codigo_empresa = @v_codigo_empresa
		END
		--print ' fin analisis'
		FETCH NEXT FROM contrato_bono_cursor   
		INTO @v_codigo_empresa_j, @v_nro_contrato, @v_codigo_personal, @v_codigo_empresa, @v_monto_contrato, @v_ventas, @v_codigo_tipo_venta, @v_codigo_moneda, @v_codigo_canal, @v_fecha_contrato, @v_monto_transferencia, @v_codigo_grupo_j
	END   
	CLOSE contrato_bono_cursor;  
	DEALLOCATE contrato_bono_cursor; 

	--Cambio de Supervisor
	UPDATE @t_Contrato
	SET codigo_personal = 1676

	SET @v_nro_registro = 1
	SET @v_total_registros = (SELECT COUNT(id) FROM @t_Contrato)

	--print 'total registros: ' + convert(varchar, @v_total_registros)
	--select 'contrato_planilla_bono', * FROM dbo.contrato_planilla_bono where codigo_planilla = @p_codigo_planilla
	--select *  from @t_Contrato

	WHILE (@v_total_registros > 0)
	BEGIN
		SELECT TOP 1
			@v_codigo_personal = codigo_personal
			,@v_codigo_personal_original = codigo_personal_original 
			,@v_codigo_grupo = codigo_grupo
			,@v_codigo_canal = codigo_canal
		FROM
			@t_Contrato

		SELECT 
			@v_monto_contrato = SUM(ISNULL(monto_contrato, 0))
			,@v_ventas = SUM(ISNULL(ventas, 0))
			,@v_monto_ingresado = SUM(ISNULL(monto_ingresado, 0))
		FROM
			@t_Contrato
		WHERE 
			codigo_personal = @v_codigo_personal
		
		SELECT
			@v_monto_meta_rango_inicio = NULL
			,@v_monto_meta_rango_fin = NULL
			,@v_monto_meta_rango_fin_tmp = NULL
			,@v_porcentaje_pago = NULL
			,@v_monto_tope = NULL
			,@v_cantidad_ventas = NULL
			,@v_meta_lograda = NULL				

		-- REGLA DE PAGO
		SELECT TOP 1
			@v_codigo_regla_bono = codigo_regla_calculo_bono
			,@v_monto_meta_rango_inicio = monto_meta
			,@v_monto_meta_rango_fin = ROUND(monto_meta*10, 2)
			,@v_porcentaje_pago = porcentaje_pago
			,@v_monto_tope = monto_tope
			,@v_cantidad_ventas = cantidad_ventas
			,@v_calcular_igv = calcular_igv
		FROM 
			dbo.pcb_regla_calculo_bono
		INNER JOIN dbo.fn_SplitReglaTipoPlanilla(@v_parametro_canales_jn) x on pcb_regla_calculo_bono.codigo_canal = x.codigo -- /*funes y gestores*/
		WHERE 
			es_jn = 1
			and GETDATE() between vigencia_inicio and vigencia_fin
		ORDER BY codigo_canal, codigo_grupo desc

		-- Procede el pago del Bono
		IF (@v_monto_meta_rango_inicio IS NOT NULL)
		BEGIN

			--print '@v_monto_ingresado [' + convert(varchar(20), @v_monto_ingresado) + ']  @v_monto_meta_rango_inicio [' + convert(varchar(20), @v_monto_meta_rango_inicio) + '] @v_monto_meta_rango_fin [' + convert(varchar(20), @v_monto_meta_rango_fin) + '] @v_porcentaje_pago [' + convert(varchar(20), @v_porcentaje_pago*100) + ']'
			SET @v_meta_lograda = ROUND((@v_monto_ingresado *100) / @v_monto_meta_rango_fin, 4)
				
			-- No cumplio el primer rango, se busca el alternativo
			IF (@v_monto_ingresado not between @v_monto_meta_rango_inicio and @v_monto_meta_rango_fin)
			BEGIN
				IF EXISTS(SELECT codigo_regla_calculo_bono FROM  dbo.pcb_regla_calculo_bono_matriz m WHERE m.codigo_regla_calculo_bono = @v_codigo_regla_bono)
				BEGIN
					SET @v_monto_meta_rango_fin = @v_monto_meta_rango_inicio
					--SELECT
					--	@v_codigo_regla_bono = NULL
					--	,@v_monto_meta_rango_inicio = NULL
					--	,@v_porcentaje_pago = NULL

					SELECT TOP 1
						@v_codigo_regla_bono = codigo_regla_calculo_bono
						,@v_monto_meta_rango_inicio = monto_meta
						,@v_porcentaje_pago = porcentaje_pago
					FROM 
						dbo.pcb_regla_calculo_bono_matriz m
					WHERE
						m.codigo_regla_calculo_bono = @v_codigo_regla_bono

					--SET @v_monto_meta_rango_fin = (CASE WHEN @v_monto_meta_rango_inicio IS NULL THEN @v_monto_meta_rango_fin ELSE @v_monto_meta_rango_fin_tmp END)
				END
			END

			-- Debe estar en el rango y tener porcentaje de pago				
			IF (@v_monto_ingresado between @v_monto_meta_rango_inicio and @v_monto_meta_rango_fin AND @v_porcentaje_pago IS NOT NULL)
			BEGIN
				--select @v_monto_contrato, @v_monto_meta_rango_inicio, @v_monto_meta_rango_fin, @v_porcentaje_pago, @v_codigo_regla_bono, @v_codigo_grupo
					
				IF @v_codigo_personal IS NOT NULL
				BEGIN
					--print ' va a pagar'
					INSERT INTO dbo.resumen_planilla_bono(codigo_planilla, codigo_personal, monto_contratado, monto_ingresado, meta_lograda, porcentaje_pago)
					VALUES(@p_codigo_planilla, @v_codigo_personal, @v_monto_contrato, @v_monto_ingresado, @v_meta_lograda, (@v_porcentaje_pago * 100))

					INSERT INTO @t_Contrato_Pagos (id, codigo_personal, codigo_empresa, codigo_canal, codigo_grupo, monto_contrato, monto_ingresado, codigo_moneda)
					SELECT ID = ROW_NUMBER() OVER (ORDER BY codigo_personal), codigo_personal, codigo_empresa, codigo_canal, codigo_grupo, monto_contrato, monto_ingresado, codigo_moneda
					FROM @t_Contrato 
					WHERE codigo_personal = @v_codigo_personal

					SET @v_total_registros_pagos = (SELECT COUNT(id) FROM @t_Contrato_Pagos)
					SET @v_nro_registro_pagos = 1
					WHILE (@v_nro_registro_pagos <= @v_total_registros_pagos)
					BEGIN
						SELECT 
							@v_codigo_personal = codigo_personal 
							,@v_codigo_empresa = codigo_empresa
							,@v_codigo_canal = codigo_canal
							,@v_codigo_grupo = codigo_grupo
							,@v_monto_contrato = monto_contrato
							--,@v_ventas = ventas
							,@v_monto_ingresado = monto_ingresado
							,@v_codigo_moneda = codigo_moneda
						FROM
							@t_Contrato_Pagos
						WHERE 
							id = @v_nro_registro_pagos

						SET @v_monto_a_pagar = @v_monto_ingresado * @v_porcentaje_pago

						--SET @v_detraccion = dbo.fn_canal_grupo_percibe_factura(@v_codigo_grupo,@v_codigo_empresa,case when @p_codigo_tipo_planilla = 1 then 0 else 1 end,1) 

						IF (@v_calcular_igv = 0)
						BEGIN
							SET @v_monto_a_pagar_bruto = (@v_monto_a_pagar / (@v_igv + 1))
							SET @v_monto_a_pagar_igv = 0
							SET @v_monto_a_pagar = @v_monto_a_pagar_bruto
						END
						ELSE
						BEGIN
								SET @v_monto_a_pagar_bruto = @v_monto_a_pagar
								SET @v_monto_a_pagar = @v_monto_a_pagar * (@v_igv + 1)								
								SET @v_monto_a_pagar_igv = @v_monto_a_pagar - @v_monto_a_pagar_bruto
						END

						INSERT INTO dbo.detalle_planilla_bono (codigo_planilla, codigo_personal, codigo_empresa, codigo_canal, codigo_grupo, codigo_moneda, monto_bruto, monto_igv, monto_neto, monto_contratado, monto_ingresado)
						VALUES(@p_codigo_planilla, @v_codigo_personal, @v_codigo_empresa, @v_codigo_canal, @v_codigo_grupo, @v_codigo_moneda, @v_monto_a_pagar_bruto, @v_monto_a_pagar_igv, @v_monto_a_pagar, @v_monto_contrato, @v_monto_ingresado)
						SET @v_nro_registro_pagos = @v_nro_registro_pagos + 1
					END --(@v_nro_registro_pagos <= @v_total_registros_pagos)
					DELETE FROM @t_Contrato_PAGOS
						
					UPDATE dbo.log_proceso_bono SET codigo_regla_calculo_bono = @v_codigo_regla_bono
					WHERE codigo_planilla = @p_codigo_planilla AND codigo_canal = @v_codigo_canal and codigo_estado = @c_estado_procesado
					AND nro_contrato IN (SELECT numero_contrato FROM dbo.contrato_planilla_bono where codigo_planilla = @p_codigo_planilla AND codigo_personal = CASE WHEN @p_codigo_tipo_planilla = @c_tipo_planilla_vendedor THEN @v_codigo_personal_original ELSE @v_codigo_personal END)
					--print 'meta [' + convert(varchar(10), @v_meta_lograda) + '] monto contratado [' + convert(varchar(10), @v_monto_contrato) + '] porcentaje_pago [' + convert(varchar(10), @v_porcentaje_pago) + ']' 
				END
			END
			ELSE
			BEGIN	
				--print 'no ubico matriz de pago'
				UPDATE dbo.log_proceso_bono SET codigo_estado = @c_estado_error, observacion = 'No cumplio matriz de pago: DI ' + convert(varchar, round(@v_monto_ingresado, 2)) + ' - Rango ' + convert(varchar, round(@v_monto_meta_rango_inicio, 2)) + ' ' + convert(varchar, round(@v_monto_meta_rango_fin, 2)) + ' - % ' + convert(varchar, isnull(round(@v_porcentaje_pago*100, 2), 0)), codigo_regla_calculo_bono = @v_codigo_regla_bono
				WHERE codigo_planilla = @p_codigo_planilla /*AND codigo_canal = @v_codigo_canal*/ and codigo_estado = @c_estado_procesado
				--AND nro_contrato IN (SELECT numero_contrato FROM dbo.contrato_planilla_bono where codigo_planilla = @p_codigo_planilla AND codigo_personal = CASE WHEN @p_codigo_tipo_planilla = @c_tipo_planilla_vendedor THEN @v_codigo_personal_original ELSE @v_codigo_personal END)
				--DELETE FROM dbo.contrato_planilla_bono where codigo_planilla = @p_codigo_planilla AND ( (@p_codigo_tipo_planilla = @c_tipo_planilla_vendedor and codigo_personal = @v_codigo_personal_original) OR (@p_codigo_tipo_planilla = @c_tipo_planilla_supervisor and codigo_supervisor = @v_codigo_personal) )
			END
		END
		ELSE
		BEGIN
			--print 'no tiene regla x grupo '  + convert(varchar(10), @v_codigo_grupo)
			UPDATE dbo.log_proceso_bono SET codigo_estado = @c_estado_error, observacion = 'No tiene regla de pago' 
			WHERE codigo_planilla = @p_codigo_planilla /*AND codigo_canal = @v_codigo_canal*/ and codigo_estado = @c_estado_procesado /*and ( @v_codigo_grupo = 0 OR (@v_codigo_grupo <> 0 AND codigo_grupo = @v_codigo_grupo) )*/
			--AND nro_contrato IN (SELECT numero_contrato FROM dbo.contrato_planilla_bono where codigo_planilla = @p_codigo_planilla AND codigo_personal = CASE WHEN @p_codigo_tipo_planilla = @c_tipo_planilla_vendedor THEN @v_codigo_personal_original ELSE @v_codigo_personal END)
			--DELETE FROM dbo.contrato_planilla_bono where codigo_planilla = @p_codigo_planilla AND ( (@p_codigo_tipo_planilla = @c_tipo_planilla_vendedor and codigo_personal = @v_codigo_personal_original) OR (@p_codigo_tipo_planilla = @c_tipo_planilla_supervisor and codigo_supervisor = @v_codigo_personal) )
		END

		DELETE FROM @t_Contrato WHERE codigo_personal = @v_codigo_personal
		SET @v_total_registros = (SELECT COUNT(id) FROM @t_Contrato)
		--print 'total registros 2: ' + convert(varchar, @v_total_registros)
	END --(@v_total_registros > 0)

	--UPDATE dbo.articulo_planilla_bono 
	--SET 
	--	dinero_ingresado = dinero_ingresado / (@v_igv + 1)
	--WHERE codigo_planilla_bono = @p_codigo_planilla

SET NOCOUNT OFF
END; -- Todo el SP