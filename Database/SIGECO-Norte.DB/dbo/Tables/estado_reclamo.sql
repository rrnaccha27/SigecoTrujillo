CREATE TABLE [dbo].[estado_reclamo] (
    [codigo_estado_reclamo] INT          IDENTITY (1, 1) NOT NULL,
    [nombre_estado_reclamo] VARCHAR (50) NOT NULL,
    [estado_registro]       BIT          NOT NULL,
    [fecha_registra]        DATETIME     NOT NULL,
    [fecha_modifica]        DATETIME     NULL,
    [usuario_registra]      VARCHAR (50) NOT NULL,
    [usuario_modifica]      VARCHAR (50) NULL,
    CONSTRAINT [codigo_estado_reclamo_pk] PRIMARY KEY CLUSTERED ([codigo_estado_reclamo] ASC)
);