CREATE PROCEDURE [dbo].[up_contrato_anular_2]
(
	 @p_codigo_empresa	VARCHAR(4)
	,@p_nro_contrato	VARCHAR(100)
)
AS
BEGIN
	SET NOEXEC OFF;
	SET NOCOUNT ON;
	
	DECLARE
		 @v_codigo_empresa	INT
		,@v_esContratoAdicional		BIT = 0
	
	DECLARE
		@t_cronogramas_borrar TABLE
		(
			codigo_cronograma	INT
		)
	
	SET @p_nro_contrato = RIGHT('0000000000' + @p_nro_contrato, 10)
	SET @v_codigo_empresa = (SELECT TOP 1 codigo_empresa FROM dbo.empresa_sigeco WHERE codigo_equivalencia = @p_codigo_empresa)
	
	IF @v_codigo_empresa IS NULL
	BEGIN
		PRINT 'La empresa no existe en el aplicativo'
		SET NOEXEC ON;
	END
	
	SET @v_esContratoAdicional = dbo.fn_generar_cronograma_comision_eval_hoja_resumen(@p_nro_contrato)

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
		DELETE FROM dbo.detalle_contrato where NumAtCard = @p_nro_contrato and Codigo_empresa = @p_codigo_empresa
		DELETE FROM dbo.difunto_contrato where NumAtCard = @p_nro_contrato and Codigo_empresa = @p_codigo_empresa
		DELETE FROM dbo.contrato_cuota where NumAtCard = @p_nro_contrato and Codigo_empresa = @p_codigo_empresa
		DELETE FROM dbo.cabecera_contrato where NumAtCard = @p_nro_contrato and Codigo_empresa = @p_codigo_empresa
		DELETE FROM dbo.contrato_migrado where NumAtCard = @p_nro_contrato and Codigo_empresa = @p_codigo_empresa

		PRINT 'No existe cronograma a eliminar'
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
		--DELETE FROM dbo.detalle_planilla where codigo_detalle_cronograma IN	(SELECT codigo_detalle FROM dbo.detalle_cronograma WHERE codigo_cronograma IN (SELECT codigo_cronograma FROM @t_cronogramas_borrar))
		DELETE FROM dbo.detalle_cronograma WHERE codigo_cronograma IN (SELECT codigo_cronograma FROM @t_cronogramas_borrar)
		DELETE FROM dbo.articulo_cronograma WHERE codigo_cronograma IN (SELECT codigo_cronograma FROM @t_cronogramas_borrar)
		DELETE FROM dbo.cronograma_pago_comision WHERE codigo_cronograma IN (SELECT codigo_cronograma FROM @t_cronogramas_borrar)
	
		DELETE FROM dbo.detalle_contrato where NumAtCard = @p_nro_contrato and Codigo_empresa = @p_codigo_empresa
		DELETE FROM dbo.difunto_contrato where NumAtCard = @p_nro_contrato and Codigo_empresa = @p_codigo_empresa
		DELETE FROM dbo.contrato_cuota where NumAtCard = @p_nro_contrato and Codigo_empresa = @p_codigo_empresa
		DELETE FROM dbo.cabecera_contrato where NumAtCard = @p_nro_contrato and Codigo_empresa = @p_codigo_empresa
		DELETE FROM dbo.contrato_migrado where NumAtCard = @p_nro_contrato and Codigo_empresa = @p_codigo_empresa

		PRINT 'El contrato fue anulado.'
	END TRY
	BEGIN CATCH
		PRINT 'El contrato no pudo ser removido.'
		SELECT  
			ERROR_NUMBER() AS ErrorNumber  
			,ERROR_SEVERITY() AS ErrorSeverity  
			,ERROR_STATE() AS ErrorState  
			,ERROR_PROCEDURE() AS ErrorProcedure  
			,ERROR_LINE() AS ErrorLine  
			,ERROR_MESSAGE() AS ErrorMessage;  
		
		SET NOEXEC ON;
	END CATCH
	
	SET NOCOUNT OFF;
	SET NOEXEC OFF;
END --SP