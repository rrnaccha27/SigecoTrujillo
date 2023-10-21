CREATE TABLE [dbo].[sexo] (
    [codigo_sexo]      CHAR (1)     NOT NULL,
    [nombre_sexo]      VARCHAR (50) NOT NULL,
    [estado_registro]  BIT          NOT NULL,
    [fecha_registra]   DATETIME     NOT NULL,
    [fecha_modifica]   DATETIME     NULL,
    [usuario_registra] VARCHAR (50) NOT NULL,
    [usuario_modifica] VARCHAR (50) NULL,
    CONSTRAINT [codigo_sexo] PRIMARY KEY CLUSTERED ([codigo_sexo] ASC)
);