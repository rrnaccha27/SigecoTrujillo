CREATE TABLE [dbo].[tipo_documento] (
    [codigo_tipo_documento] INT          IDENTITY (1, 1) NOT NULL,
    [nombre_tipo_documento] VARCHAR (50) NOT NULL,
    [estado_registro]       BIT          NOT NULL,
    [fecha_registra]        DATETIME     NOT NULL,
    [fecha_modifica]        DATETIME     NULL,
    [usuario_registra]      VARCHAR (50) NOT NULL,
    [usuario_modifica]      VARCHAR (50) NULL,
    [codigo_equivalencia]   VARCHAR (4)  NULL,
    CONSTRAINT [tipo_documento_pk] PRIMARY KEY CLUSTERED ([codigo_tipo_documento] ASC)
);