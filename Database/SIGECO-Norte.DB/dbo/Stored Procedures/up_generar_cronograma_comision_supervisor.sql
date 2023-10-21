CREATE PROCEDURE [dbo].[up_generar_cronograma_comision_supervisor]
(
	 @p_codigo_empresa		NVARCHAR(4)
	,@p_nro_contrato		NVARCHAR(200)
	,@p_codigo_cronograma	INT
	,@p_observacion			VARCHAR(200) OUTPUT
)
AS
BEGIN
SET NOEXEC OFF;

DECLARE
	 @v_codigo_vendedor			NVARCHAR(20)
	,@v_codigo_canal			INT
	,@v_codigo_tipo_venta		INT
	,@v_codigo_tipo_pago		INT
	,@v_codigo_moneda			INT
	--,@v_codigo_registro_supervisor	INT
	,@v_codigo_registro			INT
	,@v_valor_pago				DECIMAL(12, 4)
	,@v_incluye_igv				BIT
	,@v_es_canal_grupo			INT
	,@v_codigo_cronograma		INT
	,@v_codigo_articulo			INT 
	,@v_monto_comision			DECIMAL(12, 4)
	,@v_monto_comision_sin_igv	DECIMAL(12, 4) 
	,@v_monto_comision_igv		DECIMAL(12, 4)
	,@v_codigo_canal_grupo		INT
	,@v_fecha_programada		DATETIME
	,@v_cantidad				INT
	,@v_codigo_campo_santo		INT
	,@v_fecha_proceso			DATETIME
	,@v_codigo_supevisor		INT
	,@v_es_comision_precio		BIT = 0
	,@v_cantidad_articulos_contrato		INT
	,@v_cantidad_comision_supervisor	INT
	,@v_cantidad_proceso		INT

DECLARE
	@c_Estado_Registro			BIT = 1--Estado Activo
	,@c_IGV						DECIMAL(12, 4)
	,@c_Codigo_Empresa			INT
	,@c_Codigo_Tipo_Cuota		INT = 1-- Tipo Cuota Automatica
	,@c_Codigo_Estado_Cuota		INT = 1-- Estado Cuota Pendiente
	,@c_Codigo_Tipo_Planilla	INT = 2-- Tipo Planilla Supervisor
	,@c_NECESIDAD				CHAR(2) = 'NI' --Necesidad Inmediata

IF (SELECT CURSOR_STATUS('global','articulo_cursor')) >= -1
BEGIN
	IF (SELECT CURSOR_STATUS('global','articulo_cursor')) > -1
	BEGIN
		CLOSE articulo_cursor
	END
	DEALLOCATE articulo_cursor
