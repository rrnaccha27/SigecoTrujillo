CREATE TABLE [dbo].[campo_santo] (
    [codigo_campo_santo]     INT          IDENTITY (1, 1) NOT NULL,
    [codigo_corporacion]     INT          NOT NULL,
    [codigo_empresa]         INT          NOT NULL,
    [nombre_campo_santo]     VARCHAR (50) NOT NULL,
    [anio_minimo_conversion] INT          NOT NULL,
    [estado_registro]        BIT          NOT NULL,
    [fecha_registra]         DATETIME     NOT NULL,
    [fecha_modifica]         DATETIME     NULL,
    [usuario_registra]       VARCHAR (50) NOT NULL,
    [usuario_modifica]       VARCHAR (50) NULL,
    CONSTRAINT [campo_santo_pk] PRIMARY KEY CLUSTERED ([codigo_campo_santo] ASC),
    CONSTRAINT [corporacion_campo_santo_fk] FOREIGN KEY ([codigo_corporacion]) REFERENCES [dbo].[corporacion] ([codigo_corporacion]),
    CONSTRAINT [empresa_campo_santo_fk] FOREIGN KEY ([codigo_empresa]) REFERENCES [dbo].[empresa] ([codigo_empresa])
);