CREATE TABLE [dbo].[tipo_producto] (
    [codigo_tipo_producto] INT          IDENTITY (1, 1) NOT NULL,
    [nombre_tipo_producto] VARCHAR (50) NOT NULL,
    [estado_registro]      BIT          NOT NULL,
    [fecha_registra]       DATETIME     NOT NULL,
    [fecha_modifica]       DATETIME     NULL,
    [usuario_registra]     VARCHAR (50) NOT NULL,
    [usuario_modifica]     VARCHAR (50) NULL,
    CONSTRAINT [tipo_producto_pk] PRIMARY KEY CLUSTERED ([codigo_tipo_producto] ASC)
);