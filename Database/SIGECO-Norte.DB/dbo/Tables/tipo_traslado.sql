CREATE TABLE [dbo].[tipo_traslado] (
    [codigo_tipo_traslado] INT           IDENTITY (1, 1) NOT NULL,
    [nombre_tipo_traslado] VARCHAR (100) NOT NULL,
    [estado_registro]      BIT           NOT NULL,
    [fecha_registra]       DATETIME      NOT NULL,
    [fecha_modifica]       DATETIME      NULL,
    [usuario_registra]     VARCHAR (50)  NOT NULL,
    [usuario_modifica]     VARCHAR (50)  NULL,
    CONSTRAINT [tipo_traslado_pk] PRIMARY KEY CLUSTERED ([codigo_tipo_traslado] ASC)
);