CREATE PROCEDURE [dbo].[up_migracion_sap_validar_contrato]
(
	 @p_codigo_empresa	NVARCHAR(4)
	,@p_nro_contrato	NVARCHAR(100)
	,@p_validacion		INT OUTPUT
	,@p_observacion		VARCHAR(100) OUTPUT
)
AS
BEGIN
	SET NOEXEC OFF;
	SET NOCOUNT ON;
	
	DECLARE
		 @v_codigo_empresa	INT
		,@v_cuotas	INT
		,@v_esContratoAdicional		BIT = 0
		,@v_bloqueo					BIT = 0
	
	DECLARE
		@t_cronogramas_borrar TABLE
		(
			codigo_cronograma	INT
		)
	
	SET @p_validacion = 0 --Error critico
	
	SELECT TOP 1
		@v_bloqueo = bloqueo
	FROM
		dbo.contrato_migrado
	WHERE
		Codigo_empresa = @p_codigo_empresa
		AND NumAtCard = @p_nro_contrato
	
	IF ISNULL(@v_bloqueo, 0) = 1
	BEGIN
		SET @p_validacion = 1
		SET @p_observacion = 'El contrato esta bloqueado para proceso.'
		SET NOEXEC ON;
	END

	SET @v_codigo_empresa = (SELECT TOP 1 codigo_empresa FROM dbo.empresa_sigeco WHERE codigo_equivalencia = @p_codigo_empresa)
	
	IF @v_codigo_empresa IS NULL
	BEGIN
		SET @p_observacion = 'La empresa no existe en el aplicativo'
		SET NOEXEC ON;
	END
	
	SET @v_esContratoAdicional = dbo.fn_generar_cronograma_comision_eval_hoja_resumen(@p_nro_contrato)

	SET @v_cuotas = NULL

	SELECT
		@v_cuotas = COUNT(cpc.codigo_cronograma)
	FROM
		dbo.cronograma_pago_comision cpc
	INNER JOIN dbo.detalle_cronograma dc
		ON dc.codigo_cronograma = cpc.codigo_cronograma AND dc.codigo_estado_cuota in (2)
	WHERE
		cpc.codigo_empresa = @v_codigo_empresa
		AND CASE WHEN @v_esContratoAdicional = 1 THEN cpc.nro_contrato_adicional ELSE cpc.nro_contrato END = @p_nro_contrato
			
	SET @v_cuotas = ISNULL(@v_cuotas, 0)

	IF (@v_cuotas > 0)
	BEGIN
		SET @p_validacion = 1
		SET @p_observacion = 'El contrato tiene cronograma con cuotas en proceso pago.'
		SET NOEXEC ON;
	END

	SET @v_cuotas = NULL

	SELECT
		@v_cuotas = COUNT(cpc.codigo_cronograma)
	FROM
		dbo.cronograma_pago_comision cpc
	INNER JOIN dbo.detalle_cronograma dc
		ON dc.codigo_cronograma = cpc.codigo_cronograma AND dc.codigo_estado_cuota in (3)
	WHERE
		cpc.codigo_empresa = @v_codigo_empresa
		AND CASE WHEN @v_esContratoAdicional = 1 THEN cpc.nro_contrato_adicional ELSE cpc.nro_contrato END = @p_nro_contrato
			
	SET @v_cuotas = ISNULL(@v_cuotas, 0)

	IF (@v_cuotas > 0)
	BEGIN
		SET @p_validacion = 2
		SET @p_observacion = 'El contrato tiene cronograma con cuotas en pagadas.'
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
		SET @p_observacion = 'El contrato no tiene cronograma.'
		SET NOEXEC ON;
	END

	SET @p_observacion = 'El contrato puede ser reprocesado/migrado.'
	SET @p_validacion = 3
	
	SET NOCOUNT OFF;
	SET NOEXEC OFF;
END; --SP