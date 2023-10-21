CREATE TABLE [dbo].[fallecido_contrato] (
    [CODIGO_EMPRESA] NVARCHAR (4)   NOT NULL,
    [NUMATCARD]      NVARCHAR (100) NOT NULL,
    [CODIGO_DOC]     VARCHAR (4)    NOT NULL,
    [NUM_DOC]        VARCHAR (15)   NOT NULL,
    [NOMBRES]        VARCHAR (200)  NULL,
    [CREATEDATE]     DATETIME       NULL,
    [DocEntry]       INT            NULL,
    CONSTRAINT [PK_fallecido_contrato] PRIMARY KEY CLUSTERED ([CODIGO_EMPRESA] ASC, [NUMATCARD] ASC, [CODIGO_DOC] ASC, [NUM_DOC] ASC),
    CONSTRAINT [FK_fallecido_contrato_numatcard_codigo_empresa_cabecera_contrato_numatcard_codigo_empresa] FOREIGN KEY ([CODIGO_EMPRESA], [NUMATCARD]) REFERENCES [dbo].[cabecera_contrato] ([Codigo_empresa], [NumAtCard])
);