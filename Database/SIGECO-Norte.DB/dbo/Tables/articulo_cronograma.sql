CREATE TABLE [dbo].[articulo_cronograma] (
    [codigo_cronograma]  INT             NOT NULL,
    [codigo_articulo]    INT             NOT NULL,
    [monto_comision]     DECIMAL (10, 2) NOT NULL,
    [cantidad]           INT             NULL,
    [codigo_campo_santo] INT             NULL,
    [estado_registro]    BIT             DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_articulo_cronograma] PRIMARY KEY CLUSTERED ([codigo_cronograma] ASC, [codigo_articulo] ASC),
    CONSTRAINT [FK_articulo_cronograma_codigo_articulo_articulo_codigo_articulo] FOREIGN KEY ([codigo_articulo]) REFERENCES [dbo].[articulo] ([codigo_articulo]),
    CONSTRAINT [FK_articulo_cronograma_codigo_cronograma_cronograma_pago_comision_codigo_cronograma] FOREIGN KEY ([codigo_cronograma]) REFERENCES [dbo].[cronograma_pago_comision] ([codigo_cronograma])
);