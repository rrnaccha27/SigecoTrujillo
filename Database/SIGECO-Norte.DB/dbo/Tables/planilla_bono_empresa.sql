CREATE TABLE [dbo].[planilla_bono_empresa] (
    [codigo_planilla_empresa] INT IDENTITY (1, 1) NOT NULL,
    [codigo_empresa]          INT NOT NULL,
    [codigo_planilla]         INT NOT NULL,
    [estado_registro]         BIT DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_planilla_bono_empresa] PRIMARY KEY CLUSTERED ([codigo_planilla_empresa] ASC),
    CONSTRAINT [FK_planilla_bono_empresa_codigo_empresa_empresa_sigeco_codigo_empresa] FOREIGN KEY ([codigo_empresa]) REFERENCES [dbo].[empresa_sigeco] ([codigo_empresa]),
    CONSTRAINT [FK_planilla_bono_empresa_codigo_planilla_planilla_codigo_planilla] FOREIGN KEY ([codigo_planilla]) REFERENCES [dbo].[planilla_bono] ([codigo_planilla])
);