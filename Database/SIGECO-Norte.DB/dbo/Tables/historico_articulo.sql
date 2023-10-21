CREATE TABLE [dbo].[historico_articulo] (
    [codigo]                INT           IDENTITY (1, 1) NOT NULL,
    [codigo_articulo]       INT           NOT NULL,
    [nombre]                VARCHAR (250) NOT NULL,
    [abreviatura]           VARCHAR (10)  NOT NULL,
    [genera_comision]       BIT           NOT NULL,
    [genera_bono]           BIT           NOT NULL,
    [fecha_registro]        DATETIME      NOT NULL,
    [usuario_registro]      VARCHAR (10)  NOT NULL,
    [codigo_tipo_historico] INT           NOT NULL,
    CONSTRAINT [PK_historico_articulo] PRIMARY KEY CLUSTERED ([codigo] ASC),
    CONSTRAINT [FK_historico_articulo_codigo_tipo_historico_tipo_historico_codigo_tipo_historico] FOREIGN KEY ([codigo_tipo_historico]) REFERENCES [dbo].[tipo_historico] ([codigo_tipo_historico])
);