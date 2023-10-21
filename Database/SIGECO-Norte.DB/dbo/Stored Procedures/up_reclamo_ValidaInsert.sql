CREATE PROCEDURE [dbo].[up_reclamo_ValidaInsert]
	@codigo_personal	int=NULL
	,@NroContrato		varchar(20)
	,@codigo_articulo	int=NULL
	,@codigo_empresa	int=NULL
	,@Cuota				int=NULL
	,@Importe			decimal(18,2)
AS
BEGIN
	DECLARE @TIPO VARCHAR(10)
	DECLARE @MSJ VARCHAR(100)
	DECLARE @codigo_equivalencia_vendedor	NVARCHAR(20)
	DECLARE @codigo_equivalencia_empresa	NVARCHAR(4)

	SET @TIPO=''		--TIPO:ERROR,ALERT,SUCCESS
	SET @MSJ=''

	----VALIDACION
	---------------------------------------------------------------------------------------------------
	SELECT TOP 1 @codigo_equivalencia_vendedor=codigo_equivalencia FROM dbo.personal WHERE codigo_personal=@codigo_personal and estado_registro = 1
	SELECT TOP 1 @codigo_equivalencia_empresa=codigo_equivalencia FROM dbo.empresa_sigeco WHERE codigo_empresa=@codigo_empresa and estado_registro = 1

	IF EXISTS(SELECT TOP 1 * FROM dbo.cabecera_contrato 
	WHERE Codigo_empresa=@codigo_equivalencia_empresa AND CONVERT(INT,NumAtCard) = CONVERT(INT,@NroContrato) AND Cod_Vendedor=@codigo_equivalencia_vendedor)
	BEGIN
		SET @TIPO='SUCCESS'
	END
	ELSE
	BEGIN
		SET @TIPO='ALERT'
		SET @MSJ='El contrato no pertence al personal de ventas'
	END

	--IF @TIPO='SUCCESS'
	--BEGIN
	--	--Valida Existe Pago en cronograma
	--	IF NOT EXISTS(SELECT n1.codigo_cronograma FROM cronograma_pago_comision n1
	--	INNER JOIN detalle_cronograma n2 ON n1.codigo_cronograma=n2.codigo_cronograma
	--	INNER JOIN personal_canal_grupo n3 ON n1.codigo_personal_canal_grupo=n3.codigo_registro
	--	WHERE n1.codigo_empresa = CONVERT(INT,@codigo_empresa)
	--	AND n3.codigo_personal=@codigo_personal
	--	AND CONVERT(INT,n1.nro_contrato)=CONVERT(INT,@NroContrato)
	--	AND n2.codigo_articulo=@codigo_articulo
	--	AND n2.nro_cuota=@Cuota)
	--	BEGIN
	--		SET @TIPO='ALERT'
	--		SET @MSJ='El pago de la cuota no existe en cronograma'
	--	END
	--	ELSE
	--	BEGIN
	--		DECLARE @ValidaImporte	DECIMAL(18,2)
	--		SET @ValidaImporte = 0
	--		SELECT TOP 1 @ValidaImporte=n2.monto_neto FROM cronograma_pago_comision n1
	--		INNER JOIN detalle_cronograma n2 ON n1.codigo_cronograma=n2.codigo_cronograma
	--		INNER JOIN personal_canal_grupo n3 ON n1.codigo_personal_canal_grupo=n3.codigo_registro
	--		WHERE n1.codigo_empresa = CONVERT(INT,@codigo_empresa)
	--		AND n3.codigo_personal=@codigo_personal
	--		AND CONVERT(INT,n1.nro_contrato)=CONVERT(INT,@NroContrato)
	--		AND n2.codigo_articulo=@codigo_articulo
	--		AND n2.nro_cuota=@Cuota

	--		IF @ValidaImporte>=@Importe
	--		BEGIN
	--			SET @TIPO='ALERT'
	--			SET @MSJ='El importe de reclamo debe ser mayor a la cuota de pago comisión'
	--		END
	--		ELSE
	--		BEGIN
			
	--			----VALIDA EXISTE PLANILLA
	--			--IF EXISTS(SELECT * FROM vwCronogramagaPagoPlanilla
	--			--WHERE codigo_empresa = @codigo_empresa
	--			--AND codigo_personal=@codigo_personal
	--			--AND CONVERT(INT,nro_contrato)=CONVERT(INT,@NroContrato)
	--			--AND codigo_articulo=@codigo_articulo
	--			--AND nro_cuota=@Cuota
	--			--)
	--			--BEGIN
	--			--VALIDA CUOTA EXCLUIDA
	--			IF EXISTS(SELECT * FROM vwCronogramagaPagoPlanilla
	--			WHERE codigo_empresa = @codigo_empresa
	--			AND codigo_personal=@codigo_personal
	--			AND CONVERT(INT,nro_contrato)=CONVERT(INT,@NroContrato)
	--			AND codigo_articulo=@codigo_articulo
	--			AND nro_cuota=@Cuota AND codigo_estado_cuota not in (2, 3)-- and codigo_estado_planilla <> 3
	--			)
	--			BEGIN
	--				SET @TIPO='ALERT'
	--				SET @MSJ='La cuota debe estar en una planilla'
	--			END
	--			--END

	--		END
	--	END
	--END

	SELECT @TIPO + '|' + @MSJ AS Respuesta

END