﻿CREATE TABLE [dbo].[articulo] (
    [codigo_articulo]           INT           IDENTITY (1, 1) NOT NULL,
    [codigo_unidad_negocio]     INT           NOT NULL,
    [codigo_categoria]          INT           NOT NULL,
    [codigo_sku]                VARCHAR (20)  NOT NULL,
    [nombre]                    VARCHAR (250) NOT NULL,
    [abreviatura]               VARCHAR (10)  NOT NULL,
    [genera_comision]           BIT           NOT NULL,
    [genera_bono]               BIT           NOT NULL,
    [genera_bolsa_bono]         BIT           NOT NULL,
    [anio_contrato_vinculante]  INT           NULL,
    [tiene_contrato_vinculante] BIT           NULL,
    [estado_registro]           BIT           NOT NULL,
    [fecha_registra]            DATETIME      NOT NULL,
    [usuario_registra]          VARCHAR (50)  NOT NULL,
    [fecha_modifica]            DATETIME      NULL,
    [usuario_modifica]          VARCHAR (50)  NULL,
    [codigo_tipo_articulo]      INT           DEFAULT ((0)) NOT NULL,
    [cantidad_unica]            BIT           DEFAULT ((0)) NULL,
    CONSTRAINT [PK_articulo] PRIMARY KEY CLUSTERED ([codigo_articulo] ASC),
    CONSTRAINT [articulo_codigo_categoria_categoria_codigo_categoria] FOREIGN KEY ([codigo_categoria]) REFERENCES [dbo].[categoria] ([codigo_categoria]),
    CONSTRAINT [articulo_codigo_unidad_negocio_unidad_negocio_codigo_unidad_negocio] FOREIGN KEY ([codigo_unidad_negocio]) REFERENCES [dbo].[unidad_negocio] ([codigo_unidad_negocio]),
    CONSTRAINT [FK_articulo_codigo_tipo_articulo_tipo_articulo_codigo_tipo_articulo] FOREIGN KEY ([codigo_tipo_articulo]) REFERENCES [dbo].[tipo_articulo] ([codigo_tipo_articulo])
);