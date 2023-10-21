CREATE TABLE [dbo].[historico_detalle_cronograma] (
    [codigo]                INT             IDENTITY (1, 1) NOT NULL,
    [codigo_detalle]        INT             NOT NULL,
    [codigo_cronograma]     INT             NOT NULL,
    [codigo_articulo]       INT             NOT NULL,
    [nro_cuota]             INT             NOT NULL,
    [fecha_programada]      DATETIME        NOT NULL,
    [monto_bruto]           DECIMAL (10, 2) NULL,
    [igv]                   DECIMAL (10, 2) NULL,
    [monto_neto]            DECIMAL (10, 2) NOT NULL,
    [codigo_tipo_cuota]     INT             NOT NULL,
    [codigo_estado_cuota]   INT             NOT NULL,
    [fecha_historico]       DATETIME        NOT NULL,
    [codigo_tipo_historico] INT             NOT NULL,
    CONSTRAINT [PK_historico_detalle_cronograma] PRIMARY KEY CLUSTERED ([codigo] ASC)
);