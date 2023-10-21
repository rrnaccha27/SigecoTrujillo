CREATE TABLE [dbo].[perfil_usuario] (
    [codigo_perfil_usuario] INT          IDENTITY (1, 1) NOT NULL,
    [nombre_perfil_usuario] VARCHAR (50) NOT NULL,
    [estado_registro]       BIT          NOT NULL,
    [fecha_registra]        DATETIME     NOT NULL,
    [fecha_modifica]        DATETIME     NULL,
    [usuario_registra]      VARCHAR (50) NOT NULL,
    [usuario_modifica]      VARCHAR (50) NULL,
    CONSTRAINT [codigo_perfil_usuario] PRIMARY KEY CLUSTERED ([codigo_perfil_usuario] ASC)
);