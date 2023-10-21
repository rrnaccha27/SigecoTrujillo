CREATE TABLE [dbo].[item_tipo_acceso] (
    [codigo_item_tipo_acceso] INT          IDENTITY (1, 1) NOT NULL,
    [codigo_tipo_acceso_item] INT          NOT NULL,
    [codigo_perfil_usuario]   INT          NOT NULL,
    [estado_registro]         BIT          NOT NULL,
    [fecha_registra]          DATETIME     NOT NULL,
    [usuario_registra]        VARCHAR (50) NOT NULL,
    CONSTRAINT [codigo_item_tipo_acceso] PRIMARY KEY CLUSTERED ([codigo_item_tipo_acceso] ASC),
    CONSTRAINT [perfil_usuario_item_tipo_acceso_fk] FOREIGN KEY ([codigo_perfil_usuario]) REFERENCES [dbo].[perfil_usuario] ([codigo_perfil_usuario]),
    CONSTRAINT [tipo_acceso_item_item_tipo_acceso_fk] FOREIGN KEY ([codigo_tipo_acceso_item]) REFERENCES [dbo].[tipo_acceso_item] ([codigo_tipo_acceso_item])
);