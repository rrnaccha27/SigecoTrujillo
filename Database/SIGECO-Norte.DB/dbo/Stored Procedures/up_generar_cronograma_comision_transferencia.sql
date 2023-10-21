CREATE PROCEDURE dbo.up_generar_cronograma_comision_transferencia
(
	@p_codigo_empresa	VARCHAR(4)
	,@p_nro_contrato	VARCHAR(100)
	,@p_codigo_proceso	UNIQUEIDENTIFIER
	,@p_observacion		VARCHAR(200) output
)
AS
BEGIN
	SET NOEXEC OFF
	SET NOCOUNT ON

	DECLARE
		@v_ContratoTransferencia				VARCHAR(100) 
		,@v_EmpresaTransferencia				VARCHAR(4)
		,@v_MontoTransferencia					DECIMAL(12, 4)
		,@v_monto_comision						DECIMAL(12, 4)
		,@v_monto_comision_no_pagado_ref		DECIMAL(12, 4)
		,@v_monto_comision_pagado_transferencia	DECIMAL(12, 4)
		,@v_codigo_empresa_transferencia		INT
		,@v_codigo_cronograma_transferencia		INT
		,@v_motivo_transferencia				VARCHAR(200)
	
	DECLARE
		@c_codigo_estado_cuota_pendiente	INT = 1
		,@c_codigo_estado_cuota_proceso		INT = 2
		,@c_codigo_estado_cuota_pagada		INT = 3
		,@c_codigo_estado_cuota_excluida	INT = 4
		,@c_codigo_tipo_planilla			INT = 1

	
	DECLARE
		@v_total			INT
		,@v_indice			INT
		,@v_codigo_detalle	INT
	
	DECLARE
		@t_detalle_cronograma	TABLE(
			indice			int identity(1,1)
			,codigo_detalle	int
		)

	SELECT
		TOP 1
		@v_ContratoTransferencia = ContratoTransferencia
		,@v_EmpresaTransferencia = EmpresaTransferencia
		,@v_MontoTransferencia = MontoTransferencia
	FROM
		dbo.cabecera_contrato
	WHERE
		Codigo_empresa = @p_codigo_empresa
		AND NumAtCard = @p_nro_contrato	
		AND Flg_Transferencia = 1

	IF (@v_ContratoTransferencia IS NULL OR @v_ContratoTransferencia = '0' OR LEN(ISNULL(@v_ContratoTransferencia, '')) = 0)
		SET NOEXEC ON

	IF (@v_EmpresaTransferencia IS NULL OR @v_EmpresaTransferencia = '0' OR LEN(ISNULL(@v_EmpresaTransferencia, '')) = 0)
		SET NOEXEC ON

	SELECT TOP 1 
		@v_codigo_empresa_transferencia = codigo_empresa 
		,@v_motivo_transferencia = 'POR TRANSFERENCIA ' + nombre + @p_nro_contrato 
	FROM 
		dbo.empresa_sigeco 
	WHERE codigo_equivalencia = @v_EmpresaTransferencia

	IF (@v_codigo_empresa_transferencia IS NULL)
		SET NOEXEC ON

	IF (ISNULL(@v_MontoTransferencia, 0) = 0)
	BEGIN
		SET @v_codigo_cronograma_transferencia = (SELECT TOP 1 codigo_cronograma FROM dbo.cronograma_pago_comision WHERE codigo_empresa = @v_codigo_empresa_transferencia AND nro_contrato = @v_ContratoTransferencia AND codigo_tipo_planilla = @c_codigo_tipo_planilla)

		IF (@v_codigo_cronograma_transferencia IS NULL)
			SET NOEXEC ON

		SET @v_monto_comision_pagado_transferencia = (SELECT SUM(monto_neto) FROM dbo.detalle_cronograma WHERE codigo_cronograma = @v_codigo_cronograma_transferencia AND codigo_estado_cuota = @c_codigo_estado_cuota_pagada)

		IF (@v_monto_comision_pagado_transferencia = 0)
			SET NOEXEC ON
	END
	ELSE
		SET @v_monto_comision_pagado_transferencia = @v_MontoTransferencia

	SET @v_monto_comision = (SELECT SUM(monto_comision) FROM dbo.regla_pago_comision_temporal WHERE id = @p_codigo_proceso)

	IF (@v_monto_comision < @v_monto_comision_pagado_transferencia)
	BEGIN
		SET @p_observacion = 'La comision por transferencia (' + CONVERT(VARCHAR, @v_monto_comision_pagado_transferencia) + ') es mayor la comision del contrato(' + CONVERT(VARCHAR, @v_monto_comision) + ').'
		SET NOEXEC ON
	END

	UPDATE dbo.regla_pago_comision_temporal
	SET monto_comision = monto_comision - (((monto_comision/@v_monto_comision)*100)*@v_monto_comision_pagado_transferencia)/100
	WHERE id = @p_codigo_proceso

	--TODO: aquellas cuotas no pagadas hay que "anularlas"
	INSERT INTO @t_detalle_cronograma (codigo_detalle)
	SELECT
		codigo_detalle
	FROM
		dbo.detalle_cronograma
	WHERE
		codigo_cronograma = @v_codigo_cronograma_transferencia AND codigo_estado_cuota IN(@c_codigo_estado_cuota_pendiente, @c_codigo_estado_cuota_proceso, @c_codigo_estado_cuota_excluida)

	SET @v_total = ISNULL((SELECT COUNT(codigo_detalle) FROM @t_detalle_cronograma), 0)
	SET @v_indice = 1

	IF (@v_total = 0)
		SET NOEXEC ON

	WHILE(@v_indice <= @v_total)
	BEGIN

		SET @v_codigo_detalle = (SELECT codigo_detalle FROM @t_detalle_cronograma WHERE indice = @v_indice)

		BEGIN TRY		
			EXEC up_planilla_anular_pago_comision @v_codigo_detalle, 'root', @v_motivo_transferencia
		END TRY
		BEGIN CATCH
		END CATCH

		SET @v_indice = @v_indice + 1
	END

	--TODO: todo el cronograma del ref debe "anularse" si tuvo alguna cuota NO pagada

	SET NOEXEC OFF
	SET NOCOUNT OFF

END;