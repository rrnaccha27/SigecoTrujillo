-- =============================================
-- Author:		Carlos García
-- Create date: 25/05/2018
-- Description:	Procedimiento almacenado que permite obtener los contratos de un cliente en base a documento
-- =============================================
CREATE PROCEDURE [dbo].[SIGEMO_ObtenerContratosPorDocument]
	-- Add the parameters for the stored procedure here
	@pDocumento NVARCHAR(15)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	--SELECT DISTINCT
	--A.Codigo_empresa
	--,CASE A.Codigo_empresa
	--WHEN '0002' THEN 'OPERACIONES FUNERARIAS S.A.'
	--WHEN '0939' THEN 'FUNERARIA JARDINES S.A.'
	--END Descripcion--Empresa
	--,RIGHT('0000000000' + A.NumAtCard, 10) Numero_Contrato
	--FROM SIGECO.dbo.cabecera_contrato A
	--INNER JOIN SIGECO.dbo.contrato_cuota B ON B.Codigo_empresa = A.Codigo_empresa AND B.NumAtCard = A.NumAtCard
	--WHERE A.LicTradNum = @pDocumento
	--AND ISNULL(A.Cod_Estado_Contrato, '') != 'ANL'
	--ORDER BY 1, 3

	SELECT '0939' Codigo_empresa, 'FUNERARIA JARDINES S.A.' Descripcion, /*RIGHT('0000000000' + B.NumAtCard, 10)*/ B.NumAtCard Numero_Contrato FROM
	SAP.DB_FUNERARIA_JARDINES.dbo.OCRD A
	INNER JOIN SAP.DB_FUNERARIA_JARDINES.dbo.ORDR B ON B.CardCode = A.CardCode
	WHERE A.LicTradNum = @pDocumento
	AND B.NumAtCard NOT LIKE '%A%'
	AND B.U_VK_FechaRes IS NULL

	UNION

	SELECT '0002' Codigo_empresa, 'OPERACIONES FUNERARIAS S.A.' Descripcion, /*RIGHT('0000000000' + B.NumAtCard, 10)*/ B.NumAtCard Numero_Contrato FROM
	SAP.DB_OPERACIONES_FUNERARIAS.dbo.OCRD A
	INNER JOIN SAP.DB_OPERACIONES_FUNERARIAS.dbo.ORDR B ON B.CardCode = A.CardCode
	WHERE A.LicTradNum = @pDocumento
	AND B.NumAtCard NOT LIKE '%A%'
	AND B.U_VK_FechaRes IS NULL

	ORDER BY 1, 3
END