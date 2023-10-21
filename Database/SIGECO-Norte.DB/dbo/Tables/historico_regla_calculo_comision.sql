CREATE TABLE [dbo].[historico_regla_calculo_comision] (
    [codigo]                INT             IDENTITY (1, 1) NOT NULL,
    [codigo_regla]          INT             NOT NULL,
    [codigo_precio]         INT             NOT NULL,
    [codigo_canal]          INT             NOT NULL,
    [codigo_tipo_pago]      INT             NOT NULL,
    [codigo_tipo_comision]  INT             NOT NULL,
    [valor]                 DECIMAL (10, 2) NULL,
    [fecha_registro]        DATETIME        NOT NULL,
    [usuario_registro]      VARCHAR (10)    NOT NULL,
    [codigo_tipo_historico] INT             NOT NULL,
    CONSTRAINT [PK_historico_regla_calculo_comision] PRIMARY KEY CLUSTERED ([codigo_regla] ASC),
    CONSTRAINT [FK_historico_regla_calculo_comision_codigo_tipo_historico_tipo_historico_codigo_tipo_historico] FOREIGN KEY ([codigo_tipo_historico]) REFERENCES [dbo].[tipo_historico] ([codigo_tipo_historico])
);