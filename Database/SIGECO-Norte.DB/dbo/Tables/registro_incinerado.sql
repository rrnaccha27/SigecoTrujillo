CREATE TABLE [dbo].[registro_incinerado] (
    [codigo_registro_incinerado] INT          IDENTITY (1, 1) NOT NULL,
    [codigo_fallecido]           INT          NOT NULL,
    [codigo_registro_fallecido]  INT          NOT NULL,
    [fecha_registra]             DATETIME     NOT NULL,
    [fecha_modifica]             DATETIME     NULL,
    [usuario_registra]           VARCHAR (50) NOT NULL,
    [usuario_modifica]           VARCHAR (50) NULL,
    CONSTRAINT [registro_incinerado_pk] PRIMARY KEY CLUSTERED ([codigo_registro_incinerado] ASC),
    CONSTRAINT [fallecido_registro_incinerado_fk] FOREIGN KEY ([codigo_fallecido]) REFERENCES [dbo].[fallecido] ([codigo_fallecido]) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT [registro_fallecido_registro_incinerado_fk] FOREIGN KEY ([codigo_registro_fallecido]) REFERENCES [dbo].[registro_fallecido] ([codigo_registro_fallecido])
);