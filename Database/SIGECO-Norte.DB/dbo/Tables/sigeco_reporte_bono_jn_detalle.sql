CREATE TABLE [dbo].[sigeco_reporte_bono_jn_detalle] (
    [codigo_planilla]    INT             NULL,
    [codigo_canal_grupo] INT             NULL,
    [nombre_grupo]       VARCHAR (250)   NULL,
    [codigo_personal]    INT             NULL,
    [vendedor]           VARCHAR (250)   NULL,
    [codigo_empresa]     INT             NULL,
    [nombre_empresa]     VARCHAR (250)   NULL,
    [codigo_canal]       INT             NULL,
    [nombre_canal]       VARCHAR (250)   NULL,
    [nombre_moneda]      VARCHAR (250)   NULL,
    [nro_contrato]       VARCHAR (100)   NULL,
    [codigo_articulo]    INT             NULL,
    [nombre_articulo]    VARCHAR (250)   NULL,
    [fecha_contrato]     VARCHAR (10)    NULL,
    [dinero_ingresado]   DECIMAL (12, 2) NULL
);