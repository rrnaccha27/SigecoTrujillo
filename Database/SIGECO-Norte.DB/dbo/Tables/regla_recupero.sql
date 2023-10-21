CREATE TABLE [dbo].[regla_recupero] (
    [codigo_regla_recupero] INT           IDENTITY (1, 1) NOT NULL,
    [nombre]                VARCHAR (250) NOT NULL,
    [nro_cuota]             INT           NOT NULL,
    [vigencia_inicio]       DATETIME      NULL,
    [vigencia_fin]          DATETIME      NULL,
    [estado_registro]       BIT           NOT NULL,
    [fecha_registra]        DATETIME      NOT NULL,
    [usuario_registra]      VARCHAR (50)  NOT NULL,
    [fecha_modifica]        DATETIME      NULL,
    [usuario_modifica]      VARCHAR (50)  NULL,
    CONSTRAINT [PK_regla_recupero] PRIMARY KEY CLUSTERED ([codigo_regla_recupero] ASC)
);