CREATE TABLE [dbo].[tipo_acceso_item] (
    [codigo_tipo_acceso_item] INT          IDENTITY (1, 1) NOT NULL,
    [nombre_tipo_acceso_item] VARCHAR (50) NOT NULL,
    [estado_registro]         BIT          NOT NULL,
    [fecha_registra]          DATETIME     NOT NULL,
    [fecha_modifica]          DATETIME     NULL,
    [usuario_registra]        VARCHAR (50) NOT NULL,
    [usuario_modifica]        VARCHAR (50) NULL,
    CONSTRAINT [codigo_tipo_acceso_item] PRIMARY KEY CLUSTERED ([codigo_tipo_acceso_item] ASC)
);