﻿CREATE TABLE [dbo].[reserva] (
    [codigo_reserva]                 INT            IDENTITY (1, 1) NOT NULL,
    [codigo_persona]                 INT            NOT NULL,
    [numero_documento_cliente]       VARCHAR (15)   NULL,
    [ape_paterno_cliente]            VARCHAR (50)   NULL,
    [ape_materno_cliente]            VARCHAR (50)   NULL,
    [nombre_cliente]                 VARCHAR (50)   NULL,
    [fecha_caducidad]                DATETIME       NOT NULL,
    [reserva_indefinido]             BIT            DEFAULT ((0)) NOT NULL,
    [codigo_producto]                INT            NULL,
    [fecha_registra]                 DATETIME       NOT NULL,
    [fecha_modifica]                 DATETIME       NULL,
    [usuario_registra]               VARCHAR (50)   NOT NULL,
    [usuario_modifica]               VARCHAR (50)   NULL,
    [observacion_anulacion]          VARCHAR (50)   NULL,
    [codigo_espacio]                 VARCHAR (20)   NOT NULL,
    [memo_reserva]                   NVARCHAR (200) NULL,
    [fecha_reserva]                  DATETIME       NOT NULL,
    [estado_registro]                BIT            NOT NULL,
    [motivo_registro]                VARCHAR (150)  NULL,
    [codigo_estado_movimiento_tabla] INT            NOT NULL,
    CONSTRAINT [reserva_pk] PRIMARY KEY CLUSTERED ([codigo_reserva] ASC),
    CONSTRAINT [cod_estado_mov_tabla_reserva_fk] FOREIGN KEY ([codigo_estado_movimiento_tabla]) REFERENCES [dbo].[estado_movimiento_tabla] ([codigo_estado_movimiento_tabla]),
    CONSTRAINT [persona_reserva_fk] FOREIGN KEY ([codigo_persona]) REFERENCES [dbo].[persona] ([codigo_persona]),
    CONSTRAINT [producto_reserva_fk] FOREIGN KEY ([codigo_producto]) REFERENCES [dbo].[producto] ([codigo_producto]) ON DELETE CASCADE ON UPDATE CASCADE
);