USE SIGECO
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[up_proceso_generacion_bono]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].up_proceso_generacion_bono
GO

CREATE PROCEDURE [dbo].[up_proceso_generacion_bono]
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
		@v_reglas_bono		int
		,@v_codigo_regla_bono		int
		,@v_nro_contrato			varchar(100)
		,@v_codigo_personal			int
		,@v_codigo_empresa_j		nvarchar(4)
		,@v_codigo_empresa			int
		,@v_monto_contrato			decimal(12, 4)
		,@v_ventas					int
		,@v_codigo_grupo			int
		,@v_monto_meta_rango_inicio	decimal(12, 4)
		,@v_monto_meta_rango_fin	decimal(12, 4)
		,@v_porcentaje_pago			decimal(12, 4)
		,@v_monto_tope				decimal(12, 4)
		,@v_monto_ingresado			decimal(12, 4)
		,@v_monto_a_pagar			decimal(12, 4)
		,@v_igv						decimal(12, 4)
		,@v_monto_a_pagar_igv		decimal(12, 4)
		,@v_monto_a_pagar_bruto		decimal(12, 4)
		,@v_calcular_igv			bit
		,@v_cantidad_ventas			int
		,@v_nro_registro			int
		,@v_total_registros			int
		,@v_codigo_tipo_venta		int
		,@v_codigo_moneda			int
		,@v_monto_no_bolsa			decimal(12, 4)
		,@v_monto_no_bono			decimal(12, 4)
		,@v_meta_lograda			decimal(12, 4)
		,@v_nro_registro_pagos		int
		,@v_total_registros_pagos	int
		,@v_codigo_supervisor		int
		,@v_codigo_personal_original int
		,@v_codigo_empresas			varchar(100)
		,@v_codigo_canal			int
		,@v_fecha_contrato			varchar(10)
		,@v_detraccion				bit
		,@v_total_articulos_MC		decimal(12, 4)
		,@v_precio_igv	decimal(12, 4)
		,@v_factor		decimal(12, 4)
		,@v_monto_transferencia		decimal(12, 4)
		,@v_cuota_inicial			decimal(12, 4)
		,@v_total_articulos			decimal(12, 4)
		,@v_trf_articulos			decimal(12, 4)
		,@v_codigo_supervisor_2		int
		,@v_nombre_supervisor_2		varchar(250)
		,@v_email_supervisor_2		varchar(250)

	DECLARE
		@c_tipo_planilla_vendedor		int = 1
		,@c_tipo_planilla_supervisor	int = 2
		,@c_estado_no_procesado			int = 1
		,@c_estado_error				int = 2
		,@c_estado_procesado			int = 3
		,@c_planilla_estado_cerrada		int = 2
		,@c_categoria_carencia			int = 13 /*Periodo Carencia*/
	
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
		,codigo_supervisor			int
		,nombre_supervisor			varchar(250)
		,email_supervisor			varchar(250)
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
	SET @v_igv = (SELECT TOP 1 ROUND(CONVERT(DECIMAL(12, 4), valor)/100, 4) FROM dbo.parametro_sistema WHERE codigo_parametro_sistema = 9)-- Confirmar el codigo_parametro_sistema

	INSERT INTO dbo.planilla_bono_contratos (id, codigo_empresa_o, codigo_empresa, nro_contrato, codigo_tipo_venta, codigo_canal)
	SELECT
		@p_id_proceso	
		,cc.Codigo_empresa
		,e.codigo_empresa
		,cc.NumAtCard
		,v.codigo_tipo_venta
		,pcg.codigo_canal
	FROM
		dbo.cabecera_contrato cc
	INNER JOIN dbo.empresa_sigeco e
		ON e.codigo_equivalencia = cc.Codigo_empresa
	INNER JOIN dbo.tipo_venta v
		ON v.codigo_equivalencia = cc.Cod_Tipo_Venta
	INNER JOIN dbo.personal p
		ON p.codigo_equivalencia = cc.Cod_Vendedor
	--INNER JOIN dbo.personal_canal_grupo pcg
	--	ON p.codigo_personal = pcg.codigo_personal and pcg.estado_registro = 1 and pcg.es_supervisor_canal = 0 and pcg.es_supervisor_grupo = 0
	INNER JOIN dbo.vw_personal pcg 
		ON pcg.codigo_personal = p.codigo_personal and pcg.estado_canal_grupo = 1 and pcg.es_supervisor_canal = 0 and pcg.es_supervisor_grupo = 0 and cc.codigo_grupo = pcg.codigo_equivalencia_grupo
	WHERE
		(@p_codigo_tipo_planilla = @c_tipo_planilla_supervisor OR (@p_codigo_tipo_planilla = @c_tipo_planilla_vendedor AND pcg.percibe_bono = 1))
		and pcg.codigo_canal=@p_codigo_canal
		AND CONVERT(date, cc.CreateDate) BETWEEN convert(date, @p_fecha_inicio) AND convert(date, @p_fecha_fin)
		--AND dbo.fn_generar_cronograma_comision_eval_recupero_essalud(cc.NumAtCard, cc.Codigo_empresa) = 0
		AND ltrim(rtrim(ISNULL(cc.Cod_Estado_Contrato, ''))) = ''
		--AND (cc.Codigo_empresa + cc.NumAtCard not in ('09390000149912', '09390000150610'))
		  

	IF NOT EXISTS(SELECT TOP 1 nro_contrato FROM dbo.planilla_bono_contratos where id = @p_id_proceso)
		RETURN;

	DECLARE contrato_bono_cursor CURSOR FAST_FORWARD FORWARD_ONLY FOR 
	SELECT 
		cc.codigo_empresa
		,cc.NumAtCard
		,p.codigo_personal
		,pbc.codigo_empresa
		,cc.DocTotal / (@v_igv + 1)
		,cc.Num_Ventas
		,pbc.codigo_tipo_venta--(SELECT TOP 1 tv.codigo_tipo_venta FROM dbo.tipo_venta tv WHERE tv.codigo_equivalencia = cc.Cod_Tipo_Venta) AS Cod_Tipo_Venta
		,(SELECT TOP 1 m.codigo_moneda FROM dbo.moneda m WHERE m.codigo_equivalencia = cc.DocCur) AS DocCur
		,pbc.codigo_canal
		,convert(varchar(10), cc.CreateDate, 103)
		,convert(decimal(12, 4), case when ISNULL(cc.Flg_Transferencia, 0) = 1 then ISNULL(MontoTransferencia, 0.00) / (@v_igv + 1) else 0.00 end)
		,sup.codigo_personal
	FROM
		dbo.planilla_bono_contratos pbc
	INNER JOIN dbo.cabecera_contrato cc
		on pbc.id = @p_id_proceso and pbc.nro_contrato = cc.NumAtCard and pbc.codigo_empresa_o = cc.Codigo_empresa
	INNER JOIN dbo.personal p
		on p.codigo_equivalencia = cc.Cod_Vendedor
	INNER JOIN dbo.personal sup
		on sup.codigo_equivalencia = cc.Cod_Supervisor
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
		dbo.planilla_bono_contratos pbc
	INNER JOIN dbo.cabecera_contrato cc
		on pbc.id = @p_id_proceso and pbc.nro_contrato = cc.NumAtCard and pbc.codigo_empresa_o = cc.Codigo_empresa
	INNER JOIN dbo.personal p
		on p.codigo_equivalencia = cc.Cod_Vendedor
	WHERE
		--cc.Flg_Documentacion_Completa = '01' and 
		pbc.id = @p_id_proceso

	OPEN contrato_bono_cursor
	FETCH NEXT FROM contrato_bono_cursor
	INTO @v_codigo_empresa_j, @v_nro_contrato, @v_codigo_personal, @v_codigo_empresa, @v_monto_contrato, @v_ventas, @v_codigo_tipo_venta, @v_codigo_moneda, @v_codigo_canal, @v_fecha_contrato, @v_monto_transferencia, @v_codigo_supervisor_2

	WHILE @@FETCH_STATUS = 0  
	BEGIN 
		--print ' analizando: ' + convert(varchar, @v_codigo_empresa_j) + ' - ' + convert(varchar, @v_nro_contrato) + ' - ' + convert(varchar, @v_codigo_personal)
		IF (NOT EXISTS(SELECT TOP 1 ItemCode FROM dbo.detalle_contrato WHERE NumAtCard = @v_nro_contrato and Codigo_empresa = @v_codigo_empresa_j))
		BEGIN
			--print convert(varchar(10), @v_codigo_empresa_j) + ' - ' + convert(varchar(10), @v_nro_contrato) + 'no tiene articulos'
			UPDATE dbo.log_proceso_bono SET codigo_estado = @c_estado_error, observacion = 'No tiene articulos' WHERE codigo_planilla = @p_codigo_planilla AND nro_contrato = @v_nro_contrato AND codigo_empresa = @v_codigo_empresa
			FETCH NEXT FROM contrato_bono_cursor   
			INTO @v_codigo_empresa_j, @v_nro_contrato, @v_codigo_personal, @v_codigo_empresa, @v_monto_contrato, @v_ventas, @v_codigo_tipo_venta, @v_codigo_moneda, @v_codigo_canal, @v_fecha_contrato, @v_monto_transferencia, @v_codigo_supervisor_2

			CONTINUE
		END

		IF (NOT EXISTS(SELECT TOP 1 Num_Cuota FROM dbo.contrato_cuota WHERE NumAtCard = @v_nro_contrato and Codigo_empresa = @v_codigo_empresa_j AND Cod_Estado IN ('C', 'P')))
		BEGIN
			--print convert(varchar(10), @v_codigo_empresa_j) + ' - ' + convert(varchar(10), @v_nro_contrato) + 'no tiene cuotas'
			UPDATE dbo.log_proceso_bono SET codigo_estado = @c_estado_error, observacion = 'No tiene cuotas' WHERE codigo_planilla = @p_codigo_planilla AND nro_contrato = @v_nro_contrato and codigo_empresa = @v_codigo_empresa
			FETCH NEXT FROM contrato_bono_cursor
			INTO @v_codigo_empresa_j, @v_nro_contrato, @v_codigo_personal, @v_codigo_empresa, @v_monto_contrato, @v_ventas, @v_codigo_tipo_venta, @v_codigo_moneda, @v_codigo_canal, @v_fecha_contrato, @v_monto_transferencia, @v_codigo_supervisor_2

			CONTINUE
		END

		IF (EXISTS(
				SELECT TOP 1 l.nro_contrato FROM dbo.log_proceso_bono l
				INNER JOIN dbo.log_proceso_bono_cabecera c on c.codigo_planilla = l.codigo_planilla
				INNER JOIN dbo.planilla_bono p on p.codigo_planilla = c.codigo_planilla
				WHERE 
					l.nro_contrato = @v_nro_contrato
					AND l.codigo_empresa = @v_codigo_empresa
					AND c.codigo_tipo_planilla = @p_codigo_tipo_planilla
					AND p.codigo_estado_planilla = @c_planilla_estado_cerrada
					AND l.codigo_estado = @c_estado_procesado
		))
		BEGIN
			--print convert(varchar(10), @v_codigo_empresa_j) + ' - ' + convert(varchar(10), @v_nro_contrato) + 'ya fue pagada'
			UPDATE dbo.log_proceso_bono SET codigo_estado = @c_estado_error, observacion = 'Ya fue pagada' WHERE codigo_planilla = @p_codigo_planilla AND nro_contrato = @v_nro_contrato and codigo_empresa = @v_codigo_empresa
			FETCH NEXT FROM contrato_bono_cursor
			INTO @v_codigo_empresa_j, @v_nro_contrato, @v_codigo_personal, @v_codigo_empresa, @v_monto_contrato, @v_ventas, @v_codigo_tipo_venta, @v_codigo_moneda, @v_codigo_canal, @v_fecha_contrato, @v_monto_transferencia, @v_codigo_supervisor_2

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

		--IF @v_monto_ingresado <= 0 and @v_monto_no_bono <= 0
		--BEGIN
		--	--print 'no tiene monto ingresado'
		--	UPDATE dbo.log_proceso_bono SET codigo_estado = @c_estado_error, observacion = 'No tiene monto ingresado' WHERE codigo_planilla = @p_codigo_planilla AND nro_contrato = @v_nro_contrato and codigo_empresa = @v_codigo_empresa
		--	FETCH NEXT FROM contrato_bono_cursor   
		--	INTO @v_codigo_empresa_j, @v_nro_contrato, @v_codigo_personal, @v_codigo_empresa, @v_monto_contrato, @v_ventas, @v_codigo_tipo_venta, @v_codigo_moneda, @v_codigo_canal, @v_fecha_contrato

		--	CONTINUE
		--END

		--print convert(@v_codigo_canal) ''

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
			INTO @v_codigo_empresa_j, @v_nro_contrato, @v_codigo_personal, @v_codigo_empresa, @v_monto_contrato, @v_ventas, @v_codigo_tipo_venta, @v_codigo_moneda, @v_codigo_canal, @v_fecha_contrato, @v_monto_transferencia, @v_codigo_supervisor_2

			CONTINUE
		END
					
		-- Buscamos si tiene Grupo
		SELECT 
			TOP 1 @v_codigo_grupo = CASE WHEN codigo_canal_grupo <> codigo_canal THEN codigo_canal_grupo ELSE 0 END
		FROM 
			dbo.personal_canal_grupo
		WHERE 
			codigo_personal = @v_codigo_personal
			AND codigo_canal = @v_codigo_canal
			AND estado_registro = 1

		IF (@v_codigo_grupo IS NULL)
			SET @v_codigo_grupo = 0

		SET @v_codigo_supervisor = NULL
		IF (@p_codigo_tipo_planilla = @c_tipo_planilla_supervisor)
		BEGIN
			SELECT
				@v_codigo_supervisor = codigo_personal
			FROM
				dbo.personal_canal_grupo
			WHERE
				codigo_canal = @v_codigo_canal
				AND ((@v_codigo_grupo = 0) OR (@v_codigo_grupo > 0 AND codigo_canal_grupo = @v_codigo_grupo))
				AND (es_supervisor_canal = 1 OR es_supervisor_grupo = 1)
				AND percibe_bono = 1
				AND estado_registro = 1
		END

		--TODO: TRANSFERENCIA
		--EXEC dbo.up_proceso_generacion_bono_transferencia @p_codigo_planilla, @p_codigo_tipo_planilla, @v_nro_contrato, @v_codigo_empresa_j, @v_monto_ingresado OUTPUT
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
			SET @v_monto_contrato = @v_monto_contrato - @v_monto_transferencia
		END

		SET @v_total_articulos_MC = 0
		--PRORATEO GENERAL
		SELECT 
			@v_total_articulos = SUM(dinero_ingresado)
			,@v_total_articulos_MC = SUM(monto_contratado)
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
				,monto_contratado = ROUND((((monto_contratado * 100) / @v_total_articulos_MC) * @v_monto_contrato) / 100, 2)
			WHERE
				codigo_planilla_bono = @p_codigo_planilla
				and codigo_empresa = @v_codigo_empresa 
				and codigo_personal = @v_codigo_personal
				and nro_contrato = @v_nro_contrato
		END
		--PRORATEO GENERAL

		--Que tenga algun valor
		IF (ISNULL(@v_monto_ingresado + @v_monto_contrato, 0.00) > 0.01)
		BEGIN

			INSERT INTO dbo.contrato_planilla_bono (codigo_planilla, codigo_personal, numero_contrato, codigo_empresa, codigo_tipo_venta, monto_contratado, monto_ingresado, codigo_supervisor, fecha_contrato)
			VALUES(@p_codigo_planilla, @v_codigo_personal, @v_nro_contrato, @v_codigo_empresa, @v_codigo_tipo_venta, @v_monto_contrato, @v_monto_ingresado, @v_codigo_supervisor, @v_fecha_contrato)

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
				INSERT INTO @t_Contrato (codigo_personal, codigo_empresa, codigo_canal, codigo_grupo, monto_contrato, ventas, monto_ingresado, codigo_personal_original, codigo_moneda, codigo_supervisor)
				VALUES(@v_codigo_personal, @v_codigo_empresa, @v_codigo_canal, @v_codigo_grupo, @v_monto_contrato, @v_ventas, @v_monto_ingresado, @v_codigo_personal_original, @v_codigo_moneda, @v_codigo_supervisor_2)

			UPDATE dbo.log_proceso_bono SET codigo_estado = @c_estado_procesado, observacion = '', codigo_grupo = @v_codigo_grupo  WHERE codigo_planilla = @p_codigo_planilla AND nro_contrato = @v_nro_contrato and codigo_empresa = @v_codigo_empresa
		END
		ELSE
		BEGIN
			UPDATE dbo.log_proceso_bono SET codigo_estado = @c_estado_error, observacion = 'El monto de comision por transferencia es menor al monto del contrato referenciado.', codigo_grupo = @v_codigo_grupo WHERE codigo_planilla = @p_codigo_planilla AND nro_contrato = @v_nro_contrato and codigo_empresa = @v_codigo_empresa
		END
		--print ' fin analisis'
		FETCH NEXT FROM contrato_bono_cursor   
		INTO @v_codigo_empresa_j, @v_nro_contrato, @v_codigo_personal, @v_codigo_empresa, @v_monto_contrato, @v_ventas, @v_codigo_tipo_venta, @v_codigo_moneda, @v_codigo_canal, @v_fecha_contrato, @v_monto_transferencia, @v_codigo_supervisor_2
	END   
	CLOSE contrato_bono_cursor;  
	DEALLOCATE contrato_bono_cursor; 

	--print 'Ahora los pagos'
	--select 'temporal de contratos', * from @t_Contrato

	IF (@p_codigo_tipo_planilla = @c_tipo_planilla_supervisor)
	BEGIN
		SET @v_nro_registro = 1
		SET @v_total_registros = (SELECT COUNT(id) FROM @t_Contrato)

		WHILE (@v_nro_registro <= @v_total_registros)
		BEGIN
			SELECT
				@v_codigo_grupo = codigo_grupo
				,@v_codigo_canal = codigo_canal
			FROM
				@t_Contrato
			WHERE
				ID = @v_nro_registro

			SET @v_codigo_personal = NULL
			SELECT
				@v_codigo_personal = codigo_personal
			FROM
				dbo.personal_canal_grupo
			WHERE
				codigo_canal = @v_codigo_canal
				AND ((@v_codigo_grupo = 0) OR (@v_codigo_grupo > 0 AND codigo_canal_grupo = @v_codigo_grupo))
				AND (es_supervisor_canal = 1 OR es_supervisor_grupo = 1)
				AND percibe_bono = 1
				AND estado_registro = 1

			UPDATE @t_Contrato
			SET codigo_personal = @v_codigo_personal
			WHERE
				ID = @v_nro_registro

			SET @v_nro_registro = @v_nro_registro + 1
		END
		--select 'temporal de contratos 2', * from @t_Contrato

		DELETE FROM dbo.contrato_planilla_bono WHERE codigo_personal in (SELECT codigo_personal_original FROM @t_Contrato WHERE codigo_personal is null)
	END

	IF (@p_codigo_tipo_planilla = @c_tipo_planilla_vendedor)
	BEGIN
		UPDATE t
		SET
			codigo_supervisor = v.codigo_personal, nombre_supervisor = v.nombre_personal, email_supervisor = v.correo_electronico 
		FROM @t_Contrato t
		INNER JOIN vw_personal v
			ON v.es_supervisor_grupo = 1 and v.estado_canal_grupo = 1 and v.estado_persona = 1 and t.codigo_supervisor = v.codigo_personal
	END

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
			,@v_codigo_supervisor_2 = codigo_supervisor
			,@v_nombre_supervisor_2 = nombre_supervisor
			,@v_email_supervisor_2 = email_supervisor
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
			,@v_monto_meta_rango_fin = 9999999.99
			,@v_porcentaje_pago = NULL
			,@v_monto_tope = NULL
			,@v_cantidad_ventas = NULL
			,@v_meta_lograda = NULL				

		select top 1
			@v_codigo_regla_bono = codigo_regla_calculo_bono
			,@v_monto_meta_rango_inicio = monto_meta
			,@v_porcentaje_pago = porcentaje_pago
			,@v_monto_tope = monto_tope
			,@v_cantidad_ventas = cantidad_ventas
			,@v_calcular_igv = calcular_igv
		from 
			dbo.pcb_regla_calculo_bono
		where 
			codigo_canal = @v_codigo_canal
			and codigo_tipo_planilla = @p_codigo_tipo_planilla
			and GETDATE() between vigencia_inicio and vigencia_fin
			and ((codigo_grupo IS NULL) OR (codigo_grupo IS NOT NULL AND codigo_grupo = @v_codigo_grupo))
		order by codigo_canal, codigo_grupo desc

		-- Procede el pago del Bono
		IF (@v_monto_meta_rango_inicio IS NOT NULL)
		BEGIN
			IF (@v_ventas >= @v_cantidad_ventas)
			BEGIN				
				--print '@v_monto_contrato [' + convert(varchar(20), @v_monto_contrato) + ']  @v_monto_meta_rango_inicio [' + convert(varchar(20), @v_monto_meta_rango_inicio) + '] @v_monto_meta_rango_fin [' + convert(varchar(20), @v_monto_meta_rango_fin) + ']'
				SET @v_meta_lograda = ROUND((@v_monto_contrato *100) / @v_monto_meta_rango_inicio, 4)
				
				-- Procedemos con la validacion de la regla a nivel 100%
				IF (@v_monto_contrato NOT BETWEEN @v_monto_meta_rango_inicio and @v_monto_meta_rango_fin)
				BEGIN
					--print 'rango alternativo'					
					SET @v_monto_meta_rango_fin = @v_monto_meta_rango_inicio
					SET @v_porcentaje_pago = NULL
					--Obtenemos segun la matriz de posibles valores alternos
					select top 1 
						@v_porcentaje_pago = ROUND(porcentaje_pago / 100, 4)
						,@v_monto_meta_rango_inicio = monto_meta 
					from dbo.regla_calculo_bono_matriz 
					where 
						codigo_regla_calculo_bono = @v_codigo_regla_bono
						AND @v_monto_contrato >= monto_meta order by monto_meta desc				
				
					--print '@v_monto_contrato [' + convert(varchar(20), @v_monto_contrato) + ']  @v_monto_meta_rango_inicio [' + convert(varchar(20), @v_monto_meta_rango_inicio) + '] @v_monto_meta_rango_fin [' + convert(varchar(20), @v_monto_meta_rango_fin) + '] @v_porcentaje_pago [' + convert(varchar(10), @v_porcentaje_pago) + ']'
				END
				
				-- Debe estar en el rango y tener porcentaje de pago				
				IF (@v_monto_contrato between @v_monto_meta_rango_inicio and @v_monto_meta_rango_fin AND @v_porcentaje_pago IS NOT NULL)
				BEGIN
					--select @v_monto_contrato, @v_monto_meta_rango_inicio, @v_monto_meta_rango_fin, @v_porcentaje_pago, @v_codigo_regla_bono, @v_codigo_grupo
					
					--SET @v_monto_a_pagar = @v_monto_ingresado * @v_porcentaje_pago
					
					IF (@v_monto_a_pagar > @v_monto_tope)
						SET @v_monto_a_pagar = @v_monto_tope
					
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

							SET @v_detraccion = dbo.fn_canal_grupo_percibe_factura(@v_codigo_grupo,@v_codigo_empresa,case when @p_codigo_tipo_planilla = 1 then 0 else 1 end,1) 

							IF (@v_calcular_igv = 0 and @v_detraccion = 0)
							BEGIN
								SET @v_monto_a_pagar_bruto = @v_monto_a_pagar--(@v_monto_a_pagar / (@v_igv + 1))
								SET @v_monto_a_pagar_igv = 0
								SET @v_monto_a_pagar = @v_monto_a_pagar--@v_monto_a_pagar * (@v_igv + 1)
							END
							ELSE
							BEGIN
								SET @v_monto_a_pagar_bruto = @v_monto_a_pagar
								SET @v_monto_a_pagar = @v_monto_a_pagar * (@v_igv + 1)
								SET @v_monto_a_pagar_igv = @v_monto_a_pagar - @v_monto_a_pagar_bruto
							END

							INSERT INTO dbo.detalle_planilla_bono (codigo_planilla, codigo_personal, codigo_empresa, codigo_canal, codigo_grupo, codigo_moneda, monto_bruto, monto_igv, monto_neto, monto_contratado, monto_ingresado, codigo_supervisor, nombre_supervisor, email_supervisor)
							VALUES(@p_codigo_planilla, @v_codigo_personal, @v_codigo_empresa, @v_codigo_canal, @v_codigo_grupo, @v_codigo_moneda, @v_monto_a_pagar_bruto, @v_monto_a_pagar_igv, @v_monto_a_pagar, @v_monto_contrato, @v_monto_ingresado, @v_codigo_supervisor_2, @v_nombre_supervisor_2, @v_email_supervisor_2)
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
					UPDATE dbo.log_proceso_bono SET codigo_estado = @c_estado_error, observacion = 'No cumplio matriz de pago: MC ' + convert(varchar, round(@v_monto_contrato, 2)) + ' - Rango ' + convert(varchar, round(@v_monto_meta_rango_inicio, 2)) + ' ' + convert(varchar, round(@v_monto_meta_rango_fin,2)) + ' - % ' + convert(varchar, isnull(round(@v_porcentaje_pago, 2), 0)), codigo_regla_calculo_bono = @v_codigo_regla_bono
					WHERE codigo_planilla = @p_codigo_planilla AND codigo_canal = @v_codigo_canal and codigo_estado = @c_estado_procesado
					AND nro_contrato IN (SELECT numero_contrato FROM dbo.contrato_planilla_bono where codigo_planilla = @p_codigo_planilla AND codigo_personal = CASE WHEN @p_codigo_tipo_planilla = @c_tipo_planilla_vendedor THEN @v_codigo_personal_original ELSE @v_codigo_personal END)
					--DELETE FROM dbo.contrato_planilla_bono where codigo_planilla = @p_codigo_planilla AND ( (@p_codigo_tipo_planilla = @c_tipo_planilla_vendedor and codigo_personal = @v_codigo_personal_original) OR (@p_codigo_tipo_planilla = @c_tipo_planilla_supervisor and codigo_supervisor = @v_codigo_personal) )
				END
			END
			ELSE
			BEGIN
				--print 'no llego a cant ventas '  + convert(varchar(10), @v_codigo_grupo)
				UPDATE dbo.log_proceso_bono SET codigo_estado = @c_estado_error, observacion = 'No llego a cantidad de ventas: ' + CONVERT(VARCHAR, @v_ventas) + ' de ' + CONVERT(VARCHAR, @v_cantidad_ventas)
				WHERE codigo_planilla = @p_codigo_planilla AND codigo_canal = @v_codigo_canal and codigo_estado = @c_estado_procesado and ( @v_codigo_grupo = 0 OR (@v_codigo_grupo <> 0 AND codigo_grupo = @v_codigo_grupo) )
				AND nro_contrato IN (SELECT numero_contrato FROM dbo.contrato_planilla_bono where codigo_planilla = @p_codigo_planilla AND codigo_personal = CASE WHEN @p_codigo_tipo_planilla = @c_tipo_planilla_vendedor THEN @v_codigo_personal_original ELSE @v_codigo_personal END)
				--DELETE FROM dbo.contrato_planilla_bono where codigo_planilla = @p_codigo_planilla AND ( (@p_codigo_tipo_planilla = @c_tipo_planilla_vendedor and codigo_personal = @v_codigo_personal_original) OR (@p_codigo_tipo_planilla = @c_tipo_planilla_supervisor and codigo_supervisor = @v_codigo_personal) )
			END
		END
		ELSE
		BEGIN
			--print 'no tiene regla x grupo '  + convert(varchar(10), @v_codigo_grupo)
			UPDATE dbo.log_proceso_bono SET codigo_estado = @c_estado_error, observacion = 'No tiene regla de pago' 
			WHERE codigo_planilla = @p_codigo_planilla AND codigo_canal = @v_codigo_canal and codigo_estado = @c_estado_procesado and ( @v_codigo_grupo = 0 OR (@v_codigo_grupo <> 0 AND codigo_grupo = @v_codigo_grupo) )
			AND nro_contrato IN (SELECT numero_contrato FROM dbo.contrato_planilla_bono where codigo_planilla = @p_codigo_planilla AND codigo_personal = CASE WHEN @p_codigo_tipo_planilla = @c_tipo_planilla_vendedor THEN @v_codigo_personal_original ELSE @v_codigo_personal END)
			--DELETE FROM dbo.contrato_planilla_bono where codigo_planilla = @p_codigo_planilla AND ( (@p_codigo_tipo_planilla = @c_tipo_planilla_vendedor and codigo_personal = @v_codigo_personal_original) OR (@p_codigo_tipo_planilla = @c_tipo_planilla_supervisor and codigo_supervisor = @v_codigo_personal) )
		END

		DELETE FROM @t_Contrato WHERE codigo_personal = @v_codigo_personal
		SET @v_total_registros = (SELECT COUNT(id) FROM @t_Contrato)
		--print 'total registros 2: ' + convert(varchar, @v_total_registros)
	END --(@v_total_registros > 0)

	EXEC dbo.up_proceso_generacion_bono_ceros @p_codigo_planilla

	IF (@p_codigo_tipo_planilla = @c_tipo_planilla_vendedor)
		EXEC dbo.up_proceso_generacion_bono_tope @p_codigo_planilla, @p_codigo_tipo_planilla

SET NOCOUNT OFF
END; -- Todo el SP