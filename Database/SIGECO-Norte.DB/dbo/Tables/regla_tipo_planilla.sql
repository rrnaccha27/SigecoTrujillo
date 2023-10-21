CREATE TABLE [dbo].[regla_tipo_planilla] (
    [codigo_regla_tipo_planilla] INT            IDENTITY (1, 1) NOT NULL,
    [codigo_tipo_planilla]       INT            NOT NULL,
    [nombre]                     VARCHAR (200)  NOT NULL,
    [descripcion]                NVARCHAR (500) NULL,
    [estado_registro]            BIT            NOT NULL,
    [fecha_registra]             DATETIME       NOT NULL,
    [fecha_modifica]             DATETIME       NULL,
    [usuario_registra]           VARCHAR (50)   NOT NULL,
    [usuario_modifica]           VARCHAR (50)   NULL,
    [afecto_doc_completa]        BIT            DEFAULT ((1)) NOT NULL,
    [tipo_reporte]               VARCHAR (10)   DEFAULT ('') NULL,
    [detraccion_por_contrato]    BIT            DEFAULT ((0)) NULL,
    [envio_liquidacion]          BIT            DEFAULT ((0)) NULL,
	CONSTRAINT [PK_regla_tipo_planilla] PRIMARY KEY CLUSTERED ([codigo_regla_tipo_planilla] ASC),
    CONSTRAINT [FK_regla_tipo_planilla_codigo_tipo_planilla_tipo_planilla_codigo_tipo_planilla] FOREIGN KEY ([codigo_tipo_planilla]) REFERENCES [dbo].[tipo_planilla] ([codigo_tipo_planilla])
);