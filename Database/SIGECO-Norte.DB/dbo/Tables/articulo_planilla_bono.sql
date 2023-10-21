CREATE TABLE [dbo].[articulo_planilla_bono] (
    [codigo_planilla_bono] INT             NULL,
    [codigo_articulo]      INT             NULL,
    [codigo_empresa]       INT             NULL,
    [codigo_personal]      INT             NULL,
    [nro_contrato]         VARCHAR (100)   NULL,
    [monto_contratado]     DECIMAL (10, 2) NULL,
    [dinero_ingresado]     DECIMAL (10, 2) NULL,
    [excluido]             BIT             DEFAULT ((0)) NULL,
    [excluido_motivo]      VARCHAR (300)   NULL,
    [excluido_fecha]       DATETIME        NULL,
    [excluido_usuario]     VARCHAR (50)    NULL,
    CONSTRAINT [FK_articulo_planilla_bono_articulo] FOREIGN KEY ([codigo_articulo]) REFERENCES [dbo].[articulo] ([codigo_articulo]),
    CONSTRAINT [FK_articulo_planilla_bono_empresa_sigeco] FOREIGN KEY ([codigo_empresa]) REFERENCES [dbo].[empresa_sigeco] ([codigo_empresa]),
    CONSTRAINT [FK_articulo_planilla_bono_planilla_bono] FOREIGN KEY ([codigo_planilla_bono]) REFERENCES [dbo].[planilla_bono] ([codigo_planilla])
);
GO
CREATE NONCLUSTERED INDEX [idx_articulo_planilla_bono]
    ON [dbo].[articulo_planilla_bono]([codigo_planilla_bono] ASC, [codigo_empresa] ASC, [excluido] ASC)
    INCLUDE([codigo_personal], [dinero_ingresado]);