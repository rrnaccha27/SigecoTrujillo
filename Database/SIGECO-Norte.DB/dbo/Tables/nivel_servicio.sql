CREATE TABLE [dbo].[nivel_servicio] (
    [codigo_nivel_servicio] INT           IDENTITY (1, 1) NOT NULL,
    [nombre_nivel_servicio] VARCHAR (100) NOT NULL,
    [estado_registro]       BIT           NOT NULL,
    [fecha_registra]        DATETIME      NOT NULL,
    [fecha_modifica]        DATETIME      NULL,
    [usuario_registra]      VARCHAR (50)  NOT NULL,
    [usuario_modifica]      VARCHAR (50)  NULL,
    CONSTRAINT [nivel_servicio_pk] PRIMARY KEY CLUSTERED ([codigo_nivel_servicio] ASC)
);