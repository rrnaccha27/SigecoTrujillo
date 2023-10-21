CREATE TABLE [dbo].[contrato_planilla_bono] (
    [codigo_planilla]   INT             NOT NULL,
    [codigo_personal]   INT             NOT NULL,
    [numero_contrato]   NVARCHAR (100)  NOT NULL,
    [codigo_empresa]    INT             NOT NULL,
    [codigo_tipo_venta] INT             NOT NULL,
    [monto_contratado]  DECIMAL (10, 2) NULL,
    [monto_ingresado]   DECIMAL (10, 2) NULL,
    [codigo_supervisor] INT             NULL,
    [fecha_contrato]    VARCHAR (10)    NULL,
    [codigo_grupo]      INT             NULL,
    CONSTRAINT [FK_contrato_planilla_bono_empresa_sigeco] FOREIGN KEY ([codigo_empresa]) REFERENCES [dbo].[empresa_sigeco] ([codigo_empresa]),
    CONSTRAINT [FK_contrato_planilla_bono_planilla_bono] FOREIGN KEY ([codigo_planilla]) REFERENCES [dbo].[planilla_bono] ([codigo_planilla])
);