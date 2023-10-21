CREATE TABLE [dbo].[encofrado] (
    [codigo_encofrado]       INT           IDENTITY (1, 1) NOT NULL,
    [codigo_espacio]         VARCHAR (20)  NOT NULL,
    [codigo_tipo_bien]       INT           NOT NULL,
    [fecha_inicio]           DATETIME      NOT NULL,
    [fecha_fin]              DATETIME      NULL,
    [numero_orden_ejecucion] VARCHAR (30)  NULL,
    [numero_orden_finaliza]  VARCHAR (30)  NULL,
    [observacion_ejecucion]  VARCHAR (200) NULL,
    [observacion_finaliza]   VARCHAR (200) NULL,
    [observacion_anulacion]  VARCHAR (200) NULL,
    [estado_registro]        BIT           NOT NULL,
    [fecha_registra]         DATETIME      NOT NULL,
    [fecha_modifica]         DATETIME      NULL,
    [usuario_registra]       VARCHAR (50)  NOT NULL,
    [usuario_modifica]       VARCHAR (50)  NULL,
    FOREIGN KEY ([codigo_espacio]) REFERENCES [dbo].[espacio] ([codigo_espacio]),
    FOREIGN KEY ([codigo_tipo_bien]) REFERENCES [dbo].[tipo_bien] ([codigo_tipo_bien])
);