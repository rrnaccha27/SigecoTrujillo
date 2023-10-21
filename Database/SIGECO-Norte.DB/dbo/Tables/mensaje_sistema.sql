CREATE TABLE [dbo].[mensaje_sistema] (
    [codigo_mensaje_sistema] INT           NOT NULL,
    [nombre_mensaje_sistema] VARCHAR (100) NOT NULL,
    [estado_registro]        BIT           NOT NULL,
    [fecha_registra]         DATETIME      NOT NULL,
    [fecha_modifica]         DATETIME      NULL,
    [usuario_registra]       VARCHAR (50)  NOT NULL,
    [usuario_modifica]       VARCHAR (50)  NULL,
    CONSTRAINT [codigo_mensaje_sistema] PRIMARY KEY CLUSTERED ([codigo_mensaje_sistema] ASC)
);