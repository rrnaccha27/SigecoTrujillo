CREATE PROCEDURE [dbo].[up_generar_cronograma_validar_contrato]
(
	 @p_codigo_empresa	NVARCHAR(4)
	,@p_nro_contrato	NVARCHAR(100)
	,@p_genera_comision	BIT OUTPUT
	,@p_observacion		VARCHAR(100) OUTPUT
)
AS
BEGIN
	SET NOEXEC OFF;
	SET NOCOUNT ON;
	
	DECLARE
		 @v_codigo_empresa	INT
		,@v_cuotas_pagadas	INT
		,@v_esContratoAdicional		BIT = 0
		,@v_bloqueo					BIT = 0
	
	DECLARE
		@t_cronogramas_borrar TABLE
		(
			codigo_cronograma	INT
		)
	
	SET @p_genera_comision = 0
	
	SET @v_codigo_empresa = (SELECT TOP 1 codigo_empresa FROM dbo.empresa_sigeco WHERE codigo_equivalencia = @p_codigo_empresa)
	
	IF @v_codigo_empresa IS NULL
	BEGIN
		SET @p_observacion = 'La empresa no existe en el aplicativo'
		SET NOEXEC ON;
	END
	
	SELECT TOP 1
		@v_bloqueo = bloqueo
	FROM
		dbo.contrato_migrado
	WHERE
		Codigo_empresa = @p_codigo_empresa
		AND NumAtCard = @p_nro_contrato
	
	IF ISNULL(@v_bloqueo, 0) = 1
	BEGIN
		SET @p_observacion = 'El contrato esta bloqueado para proceso.'
		SET NOEXEC ON;
	END

	SET @v_esContratoAdicional = dbo.fn_generar_cronograma_comision_eval_hoja_resumen(@p_nro_contrato)

	IF (@v_esContratoAdicional = 1)
	BEGIN
		UPDATE cab
		SET cab.codigo_grupo = p.codigo_equivalencia_grupo
		FROM dbo.cabecera_contrato cab 
		INNER JOIN vw_personal p ON p.codigo_equivalencia = cab.Cod_Supervisor
		WHERE cab.NumAtCard = @p_nro_contrato AND cab.codigo_empresa = @p_codigo_empresa AND cab.codigo_grupo IS NULL
	END
	
	SELECT 
		@v_cuotas_pagadas = COUNT(cpc.codigo_cronograma)
	FROM
		dbo.cronograma_pago_comision cpc
	INNER JOIN dbo.detalle_cronograma dc
		ON dc.codigo_cronograma = cpc.codigo_cronograma AND dc.codigo_estado_cuota in (2, 3)
	WHERE
		cpc.codigo_empresa = @v_codigo_empresa
		AND CASE WHEN @v_esContratoAdicional = 1 THEN cpc.nro_contrato_adicional ELSE cpc.nro_contrato END = @p_nro_contrato
			
	SET @v_cuotas_pagadas = ISNULL(@v_cuotas_pagadas, 0)

	IF (@v_cuotas_pagadas > 0)
	BEGIN
		IF EXISTS(SELECT DocEntry FROM dbo.cabecera_contrato WHERE Codigo_empresa = @p_codigo_empresa and NumAtCard = @p_nro_contrato AND LTRIM(RTRIM(Cod_Estado_Contrato)) <> '')
			--OR
			--((charindex('ANL', @p_nro_contrato) > 0) or (charindex('A', @p_nro_contrato) > 0 and charindex('N', @p_nro_contrato) > 0 AND charindex('L', @p_nro_contrato) > 0 ))
			--OR
			--((charindex('RES', @p_nro_contrato) > 0) or (charindex('R', @p_nro_contrato) > 0 and charindex('E', @p_nro_contrato) > 0 AND charindex('S', @p_nro_contrato) > 0 ))
		BEGIN
			SET @p_observacion = 'El contrato esta anulado o rescindido y tiene cuotas en proceso de pago o pagadas.'
		END
		ELSE
			SET @p_observacion = 'El contrato tiene cronograma con cuotas en proceso de pago o pagadas.'

		SET NOEXEC ON;
	END

	INSERT INTO @t_cronogramas_borrar(codigo_cronograma)
	SELECT 
		codigo_cronograma
	FROM 
		dbo.cronograma_pago_comision 
	WHERE 
		codigo_empresa = @v_codigo_empresa 
		AND CASE WHEN @v_esContratoAdicional = 1 THEN nro_contrato_adicional ELSE nro_contrato END = @p_nro_contrato
	
	IF NOT EXISTS(SELECT TOP 1 codigo_cronograma FROM @t_cronogramas_borrar)
	BEGIN

		IF EXISTS(SELECT DocEntry FROM dbo.cabecera_contrato WHERE Codigo_empresa = @p_codigo_empresa and NumAtCard = @p_nro_contrato AND LTRIM(RTRIM(Cod_Estado_Contrato)) <> '')
		BEGIN
			SET @p_observacion = 'El contrato esta anulado o rescindido.'
			SET NOEXEC ON;
		END

		SET @p_observacion = ''
		SET @p_genera_comision = 1
		SET NOEXEC ON;
	END

	BEGIN TRY
		--MOVEMOS AL LOG LOS DATOS ANTES DE ELIMINARLOS
		INSERT INTO dbo.operacion_cuota_comision_log
		SELECT GETDATE() AS fecha_log, * FROM dbo.operacion_cuota_comision where codigo_detalle_cronograma IN (SELECT codigo_detalle FROM dbo.detalle_cronograma WHERE codigo_cronograma IN (SELECT codigo_cronograma FROM @t_cronogramas_borrar))

		INSERT INTO dbo.detalle_cronograma_log
		SELECT GETDATE() AS fecha_log, * FROM dbo.detalle_cronograma WHERE codigo_cronograma IN (SELECT codigo_cronograma FROM @t_cronogramas_borrar)

		INSERT INTO dbo.articulo_cronograma_log
		SELECT GETDATE() AS fecha_log, * FROM dbo.articulo_cronograma WHERE codigo_cronograma IN (SELECT codigo_cronograma FROM @t_cronogramas_borrar)

		INSERT INTO dbo.cronograma_pago_comision_log
		SELECT GETDATE() AS fecha_log, * FROM dbo.cronograma_pago_comision WHERE codigo_cronograma IN (SELECT codigo_cronograma FROM @t_cronogramas_borrar)

		--Eliminar el cronograma para generar otro activo
		DELETE FROM dbo.operacion_cuota_comision where codigo_detalle_cronograma IN	(SELECT codigo_detalle FROM dbo.detalle_cronograma WHERE codigo_cronograma IN (SELECT codigo_cronograma FROM @t_cronogramas_borrar))
		DELETE FROM dbo.detalle_cronograma WHERE codigo_cronograma IN (SELECT codigo_cronograma FROM @t_cronogramas_borrar)
		DELETE FROM dbo.articulo_cronograma WHERE codigo_cronograma IN (SELECT codigo_cronograma FROM @t_cronogramas_borrar)
		DELETE FROM dbo.cronograma_pago_comision WHERE codigo_cronograma IN (SELECT codigo_cronograma FROM @t_cronogramas_borrar)
	END TRY
	BEGIN CATCH
		SET @p_observacion = 'El contrato no pudo ser removido.'
		SET NOEXEC ON;
	END CATCH

	IF EXISTS(SELECT DocEntry FROM dbo.cabecera_contrato WHERE Codigo_empresa = @p_codigo_empresa and NumAtCard = @p_nro_contrato AND LTRIM(RTRIM(Cod_Estado_Contrato)) <> '')
	BEGIN
		SET @p_observacion = 'El contrato esta anulado o rescindido.'
		SET NOEXEC ON;
	END

	SET @p_genera_comision = 1	
	
	SET NOCOUNT OFF;
	SET NOEXEC OFF;
END; --SP