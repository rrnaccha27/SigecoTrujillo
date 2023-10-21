CREATE PROCEDURE [dbo].up_generar_cronograma_comision_vendedor_cuotas
(
	@p_codigo_proceso		uniqueidentifier,
	@p_codigo_regla_pago	int,
	@p_codigo_empresa		int,
	@p_nro_contrato			varchar(100),
	@p_codigo_campo_santo	int,
	@p_codigo_registro		int, 
	@p_codigo_tipo_venta	int, 
	@p_codigo_tipo_pago		int, 
	@p_codigo_moneda		int,
	@p_es_transferencia		bit,
	@p_codigo_cronograma	int output
)
AS
BEGIN
	SET NOCOUNT ON
	SET NOEXEC OFF

	DECLARE
		@v_codigo_empresa			int,
		@v_genera_comision			bit,
		@v_codigo_vendedor			nvarchar(20),
		@v_codigo_articulo			int, 
		@v_cantidad					int,
		@v_codigo_canal				int,
		@v_codigo_precio			int, 
		@v_codigo_tipo_comision		int,
		@v_valor					DECIMAL(12, 4),
		@v_monto_comision			DECIMAL(12, 4),
		@v_monto_comision_frac		DECIMAL(12, 4),
		@v_monto_a_cubrir			decimal(12, 4),
		@v_monto_comision_total		DECIMAL(12, 4),
		@v_tipo_pago_comision		int, 
		@v_valor_inicial_pago		DECIMAL(12, 4), 
		@v_valor_cuota_pago			DECIMAL(12, 4),
		@v_fecha_habilitacion		datetime,
		@v_fecha_habilitacion_CI	datetime,
		@v_importe_cuota			DECIMAL(12, 4),
		@v_NumCuota					int,
		@v_nro_cuotas_exc			int,
		@v_nro_cuotas_contrato		int,
		@v_importe_sobrante			decimal(12, 4),
		@v_cuotas_a_derivar			int
		
	DECLARE 
		--@p_codigo_cronograma			int,
		@c_codigo_tipo_planilla			int,
		@v_fecha_programada				int,
		@c_igv							DECIMAL(12, 4),
		@v_monto_neto					DECIMAL(12, 4),
		@v_monto_igv					DECIMAL(12, 4),
		@c_codigo_tipo_cuota			int,
		@c_codigo_estado_cuota			int,
		@v_cantidad_cuotas				int,
		@v_articulos_contrato			int,
		@v_articulos_comision			int,
		@v_codigo_regla_pago			int,
		@v_indice						int,
		@v_codigo_empresa_jdlp			varchar(4),
		@v_es_comision_manual			bit,
		@v_codigo_detalle_cronograma	int

	DECLARE
		@c_OFSA					CHAR(4) = '0002'
		,@c_FUNJAR				CHAR(4) = '0939'
		,@c_EsContratoDoble		BIT = 0
		,@c_Estado_Registro		BIT =1
		,@c_Es_Recupero_Total	BIT = 0
		,@c_NECESIDAD			CHAR(2) = 'NI'

	DECLARE	
		@t_Cuotas				TABLE
		(
			id					INT IDENTITY(1, 1)
			,codigo_articulo	int
			,nro_cuota			int
			,fecha_habilitacion	DATETIME
			,importe			DECIMAL(12, 4)
			,importe_negativo	DECIMAL(12, 4)
		)
	DECLARE 
		@t_articulos_reprocesar	TABLE
		(
			codigo	INT
		)
		/*
	DECLARE @v_codigo_canal_grupo         int,
	        @v_codigo_registro_supervisor int;
			*/
	SET @c_codigo_tipo_cuota = 1-- Tipo Cuota Automatica
	SET @c_codigo_estado_cuota = 1-- Estado Cuota Pendiente
	SET @c_codigo_tipo_planilla = 1-- Tipo Planilla Personal (como vendedor)
	SET @c_igv = (SELECT TOP 1 CONVERT(DECIMAL(12, 4), valor) FROM dbo.parametro_sistema WHERE codigo_parametro_sistema = 9)-- Confirmar el codigo_parametro_sistema
	SET @v_codigo_empresa_jdlp = (SELECT TOP 1 codigo_equivalencia FROM dbo.empresa_sigeco WHERE codigo_empresa = @p_codigo_empresa)

	--Contrato Doble
	IF (@v_codigo_empresa_jdlp = @c_FUNJAR)
		IF EXISTS(SELECT TOP 1 NumAtCard FROM dbo.cabecera_contrato WHERE NumAtCard = @p_nro_contrato AND Codigo_empresa = @c_OFSA)
			SET @c_EsContratoDoble = 1

	-- Recupero 
	IF EXISTS(SELECT TOP 1 NumAtCard FROM cabecera_contrato WHERE Codigo_empresa = @v_codigo_empresa_jdlp and NumAtCard =  @p_nro_contrato and Flg_Recupero = 1 and ISNULL(Monto_Recupero, 0) >= DocTotal) 
		AND NOT EXISTS(SELECT NumAtCard FROM contrato_cuota WHERE Codigo_empresa = @v_codigo_empresa_jdlp and NumAtCard =  @p_nro_contrato)
		SET @c_Es_Recupero_Total = 1

	--Cronograma
	INSERT INTO dbo.cronograma_pago_comision
		(codigo_tipo_planilla, codigo_empresa, codigo_personal_canal_grupo, nro_contrato, codigo_tipo_venta, codigo_tipo_pago, codigo_moneda, fecha_registro, estado_registro, codigo_regla_pago, tiene_transferencia)
	VALUES
		(@c_codigo_tipo_planilla, @p_codigo_empresa, @p_codigo_registro, @p_nro_contrato, @p_codigo_tipo_venta, @p_codigo_tipo_pago, @p_codigo_moneda, GETDATE(), @c_Estado_Registro, @p_codigo_regla_pago, @p_es_transferencia)
	SET @p_codigo_cronograma = @@IDENTITY

	SET @v_indice = 1
	SET @v_articulos_comision = (SELECT COUNT(codigo_articulo) FROM dbo.regla_pago_comision_temporal WHERE id = @p_codigo_proceso)

	-- Articulos de Cronograma
	WHILE (@v_indice <= @v_articulos_comision)
	BEGIN

		SELECT
			@v_codigo_articulo = codigo_articulo
			,@v_monto_comision = monto_comision
			,@v_cantidad = cantidad
			,@v_nro_cuotas_exc = cantidad_cuotas_excepcion
		FROM 
			dbo.regla_pago_comision_temporal 
		WHERE 
			id = @p_codigo_proceso
			and indice = @v_indice

		-- Cada Articulo Cronograma
		INSERT INTO dbo.articulo_cronograma
			(codigo_cronograma, codigo_articulo, monto_comision, cantidad, codigo_campo_santo, estado_registro)
		VALUES
			(@p_codigo_cronograma, @v_codigo_articulo, @v_monto_comision, @v_cantidad, @p_codigo_campo_santo, @c_Estado_Registro)

		-- Regla Excepcional de Pago 
		IF (@v_nro_cuotas_exc > 0)
		BEGIN
				
			SET @v_NumCuota = 1
			SET @v_cantidad_cuotas = (SELECT COUNT(Fec_Pago) FROM dbo.contrato_cuota WHERE Num_Cuota = @v_NumCuota AND NumAtCard = @p_nro_contrato and Codigo_empresa = @v_codigo_empresa_jdlp AND Cod_Estado in ('C', 'P'))
			SET @v_nro_cuotas_exc = CASE WHEN @v_nro_cuotas_exc > @v_cantidad_cuotas THEN @v_cantidad_cuotas ELSE @v_nro_cuotas_exc END
			SET @v_monto_comision_frac = ROUND(@v_monto_comision / @v_nro_cuotas_exc, 4)
			SET @v_monto_igv = ROUND((@v_monto_comision_frac * @c_igv) / (100 + @c_igv), 4)

			WHILE (@v_NumCuota <= @v_nro_cuotas_exc)
			BEGIN
				SET @v_fecha_habilitacion = NULL
				SET @v_fecha_habilitacion = (SELECT TOP 1 Fec_Pago FROM dbo.contrato_cuota WHERE Num_Cuota = @v_NumCuota AND NumAtCard = @p_nro_contrato and Codigo_empresa = CASE WHEN @c_EsContratoDoble = 1 THEN @c_OFSA ELSE @v_codigo_empresa_jdlp END AND Cod_Estado= 'C')

				INSERT INTO dbo.detalle_cronograma
					(codigo_cronograma, codigo_articulo, nro_cuota, fecha_programada, monto_bruto, igv, monto_neto, codigo_tipo_cuota, codigo_estado_cuota, estado_registro)
				VALUES
					(@p_codigo_cronograma, @v_codigo_articulo, @v_NumCuota, @v_fecha_habilitacion, (@v_monto_comision_frac - @v_monto_igv), @v_monto_igv, @v_monto_comision_frac, @c_codigo_tipo_cuota, @c_codigo_estado_cuota, @c_Estado_Registro)
					
				SET @v_NumCuota = @v_NumCuota + 1
			END

			DELETE FROM dbo.regla_pago_comision_temporal WHERE id = @p_codigo_proceso AND indice = @v_indice
		END

		SET @v_indice = @v_indice + 1
	END

	SELECT TOP 1 @v_tipo_pago_comision = tipo_pago_comision, @v_valor_inicial_pago = valor_inicial_pago, @v_valor_cuota_pago = valor_cuota_pago
	FROM
		dbo.pcc_regla_pago_comision
	WHERE 
		codigo_regla_pago = @p_codigo_regla_pago

	SET @v_indice = 1
	SET @v_articulos_comision = (SELECT COUNT(codigo_articulo) FROM dbo.regla_pago_comision_temporal WHERE id = @p_codigo_proceso)

	--print convert(varchar, @c_Es_Recupero_Total)
	IF (@c_Es_Recupero_Total = 1)
		SET @v_fecha_habilitacion_CI = (SELECT TOP 1 CreateDate FROM dbo.cabecera_contrato WHERE NumAtCard = @p_nro_contrato and Codigo_empresa = @v_codigo_empresa_jdlp)
	ELSE
	BEGIN
		SET @v_fecha_habilitacion_CI = (SELECT TOP 1 Fec_Pago FROM dbo.contrato_cuota WHERE Num_Cuota = 0 AND NumAtCard = @p_nro_contrato and Codigo_empresa = @v_codigo_empresa_jdlp AND Cod_Estado = 'C')
		IF (@v_fecha_habilitacion_CI IS NULL OR YEAR(@v_fecha_habilitacion_CI) = 1900)
			SET @v_fecha_habilitacion_CI = (SELECT TOP 1 CreateDate FROM dbo.cabecera_contrato WHERE NumAtCard = @p_nro_contrato and Codigo_empresa = @v_codigo_empresa_jdlp AND Cod_Tipo_Venta = @c_NECESIDAD)
	END

	--select @v_tipo_pago_comision as '@v_tipo_pago_comision original'

	-- Validacion si cubre el 100% de comision el monto o % de la CI
	IF (@v_tipo_pago_comision IN (3, 4))
	BEGIN
		SET @v_NumCuota = 0
		SET @v_monto_comision_total = (SELECT SUM(monto_comision) FROM dbo.regla_pago_comision_temporal WHERE id = @p_codigo_proceso)

		SELECT
			@v_monto_a_cubrir = CONVERT(DECIMAL(12, 4), Num_Importe_Total) 
		FROM dbo.contrato_cuota 
		WHERE 
			Num_Cuota = @v_NumCuota
			AND NumAtCard = @p_nro_contrato 
			AND Codigo_empresa = @v_codigo_empresa_jdlp
			AND Cod_Estado IN ('C', 'P')

		IF (@v_tipo_pago_comision = 3)
		BEGIN
			SET @v_valor_inicial_pago = ROUND(@v_valor_inicial_pago/100, 4)--Se vuelve % 
			SET @v_monto_a_cubrir = ROUND(@v_monto_a_cubrir * @v_valor_inicial_pago, 4)
		END
		ELSE--(@v_tipo_pago_comision = 4)
		BEGIN
			SET @v_monto_a_cubrir = ROUND(@v_valor_inicial_pago, 4)
		END

		IF @v_monto_a_cubrir >= @v_monto_comision_total
			SET @v_tipo_pago_comision = 1
	END
	--select @v_tipo_pago_comision as '@v_tipo_pago_comision validado'

	IF (@v_tipo_pago_comision IN (1, 2))
	BEGIN
		SET @v_valor_inicial_pago = ROUND(@v_valor_inicial_pago/100, 4)
		SET @v_valor_cuota_pago = ROUND(@v_valor_cuota_pago/100, 4)

		WHILE (@v_indice < = @v_articulos_comision)
		BEGIN
			SELECT
				@v_codigo_articulo = codigo_articulo
				,@v_monto_comision = monto_comision
				,@v_cantidad = cantidad
				,@v_nro_cuotas_exc = cantidad_cuotas_excepcion
				,@v_es_comision_manual = es_comision_manual
			FROM 
				dbo.regla_pago_comision_temporal 
			WHERE 
				id = @p_codigo_proceso
				and indice = @v_indice

			-- Un unico registro de Detalle Cronograma
			IF (@v_tipo_pago_comision = 1)
			BEGIN
				SET @v_monto_igv = ROUND((@v_monto_comision * @c_igv) / (100 + @c_igv), 4)
				INSERT INTO dbo.detalle_cronograma
					(codigo_cronograma, codigo_articulo, nro_cuota, fecha_programada, monto_bruto, igv, monto_neto, codigo_tipo_cuota, codigo_estado_cuota, estado_registro, es_registro_manual_comision, es_transferencia)
				VALUES
					(@p_codigo_cronograma, @v_codigo_articulo, 1, @v_fecha_habilitacion_CI, (@v_monto_comision - @v_monto_igv), @v_monto_igv, @v_monto_comision, @c_codigo_tipo_cuota, @c_codigo_estado_cuota, @c_Estado_Registro, @v_es_comision_manual, @p_es_transferencia)
				
				SET @v_codigo_detalle_cronograma = @@IDENTITY
				
				IF (@v_es_comision_manual = 1)
				BEGIN
					UPDATE dbo.comision_manual
					SET
						codigo_detalle_cronograma = @v_codigo_detalle_cronograma
						,nro_contrato = @p_nro_contrato
					WHERE
						codigo_comision_manual = (SELECT TOP 1 codigo_comision_manual FROM dbo.detalle_comision_manual where id = @p_codigo_proceso and codigo_articulo = @v_codigo_articulo)
					DELETE FROM dbo.detalle_comision_manual where id = @p_codigo_proceso and codigo_articulo = @v_codigo_articulo
				END

				DELETE FROM dbo.regla_pago_comision_temporal WHERE id = @p_codigo_proceso AND indice = @v_indice
			END

			-- 2 registros de Detalle Cronograma, 1 % en Cuota Inicial, 2 % en Primera Cuota
			IF (@v_tipo_pago_comision = 2)
			BEGIN
				SET @v_monto_igv = ROUND((@v_monto_comision * @c_igv) / (100 + @c_igv), 4)
				INSERT INTO dbo.detalle_cronograma
					(codigo_cronograma, codigo_articulo, nro_cuota, fecha_programada, monto_bruto, igv, monto_neto, codigo_tipo_cuota, codigo_estado_cuota, estado_registro, es_transferencia)
				VALUES
					(@p_codigo_cronograma, @v_codigo_articulo, 1, @v_fecha_habilitacion_CI, @v_valor_inicial_pago * (@v_monto_comision - (@v_monto_igv)), @v_valor_inicial_pago * (@v_monto_igv), @v_valor_inicial_pago * @v_monto_comision, @c_codigo_tipo_cuota, @c_codigo_estado_cuota, @c_Estado_Registro, @p_es_transferencia)

				SET @v_fecha_habilitacion = NULL
				SET @v_fecha_habilitacion = (SELECT TOP 1 Fec_Pago FROM dbo.contrato_cuota WHERE Num_Cuota = 1 AND NumAtCard = @p_nro_contrato and Codigo_empresa = CASE WHEN @c_EsContratoDoble = 1 THEN @c_OFSA ELSE @v_codigo_empresa_jdlp END AND Cod_Estado = 'C')

				INSERT INTO dbo.detalle_cronograma
					(codigo_cronograma, codigo_articulo, nro_cuota, fecha_programada, monto_bruto, igv, monto_neto, codigo_tipo_cuota, codigo_estado_cuota, estado_registro, es_transferencia)
				VALUES
					(@p_codigo_cronograma, @v_codigo_articulo, 2, @v_fecha_habilitacion, @v_valor_cuota_pago * (@v_monto_comision - (@v_monto_igv)), @v_valor_cuota_pago * (@v_monto_igv), @v_valor_cuota_pago * @v_monto_comision, @c_codigo_tipo_cuota, @c_codigo_estado_cuota, @c_Estado_Registro, @p_es_transferencia)

				DELETE FROM dbo.regla_pago_comision_temporal WHERE id = @p_codigo_proceso AND indice = @v_indice
			END
		
			SET @v_indice = @v_indice + 1
		END --WHILE (@v_indice < = @v_articulos_comision)

		SET NOEXEC ON
	END--IF (@v_tipo_pago_comision IN (1, 2))


	-- N registros de Detalle Cronograma, 1 % en Cuota Inicial, N % de las Cuotas (resta fraccionada)
	-- N registros de Detalle Cronograma, 1 Monto Fijo en Cuota Inicial, N % de las Cuotas (resta fraccionada)
	IF (@v_tipo_pago_comision = 3 OR @v_tipo_pago_comision = 4)
	BEGIN
		--select '1', * from regla_pago_comision_temporal WHERE id = @p_codigo_proceso
		--select '1', * from @t_Cuotas

		SET @v_NumCuota = 0
		SET @v_valor_cuota_pago = ROUND(@v_valor_cuota_pago/100, 4)--% de las cuotas (resta fraccionada)
		SET @v_monto_comision_total = (SELECT SUM(monto_comision) FROM dbo.regla_pago_comision_temporal WHERE id = @p_codigo_proceso)
		SET @v_nro_cuotas_contrato = (SELECT COUNT(Num_Cuota) FROM dbo.contrato_cuota WHERE NumAtCard = @p_nro_contrato AND Codigo_empresa = @v_codigo_empresa_jdlp AND Cod_Estado IN ('C', 'P'))
		
		--select @v_monto_comision_total as '@v_monto_comision_total', @v_NumCuota as '@v_NumCuota', @v_nro_cuotas_contrato as 'de @v_nro_cuotas_contrato' 

		WHILE (@v_monto_comision_total > 0) AND (@v_NumCuota <= (@v_nro_cuotas_contrato - 1))
		BEGIN
			
			SET @v_fecha_habilitacion = NULL
			SET @v_fecha_habilitacion = (SELECT TOP 1 Fec_Pago FROM dbo.contrato_cuota WHERE Num_Cuota = @v_NumCuota AND NumAtCard = @p_nro_contrato and Codigo_empresa = CASE WHEN @c_EsContratoDoble = 1 THEN @c_OFSA ELSE @v_codigo_empresa_jdlp END AND Cod_Estado = 'C')

			--Monto de la Cuota Inicial
			SELECT
				@v_monto_a_cubrir = CONVERT(DECIMAL(12, 4), Num_Importe_Total) 
			FROM dbo.contrato_cuota 
			WHERE 
				Num_Cuota = @v_NumCuota
				AND NumAtCard = @p_nro_contrato 
				AND Codigo_empresa = @v_codigo_empresa_jdlp
				AND Cod_Estado IN ('C', 'P')

			--select @v_monto_a_cubrir as '@v_monto_a_cubrir'

			IF (@v_NumCuota = 0)-- Cuota Inicial
			BEGIN
				IF (@v_tipo_pago_comision = 3)
				BEGIN
					--SET @v_valor_inicial_pago = ROUND(@v_valor_inicial_pago/100, 4)--Se vuelve % 
					SET @v_monto_a_cubrir = ROUND((@v_monto_a_cubrir * @v_valor_inicial_pago)/@v_articulos_comision, 4)
				END
				ELSE--(@v_tipo_pago_comision = 4)
				BEGIN
					--SET @v_valor_inicial_pago = ROUND(@v_valor_inicial_pago / @v_articulos_comision, 4)
					SET @v_monto_a_cubrir = ROUND(@v_valor_inicial_pago / @v_articulos_comision, 4)
				END
			END
			ELSE--Demas Cuotas
			BEGIN
				SET @v_monto_a_cubrir = ROUND((@v_monto_a_cubrir * @v_valor_cuota_pago)/@v_articulos_comision, 4)
				--SET @v_monto_a_cubrir = ROUND(@v_valor_cuota_pago / @v_articulos_comision, 4)
			END

			--SELECT @v_monto_a_cubrir as '@v_monto_a_cubrir', @v_NumCuota as 'en cuota', @v_articulos_comision as ' articulos' 

			INSERT INTO @t_Cuotas(codigo_articulo, nro_cuota, fecha_habilitacion, importe, importe_negativo) 
			SELECT 
				codigo_articulo,
				@v_NumCuota,
				@v_fecha_habilitacion,
				@v_monto_a_cubrir,
				monto_comision - @v_monto_a_cubrir
			FROM
				dbo.regla_pago_comision_temporal
			WHERE
				id = @p_codigo_proceso
				--and monto_comision > 0

			DELETE FROM @t_Cuotas WHERE importe = 0 or (importe + importe_negativo = 0)

			SET @v_importe_sobrante = (SELECT SUM(importe_negativo * -1) FROM @t_Cuotas WHERE importe_negativo < 0 and nro_cuota = @v_NumCuota)
			SET @v_cuotas_a_derivar = (SELECT COUNT(importe_negativo) FROM @t_Cuotas WHERE importe_negativo > 0 and nro_cuota = @v_NumCuota)
			
			IF (@v_importe_sobrante > 0 AND @v_cuotas_a_derivar > 0)
			BEGIN

				--WHILE (@v_importe_sobrante > 0) AND  (@v_cuotas_a_derivar > 0) AND ((SELECT SUM(monto_comision) FROM dbo.regla_pago_comision_temporal WHERE id = @p_codigo_proceso) > 0)
				--BEGIN

					--SELECT @v_importe_sobrante as '@v_importe_sobrante', @v_cuotas_a_derivar as '@v_cuotas_a_derivar', @v_NumCuota as '@v_NumCuota'
				
					SET @v_importe_sobrante = (@v_importe_sobrante/@v_cuotas_a_derivar)
					SET @v_monto_a_cubrir = @v_monto_a_cubrir + @v_importe_sobrante

					DELETE FROM @t_articulos_reprocesar

					INSERT INTO @t_articulos_reprocesar(codigo) 
					SELECT codigo_articulo FROM @t_Cuotas 					
					WHERE 
						importe > 0
						AND importe_negativo > 0 
						AND nro_cuota = @v_NumCuota

					DELETE FROM @t_Cuotas WHERE codigo_articulo IN (SELECT codigo FROM @t_articulos_reprocesar ) AND nro_cuota = @v_NumCuota

					INSERT INTO @t_Cuotas(codigo_articulo, nro_cuota, fecha_habilitacion, importe, importe_negativo) 
					SELECT 
						codigo_articulo,
						@v_NumCuota,
						@v_fecha_habilitacion,
						@v_monto_a_cubrir,
						monto_comision - @v_monto_a_cubrir
					FROM
						dbo.regla_pago_comision_temporal
					WHERE
						id = @p_codigo_proceso
						AND codigo_articulo IN (SELECT codigo FROM @t_articulos_reprocesar)

					UPDATE 
						dbo.regla_pago_comision_temporal
					SET 
						monto_comision = CASE WHEN monto_comision - @v_monto_a_cubrir > 0 THEN monto_comision - @v_monto_a_cubrir ELSE 0 END
					WHERE 
						id = @p_codigo_proceso


					SET @v_importe_sobrante = (SELECT SUM(importe_negativo * -1) FROM @t_Cuotas WHERE importe_negativo < 0 and nro_cuota = @v_NumCuota)
					SET @v_cuotas_a_derivar = (SELECT COUNT(importe_negativo) FROM @t_Cuotas WHERE importe_negativo > 0 and nro_cuota = @v_NumCuota)

				--END
			END	
			ELSE
				UPDATE 
					dbo.regla_pago_comision_temporal
				SET 
					monto_comision = CASE WHEN monto_comision - @v_monto_a_cubrir > 0 THEN monto_comision - @v_monto_a_cubrir ELSE 0 END
				WHERE 
					id = @p_codigo_proceso

			--select * from @t_Cuotas

			SET @v_articulos_comision = (SELECT COUNT(codigo_articulo) FROM dbo.regla_pago_comision_temporal WHERE id = @p_codigo_proceso AND monto_comision > 0)
			SET @v_monto_comision_total = (SELECT SUM(monto_comision) FROM dbo.regla_pago_comision_temporal WHERE id = @p_codigo_proceso)
			SET @v_NumCuota = @v_NumCuota + 1

		END--WHILE (@v_monto_comision_total > 0) AND (@v_NumCuota < (@v_nro_cuotas_contrato - 1))

		IF (@v_monto_comision_total > 0)
		BEGIN
			--select @v_monto_comision_total as '@v_monto_comision_total'
			--select * from regla_pago_comision_temporal WHERE id = @p_codigo_proceso

			UPDATE c
			SET c.importe = c.importe + ISNULL(temp.monto_comision, 0)
			FROM  @t_Cuotas c
			INNER JOIN dbo.regla_pago_comision_temporal temp 
				ON temp.codigo_articulo = c.codigo_articulo
				AND temp.id = @p_codigo_proceso 
		END

		--select '3', * from @t_Cuotas order by id

		UPDATE
			@t_Cuotas
		SET
			importe = importe + importe_negativo
		WHERE 
			importe_negativo < 0 
					
		DELETE FROM @t_Cuotas WHERE importe = 0

		--select '4', * from @t_Cuotas order by id
	
		INSERT INTO dbo.detalle_cronograma
			(codigo_cronograma, codigo_articulo, nro_cuota, fecha_programada, monto_bruto, igv, monto_neto, codigo_tipo_cuota, codigo_estado_cuota, estado_registro, es_transferencia)
		SELECT 
			@p_codigo_cronograma, c.codigo_articulo, (c.nro_cuota + 1), c.fecha_habilitacion, c.importe - ROUND((c.importe * @c_igv) / (100 + @c_igv), 4), ROUND((c.importe * @c_igv) / (100 + @c_igv), 4), c.importe, @c_codigo_tipo_cuota, @c_codigo_estado_cuota, @c_Estado_Registro, @p_es_transferencia
		FROM @t_Cuotas c
		INNER JOIN dbo.regla_pago_comision_temporal  r
			ON r.codigo_articulo = c.codigo_articulo
		WHERE r.id = @p_codigo_proceso

		--variables libres
		--@v_monto_comision_frac

	END--IF (@v_tipo_pago_comision = 3 OR @v_tipo_pago_comision = 4)
	
	SET NOEXEC OFF
	SET NOCOUNT OFF
END