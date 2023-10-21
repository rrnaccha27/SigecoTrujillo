CREATE TABLE [dbo].[historico_articulo_cronograma] (
    [codigo]                INT             IDENTITY (1, 1) NOT NULL,
    [codigo_cronograma]     INT             NOT NULL,
    [codigo_articulo]       INT             NOT NULL,
    [monto_comision]        DECIMAL (10, 2) NOT NULL,
    [fecha_historico]       DATETIME        NOT NULL,
    [codigo_tipo_historico] INT             NOT NULL,
    CONSTRAINT [PK_historico_articulo_cronograma] PRIMARY KEY CLUSTERED ([codigo] ASC)
);