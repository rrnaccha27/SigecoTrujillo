CREATE TABLE [dbo].[cabecera_lapida] (
    [codigo_cabecera_lapida]  INT           IDENTITY (1, 1) NOT NULL,
    [titulo]                  VARCHAR (200) NULL,
    [descripcion]             VARCHAR (200) NULL,
    [estado_registro]         BIT           NOT NULL,
    [fecha_registra]          DATETIME      NOT NULL,
    [fecha_modifica]          DATETIME      NULL,
    [usuario_registra]        VARCHAR (50)  NOT NULL,
    [usuario_modifica]        VARCHAR (50)  NULL,
    [codigo_estado_lapida]    INT           NOT NULL,
    [codigo_espacio]          VARCHAR (20)  NULL,
    [fecha_inicio_confeccion] DATETIME      NULL,
    [fecha_fin_confeccion]    DATETIME      NULL,
    [fecha_colocacion]        DATETIME      NULL,
    CONSTRAINT [codigo_cabecera_lapida_pk] PRIMARY KEY CLUSTERED ([codigo_cabecera_lapida] ASC),
    FOREIGN KEY ([codigo_estado_lapida]) REFERENCES [dbo].[estado_lapida] ([codigo_estado_lapida]),
    CONSTRAINT [cabecera_lapida_espacio_fk] FOREIGN KEY ([codigo_espacio]) REFERENCES [dbo].[espacio] ([codigo_espacio])
);