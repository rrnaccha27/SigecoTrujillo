﻿CREATE TABLE [dbo].[regla_calculo_comision] (
    [codigo_regla]         INT             IDENTITY (1, 1) NOT NULL,
    [codigo_precio]        INT             NOT NULL,
    [codigo_canal]         INT             NOT NULL,
    [codigo_tipo_pago]     INT             NOT NULL,
    [codigo_tipo_comision] INT             NOT NULL,
    [valor]                DECIMAL (10, 2) NULL,
    [vigencia_inicio]      DATETIME        NOT NULL,
    [vigencia_fin]         DATETIME        NOT NULL,
    [estado_registro]      BIT             NOT NULL,
    [fecha_registra]       DATETIME        NOT NULL,
    [usuario_registra]     VARCHAR (50)    NOT NULL,
    [fecha_modifica]       DATETIME        NULL,
    [usuario_modifica]     VARCHAR (50)    NULL,
    CONSTRAINT [PK_regla_calculo_comision] PRIMARY KEY CLUSTERED ([codigo_regla] ASC),
    CONSTRAINT [FK_regla_calculo_comision_codigo_canal_canal_grupo_codigo_canal_grupo] FOREIGN KEY ([codigo_canal]) REFERENCES [dbo].[canal_grupo] ([codigo_canal_grupo]),
    CONSTRAINT [FK_regla_calculo_comision_codigo_precio_precio_articulo_codigo_precio] FOREIGN KEY ([codigo_precio]) REFERENCES [dbo].[precio_articulo] ([codigo_precio]),
    CONSTRAINT [FK_regla_calculo_comision_codigo_tipo_comision_tipo_comision_codigo_tipo_comision] FOREIGN KEY ([codigo_tipo_comision]) REFERENCES [dbo].[tipo_comision] ([codigo_tipo_comision]),
    CONSTRAINT [FK_regla_calculo_comision_codigo_tipo_pago_tipo_pago_codigo_tipo_pago] FOREIGN KEY ([codigo_tipo_pago]) REFERENCES [dbo].[tipo_pago] ([codigo_tipo_pago])
);