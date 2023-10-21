CREATE TABLE [dbo].[empresa_configuracion] (
    [codigo_empresa_configuracion] INT IDENTITY (1, 1) NOT NULL,
    [codigo_configuracion]         INT NOT NULL,
    [codigo_empresa]               INT NOT NULL,
    [planilla_factura]             BIT NOT NULL,
    CONSTRAINT [PK_empresa_configuracion] PRIMARY KEY CLUSTERED ([codigo_empresa_configuracion] ASC),
    CONSTRAINT [FK_empresa_configuracion_codigo_configuracion_configuracion_canal_grupo_codigo_configuracion] FOREIGN KEY ([codigo_configuracion]) REFERENCES [dbo].[configuracion_canal_grupo] ([codigo_configuracion]),
    CONSTRAINT [FK_empresa_configuracion_codigo_empresa_empresa_sigeco_codigo_empresa] FOREIGN KEY ([codigo_empresa]) REFERENCES [dbo].[empresa_sigeco] ([codigo_empresa])
);