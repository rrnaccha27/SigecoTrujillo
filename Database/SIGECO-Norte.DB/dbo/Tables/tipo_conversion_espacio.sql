CREATE TABLE [dbo].[tipo_conversion_espacio] (
    [codigo_tipo_conversion_espacio] INT          IDENTITY (1, 1) NOT NULL,
    [nombre_tipo_conversion_espacio] VARCHAR (50) NOT NULL,
    [cantidad_conversion]            INT          DEFAULT ((1)) NOT NULL,
    [codigo_tipo_nivel]              CHAR (1)     NOT NULL,
    [estado_registro]                BIT          DEFAULT ((1)) NOT NULL,
    [fecha_registra]                 DATETIME     NOT NULL,
    [fecha_modifica]                 DATETIME     NULL,
    [usuario_registra]               VARCHAR (50) NOT NULL,
    [usuario_modifica]               VARCHAR (50) NULL,
    CONSTRAINT [codigo_tipo_conversion_espacio] PRIMARY KEY CLUSTERED ([codigo_tipo_conversion_espacio] ASC),
    CONSTRAINT [tipo_nivel_tipo_conversion_espacio_fk] FOREIGN KEY ([codigo_tipo_nivel]) REFERENCES [dbo].[tipo_nivel] ([codigo_tipo_nivel])
);