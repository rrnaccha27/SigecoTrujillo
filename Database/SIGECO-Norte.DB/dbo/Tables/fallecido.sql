﻿CREATE TABLE [dbo].[fallecido] (
    [codigo_fallecido]           INT           IDENTITY (1, 1) NOT NULL,
    [codigo_campo_santo]         INT           NULL,
    [codigo_persona]             INT           NULL,
    [numero_contrato]            VARCHAR (30)  NULL,
    [codigo_tipo_fallecido]      INT           NOT NULL,
    [numero_documento]           VARCHAR (20)  NULL,
    [codigo_tipo_documento]      INT           NOT NULL,
    [apellido_paterno]           VARCHAR (30)  NOT NULL,
    [apellido_materno]           VARCHAR (30)  NULL,
    [nombres]                    VARCHAR (30)  NOT NULL,
    [edad]                       INT           NOT NULL,
    [fecha_nacimiento]           DATETIME      NOT NULL,
    [fecha_defuncion]            DATETIME      NOT NULL,
    [fecha_traslado_plaza]       DATETIME      NULL,
    [codigo_motivo_traslado]     INT           NULL,
    [observacion_traslado]       VARCHAR (500) NULL,
    [codigo_sexo]                CHAR (1)      NOT NULL,
    [codigo_agencia]             INT           NULL,
    [codigo_nivel_servicio]      INT           NULL,
    [codigo_caracteristica_lote] INT           NULL,
    [codigo_tipo_bien_servicio]  INT           NULL,
    [codigo_tipo_bien]           INT           NULL,
    [codigo_tipo_traslado]       INT           NULL,
    [estado_registro]            BIT           NOT NULL,
    [fecha_registra]             DATETIME      NOT NULL,
    [fecha_modifica]             DATETIME      NULL,
    [usuario_registra]           VARCHAR (50)  NOT NULL,
    [usuario_modifica]           VARCHAR (50)  NULL,
    CONSTRAINT [fallecido_pk] PRIMARY KEY CLUSTERED ([codigo_fallecido] ASC),
    FOREIGN KEY ([codigo_campo_santo]) REFERENCES [dbo].[campo_santo] ([codigo_campo_santo]),
    CONSTRAINT [agencia_fallecido_fk] FOREIGN KEY ([codigo_agencia]) REFERENCES [dbo].[agencia] ([codigo_agencia]) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT [persona_fallecido_fk] FOREIGN KEY ([codigo_persona]) REFERENCES [dbo].[persona] ([codigo_persona]) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT [tipo_fallecido_fallecido_fk] FOREIGN KEY ([codigo_tipo_fallecido]) REFERENCES [dbo].[tipo_fallecido] ([codigo_tipo_fallecido]),
    CONSTRAINT [tipo_traslado_fallecido_fk] FOREIGN KEY ([codigo_tipo_traslado]) REFERENCES [dbo].[tipo_traslado] ([codigo_tipo_traslado]) ON DELETE CASCADE ON UPDATE CASCADE
);