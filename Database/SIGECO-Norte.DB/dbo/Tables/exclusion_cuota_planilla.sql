CREATE TABLE [dbo].[exclusion_cuota_planilla] (
    [codigo_exclusion]           INT           IDENTITY (1, 1) NOT NULL,
    [codigo_detalle_planilla]    INT           NOT NULL,
    [codigo_detalle_cronograma]  INT           NOT NULL,
    [codigo_planilla]            INT           NOT NULL,
    [codigo_planilla_destino]    INT           NULL,
    [usuario_exclusion]          VARCHAR (20)  NOT NULL,
    [fecha_exclusion]            DATETIME      NOT NULL,
    [motivo_exclusion]           VARCHAR (400) NOT NULL,
    [usuario_habilita]           VARCHAR (20)  NULL,
    [fecha_habilita]             DATETIME      NULL,
    [motivo_habilita]            VARCHAR (400) NULL,
    [estado_exclusion]           BIT           NOT NULL,
    [estado_registro]            BIT           NOT NULL,
    [codigo_regla_tipo_planilla] INT           NULL,
    CONSTRAINT [PK_exclusion_cuota_planilla] PRIMARY KEY CLUSTERED ([codigo_exclusion] ASC)
);