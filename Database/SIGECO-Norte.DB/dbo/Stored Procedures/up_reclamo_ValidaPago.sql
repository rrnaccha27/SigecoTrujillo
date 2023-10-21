CREATE PROCEDURE [dbo].[up_reclamo_ValidaPago]
	@codigo_personal		INT
	,@nro_contrato			VARCHAR(20)
	,@codigo_empresa		VARCHAR(20)
	,@codigo_articulo		INT
	,@nro_cuota				INT
	,@Importe               decimal(18,2)
AS
BEGIN
	DECLARE @RESPUESTA AS VARCHAR	
	--caso 1 valida pago existe en detalla cronograma, y si la planilla esta
	IF NOT EXISTS(SELECT n1.codigo_cronograma FROM cronograma_pago_comision n1
	INNER JOIN detalle_cronograma n2 ON n1.codigo_cronograma=n2.codigo_cronograma
	INNER JOIN personal_canal_grupo n3 ON n1.codigo_personal_canal_grupo=n3.codigo_registro
	WHERE n1.codigo_empresa = CONVERT(INT,@codigo_empresa)
	AND n3.codigo_personal=@codigo_personal
	AND n1.nro_contrato = @nro_contrato
	AND n2.codigo_articulo=@codigo_articulo
	AND n2.nro_cuota=@nro_cuota)
	BEGIN
		SELECT 'NO_EXISTE_PAGO_CRONOGRAMA'
	END
	ELSE
	BEGIN
		DECLARE @ValidaImporte	DECIMAL(18,2)
		SET @ValidaImporte = 0
		SELECT TOP 1 @ValidaImporte=n2.monto_neto FROM cronograma_pago_comision n1
		INNER JOIN detalle_cronograma n2 ON n1.codigo_cronograma=n2.codigo_cronograma
		INNER JOIN personal_canal_grupo n3 ON n1.codigo_personal_canal_grupo=n3.codigo_registro
		WHERE n1.codigo_empresa = CONVERT(INT,@codigo_empresa)
		AND n3.codigo_personal=@codigo_personal
		AND n1.nro_contrato = @nro_contrato
		AND n2.codigo_articulo=@codigo_articulo
		AND n2.nro_cuota=@nro_cuota

		IF @ValidaImporte>=@Importe
		BEGIN
			SELECT 'IMPORTE_DEBE_SER_MAYOR'
		END
		ELSE
		BEGIN
			
			IF NOT EXISTS(SELECT * FROM vwCronogramagaPagoPlanilla
			WHERE codigo_empresa = CONVERT(INT,@codigo_empresa)
			AND codigo_personal=@codigo_personal
			AND nro_contrato = @nro_contrato
			AND codigo_articulo=@codigo_articulo
			AND nro_cuota=@nro_cuota
			)
			BEGIN
				SELECT 'NO_EXISTE_PAGO_PLANILLA'
			END
			ELSE
			BEGIN
				IF EXISTS(SELECT * FROM vwCronogramagaPagoPlanilla
				WHERE codigo_empresa = CONVERT(INT,@codigo_empresa)
				AND codigo_personal=@codigo_personal
				AND nro_contrato = @nro_contrato
				AND codigo_articulo=@codigo_articulo
				AND nro_cuota=@nro_cuota
				AND codigo_estado_planilla=1		--1:Abierta,2:Cerrada
				)
				BEGIN
					SELECT 'PLANILLA_ABIERTA'
					/*
					IF EXISTS(SELECT * FROM vwCronogramagaPagoPlanilla
					WHERE codigo_empresa = CONVERT(INT,@codigo_empresa)
					AND codigo_personal=@codigo_personal
					AND nro_contrato=@nro_contrato
					AND codigo_articulo=@codigo_articulo
					AND nro_cuota=@nro_cuota
					AND codigo_estado_cuota=2		--1:Pendiente,2:En proceso pago,3:Pagada
					AND codigo_estado_planilla=1		--1:Abierta,2:Cerrada
					)
					BEGIN
						SELECT 'PLANILLA_ABIERTA_CUOTA_ENPROCESO'
					END
					ELSE
					BEGIN
						SELECT 'PLANILLA_ABIERTA_CUOTA_ENPROCESO'
					END
					*/
				END
				ELSE
				BEGIN
					SELECT 'PLANILLA_CERRADA'				
				END
			END			
		END		
	END
	
END