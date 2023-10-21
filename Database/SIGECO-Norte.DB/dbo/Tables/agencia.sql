CREATE TABLE [dbo].[agencia] (
    [codigo_agencia]   INT           IDENTITY (1, 1) NOT NULL,
    [nombre_agencia]   VARCHAR (100) NOT NULL,
    [estado_registro]  BIT           NOT NULL,
    [fecha_registra]   DATETIME      NOT NULL,
    [fecha_modifica]   DATETIME      NULL,
    [usuario_registra] VARCHAR (50)  NOT NULL,
    [usuario_modifica] VARCHAR (50)  NULL,
    CONSTRAINT [Agencia_pk] PRIMARY KEY CLUSTERED ([codigo_agencia] ASC)
);