-- =============================================
-- Author:		Carlos García
-- Create date: 25/05/2018
-- Description:	Procedimiento almacenado que obtiene el detalle de las cuotas en base a un código de empresa y número de contrato
-- =============================================
CREATE PROCEDURE [dbo].[SIGEMO_ObtenerCuotasPorEmpresaContrato]
	-- Add the parameters for the stored procedure here
	@pCodigoEmpresa NVARCHAR(4),
	@pNumeroContrato NVARCHAR(10)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	--SELECT
	--T.Num_Cuota_Texto Num_Cuota
	--,T.simbolo_moneda
	--,T.Num_Importe_Total_SinIgv
	--,T.Num_Importe_Igv
	--,T.Num_Importe_Total
	--,T.Fec_Vencimiento
	--,T.Fec_Pago
	--,T.Cod_Estado
	--,T.Interes_Moratorio
	--FROM (
	--SELECT
	--CAST(A.Num_Cuota AS INT) Num_Cuota,
	--CASE WHEN CAST(A.Num_Cuota AS INT) = 0 THEN 'INICIAL' ELSE CAST(CAST(A.Num_Cuota AS INT) AS NVARCHAR(2)) END Num_Cuota_Texto
	--,CASE B.DocCur
	--WHEN 'SOL' THEN 'S/'
	--WHEN 'DOL' THEN '$'
	--END simbolo_moneda
	--,CAST(A.Num_Importe_Total_SinIgv AS DECIMAL(10,0)) Num_Importe_Total_SinIgv
	--,CAST(A.Num_Importe_Igv AS DECIMAL(10,0)) Num_Importe_Igv
	--,CAST(A.Num_Importe_Total AS DECIMAL(10,0)) Num_Importe_Total
	--,isnull(convert(nvarchar(10), A.Fec_Vencimiento, 103), '') Fec_Vencimiento
	--,isnull(convert(nvarchar(10), A.Fec_Pago, 103), '') Fec_Pago
	--,A.Cod_Estado
	--,0 Interes_Moratorio
	--FROM SIGECO.dbo.contrato_cuota A
	--INNER JOIN SIGECO.dbo.cabecera_contrato B ON B.Codigo_empresa = A.Codigo_empresa AND B.DocEntry = A.DocEntry
	--WHERE A.Codigo_empresa = @pCodigoEmpresa
	--AND A.NumAtCard = @pNumeroContrato ) T
	--ORDER BY T.Num_Cuota

	DECLARE @pDocEntry INT
	DECLARE @TEMPORAL AS TABLE (ID INT IDENTITY(1,1), ORDEN INT, Num_Cuota NVARCHAR(100), Fec_Vencimiento DATETIME, Num_Importe_Total NUMERIC(19,2), SALDO NUMERIC(19,2), MONEDA NVARCHAR(6), TAMEX_MENSUAL INT, TAMEX_DIARIO INT, DIAS INT, Fec_Pago DATETIME, MORA NUMERIC(19,2), ESTADO_CUOTA NVARCHAR(100), TIPO_CUOTA NVARCHAR(100))

	IF @pCodigoEmpresa = '0939'
	BEGIN
		SELECT @pDocEntry = A.DocEntry FROM SAP.DB_FUNERARIA_JARDINES.dbo.ORDR A WHERE /*RIGHT('0000000000'+A.NumAtCard,10)*/A.NumAtCard = @pNumeroContrato
		INSERT INTO @TEMPORAL
		EXEC SAP.SAP_JARDINES.Jardines.USP_Jardines_SAP_JARDINES_ObtenerInformacionCronogramaCuota '0939', @pDocEntry
	END

	IF @pCodigoEmpresa = '0002'
	BEGIN
		SELECT @pDocEntry = A.DocEntry FROM SAP.DB_OPERACIONES_FUNERARIAS.dbo.ORDR A WHERE /*RIGHT('0000000000'+A.NumAtCard,10)*/A.NumAtCard = @pNumeroContrato
		INSERT INTO @TEMPORAL
		EXEC SAP.SAP_JARDINES.Jardines.USP_Jardines_SAP_JARDINES_ObtenerInformacionCronogramaCuota '0002', @pDocEntry
	END	

	SELECT A.Num_Cuota Num_Cuota, CASE A.MONEDA WHEN 'SOL' THEN 'S/ ' WHEN 'DOL' THEN '$ ' END simbolo_moneda, A.Num_Importe_Total / 1.18 Num_Importe_Total_SinIgv, A.Num_Importe_Total / 1.18 * 0.18 Num_Importe_Igv,
	A.Num_Importe_Total Num_Importe_Total, ISNULL(CONVERT(NVARCHAR(10), A.Fec_Vencimiento, 103), '') Fec_Vencimiento, A.Fec_Pago Fec_Pago,
	CASE A.ESTADO_CUOTA
	WHEN 'VENCIDO' THEN 'V'
	WHEN 'PENDIENTE' THEN 'P'
	WHEN 'CANCELADO' THEN 'C'
	END Cod_Estado,
	A.MORA Interes_Moratorio
	FROM @TEMPORAL A
END