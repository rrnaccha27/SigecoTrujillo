-- =============================================
-- Author:		Carlos García
-- Create date: 05/03/2018
-- Description:	Procedimiento almacenado que elimina la información de un contrato en base al DocEntry
-- =============================================
CREATE PROCEDURE USP_WCFSAPSIGECO_SIGECO_EliminarInformacionContrato
	-- Add the parameters for the stored procedure here
	@pCodigoEmpresa NVARCHAR(100),
	@pDocEntry INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	DECLARE @NumeroContrato NVARCHAR(10)
	SELECT @NumeroContrato = A.NumAtCard FROM cabecera_contrato A WHERE A.Codigo_empresa = @pCodigoEmpresa AND A.DocEntry = @pDocEntry
	--DELETE FROM contrato_migrado WHERE Codigo_empresa = @pCodigoEmpresa AND DocEntry = @pDocEntry
	DELETE FROM difunto_contrato WHERE CODIGO_EMPRESA = @pCodigoEmpresa AND DocEntry = @pDocEntry
	DELETE FROM contrato_cuota WHERE Codigo_empresa = @pCodigoEmpresa AND DocEntry = @pDocEntry
	DELETE FROM detalle_contrato WHERE Codigo_empresa = @pCodigoEmpresa AND DocEntry = @pDocEntry
	DELETE FROM cabecera_contrato WHERE Codigo_empresa = @pCodigoEmpresa AND DocEntry = @pDocEntry
END