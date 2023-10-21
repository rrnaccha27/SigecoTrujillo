CREATE TABLE [dbo].[plan_integral_detalle] (
    [codigo_plan_integral_detalle] INT          IDENTITY (1, 1) NOT NULL,
    [codigo_plan_integral]         INT          NULL,
    [codigo_campo_santo]           INT          NOT NULL,
    [codigo_tipo_articulo]         INT          NOT NULL,
    [codigo_tipo_articulo_2]       INT          NOT NULL,
    [estado_registro]              BIT          NOT NULL,
    [fecha_registra]               DATETIME     NOT NULL,
    [usuario_registra]             VARCHAR (50) NOT NULL,
    [fecha_modifica]               DATETIME     NULL,
    [usuario_modifica]             VARCHAR (50) NULL,
    CONSTRAINT [PK_plan_integral_detalle] PRIMARY KEY CLUSTERED ([codigo_plan_integral_detalle] ASC),
    CONSTRAINT [FK_plan_integral_detalle_codigo_campo_santo_tipo_articulo_codigo_tipo_articulo] FOREIGN KEY ([codigo_campo_santo]) REFERENCES [dbo].[campo_santo_sigeco] ([codigo_campo_santo]),
    CONSTRAINT [FK_plan_integral_detalle_codigo_plan_integral_plan_integral_codigo_plan_integral] FOREIGN KEY ([codigo_plan_integral]) REFERENCES [dbo].[plan_integral] ([codigo_plan_integral]),
    CONSTRAINT [FK_plan_integral_detalle_codigo_tipo_articulo_2_tipo_articulo_codigo_tipo_articulo] FOREIGN KEY ([codigo_tipo_articulo_2]) REFERENCES [dbo].[tipo_articulo] ([codigo_tipo_articulo]),
    CONSTRAINT [FK_plan_integral_detalle_codigo_tipo_articulo_tipo_articulo_codigo_tipo_articulo] FOREIGN KEY ([codigo_tipo_articulo]) REFERENCES [dbo].[tipo_articulo] ([codigo_tipo_articulo])
);