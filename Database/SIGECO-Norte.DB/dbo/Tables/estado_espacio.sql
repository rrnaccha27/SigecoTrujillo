CREATE TABLE [dbo].[estado_espacio] (
    [codigo_estado_espacio] INT          IDENTITY (1, 1) NOT NULL,
    [nombre_estado_espacio] VARCHAR (50) NOT NULL,
    [estado_registro]       BIT          NOT NULL,
    [fecha_registra]        DATETIME     NOT NULL,
    [fecha_modifica]        DATETIME     NULL,
    [usuario_registra]      VARCHAR (50) NOT NULL,
    [usuario_modifica]      VARCHAR (50) NULL,
    CONSTRAINT [estado_espacio_pk] PRIMARY KEY CLUSTERED ([codigo_estado_espacio] ASC)
);