END

	--Identificadores del Contrato/Proceso
	SELECT TOP 1
		@v_codigo_vendedor = Cod_Vendedor
		,@v_codigo_tipo_venta = tv.codigo_tipo_venta--CASE WHEN cc.Cod_Tipo_Venta = 'NF' THEN 2 ELSE 1 END,
		,@v_codigo_tipo_pago = tp.codigo_tipo_pago--CASE WHEN Cod_FormaPago = 'CRED' THEN 2 ELSE 1 END,
		,@v_codigo_moneda = m.codigo_moneda--CASE WHEN DocCur = 'SOL' THEN 1 ELSE 2 END,
		,@v_fecha_proceso = cc.CreateDate
	FROM dbo.cabecera_contrato cc
	INNER JOIN dbo.tipo_venta tv
		ON tv.codigo_equivalencia = cc.Cod_Tipo_Venta
	INNER JOIN dbo.tipo_pago tp
		ON tp.codigo_equivalencia = cc.Cod_FormaPago
	INNER JOIN dbo.moneda m
		ON m.codigo_equivalencia = cc.DocCur
	WHERE cc.NumAtCard = @p_nro_contrato AND cc.Codigo_empresa = @p_codigo_empresa

	--Algunos valores que no cambiaran en todo el proceso
	SET @c_IGV = (SELECT TOP 1 ROUND(CONVERT(DECIMAL(12, 4), valor)/100, 4) FROM dbo.parametro_sistema WHERE codigo_parametro_sistema = 9)-- Confirmar el codigo_parametro_sistema
	SET @c_Codigo_Empresa = (SELECT TOP 1 codigo_empresa FROM dbo.empresa_sigeco WHERE codigo_equivalencia = @p_codigo_empresa)

	SET @v_codigo_registro = (SELECT TOP 1 codigo_personal_canal_grupo FROM dbo.cronograma_pago_comision WHERE codigo_cronograma = @p_codigo_cronograma)
	--SET @v_codigo_canal_grupo = (SELECT TOP 1 codigo_canal_grupo FROM dbo.personal_canal_grupo WHERE codigo_registro = @v_codigo_registro)
	SET @v_codigo_supevisor = dbo.fn_obtener_personal_supervisor(@p_nro_contrato, @p_codigo_empresa, 1)
	SET @p_observacion = 'No generó comision de supervisor'
	--------------------------------------------------------------------
	-- obtiene el codigo_registro del supervisor
	--------------------------------------------------------------------
	SELECT TOP 1
		@v_codigo_registro = codigo_registro, @v_codigo_canal = codigo_canal, @v_es_canal_grupo = CASE WHEN codigo_canal_grupo <> codigo_canal THEN 2 ELSE 1 END
	FROM
		dbo.personal_canal_grupo
	WHERE
		codigo_personal = @v_codigo_supevisor
		--AND codigo_canal_grupo = @v_codigo_canal_grupo
		AND (es_supervisor_canal = 1 OR es_supervisor_grupo = 1)
		AND percibe_comision = 1 AND estado_registro = 1

	IF (@v_codigo_canal IS NULL)-- No tiene Supervisor
	BEGIN
		IF EXISTS(SELECT TOP 1 codigo_registro FROM dbo.personal_canal_grupo WHERE codigo_personal = @v_codigo_supevisor AND percibe_comision = 0)
			SET @p_observacion = NULL--@p_observacion + ' porque no percibe comision.'
		ELSE
		BEGIN
			IF EXISTS(SELECT TOP 1 codigo_registro FROM dbo.personal_canal_grupo WHERE codigo_personal = @v_codigo_supevisor AND estado_registro = 0)
				SET @p_observacion = @p_observacion + ' por estar inactivo su registro de canal/grupo.'
			ELSE
			BEGIN
				IF EXISTS(SELECT TOP 1 codigo_registro FROM dbo.personal_canal_grupo WHERE codigo_personal = @v_codigo_supevisor AND es_supervisor_canal = 0 AND es_supervisor_grupo = 0)
					SET @p_observacion = @p_observacion + ' por no estar registrado como supervisor.'
			END
		END
		SET NOEXEC ON;
	END
	
	SELECT TOP 1 
		@v_codigo_campo_santo = codigo_campo_santo
	FROM
		dbo.articulo_cronograma
	WHERE
		codigo_cronograma = @p_codigo_cronograma

	--Obtenemos la Forma Pago de Comision Supervisor, solo 1 comision
	SELECT TOP 1 @v_valor_pago = valor_pago, @v_incluye_igv = incluye_igv
	FROM
		dbo.pcc_regla_calculo_comision_supervisor
	WHERE 
		(codigo_campo_santo = @v_codigo_campo_santo or codigo_campo_santo = 0)
		and (codigo_empresa = @c_Codigo_Empresa or codigo_empresa = 0)
		and (codigo_canal_grupo = @v_codigo_canal or codigo_canal_grupo = 0)
		and tipo_supervisor = @v_es_canal_grupo
		and @v_fecha_proceso between vigencia_inicio and vigencia_fin 
	ORDER BY
		codigo_campo_santo desc, codigo_empresa desc, codigo_canal_grupo desc

	IF (@v_valor_pago IS NULL)
	BEGIN
		SET @v_cantidad_articulos_contrato = (SELECT COUNT(codigo_articulo) from articulo_cronograma WHERE codigo_cronograma = @p_codigo_cronograma)
		SELECT @v_cantidad_comision_supervisor = COUNT(c.codigo_comision)
		FROM dbo.comision_precio_supervisor c
		INNER JOIN dbo.precio_articulo p 
			ON p.codigo_precio = c.codigo_precio and p.estado_registro = 1
		INNER JOIN articulo_cronograma ac 
			ON ac.codigo_cronograma = @p_codigo_cronograma AND ac.codigo_articulo = p.codigo_articulo
		WHERE c.estado_registro = 1 
			AND c.codigo_canal_grupo = @v_codigo_canal
			AND p.codigo_tipo_venta = @v_codigo_tipo_venta
			AND c.codigo_tipo_pago = @v_codigo_tipo_pago
			AND p.codigo_empresa = @c_Codigo_Empresa
			AND p.codigo_moneda = @v_codigo_moneda
			AND convert(date, getdate()) between convert(date, c.vigencia_inicio) and convert(date, c.vigencia_fin)

		IF (@v_cantidad_articulos_contrato = @v_cantidad_comision_supervisor)
		BEGIN
			SET @v_es_comision_precio = 1
			SET @v_incluye_igv = 1
		END
		ELSE
		BEGIN
			IF (@v_cantidad_comision_supervisor = 0)
				SET @p_observacion = @p_observacion + ' porque no tiene regla de pago.'
			ELSE
				SET @p_observacion = @p_observacion + ' porque algún artículo no tiene comisión registrada.'
			SET NOEXEC ON;
		END
	END

	--print '@v_es_comision_precio ' + convert(varchar, @v_es_comision_precio)

	-- Cabecera de Cronograma
	INSERT INTO dbo.cronograma_pago_comision
		(codigo_tipo_planilla, codigo_empresa, codigo_personal_canal_grupo, nro_contrato, codigo_tipo_venta, codigo_tipo_pago, codigo_moneda, fecha_registro, estado_registro, nro_contrato_adicional)
	SELECT
		@c_Codigo_Tipo_Planilla, codigo_empresa, @v_codigo_registro, nro_contrato, codigo_tipo_venta, codigo_tipo_pago, codigo_moneda, fecha_registro, @c_Estado_Registro, CASE WHEN dbo.fn_generar_cronograma_comision_eval_hoja_resumen(@p_nro_contrato) = 1 THEN @p_nro_contrato ELSE NULL END
	FROM
		dbo.cronograma_pago_comision
	WHERE
		codigo_cronograma = @p_codigo_cronograma
	SET @v_codigo_cronograma = @@IDENTITY

	SELECT
		@v_fecha_programada = Fec_Pago
	FROM
		dbo.contrato_cuota
	WHERE
		codigo_empresa = @p_codigo_empresa
		AND NumAtCard = @p_nro_contrato
		AND Num_Cuota = 0
		AND Cod_Estado = 'C'
		AND Fec_Pago IS NOT NULL

	IF (@v_fecha_programada IS NULL OR YEAR(@v_fecha_programada) = 1900)
		SET @v_fecha_programada = (SELECT TOP 1 CreateDate FROM dbo.cabecera_contrato WHERE NumAtCard = @p_nro_contrato and Codigo_empresa = @p_codigo_empresa AND Cod_Tipo_Venta = @c_NECESIDAD)

	DECLARE articulo_cursor CURSOR FAST_FORWARD FORWARD_ONLY FOR 
	SELECT
		codigo_articulo, monto_comision, cantidad, (select top 1 case when a.cantidad_unica = 0 then dbo.articulo_cronograma.cantidad else 1 end  from dbo.articulo a where a.codigo_articulo = dbo.articulo_cronograma.codigo_articulo), codigo_campo_santo
	FROM
		dbo.articulo_cronograma
	WHERE
		codigo_cronograma = @p_codigo_cronograma
		
	OPEN articulo_cursor
	FETCH NEXT FROM articulo_cursor
	INTO @v_codigo_articulo, @v_monto_comision, @v_cantidad, @v_cantidad_proceso, @v_codigo_campo_santo

	WHILE @@FETCH_STATUS = 0  
	BEGIN 

		IF (@v_es_comision_precio = 1)
		BEGIN
			SET @v_monto_comision = NULL
			SELECT 
				@v_monto_comision =
				CASE c.codigo_tipo_comision_supervisor 
					WHEN 1 THEN c.valor
					WHEN 2 THEN (c.valor / 100) * p.precio_total
					WHEN 3 THEN (c.valor / 100) * @v_monto_comision
				END
			FROM dbo.comision_precio_supervisor c
			INNER JOIN dbo.precio_articulo p 
				ON p.codigo_precio = c.codigo_precio and p.estado_registro = 1
			WHERE c.estado_registro = 1 
				AND c.codigo_canal_grupo = @v_codigo_canal
				AND p.codigo_tipo_venta = @v_codigo_tipo_venta
				AND c.codigo_tipo_pago = @v_codigo_tipo_pago
				AND p.codigo_empresa = @c_Codigo_Empresa
				AND p.codigo_moneda = @v_codigo_moneda
				AND p.codigo_articulo = @v_codigo_articulo

			SET @v_monto_comision = ROUND(@v_cantidad_proceso * @v_monto_comision, 4)
		END
		ELSE
			SET @v_monto_comision = ROUND(@v_valor_pago * @v_monto_comision, 4)

		IF (@v_incluye_igv = 1)
		BEGIN
			SET @v_monto_comision_igv = ROUND( ((@c_IGV * 100) * @v_monto_comision) / (100 + (@c_IGV * 100)) , 4)
			SET @v_monto_comision_sin_igv = ROUND(@v_monto_comision - @v_monto_comision_igv, 4)
		END
		ELSE
		BEGIN
			SET @v_monto_comision_sin_igv = @v_monto_comision 
			SET @v_monto_comision_igv = @v_monto_comision * @c_IGV
			SET @v_monto_comision = @v_monto_comision  + @v_monto_comision_igv				
		END

		-- Cada Articulo Cronograma
		INSERT INTO dbo.articulo_cronograma
			(codigo_cronograma, codigo_articulo, monto_comision, cantidad, codigo_campo_santo, estado_registro)
		VALUES
			(@v_codigo_cronograma, @v_codigo_articulo, @v_monto_comision, @v_cantidad, @v_codigo_campo_santo, @c_Estado_Registro)

		INSERT INTO dbo.detalle_cronograma
			(codigo_cronograma, codigo_articulo, nro_cuota, fecha_programada, monto_bruto, igv, monto_neto, codigo_tipo_cuota, codigo_estado_cuota, estado_registro)
		VALUES
			(@v_codigo_cronograma, @v_codigo_articulo, 1, @v_fecha_programada, @v_monto_comision_sin_igv , @v_monto_comision_igv, @v_monto_comision, @c_Codigo_Tipo_Cuota, @c_Codigo_Estado_Cuota, @c_Estado_Registro)

		FETCH NEXT FROM articulo_cursor   
		INTO @v_codigo_articulo, @v_monto_comision, @v_cantidad, @v_cantidad_proceso, @v_codigo_campo_santo
	END   
	
	CLOSE articulo_cursor;  
	DEALLOCATE articulo_cursor; 
	
	SET @p_observacion = NULL
SET NOEXEC OFF;
END