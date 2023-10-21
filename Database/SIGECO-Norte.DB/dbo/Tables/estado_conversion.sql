CREATE TABLE [dbo].[estado_conversion] (
    [codigo_estado_conversion] INT          IDENTITY (1, 1) NOT NULL,
    [nombre_estado_conversion] VARCHAR (50) NOT NULL,
    [estado_registro]          BIT          NOT NULL,
    [fecha_registra]           DATETIME     NOT NULL,
    [fecha_modifica]           DATETIME     NULL,
    [usuario_registra]         VARCHAR (50) NOT NULL,
    [usuario_modifica]         VARCHAR (50) NULL,
    CONSTRAINT [codigo_estado_conversion] PRIMARY KEY CLUSTERED ([codigo_estado_conversion] ASC)
);