CREATE TABLE [dbo].[documento] (
    [codigo_documento] INT          IDENTITY (1, 1) NOT NULL,
    [numero_documento] VARCHAR (50) NOT NULL,
    [fecha_registra]   DATETIME     NOT NULL,
    [fecha_modifica]   DATETIME     NULL,
    [usuario_registra] VARCHAR (50) NOT NULL,
    [usuario_modifica] VARCHAR (50) NULL,
    CONSTRAINT [documento_pk] PRIMARY KEY CLUSTERED ([codigo_documento] ASC)
);