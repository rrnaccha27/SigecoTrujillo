CREATE TABLE [dbo].[resumen_planilla_bono] (
    [codigo_planilla]  INT             NOT NULL,
    [codigo_personal]  INT             NOT NULL,
    [monto_contratado] DECIMAL (10, 2) NULL,
    [monto_ingresado]  DECIMAL (10, 2) NULL,
    [meta_lograda]     DECIMAL (10, 2) NULL,
    [porcentaje_pago]  DECIMAL (10, 2) NULL,
    CONSTRAINT [FK_resumen_planilla_bono_planilla_bono] FOREIGN KEY ([codigo_planilla]) REFERENCES [dbo].[planilla_bono] ([codigo_planilla])
);