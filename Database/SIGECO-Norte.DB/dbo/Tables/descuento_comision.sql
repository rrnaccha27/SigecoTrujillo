CREATE TABLE [dbo].[descuento_comision] (
    [codigo_descuento_comision] INT             IDENTITY (1, 1) NOT NULL,
    [codigo_empresa]            INT             NOT NULL,
    [codigo_personal]           INT             NOT NULL,
    [motivo]                    VARCHAR (300)   NULL,
    [monto]                     DECIMAL (10, 2) NOT NULL,
    [saldo]                     DECIMAL (10, 2) NOT NULL,
    [estado_registro]           BIT             NOT NULL,
    [fecha_registra]            DATETIME        NOT NULL,
    [usuario_registra]          VARCHAR (50)    NOT NULL,
    [fecha_modifica]            DATETIME        NULL,
    [usuario_modifica]          VARCHAR (50)    NULL,
    CONSTRAINT [PK_descuento_comision] PRIMARY KEY CLUSTERED ([codigo_descuento_comision] ASC),
    CONSTRAINT [FK_descuento_comision_empresa_sigeco] FOREIGN KEY ([codigo_empresa]) REFERENCES [dbo].[empresa_sigeco] ([codigo_empresa]),
    CONSTRAINT [FK_descuento_comision_personal] FOREIGN KEY ([codigo_personal]) REFERENCES [dbo].[personal] ([codigo_personal])
);