CREATE TABLE [dbo].[tipo_articulo] (
    [codigo_tipo_articulo] INT           IDENTITY (1, 1) NOT NULL,
    [nombre]               VARCHAR (200) NOT NULL,
    [estado_registro]      BIT           NOT NULL,
    [fecha_registra]       DATETIME      NOT NULL,
    [usuario_registra]     VARCHAR (50)  NOT NULL,
    [fecha_modifica]       DATETIME      NULL,
    [usuario_modifica]     VARCHAR (50)  NULL,
    CONSTRAINT [PK_tipo_articulo] PRIMARY KEY CLUSTERED ([codigo_tipo_articulo] ASC)
);