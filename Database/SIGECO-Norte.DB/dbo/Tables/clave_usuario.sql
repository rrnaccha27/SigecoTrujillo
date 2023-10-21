CREATE TABLE [dbo].[clave_usuario] (
    [codigo]           INT           IDENTITY (1, 1) NOT NULL,
    [codigo_usuario]   VARCHAR (50)  NOT NULL,
    [clave]            VARCHAR (200) NOT NULL,
    [estado_registro]  BIT           NOT NULL,
    [fecha_registra]   DATETIME      NOT NULL,
    [fecha_modifica]   DATETIME      NULL,
    [usuario_registra] VARCHAR (50)  NOT NULL,
    [usuario_modifica] VARCHAR (50)  NULL,
    CONSTRAINT [clave_usuario_pk] PRIMARY KEY CLUSTERED ([codigo] ASC),
    CONSTRAINT [usuario_New_table_fk] FOREIGN KEY ([codigo_usuario]) REFERENCES [dbo].[usuario] ([codigo_usuario]) ON DELETE CASCADE ON UPDATE CASCADE
);