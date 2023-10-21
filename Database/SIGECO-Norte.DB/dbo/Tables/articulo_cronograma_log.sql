CREATE TABLE [dbo].[articulo_cronograma_log] (
    [fecha_log]          DATETIME        NOT NULL,
    [codigo_cronograma]  INT             NULL,
    [codigo_articulo]    INT             NULL,
    [monto_comision]     DECIMAL (10, 2) NULL,
    [cantidad]           INT             NULL,
    [codigo_campo_santo] INT             NULL,
    [estado_registro]    BIT             NULL
);