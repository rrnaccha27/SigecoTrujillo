create PROCEDURE [dbo].[SIGEMO_listarContratoCliente]
  @codigoPersona VARCHAR(50)
    
    
AS
BEGIN
    SET NOCOUNT ON;
 
 select '1202' codigo_contrato  ,'HE6569-Lote cinerario' nombre
 union all
  select '25625' codigo_contrato  ,'HE25599-Lote' nombre


END