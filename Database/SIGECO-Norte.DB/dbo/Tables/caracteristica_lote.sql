CREATE TABLE [dbo].[caracteristica_lote] (
    [codigo_caracteristica_lote] INT           IDENTITY (1, 1) NOT NULL,
    [nombre_caracteristica_lote] VARCHAR (100) NOT NULL,
    [estado_registro]            BIT           NOT NULL,
    [fecha_registra]             DATETIME      NOT NULL,
    [fecha_modifica]             DATETIME      NULL,
    [usuario_registra]           VARCHAR (50)  NOT NULL,
    [usuario_modifica]           VARCHAR (50)  NULL,
    CONSTRAINT [caracteristica_lote_pk] PRIMARY KEY CLUSTERED ([codigo_caracteristica_lote] ASC)
);