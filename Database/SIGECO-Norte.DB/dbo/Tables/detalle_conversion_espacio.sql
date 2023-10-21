CREATE TABLE [dbo].[detalle_conversion_espacio] (
    [codigo_detalle_conversion_espacio] INT          IDENTITY (1, 1) NOT NULL,
    [codigo_estado_conversion]          INT          NOT NULL,
    [codigo_conversion_espacio]         INT          NOT NULL,
    [estado_registro]                   BIT          DEFAULT ((1)) NOT NULL,
    [fecha_registra]                    DATETIME     DEFAULT (getdate()) NOT NULL,
    [usuario_registra]                  VARCHAR (30) NOT NULL,
    [fecha_modifica]                    DATETIME     NULL,
    [usuario_modifica]                  VARCHAR (30) NULL,
    CONSTRAINT [codigo_detalle_conversion_espacio] PRIMARY KEY CLUSTERED ([codigo_detalle_conversion_espacio] ASC),
    CONSTRAINT [det_conversion_espacio_estado_conversion_fk] FOREIGN KEY ([codigo_estado_conversion]) REFERENCES [dbo].[estado_conversion] ([codigo_estado_conversion]),
    CONSTRAINT [det_conversion_espacio_fk] FOREIGN KEY ([codigo_conversion_espacio]) REFERENCES [dbo].[conversion_espacio] ([codigo_conversion_espacio])
);