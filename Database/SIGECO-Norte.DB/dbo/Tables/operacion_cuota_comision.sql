CREATE TABLE [dbo].[operacion_cuota_comision] (
    [codigo_operacion_cuota_comision] INT           IDENTITY (1, 1) NOT NULL,
    [codigo_detalle_cronograma]       INT           NOT NULL,
    [codigo_tipo_operacion_cuota]     INT           NOT NULL,
    [motivo_movimiento]               VARCHAR (200) NULL,
    [fecha_movimiento]                DATETIME      NOT NULL,
    [estado_registro]                 BIT           NOT NULL,
    [usuario_registra]                NVARCHAR (20) NULL,
    [fecha_registra]                  DATETIME      NOT NULL,
    CONSTRAINT [PK_operacion_cuota_comision] PRIMARY KEY CLUSTERED ([codigo_operacion_cuota_comision] ASC),
    CONSTRAINT [FK_operacion_cuota_comision_codigo_tipo_operacion_cuota_tipo_operacion_cuota_codigo_tipo_operacion_cuota] FOREIGN KEY ([codigo_tipo_operacion_cuota]) REFERENCES [dbo].[tipo_operacion_cuota] ([codigo_tipo_operacion_cuota])
);