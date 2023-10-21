CREATE TABLE [dbo].[corporacion] (
    [codigo_corporacion] INT          IDENTITY (1, 1) NOT NULL,
    [nombre_corporacion] VARCHAR (50) NOT NULL,
    [estado_registro]    BIT          NOT NULL,
    [fecha_registra]     DATETIME     NOT NULL,
    [fecha_modifica]     DATETIME     NULL,
    [usuario_registra]   VARCHAR (50) NOT NULL,
    [usuario_modifica]   VARCHAR (50) NULL,
    CONSTRAINT [corporacion_pk] PRIMARY KEY CLUSTERED ([codigo_corporacion] ASC)
);