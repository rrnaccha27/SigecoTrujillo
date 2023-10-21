CREATE TABLE [dbo].[detalle_producto] (
    [codigo_detalle_producto] INT          IDENTITY (1, 1) NOT NULL,
    [codigo_espacio]          VARCHAR (20) NOT NULL,
    [codigo_producto]         INT          NOT NULL,
    [fecha_registra]          DATETIME     NOT NULL,
    [usuario_registra]        VARCHAR (50) NOT NULL,
    CONSTRAINT [codigo_detalle_producto] PRIMARY KEY CLUSTERED ([codigo_detalle_producto] ASC),
    CONSTRAINT [espacio_detalle_producto_fk] FOREIGN KEY ([codigo_espacio]) REFERENCES [dbo].[espacio] ([codigo_espacio]),
    CONSTRAINT [producto_detalle_producto_fk] FOREIGN KEY ([codigo_producto]) REFERENCES [dbo].[producto] ([codigo_producto])
);