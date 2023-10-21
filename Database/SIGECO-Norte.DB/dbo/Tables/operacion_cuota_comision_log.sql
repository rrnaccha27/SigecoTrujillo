CREATE TABLE [dbo].[operacion_cuota_comision_log] (
    [fecha_log]                       DATETIME      NOT NULL,
    [codigo_operacion_cuota_comision] INT           NULL,
    [codigo_detalle_cronograma]       INT           NULL,
    [codigo_tipo_operacion_cuota]     INT           NULL,
    [motivo_movimiento]               VARCHAR (200) NULL,
    [fecha_movimiento]                DATETIME      NULL,
    [estado_registro]                 BIT           NULL,
    [usuario_registra]                NVARCHAR (20) NULL,
    [fecha_registra]                  DATETIME      NULL
);