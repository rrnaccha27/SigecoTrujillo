CREATE TABLE [dbo].[detalle_entidad] (
    [codigo_detalle_entidad] INT          IDENTITY (1, 1) NOT NULL,
    [codigo_evento_usuario]  INT          NOT NULL,
    [nombre_entidad]         VARCHAR (50) NOT NULL,
    [codigo_entidad]         VARCHAR (50) NOT NULL,
    CONSTRAINT [codigo_detalle_entidad] PRIMARY KEY CLUSTERED ([codigo_detalle_entidad] ASC),
    CONSTRAINT [evento_usuario_detalle_entidad_fk] FOREIGN KEY ([codigo_evento_usuario]) REFERENCES [dbo].[evento_usuario] ([codigo_evento_usuario])
);