CREATE TABLE [dbo].[descuento] (
    [codigo_descuento]          INT             IDENTITY (1, 1) NOT NULL,
    [codigo_planilla]           INT             NOT NULL,
    [codigo_empresa]            INT             NOT NULL,
    [codigo_personal]           INT             NOT NULL,
    [motivo]                    VARCHAR (300)   NULL,
    [monto]                     DECIMAL (10, 2) NOT NULL,
    [estado_registro]           BIT             NOT NULL,
    [fecha_registra]            DATETIME        NOT NULL,
    [usuario_registra]          VARCHAR (50)    NOT NULL,
    [fecha_modifica]            DATETIME        NULL,
    [usuario_modifica]          VARCHAR (50)    NULL,
    [codigo_descuento_comision] INT             NOT NULL,
    CONSTRAINT [PK_descuento] PRIMARY KEY CLUSTERED ([codigo_descuento] ASC),
    CONSTRAINT [descuento_codigo_empresa_empresa_sigeco_codigo_empresa] FOREIGN KEY ([codigo_empresa]) REFERENCES [dbo].[empresa_sigeco] ([codigo_empresa]),
    CONSTRAINT [FK_descuento_codigo_personal_personal_codigo_personal] FOREIGN KEY ([codigo_personal]) REFERENCES [dbo].[personal] ([codigo_personal]),
    CONSTRAINT [FK_descuento_codigo_planilla_planilla_codigo_planilla] FOREIGN KEY ([codigo_planilla]) REFERENCES [dbo].[planilla] ([codigo_planilla]),
    CONSTRAINT [FK_descuento_descuento_comision] FOREIGN KEY ([codigo_descuento_comision]) REFERENCES [dbo].[descuento_comision] ([codigo_descuento_comision])
);