CREATE TABLE [dbo].[detalle_cronograma_log] (
    [fecha_log]                   DATETIME        NOT NULL,
    [codigo_detalle]              INT             NULL,
    [codigo_cronograma]           INT             NULL,
    [codigo_articulo]             INT             NULL,
    [nro_cuota]                   INT             NULL,
    [fecha_programada]            DATETIME        NULL,
    [monto_bruto]                 DECIMAL (10, 2) NULL,
    [igv]                         DECIMAL (10, 2) NULL,
    [monto_neto]                  DECIMAL (10, 2) NULL,
    [codigo_tipo_cuota]           INT             NULL,
    [codigo_estado_cuota]         INT             NULL,
    [estado_registro]             BIT             NULL,
    [es_registro_manual_comision] BIT             NULL,
    [es_transferencia]            BIT             DEFAULT ((0)) NULL
);