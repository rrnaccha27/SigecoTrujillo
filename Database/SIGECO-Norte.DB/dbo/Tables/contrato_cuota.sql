CREATE TABLE [dbo].[contrato_cuota] (
    [Codigo_empresa]           NVARCHAR (4)    NOT NULL,
    [NumAtCard]                NVARCHAR (100)  NOT NULL,
    [Num_Cuota]                NUMERIC (19, 6) NOT NULL,
    [Num_Importe_Total]        NUMERIC (19, 6) NOT NULL,
    [Num_Importe_Igv]          NUMERIC (19, 6) NOT NULL,
    [Num_Importe_Total_SinIgv] NUMERIC (19, 6) NOT NULL,
    [ImporteMora]              NUMERIC (19, 6) CONSTRAINT [DF_contrato_cuota_ImporteMora] DEFAULT ((0)) NOT NULL,
    [Cod_Estado]               NVARCHAR (2)    NOT NULL,
    [Fec_Vencimiento]          DATETIME        NOT NULL,
    [Fec_Pago]                 DATETIME        NULL,
    [DocEntry]                 INT             NULL,
    CONSTRAINT [PK_contrato_cuota] PRIMARY KEY CLUSTERED ([Codigo_empresa] ASC, [NumAtCard] ASC, [Num_Cuota] ASC, [Num_Importe_Total] ASC, [Cod_Estado] ASC, [Fec_Vencimiento] ASC),
    CONSTRAINT [contrato_cuota_Codigo_empresa_NumAtCard_cabecera_contrato_Codigo_empresa_NumAtCard] FOREIGN KEY ([Codigo_empresa], [NumAtCard]) REFERENCES [dbo].[cabecera_contrato] ([Codigo_empresa], [NumAtCard])
);
GO
-- =============================================
-- Author:		Carlos García
-- Create date: 02/02/2018
-- Description:	Disparador que notifica los contratos que ingresen con cuota inicial sin un pago concreto
-- =============================================
CREATE TRIGGER UT_SIGECO_SIGECO_NotificarCuotaInicialSinPago
   ON dbo.contrato_cuota
   AFTER INSERT, UPDATE
AS 
BEGIN
-- SET NOCOUNT ON added to prevent extra result sets from
-- interfering with SELECT statements.
SET NOCOUNT ON;

    -- Insert statements for trigger here
	IF( SELECT
		COUNT(1)
	FROM INSERTED A
	WHERE A.Num_Cuota = 0
	AND A.Cod_Estado != 'C'
	AND A.Fec_Pago IS NULL)
> 0
BEGIN
DECLARE @BODY NVARCHAR(MAX)
SET @BODY = ISNULL(@BODY, '') + 'Codigo de Empresa: ' + ISNULL((SELECT
		A.Codigo_empresa
	FROM INSERTED A)
, '-') + ' - Contrato: ' + ISNULL((SELECT
		A.NumAtCard
	FROM INSERTED A)
, '-')
EXEC msdb.dbo.sp_send_dbmail @profile_name = 'JDLP'
							,@recipients = 'soporte@jardines.pe'
							,@subject = '[CLR - Migración de cuota]: Cuota inicial sin pago concreto'
							,
							 --@file_attachments = 'C:\MyFolder\Test\Google.gif;C:\MyFolder\Test\Yahoo.gif',
							 --@body=N'<p>Image Test</p><img src="Google.gif" /><p>See image there?</p>
							 --     <img src="Yaoo.gif" /><p>Yahoo!</p>', 
							 @body = @BODY
							,@body_format = 'HTML';
END
END
GO

DISABLE TRIGGER [dbo].[UT_SIGECO_SIGECO_NotificarCuotaInicialSinPago]
    ON [dbo].[contrato_cuota];