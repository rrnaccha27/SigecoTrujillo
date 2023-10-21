CREATE TABLE [dbo].[registro_fallecido] (
    [codigo_registro_fallecido] INT          IDENTITY (1, 1) NOT NULL,
    [codigo_espacio]            VARCHAR (20) NOT NULL,
    [codigo_fallecido]          INT          NOT NULL,
    [nro_nivel_nicho]           INT          NOT NULL,
    [estado_registro]           BIT          NOT NULL,
    [fecha_registra]            DATETIME     NOT NULL,
    [fecha_entierro]            DATETIME     NOT NULL,
    [fecha_modifica]            DATETIME     NULL,
    [usuario_registra]          VARCHAR (50) NOT NULL,
    [usuario_modifica]          VARCHAR (50) NULL,
    [codigo_estado_fallecido]   INT          NOT NULL,
    CONSTRAINT [registro_fallecido_pk] PRIMARY KEY CLUSTERED ([codigo_registro_fallecido] ASC),
    CONSTRAINT [espacio_registro_fallecido_fk] FOREIGN KEY ([codigo_espacio]) REFERENCES [dbo].[espacio] ([codigo_espacio]),
    CONSTRAINT [fallecido_registro_fallecido_fk] FOREIGN KEY ([codigo_fallecido]) REFERENCES [dbo].[fallecido] ([codigo_fallecido]) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT [registro_fallecido_estado_fk] FOREIGN KEY ([codigo_estado_fallecido]) REFERENCES [dbo].[estado_fallecido] ([codigo_estado_fallecido])
);