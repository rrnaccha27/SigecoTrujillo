CREATE TABLE [dbo].[grupo_lapida] (
    [codigo_grupo_lapida] INT          IDENTITY (1, 1) NOT NULL,
    [nombre_grupo_lapida] VARCHAR (50) NOT NULL,
    [codigo_asignacion]   VARCHAR (10) NULL,
    [estado_registro]     BIT          NOT NULL,
    [fecha_registra]      DATETIME     NOT NULL,
    [fecha_modifica]      DATETIME     NULL,
    [usuario_registra]    VARCHAR (50) NOT NULL,
    [usuario_modifica]    VARCHAR (50) NULL,
    PRIMARY KEY CLUSTERED ([codigo_grupo_lapida] ASC)
);