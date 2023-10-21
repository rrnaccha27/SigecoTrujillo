CREATE TABLE [dbo].[log_proceso_bono_cabecera] (
    [id]                   UNIQUEIDENTIFIER NOT NULL,
    [codigo_planilla]      INT              NULL,
    [codigo_canal]         INT              NULL,
    [codigo_tipo_planilla] INT              NULL,
    [fecha_inicio]         DATETIME         NULL,
    [fecha_fin]            DATETIME         NULL,
    [usuario_registra]     VARCHAR (50)     NULL,
    [fecha_registra]       DATETIME         NULL,
    CONSTRAINT [PK_log_proceso_bono_cabecera] PRIMARY KEY CLUSTERED ([id] ASC)
);