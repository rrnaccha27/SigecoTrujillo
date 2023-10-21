CREATE TABLE [dbo].[contrato_cuota_backup] (
    [Codigo_empresa]           NVARCHAR (4)    NOT NULL,
    [NumAtCard]                NVARCHAR (100)  NOT NULL,
    [Num_Cuota]                NUMERIC (19, 6) NOT NULL,
    [Num_Importe_Total]        NUMERIC (19, 6) NOT NULL,
    [Num_Importe_Igv]          NUMERIC (19, 6) NOT NULL,
    [Num_Importe_Total_SinIgv] NUMERIC (19, 6) NOT NULL,
    [Cod_Estado]               NVARCHAR (2)    NOT NULL,
    [Fec_Vencimiento]          DATETIME        NOT NULL,
    [Fec_Pago]                 DATETIME        NULL
);