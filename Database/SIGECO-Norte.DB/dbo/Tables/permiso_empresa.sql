CREATE TABLE [dbo].[permiso_empresa] (
    [codigo_permiso_empresa] INT          IDENTITY (1, 1) NOT NULL,
    [codigo_usuario]         VARCHAR (50) NOT NULL,
    [codigo_empresa]         INT          NOT NULL,
    [estado_registro]        BIT          NOT NULL,
    [fecha_registra]         DATETIME     NOT NULL,
    [usuario_registra]       VARCHAR (50) NOT NULL,
    CONSTRAINT [permiso_empresa_pk] PRIMARY KEY CLUSTERED ([codigo_permiso_empresa] ASC),
    CONSTRAINT [empresa_permiso_empresa_fk] FOREIGN KEY ([codigo_empresa]) REFERENCES [dbo].[empresa] ([codigo_empresa]),
    CONSTRAINT [usuario_permiso_empresa_fk] FOREIGN KEY ([codigo_usuario]) REFERENCES [dbo].[usuario] ([codigo_usuario]) ON DELETE CASCADE ON UPDATE CASCADE
);