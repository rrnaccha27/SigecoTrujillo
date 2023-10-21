CREATE PROCEDURE [dbo].up_generar_cronograma_comision_vendedor
(
	@p_codigo_empresa		nvarchar(4),
	@p_nro_contrato			nvarchar(200),
	@p_resultado			bit output,
	@p_observacion			varchar(200) output,
	@p_codigo_cronograma	int output,
	@p_codigo_proceso		uniqueidentifier output
)
AS
BEGIN

	SET NOEXEC OFF
	SET NOCOUNT ON

	DECLARE
		@v_codigo_proceso		uniqueidentifier,
		@v_codigo_empresa		int,
		@v_genera_comision		bit,
		@v_codigo_vendedor      nvarchar(20),
		@v_codigo_tipo_venta    int,
		@v_codigo_tipo_pago     int,
		@v_codigo_tipo_pago_o   int,
		@v_codigo_moneda        int,
		@v_flag_documentacion	bit,
		@v_monto_recupero		DECIMAL(12, 4),
		@v_doc_total			DECIMAL(12, 4),
		@v_procesado			bit,
		@v_codigo_camposanto	int, 
		@v_codigo_articulo		int, 
		@v_nombre_articulo		varchar(100),
		@v_cantidad				int,
		@v_cantidad_calculo		int,
		@v_codigo_registro		int, 
		@v_codigo_canal			int,
		@v_codigo_precio		int, 
		@v_precio_total			DECIMAL(12, 4),
		@v_codigo_tipo_comision	int,
		@v_valor				DECIMAL(12, 4),
		@v_monto_comision		DECIMAL(12, 4),
		@v_monto_comision_frac	DECIMAL(12, 4),
		@v_tipo_pago_comision	int, 
		@v_valor_inicial_pago	DECIMAL(12, 4), 
		@v_valor_cuota_pago		DECIMAL(12, 4),
		@v_fecha_habilitacion	datetime,
		@v_fecha_pago_CI		datetime,
		@v_importe_cuota		DECIMAL(12, 4),
		@v_NumCuota				int,
		@v_crear_cronograma		bit,
		@v_nro_cuotas_exc		int,
		@v_tiene_excepcion		int,
		@v_valor_promocion		int
		
	DECLARE 
		@v_codigo_cronograma		int,
		@v_codigo_tipo_planilla		int,
		@v_fecha_programada			int,
		@v_igv						DECIMAL(12, 4),
		@v_monto_neto				DECIMAL(12, 4),
		@v_monto_igv				DECIMAL(12, 4),
		@v_codigo_tipo_cuota		int,
		@v_codigo_estado_cuota		int,
		@v_cantidad_cuotas			int,
		@v_articulos_contrato		int,
		@v_articulos_comision		int,
		@v_articulos_no_comision	int,
		@v_articulos_no_codigo		int,
		@p_retorno					int,
		@p_codigo_regla_pago		int,
		@v_indice					int,
		@v_esArticuloAdicional		bit = 0,
		@v_precio_venta				decimal(12, 4)

	DECLARE
		@v_codigo_personal_cm			INT
		,@v_codigo_vendedor_cm			VARCHAR(10)
		,@v_cantidad_articulos_cm		INT = 0
		,@v_codigo_estado_proceso_cm	INT
		,@v_cantidad_analizados_cm		INT = 0
		,@v_es_comision_manual			BIT = 0
		,@v_esContratoAdicional			BIT = 0
		,@v_fecha_proceso				DATETIME
		,@v_esContratoServFun			BIT = 0
		,@v_es_transferencia			BIT = 0
		,@v_codigo_grupo_j				VARCHAR(60)

	IF (SELECT CURSOR_STATUS('global','articulo_cursor')) >= -1
	BEGIN
		IF (SELECT CURSOR_STATUS('global','articulo_cursor')) > -1
		BEGIN
			CLOSE articulo_cursor
		END
		DEALLOCATE articulo_cursor
	END

	--IF (SELECT CURSOR_STATUS('global','canal_cursor')) >= -1
	--BEGIN
	--	IF (SELECT CURSOR_STATUS('global','canal_cursor')) > -1
	--	BEGIN
	--		CLOSE canal_cursor
	--	END
	--	DEALLOCATE canal_cursor
	--END

	SET @v_codigo_proceso = NEWID()
	SET @p_codigo_proceso = @v_codigo_proceso
	SET @p_resultado = 0

	--INSERT INTO dbo.log_proceso_contrato(id, nro_contrato, codigo_empresa, mensaje) values(@v_codigo_proceso, @p_nro_contrato, @p_codigo_empresa, 'Inicio Proceso Calculo Comision')
	print 'Inicio Proceso Contrato'

	--Identificadores del Contrato/Proceso
	SELECT TOP 1
		@v_codigo_vendedor = cc.Cod_Vendedor,
		@v_codigo_tipo_venta = tv.codigo_tipo_venta,--CASE WHEN cc.Cod_Tipo_Venta = 'NF' THEN 2 ELSE 1 END,
		@v_codigo_tipo_pago = tp.codigo_tipo_pago,--CASE WHEN Cod_FormaPago = 'CRED' THEN 2 ELSE 1 END,
		@v_codigo_tipo_pago_o = tp.codigo_tipo_pago,
		@v_codigo_moneda = m.codigo_moneda,--CASE WHEN DocCur = 'SOL' THEN 1 ELSE 2 END,
		@v_flag_documentacion = CASE WHEN cc.Flg_Documentacion_Completa = '01' THEN 1 ELSE 0 END,
		@v_monto_recupero = CASE WHEN cc.Flg_Recupero = 1 THEN cc.Monto_Recupero ELSE 0 END,
		@v_doc_total = cc.DocTotal,
		@v_fecha_proceso = cc.CreateDate,
		@v_es_transferencia = ISNULL(cc.Flg_Transferencia, 0),
		@v_codigo_grupo_j = ISNULL(cc.codigo_grupo,'')
	FROM dbo.cabecera_contrato cc
	INNER JOIN dbo.tipo_venta tv
		ON tv.codigo_equivalencia = cc.Cod_Tipo_Venta
	INNER JOIN dbo.tipo_pago tp
		ON tp.codigo_equivalencia = cc.Cod_FormaPago
	INNER JOIN dbo.moneda m
		ON m.codigo_equivalencia = cc.DocCur
	WHERE cc.NumAtCard = @p_nro_contrato AND cc.Codigo_empresa = @p_codigo_empresa

	--IF (@p_codigo_empresa = @c_FUNJAR)
	--	IF EXISTS(SELECT TOP 1 NumAtCard FROM dbo.cabecera_contrato WHERE NumAtCard = @p_nro_contrato AND Codigo_empresa = @c_OFSA)
	--		SET @v_EsContratoDoble = 1

	IF (@v_codigo_vendedor IS NULL OR @v_codigo_tipo_venta IS NULL)
	BEGIN
		SET @p_observacion = 'Datos de contrato no coinciden cron los del aplicativo.'
		--PRINT @p_observacion	
		SET NOEXEC ON;
	END

	-- REGLA RECUPERO
	IF (@v_monto_recupero > 0)
	BEGIN
		DECLARE @v_nro_cuota INT

		SELECT TOP 1
			@v_nro_cuota = nro_cuota
		FROM dbo.pcc_regla_recupero
		WHERE 
			@v_fecha_proceso BETWEEN vigencia_inicio AND vigencia_fin
 
		IF (@v_nro_cuota IS NOT NULL)
		BEGIN
			SET @v_importe_cuota = ISNULL((SELECT TOP 1 Num_Importe_Total FROM dbo.contrato_cuota WHERE Num_Cuota = @v_nro_cuota AND NumAtCard = @p_nro_contrato and Codigo_empresa = @p_codigo_empresa AND Cod_Estado IN ('C', 'P') ORDER BY Fec_Vencimiento ASC), 0)
			IF EXISTS (SELECT NumAtcard FROM dbo.contrato_cuota WHERE NumAtCard = @p_nro_contrato and Codigo_empresa = @p_codigo_empresa AND Cod_Estado IN ('C', 'P') GROUP BY NumAtCard HAVING MAX(Num_Cuota) = @v_nro_cuota)
				IF (@v_importe_cuota = @v_monto_recupero)
					SET @v_codigo_tipo_pago = 1-- Tipo Pago al Contado
		END
	END

	--Algunos valores que no cambiaran en todo el proceso
	SET @v_igv = (SELECT TOP 1 ((CONVERT(DECIMAL(12, 4), valor)/100) + 1) FROM dbo.parametro_sistema WHERE codigo_parametro_sistema = 9)-- Confirmar el codigo_parametro_sistema
	SET @v_codigo_empresa = (SELECT TOP 1 codigo_empresa FROM dbo.empresa_sigeco WHERE codigo_equivalencia = @p_codigo_empresa)
	SET @v_codigo_camposanto = (SELECT TOP 1 cs.codigo_campo_santo FROM dbo.detalle_contrato dc INNER JOIN dbo.campo_santo_sigeco cs on cs.codigo_equivalencia = dc.codigo_camposanto WHERE dc.numatcard = @p_nro_contrato and dc.codigo_empresa = @p_codigo_empresa)
 
	SET @v_articulos_contrato = ISNULL((SELECT COUNT(ItemCode) FROM dbo.detalle_contrato WHERE NumAtCard = @p_nro_contrato and Codigo_empresa = @p_codigo_empresa), 0)
	IF (@v_articulos_contrato = 0)
	BEGIN
		SET @p_observacion = 'No existen articulos para este contrato'
		--PRINT @p_observacion	
		SET NOEXEC ON;
	END

	IF (NOT EXISTS(SELECT TOP 1 Num_Cuota FROM dbo.contrato_cuota WHERE NumAtCard = @p_nro_contrato and Codigo_empresa = @p_codigo_empresa AND Cod_Estado IN ('C', 'P')))
	BEGIN
		IF (@v_monto_recupero < @v_doc_total)
		BEGIN
			SET @p_observacion = 'No existen cuotas para este contrato'
			--PRINT @p_observacion 
			SET NOEXEC ON;
		END
	END
	ELSE
		IF NOT EXISTS(
			SELECT NumAtCard FROM dbo.contrato_cuota
			WHERE 
				NumAtCard = @p_nro_contrato AND Codigo_empresa = @p_codigo_empresa AND Cod_Estado IN ('C', 'P')
			GROUP BY NumAtCard 
				HAVING COUNT(DISTINCT(num_cuota)) = COUNT(num_cuota))
		BEGIN
			SET @p_observacion = 'Tiene cuotas duplicadas con estado cancelado o pendiente'
			--PRINT @p_observacion 
			SET NOEXEC ON;
		END

	IF (NOT EXISTS(SELECT TOP 1 codigo_personal FROM dbo.personal where codigo_equivalencia = @v_codigo_vendedor and estado_registro = 1))
	BEGIN
		SET @p_observacion = 'No existe (o esta inactivo) el personal de ventas: ' + @v_codigo_vendedor
		--PRINT @p_observacion 
		SET NOEXEC ON;
	END

	INSERT INTO dbo.detalle_comision_manual
	SELECT DISTINCT @v_codigo_proceso, cm.codigo_comision_manual, cm.codigo_articulo, cm.comision FROM dbo.comision_manual cm
	INNER JOIN 
		(select dif.NUMATCARD, e.codigo_empresa, td.codigo_tipo_documento, dif.NUM_DOC, (select top 1 p.codigo_personal from cabecera_contrato cc inner join personal p on Cod_Vendedor = p.codigo_equivalencia  where cc.NumAtCard = dif.NUMATCARD and cc.Codigo_empresa = dif.CODIGO_EMPRESA) as cod_vendedor
		from dbo.difunto_contrato dif
		inner join dbo.tipo_documento td on td.codigo_equivalencia = dif.CODIGO_DOC
		inner join dbo.empresa_sigeco e on e.codigo_equivalencia = dif.CODIGO_EMPRESA
		where dif.CODIGO_EMPRESA = @p_codigo_empresa and dif.NUMATCARD = @p_nro_contrato) t on 
			t.codigo_empresa = cm.codigo_empresa and t.codigo_tipo_documento = cm.codigo_tipo_documento and t.NUM_DOC = cm.nro_documento /*and t.Cod_Vendedor = cm.codigo_personal*/
	WHERE cm.estado_registro = 1 and cm.codigo_estado_proceso = 1 and cm.codigo_tipo_venta = @v_codigo_tipo_venta /*and cm.codigo_tipo_pago = @v_codigo_tipo_pago*/
	
	SET @v_cantidad_articulos_cm = ISNULL((SELECT COUNT(ID) FROM dbo.detalle_comision_manual WHERE id = @v_codigo_proceso), 0)

	--Adicional
	SET @v_esContratoAdicional = dbo.fn_generar_cronograma_comision_eval_hoja_resumen(@p_nro_contrato)
	--Servicio Funerario
	SET @v_esContratoServFun = dbo.fn_generar_cronograma_comision_eval_servicio_funerario(@p_nro_contrato, @p_codigo_empresa)
		
	--Obtenemos los Canal/Grupo del Vendedor/Personal (solo como vendedor)
	IF LEN(@v_codigo_grupo_j) > 0
	BEGIN
		SELECT TOP 1
			@v_codigo_registro = codigo_canal_grupo, @v_codigo_canal= codigo_canal
		FROM
			vw_personal
		WHERE
			percibe_comision = 1
			AND estado_canal_grupo = 1 AND es_supervisor_canal = 0 AND es_supervisor_grupo = 0
			AND codigo_equivalencia_grupo = @v_codigo_grupo_j
			AND codigo_equivalencia = @v_codigo_vendedor
	END	
	ELSE
	BEGIN
		SELECT TOP 1
			@v_codigo_registro = pcg.codigo_registro, @v_codigo_canal= pcg.codigo_canal
		FROM
			dbo.personal p
		INNER JOIN dbo.personal_canal_grupo pcg
			on pcg.codigo_personal = p.codigo_personal and pcg.percibe_comision = 1 and pcg.estado_registro = 1 and pcg.es_supervisor_canal = 0 and pcg.es_supervisor_grupo = 0
		WHERE 
			p.codigo_equivalencia = @v_codigo_vendedor and p.estado_registro = 1
		ORDER BY
			pcg.fecha_registra desc
	END

	IF (@v_codigo_registro IS NULL)
	BEGIN
		IF NOT EXISTS(
				SELECT 
					codigo_canal_grupo
				FROM
					vw_personal
				WHERE
					estado_persona = 1
					AND estado_canal_grupo = 1 
					AND codigo_equivalencia_grupo = @v_codigo_grupo_j
					AND codigo_equivalencia = @v_codigo_vendedor)
		BEGIN
			SET @p_observacion = 'No tiene grupo activo, Vendedor: ' + @v_codigo_vendedor + ' - Grupo: ' + @v_codigo_grupo_j
			SET NOEXEC ON;
		END

		IF NOT EXISTS(
				SELECT 
					codigo_canal_grupo
				FROM
					vw_personal
				WHERE
					estado_persona = 1
					AND estado_canal_grupo = 1 
					AND codigo_equivalencia_grupo = @v_codigo_grupo_j
					AND codigo_equivalencia = @v_codigo_vendedor
					AND es_supervisor_canal = 0 AND es_supervisor_grupo = 0)
		bEGIN
			SET @p_observacion = 'No esta configurado como Vendedor: ' + @v_codigo_vendedor + ' - Grupo:' + @v_codigo_grupo_j
			SET NOEXEC ON;
		END

		IF NOT EXISTS(
				SELECT 
					codigo_canal_grupo
				FROM
					vw_personal
				WHERE
					estado_persona = 1
					AND estado_canal_grupo = 1 
					AND codigo_equivalencia_grupo = @v_codigo_grupo_j
					AND codigo_equivalencia = @v_codigo_vendedor
					AND percibe_comision = 1)
		BEGIN
			SET @p_resultado = 1
			SET @p_observacion = 'No comisiona Vendedor: ' + @v_codigo_vendedor
		END

		--PRINT @p_observacion 
		SET NOEXEC ON;
	END

	--OPEN canal_cursor
	--FETCH NEXT FROM canal_cursor
	--INTO @v_codigo_registro, @v_codigo_canal

	--WHILE @@FETCH_STATUS = 0  
	--BEGIN 

		SET @v_crear_cronograma = 1
		
		--Obtenemos los Articulos homologados a SIGECO
		DECLARE articulo_cursor CURSOR FAST_FORWARD FORWARD_ONLY FOR   
		SELECT 
			cs.codigo_campo_santo, a.codigo_articulo, a.nombre, dc.Quantity, case when a.cantidad_unica = 0 then dc.Quantity else 1 end, dc.valor_promocion, case when a.codigo_tipo_articulo = 5 or a.codigo_categoria = 2 then 1 else 0 end, dc.Price
		FROM 
			dbo.detalle_contrato dc
		INNER JOIN dbo.articulo a 
			on dc.ItemCode = a.codigo_sku and a.estado_registro = 1 and a.genera_comision = 1
		INNER JOIN dbo.campo_santo_sigeco cs
			ON cs.codigo_equivalencia = dc.Codigo_Camposanto
		WHERE 
			dc.NumAtCard = @p_nro_contrato and dc.Codigo_empresa = @p_codigo_empresa 

		SELECT 
			@v_articulos_comision = COUNT(dc.ItemCode)
		FROM 
			dbo.detalle_contrato dc
		INNER JOIN dbo.articulo a 
			on dc.ItemCode = a.codigo_sku and a.estado_registro = 1 and a.genera_comision = 1
		INNER JOIN dbo.campo_santo_sigeco cs
			ON cs.codigo_equivalencia = dc.Codigo_Camposanto
		WHERE 
			dc.NumAtCard = @p_nro_contrato and dc.Codigo_empresa = @p_codigo_empresa 

		SET @v_articulos_no_comision = 0
		SELECT 
			@v_articulos_no_comision = COUNT(dc.ItemCode)
		FROM 
			dbo.detalle_contrato dc
		INNER JOIN dbo.articulo a 
			on dc.ItemCode = a.codigo_sku and a.estado_registro = 1 and a.genera_comision = 0
		INNER JOIN dbo.campo_santo_sigeco cs
			ON cs.codigo_equivalencia = dc.Codigo_Camposanto
		WHERE 
			dc.NumAtCard = @p_nro_contrato and dc.Codigo_empresa = @p_codigo_empresa 

		SET @v_articulos_no_codigo = 0
		SELECT 
			@v_articulos_no_codigo = SUM(case when a.codigo_sku is null then 1 else 0 end)
		FROM 
			dbo.detalle_contrato dc
		LEFT JOIN dbo.articulo a 
			on dc.ItemCode = a.codigo_sku and a.estado_registro = 1
		INNER JOIN dbo.campo_santo_sigeco cs
			ON cs.codigo_equivalencia = dc.Codigo_Camposanto
		WHERE 
			dc.NumAtCard = @p_nro_contrato and dc.Codigo_empresa = @p_codigo_empresa 

		SET @v_articulos_no_codigo = ISNULL(@v_articulos_no_codigo, 0)

		IF (@v_articulos_no_codigo > 0)
		BEGIN
			SET @p_observacion = 'Existen ' + convert(varchar(10), @v_articulos_no_codigo) + ' articulos de ' + convert(varchar(10), @v_articulos_contrato) + ' sin equivalencia en el aplicativo.'
			SET NOEXEC ON;
		END

		--IF (@v_cantidad_articulos_cm > 0)
		--BEGIN
		--	IF (@v_cantidad_articulos_cm <> @v_articulos_comision)
		--	BEGIN
		--		SET @p_observacion = 'Comision Manual - La cantidad de articulos (' + convert(varchar(10), @v_cantidad_articulos_cm) + ') no coindice con los del contrato(' + convert(varchar(10), @v_articulos_comision) + ').'
		--		SET NOEXEC ON;
		--	END
		--END

		OPEN articulo_cursor
		FETCH NEXT FROM articulo_cursor
		INTO @v_codigo_camposanto, @v_codigo_articulo, @v_nombre_articulo, @v_cantidad, @v_cantidad_calculo, @v_valor_promocion, @v_esArticuloAdicional, @v_precio_venta

		PRINT 'Analizando : ' + @p_nro_contrato + '-' + convert(varchar, @p_codigo_empresa)
		SET @v_indice = 0
		
		WHILE @@FETCH_STATUS = 0  
		BEGIN 

			PRINT 'Articulo : ' + @v_nombre_articulo

			SET @v_genera_comision = 1
			IF (@v_esContratoAdicional = 0)
			BEGIN
				EXEC up_generar_cronograma_validar_asociado @p_codigo_empresa, @p_nro_contrato, @v_codigo_articulo, @v_genera_comision OUTPUT 
				--print 'valido asociado: ' + convert(varchar, @v_genera_comision)
			END
			
			IF (@v_genera_comision = 1)
			BEGIN
				--BEGIN TRAN Cronograma
				
				SET @v_codigo_precio = NULL
				SET @v_codigo_tipo_comision = NULL
				SET @v_tipo_pago_comision = NULL
				SET @v_tiene_excepcion = NULL
				SET @v_es_comision_manual = 0
				
				--Obtenemos en Precio del Articulo, solo 1 precio
				SELECT TOP 1 @v_codigo_precio = codigo_precio, @v_precio_total = precio_total 
				FROM
					dbo.pcc_precio_articulo
				WHERE 
					codigo_articulo = @v_codigo_articulo 
					and codigo_empresa = @v_codigo_empresa 
					and codigo_tipo_venta = @v_codigo_tipo_venta
					and codigo_moneda = @v_codigo_moneda 
					and @v_fecha_proceso between vigencia_inicio and vigencia_fin 
				
				if (@v_codigo_precio IS NULL)
				begin
					SET @p_observacion = 'No tiene Precio vigente: ' + @v_nombre_articulo
					--PRINT @p_observacion
					SET NOEXEC ON
				end

				--Obtenemos la Comision del Precio, solo 1 comision
				SELECT TOP 1 @v_codigo_tipo_comision = codigo_tipo_comision, @v_valor = valor
				FROM
					dbo.pcc_regla_calculo_comision
				WHERE 
					codigo_precio = @v_codigo_precio
					and codigo_canal = @v_codigo_canal
					and codigo_tipo_pago = @v_codigo_tipo_pago
					and @v_fecha_proceso between vigencia_inicio and vigencia_fin 

				IF (@v_codigo_tipo_comision IS NULL)
				BEGIN
					SET @p_observacion = 'No tiene Comision vigente: ' + @v_nombre_articulo
					--print @p_observacion
					SET NOEXEC ON;
				END
				
				SET @v_monto_comision = ROUND((CASE WHEN @v_codigo_tipo_comision = 1 THEN @v_valor ELSE ROUND((@v_precio_total * @v_valor)/100, 4) END) * @v_cantidad_calculo, 4)

				--Adicionales en Servicio Funerario
				IF (@v_esContratoServFun = 1)
					IF (@v_esArticuloAdicional = 1)
						SET @v_monto_comision = ROUND((CASE WHEN @v_codigo_tipo_comision = 1 THEN @v_valor ELSE ROUND(((@v_precio_venta * @v_igv) * @v_valor)/100, 4) END) * @v_cantidad_calculo, 4)

				--Comision Manual
				IF (@v_cantidad_articulos_cm > 0)
				BEGIN
					IF EXISTS((SELECT comision FROM dbo.detalle_comision_manual WHERE id = @v_codigo_proceso and codigo_articulo = @v_codigo_articulo ))
					BEGIN
						SET @v_monto_comision = ISNULL((SELECT ROUND(comision * @v_cantidad_calculo, 4) FROM dbo.detalle_comision_manual WHERE id = @v_codigo_proceso and codigo_articulo = @v_codigo_articulo ), 0)
						SET @v_cantidad_analizados_cm = @v_cantidad_analizados_cm + (CASE WHEN @v_monto_comision > 0 THEN 1 ELSE 0 END)
						SET @v_es_comision_manual = CASE WHEN @v_monto_comision > 0  THEN 1 ELSE 0 END
					END
				END
				
				IF @v_valor_promocion > 0
				BEGIN
					--Validamos si tiene Regla de Pago Excepcion
					SELECT TOP 1 
						@v_nro_cuotas_exc = cuotas
					FROM
						dbo.pcc_regla_pago_comision_excepcion
					WHERE 
						codigo_empresa = @v_codigo_empresa
						AND codigo_canal_grupo = @v_codigo_canal
						AND codigo_articulo = @v_codigo_articulo
						AND valor_promocion = @v_valor_promocion
						AND @v_fecha_proceso between vigencia_inicio and vigencia_fin 

					SET @v_nro_cuotas_exc = ISNULL(@v_nro_cuotas_exc, 0)
				END
				
				SET @v_indice = @v_indice + 1 
				INSERT INTO dbo.regla_pago_comision_temporal(id, indice, codigo_articulo, cantidad, monto_comision, cantidad_cuotas_excepcion, es_comision_manual) VALUES(@v_codigo_proceso, @v_indice, @v_codigo_articulo, @v_cantidad, @v_monto_comision, CASE WHEN @v_valor_promocion > 0 THEN @v_nro_cuotas_exc ELSE 0 END, @v_es_comision_manual)
				
			END --Genera Comision
			
			FETCH NEXT FROM articulo_cursor
			INTO @v_codigo_camposanto, @v_codigo_articulo, @v_nombre_articulo, @v_cantidad, @v_cantidad_calculo, @v_valor_promocion, @v_esArticuloAdicional, @v_precio_venta
		END   
		CLOSE articulo_cursor;  
		DEALLOCATE articulo_cursor; 

	--	FETCH NEXT FROM canal_cursor
	--	INTO @v_codigo_registro, @v_codigo_canal
	--END   
	--CLOSE canal_cursor;  
	--DEALLOCATE canal_cursor; 

	IF (@v_articulos_comision = 0)
	BEGIN
		SET @p_resultado = 1
		SET @p_observacion = 'No generó comision, ' + convert(varchar(10), @v_articulos_no_comision) + ' artículos de ' + convert(varchar(10), @v_articulos_contrato) + ' no generan comisión.'
		SET NOEXEC ON
	END

	SET @v_articulos_comision = (SELECT COUNT(codigo_articulo) FROM dbo.regla_pago_comision_temporal WHERE id = @v_codigo_proceso)

	IF (@v_articulos_comision = 0)
	BEGIN
		SET @p_resultado = 1
		SET @p_observacion = 'No comisionó ningún artículo de ' + convert(varchar(10), @v_articulos_contrato) + ' por contrato referenciado.'
		SET NOEXEC ON
	END

	--Comision Manual
	--IF (@v_cantidad_articulos_cm > 0)
	--BEGIN
	--	IF (@v_cantidad_articulos_cm <> @v_cantidad_analizados_cm)
	--	BEGIN
	--		SET @p_observacion = 'Comision Manual - La cantidad de articulos procesados(' + CONVERT(VARCHAR(2), @v_cantidad_analizados_cm) + ') no coincide con la cantidad de articulos registrados(' + CONVERT(VARCHAR(2), @v_cantidad_articulos_cm) + ').'
	--		--SET NOEXEC ON
	--	END
	--END

	-- REGLA DE PAGO
	SET @p_retorno = 0
	SET @p_codigo_regla_pago = 0

	--Obtenemos la Forma Pago de Comision para todo el contrato
	exec up_generar_comision_regla_pago @p_nro_contrato, @p_codigo_empresa, @v_codigo_camposanto, @v_codigo_empresa, @v_codigo_canal, @v_codigo_tipo_venta, @v_codigo_tipo_pago, @v_codigo_articulo, @p_retorno out, @p_codigo_regla_pago out, @p_observacion out
	
	IF (@p_retorno = 0)
	BEGIN
		SET @p_observacion = 'No tiene Regla de Pago Comision configurada.' + @p_observacion
		--PRINT @p_observacion
		--ROLLBACK TRAN Cronograma
		SET NOEXEC ON;
	END

	--SET @p_observacion = ''
	
	-- TRANSFERENCIA
	--IF (@v_esContratoAdicional = 0)
	--BEGIN
	--	EXEC up_generar_cronograma_comision_transferencia @p_codigo_empresa, @p_nro_contrato, @v_codigo_proceso, @p_observacion output
	--END

	-- NUEVA FORMA DE PAGO
	EXEC up_generar_cronograma_comision_vendedor_cuotas @v_codigo_proceso, @p_codigo_regla_pago, @v_codigo_empresa, @p_nro_contrato, @v_codigo_camposanto, @v_codigo_registro, @v_codigo_tipo_venta, @v_codigo_tipo_pago_o, @v_codigo_moneda, @v_es_transferencia, @v_codigo_cronograma output

	SET @p_codigo_cronograma = @v_codigo_cronograma
	SET @p_resultado = 1

	DELETE FROM dbo.regla_pago_comision_temporal WHERE id = @v_codigo_proceso

	print 'Termino Proceso Calculo Comision'

	SET NOEXEC OFF;
	SET NOCOUNT OFF
END;