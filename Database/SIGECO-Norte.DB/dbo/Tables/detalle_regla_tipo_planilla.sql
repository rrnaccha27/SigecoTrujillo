CREATE TABLE [dbo].[detalle_regla_tipo_planilla] (
    [codigo_detalle_regla_tipo_planilla] INT            IDENTITY (1, 1) NOT NULL,
    [codigo_regla_tipo_planilla]         INT            NOT NULL,
    [codigo_canal]                       INT            NOT NULL,
    [codigo_empresa]                     NVARCHAR (400) NOT NULL,
    [codigo_tipo_venta]                  NVARCHAR (400) NOT NULL,
    [codigo_campo_santo]                 NVARCHAR (400) NOT NULL,
    [estado_registro]                    BIT            NOT NULL,
    [fecha_registra]                     DATETIME       NOT NULL,
    [fecha_modifica]                     DATETIME       NULL,
    [usuario_registra]                   VARCHAR (50)   NOT NULL,
    [usuario_modifica]                   VARCHAR (50)   NULL,
    CONSTRAINT [PK_detalle_regla_tipo_planilla] PRIMARY KEY CLUSTERED ([codigo_detalle_regla_tipo_planilla] ASC),
    CONSTRAINT [FK_detalle_regla_tipo_planilla_codigo_canal_canal_grupo_codigo_canal_grupo] FOREIGN KEY ([codigo_canal]) REFERENCES [dbo].[canal_grupo] ([codigo_canal_grupo]),
    CONSTRAINT [FK_detalle_regla_tipo_planilla_codigo_regla_tipo_planilla_regla_tipo_planilla_codigo_regla_tipo_planilla] FOREIGN KEY ([codigo_regla_tipo_planilla]) REFERENCES [dbo].[regla_tipo_planilla] ([codigo_regla_tipo_planilla])
);