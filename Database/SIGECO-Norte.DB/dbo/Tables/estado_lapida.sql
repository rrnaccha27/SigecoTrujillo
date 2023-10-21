CREATE TABLE [dbo].[estado_lapida] (
    [codigo_estado_lapida] INT            IDENTITY (1, 1) NOT NULL,
    [nombre_estado_lapida] VARCHAR (1009) NOT NULL,
    [estado_registro]      BIT            NOT NULL,
    [fecha_registra]       DATETIME       NOT NULL,
    [fecha_modifica]       DATETIME       NULL,
    [usuario_registra]     VARCHAR (50)   NOT NULL,
    [usuario_modifica]     VARCHAR (50)   NULL,
    CONSTRAINT [codigo_estado_lapida_pk] PRIMARY KEY CLUSTERED ([codigo_estado_lapida] ASC)
);