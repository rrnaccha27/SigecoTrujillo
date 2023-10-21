CREATE TABLE [dbo].[detalle_cronograma] (
    [codigo_detalle]              INT             IDENTITY (1, 1) NOT NULL,
    [codigo_cronograma]           INT             NOT NULL,
    [codigo_articulo]             INT             NOT NULL,
    [nro_cuota]                   INT             NOT NULL,
    [fecha_programada]            DATETIME        NULL,
    [monto_bruto]                 DECIMAL (10, 2) NULL,
    [igv]                         DECIMAL (10, 2) NULL,
    [monto_neto]                  DECIMAL (10, 2) NULL,
    [codigo_tipo_cuota]           INT             NOT NULL,
    [codigo_estado_cuota]         INT             NOT NULL,
    [estado_registro]             BIT             DEFAULT ((1)) NOT NULL,
    [es_registro_manual_comision] BIT             DEFAULT ((0)) NOT NULL,
    [es_transferencia]            BIT             DEFAULT ((0)) NULL,
    CONSTRAINT [PK_detalle_cronograma] PRIMARY KEY CLUSTERED ([codigo_detalle] ASC),
    CONSTRAINT [FK_detalle_cronograma_articulo_cronograma] FOREIGN KEY ([codigo_cronograma], [codigo_articulo]) REFERENCES [dbo].[articulo_cronograma] ([codigo_cronograma], [codigo_articulo]),
    CONSTRAINT [FK_detalle_cronograma_codigo_estado_cuota_estado_cuota_codigo_estado_cuota] FOREIGN KEY ([codigo_estado_cuota]) REFERENCES [dbo].[estado_cuota] ([codigo_estado_cuota]),
    CONSTRAINT [FK_detalle_cronograma_codigo_tipo_cuota_tipo_cuota_codigo_tipo_cuota] FOREIGN KEY ([codigo_tipo_cuota]) REFERENCES [dbo].[tipo_cuota] ([codigo_tipo_cuota])
);