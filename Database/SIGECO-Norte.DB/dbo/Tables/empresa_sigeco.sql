CREATE TABLE [dbo].[empresa_sigeco] (
    [codigo_empresa]       INT           IDENTITY (1, 1) NOT NULL,
    [codigo_equivalencia]  NVARCHAR (4)  NULL,
    [nombre]               VARCHAR (10)  NOT NULL,
    [nombre_largo]         VARCHAR (100) NOT NULL,
    [ruc]                  VARCHAR (11)  NOT NULL,
    [direccion_fiscal]     VARCHAR (250) NULL,
    [estado_registro]      BIT           NOT NULL,
    [fecha_registra]       DATETIME      NOT NULL,
    [usuario_registra]     VARCHAR (50)  NOT NULL,
    [fecha_modifica]       DATETIME      NULL,
    [usuario_modifica]     VARCHAR (50)  NULL,
    [codigo_tipo_cuenta]   INT           NULL,
    [nro_cuenta]           NVARCHAR (30) NULL,
    [codigo_cuenta_moneda] INT           NULL,
    CONSTRAINT [PK_empresa_sigeco] PRIMARY KEY CLUSTERED ([codigo_empresa] ASC),
    CONSTRAINT [FK_empresa_sigeco_codigo_moneda_moneda_codigo_moneda] FOREIGN KEY ([codigo_cuenta_moneda]) REFERENCES [dbo].[moneda] ([codigo_moneda]),
    CONSTRAINT [FK_empresa_sigeco_codigo_tipo_cuenta_tipo_cuenta_codigo_tipo_cuenta] FOREIGN KEY ([codigo_tipo_cuenta]) REFERENCES [dbo].[tipo_cuenta] ([codigo_tipo_cuenta])
);