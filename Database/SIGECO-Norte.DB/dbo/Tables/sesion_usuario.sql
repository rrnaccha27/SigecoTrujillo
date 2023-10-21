CREATE TABLE [dbo].[sesion_usuario] (
    [codigo_sesion_usuario] INT          IDENTITY (1, 1) NOT NULL,
    [codigo_usuario]        VARCHAR (50) NOT NULL,
    [fecha_inicio]          DATETIME     NOT NULL,
    [fecha_fin]             DATETIME     NULL,
    [ip_host]               VARCHAR (15) NULL,
    CONSTRAINT [codigo_sesion_usuario] PRIMARY KEY CLUSTERED ([codigo_sesion_usuario] ASC),
    CONSTRAINT [usuario_sesion_usuario_fk] FOREIGN KEY ([codigo_usuario]) REFERENCES [dbo].[usuario] ([codigo_usuario])
);