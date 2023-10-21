CREATE TABLE [dbo].[cronograma_pago_comision_log] (
    [fecha_log]                   DATETIME       NOT NULL,
    [codigo_cronograma]           INT            NULL,
    [codigo_tipo_planilla]        INT            NULL,
    [codigo_empresa]              INT            NULL,
    [codigo_personal_canal_grupo] INT            NULL,
    [nro_contrato]                NVARCHAR (200) NULL,
    [codigo_tipo_venta]           INT            NULL,
    [codigo_tipo_pago]            INT            NULL,
    [codigo_moneda]               INT            NULL,
    [fecha_registro]              DATETIME       NULL,
    [estado_registro]             BIT            NULL,
    [nro_contrato_adicional]      VARCHAR (100)  NULL,
    [codigo_regla_pago]           INT            NULL,
    [tiene_transferencia]         BIT            DEFAULT ((0)) NULL
);