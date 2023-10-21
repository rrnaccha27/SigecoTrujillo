CREATE TABLE [dbo].[tipo_espacio] (
    [codigo_tipo_espacio] INT          IDENTITY (1, 1) NOT NULL,
    [nombre_tipo_espacio] VARCHAR (50) NOT NULL,
    [estado_registro]     BIT          NOT NULL,
    [numero_columnas]     INT          DEFAULT ((0)) NOT NULL,
    [numero_filas]        INT          DEFAULT ((0)) NOT NULL,
    [tipo_lote]           BIT          DEFAULT ((0)) NOT NULL,
    [constante]           VARCHAR (2)  NULL,
    [fecha_registra]      DATETIME     NOT NULL,
    [fecha_modifica]      DATETIME     NULL,
    [usuario_registra]    VARCHAR (50) NOT NULL,
    [usuario_modifica]    VARCHAR (50) NULL,
    CONSTRAINT [tipo_espacio_pk] PRIMARY KEY CLUSTERED ([codigo_tipo_espacio] ASC)
);