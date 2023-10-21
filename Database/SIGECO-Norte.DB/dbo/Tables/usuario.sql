CREATE TABLE [dbo].[usuario] (
    [codigo_usuario]        VARCHAR (50) NOT NULL,
    [codigo_perfil_usuario] INT          NOT NULL,
    [estado_registro]       VARCHAR (2)  NOT NULL,
    [codigo_persona]        INT          NULL,
    [fecha_registra]        DATETIME     NOT NULL,
    [fecha_modifica]        DATETIME     NULL,
    [usuario_registra]      VARCHAR (50) NOT NULL,
    [usuario_modifica]      VARCHAR (50) NULL,
    CONSTRAINT [usuario_pk] PRIMARY KEY CLUSTERED ([codigo_usuario] ASC),
    CONSTRAINT [perfil_usuario_fk] FOREIGN KEY ([codigo_perfil_usuario]) REFERENCES [dbo].[perfil_usuario] ([codigo_perfil_usuario]) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT [persona_usuario_fk] FOREIGN KEY ([codigo_persona]) REFERENCES [dbo].[persona] ([codigo_persona]) ON DELETE CASCADE ON UPDATE CASCADE
);