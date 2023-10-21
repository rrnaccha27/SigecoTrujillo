CREATE TABLE [dbo].[evento_usuario] (
    [codigo_evento_usuario] INT          IDENTITY (1, 1) NOT NULL,
    [codigo_usuario]        VARCHAR (50) NOT NULL,
    [codigo_menu]           INT          NOT NULL,
    [codigo_tipo_evento]    INT          NOT NULL,
    [fecha_suceso]          DATETIME     NOT NULL,
    [estado_evento]         BIT          NOT NULL,
    CONSTRAINT [codigo_evento_usuario] PRIMARY KEY CLUSTERED ([codigo_evento_usuario] ASC),
    CONSTRAINT [menu_evento_usuario_fk] FOREIGN KEY ([codigo_menu]) REFERENCES [dbo].[menu] ([codigo_menu]),
    CONSTRAINT [tipo_evento_evento_usuario_fk] FOREIGN KEY ([codigo_tipo_evento]) REFERENCES [dbo].[tipo_evento] ([codigo_tipo_evento]),
    CONSTRAINT [usuario_evento_usuario_fk] FOREIGN KEY ([codigo_usuario]) REFERENCES [dbo].[usuario] ([codigo_usuario])
);