CREATE TABLE [dbo].[cliente] (
    [codigo_cliente]   VARCHAR (20) NOT NULL,
    [codigo_persona]   INT          NOT NULL,
    [estado_registro]  VARCHAR (2)  NOT NULL,
    [fecha_registra]   DATETIME     NOT NULL,
    [fecha_modifica]   DATETIME     NULL,
    [usuario_registra] VARCHAR (50) NOT NULL,
    [usuario_modifica] VARCHAR (50) NULL,
    CONSTRAINT [cliente_pk] PRIMARY KEY CLUSTERED ([codigo_cliente] ASC),
    CONSTRAINT [persona_cliente_fk] FOREIGN KEY ([codigo_persona]) REFERENCES [dbo].[persona] ([codigo_persona]) ON DELETE CASCADE ON UPDATE CASCADE
);