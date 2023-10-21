CREATE TABLE [dbo].[producto] (
    [codigo_producto]      INT          IDENTITY (1, 1) NOT NULL,
    [nombre_producto]      VARCHAR (50) NOT NULL,
    [codigo_tipo_producto] INT          NOT NULL,
    [estado_registro]      BIT          NOT NULL,
    [fecha_registra]       DATETIME     NOT NULL,
    [fecha_modifica]       DATETIME     NULL,
    [usuario_registra]     VARCHAR (50) NOT NULL,
    [usuario_modifica]     VARCHAR (50) NULL,
    CONSTRAINT [producto_pk] PRIMARY KEY CLUSTERED ([codigo_producto] ASC),
    CONSTRAINT [tipo_producto_producto_fk] FOREIGN KEY ([codigo_tipo_producto]) REFERENCES [dbo].[tipo_producto] ([codigo_tipo_producto]) ON DELETE CASCADE ON UPDATE CASCADE
);