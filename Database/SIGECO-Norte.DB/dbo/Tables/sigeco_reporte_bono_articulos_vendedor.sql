CREATE TABLE [dbo].[sigeco_reporte_bono_articulos_vendedor] (
    [codigo_planilla]  INT             NULL,
    [nombre_articulo]  VARCHAR (250)   NULL,
    [codigo_empresa]   INT             NULL,
    [nro_contrato]     VARCHAR (100)   NULL,
    [monto_contratado] DECIMAL (12, 2) NULL,
    [dinero_ingresado] DECIMAL (12, 2) NULL,
    [cantidad]         INT             NULL
);