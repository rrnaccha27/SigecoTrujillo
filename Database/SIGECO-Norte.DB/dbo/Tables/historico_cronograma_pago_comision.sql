CREATE TABLE [dbo].[historico_cronograma_pago_comision] (
    [codigo]                      INT            IDENTITY (1, 1) NOT NULL,
    [codigo_cronograma]           INT            NOT NULL,
    [codigo_tipo_planilla]        INT            NOT NULL,
    [codigo_empresa]              INT            NOT NULL,
    [codigo_personal_canal_grupo] INT            NOT NULL,
    [nro_contrato]                NVARCHAR (200) NOT NULL,
    [codigo_tipo_venta]           INT            NOT NULL,
    [codigo_tipo_pago]            INT            NOT NULL,
    [codigo_moneda]               INT            NOT NULL,
    [fecha_registro]              DATETIME       NOT NULL,
    [fecha_historico]             DATETIME       NOT NULL,
    [codigo_tipo_historico]       INT            NOT NULL,
    CONSTRAINT [PK_historico_cronograma_pago_comision] PRIMARY KEY CLUSTERED ([codigo] ASC)
);