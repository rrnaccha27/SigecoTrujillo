CREATE TABLE [dbo].[sigeco_reporte_bono_jn] (
    [codigo_planilla]        INT             NULL,
    [numero_planilla]        VARCHAR (25)    NULL,
    [codigo_tipo_planilla]   INT             NULL,
    [codigo_estado_planilla] INT             NULL,
    [fecha_inicio]           VARCHAR (10)    NULL,
    [fecha_fin]              VARCHAR (10)    NULL,
    [fecha_registra]         DATETIME        NULL,
    [dinero_ingresado]       DECIMAL (12, 2) NULL,
    [bono]                   DECIMAL (12, 2) NULL,
    [porcentaje]             VARCHAR (25)    NULL,
    [meta_100]               DECIMAL (12, 2) NULL,
    [meta_90]                DECIMAL (12, 2) NULL
);