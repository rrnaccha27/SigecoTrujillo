CREATE TABLE [dbo].[tipo_plataforma] (
    [codigo_tipo_plataforma] INT          IDENTITY (1, 1) NOT NULL,
    [nombre_tipo_plataforma] VARCHAR (50) NOT NULL,
    [estado_registro]        BIT          NOT NULL,
    [numero_espacios]        INT          NOT NULL,
    [fecha_registra]         DATETIME     NOT NULL,
    [fecha_modifica]         DATETIME     NULL,
    [usuario_registra]       VARCHAR (50) NOT NULL,
    [usuario_modifica]       VARCHAR (50) NULL,
    CONSTRAINT [tipo_plataforma_pk] PRIMARY KEY CLUSTERED ([codigo_tipo_plataforma] ASC)
);