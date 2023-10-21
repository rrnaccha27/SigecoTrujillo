CREATE TABLE [dbo].[historico_precio_articulo] (
    [codigo]                INT             IDENTITY (1, 1) NOT NULL,
    [codigo_precio]         INT             NOT NULL,
    [codigo_articulo]       INT             NOT NULL,
    [codigo_empresa]        INT             NOT NULL,
    [codigo_tipo_venta]     INT             NOT NULL,
    [codigo_moneda]         INT             NOT NULL,
    [precio]                DECIMAL (10, 2) NOT NULL,
    [fecha_registro]        DATETIME        NOT NULL,
    [usuario_registro]      VARCHAR (10)    NOT NULL,
    [codigo_tipo_historico] INT             NOT NULL,
    CONSTRAINT [PK_historico_precio_articulo] PRIMARY KEY CLUSTERED ([codigo] ASC),
    CONSTRAINT [FK_historico_precio_articulo_codigo_tipo_historico_tipo_historico_codigo_tipo_historico] FOREIGN KEY ([codigo_tipo_historico]) REFERENCES [dbo].[tipo_historico] ([codigo_tipo_historico])
);