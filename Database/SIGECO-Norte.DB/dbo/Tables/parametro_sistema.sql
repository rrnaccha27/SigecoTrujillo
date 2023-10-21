CREATE TABLE [dbo].[parametro_sistema] (
    [codigo_parametro_sistema] INT           NOT NULL,
    [nombre_parametro_sistema] VARCHAR (100) NOT NULL,
    [valor]                    VARCHAR (200) NOT NULL,
    [tokenizar]                BIT           NOT NULL,
    [estado_registro]          BIT           NOT NULL,
    [fecha_registra]           DATETIME      NOT NULL,
    [fecha_modifica]           DATETIME      NULL,
    [usuario_registra]         VARCHAR (50)  NOT NULL,
    [usuario_modifica]         VARCHAR (50)  NULL,
    CONSTRAINT [codigo_parametro_sistema] PRIMARY KEY CLUSTERED ([codigo_parametro_sistema] ASC)
);