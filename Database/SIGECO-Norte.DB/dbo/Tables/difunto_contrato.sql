CREATE TABLE [dbo].[difunto_contrato] (
    [CODIGO_EMPRESA] NVARCHAR (4)   NOT NULL,
    [NUMATCARD]      NVARCHAR (100) NOT NULL,
    [CODIGO_DOC]     VARCHAR (4)    NOT NULL,
    [NUM_DOC]        VARCHAR (15)   NOT NULL,
    [NOMBRES]        VARCHAR (200)  NULL,
    [CREATEDATE]     DATETIME       NULL,
    [DocEntry]       INT            NULL,
    CONSTRAINT [PK_DIFUNTO_CONTRATO] PRIMARY KEY CLUSTERED ([CODIGO_EMPRESA] ASC, [NUMATCARD] ASC, [CODIGO_DOC] ASC, [NUM_DOC] ASC),
    CONSTRAINT [FK_difunto_contrato_numatcard_codigo_empresa_cabecera_contrato_numatcard_codigo_empresa] FOREIGN KEY ([CODIGO_EMPRESA], [NUMATCARD]) REFERENCES [dbo].[cabecera_contrato] ([Codigo_empresa], [NumAtCard])
);