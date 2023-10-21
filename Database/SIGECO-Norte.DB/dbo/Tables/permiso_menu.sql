CREATE TABLE [dbo].[permiso_menu] (
    [codigo_permiso_menu]   INT          IDENTITY (1, 1) NOT NULL,
    [codigo_perfil_usuario] INT          NOT NULL,
    [codigo_menu]           INT          NOT NULL,
    [estado_registro]       BIT          NOT NULL,
    [fecha_registra]        DATETIME     NOT NULL,
    [usuario_registra]      VARCHAR (50) NOT NULL,
    CONSTRAINT [codigo_permiso_menu] PRIMARY KEY CLUSTERED ([codigo_permiso_menu] ASC),
    CONSTRAINT [menu_permiso_menu_fk] FOREIGN KEY ([codigo_menu]) REFERENCES [dbo].[menu] ([codigo_menu]),
    CONSTRAINT [perfil_usuario_permiso_menu_fk] FOREIGN KEY ([codigo_perfil_usuario]) REFERENCES [dbo].[perfil_usuario] ([codigo_perfil_usuario])
);