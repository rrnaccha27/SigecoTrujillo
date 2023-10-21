CREATE TABLE [dbo].[tipo_nivel] (
    [codigo_tipo_nivel] CHAR (1)     NOT NULL,
    [nombre_tipo_nivel] VARCHAR (50) NOT NULL,
    [cantidad]          INT          NOT NULL,
    [estado_registro]   BIT          NOT NULL,
    [fecha_registra]    DATETIME     NOT NULL,
    [fecha_modifica]    DATETIME     NULL,
    [usuario_registra]  VARCHAR (50) NOT NULL,
    [usuario_modifica]  VARCHAR (50) NULL,
    CONSTRAINT [tipo_nivel_pk] PRIMARY KEY CLUSTERED ([codigo_tipo_nivel] ASC)
);