CREATE TABLE [dbo].[empresa] (
    [codigo_empresa]     INT          IDENTITY (1, 1) NOT NULL,
    [codigo_corporacion] INT          NOT NULL,
    [nombre_empresa]     VARCHAR (50) NOT NULL,
    [estado_registro]    BIT          NOT NULL,
    [fecha_registra]     DATETIME     NOT NULL,
    [fecha_modifica]     DATETIME     NULL,
    [usuario_registra]   VARCHAR (50) NOT NULL,
    [usuario_modifica]   VARCHAR (50) NULL,
    CONSTRAINT [empresa_pk] PRIMARY KEY CLUSTERED ([codigo_empresa] ASC),
    CONSTRAINT [corporacion_empresa_fk] FOREIGN KEY ([codigo_corporacion]) REFERENCES [dbo].[corporacion] ([codigo_corporacion])
);