﻿CREATE TABLE [dbo].[sector] (
    [id_sector]               INT           IDENTITY (1, 1) NOT NULL,
    [codigo_sector]           VARCHAR (20)  NULL,
    [numero_sector]           INT           NOT NULL,
    [codigo_plataforma]       INT           NOT NULL,
    [eje_x_numero]            INT           NULL,
    [eje_x]                   VARCHAR (10)  NULL,
    [eje_y]                   VARCHAR (10)  NULL,
    [eje_x_superior]          VARCHAR (10)  NULL,
    [eje_y_superior]          VARCHAR (10)  NULL,
    [es_nuevo]                BIT           DEFAULT ((1)) NOT NULL,
    [estado]                  INT           DEFAULT ((1)) NOT NULL,
    [nombre_pabellon]         VARCHAR (150) NULL,
    [identificador_pabellon]  VARCHAR (20)  NULL,
    [numero_pisos_pabellon]   INT           DEFAULT ((0)) NULL,
    [numero_columna_pabellon] INT           DEFAULT ((0)) NULL,
    [codigo_piso_pabellon]    INT           NULL,
    [tiene_pabellon]          BIT           DEFAULT ((0)) NOT NULL,
    [fecha_registra]          DATETIME      NOT NULL,
    [fecha_modifica]          DATETIME      NULL,
    [usuario_registra]        VARCHAR (50)  NOT NULL,
    [usuario_modifica]        VARCHAR (50)  NULL,
    PRIMARY KEY CLUSTERED ([id_sector] ASC),
    FOREIGN KEY ([codigo_plataforma]) REFERENCES [dbo].[plataforma] ([codigo_plataforma])
);