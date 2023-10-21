CREATE TABLE [dbo].[detalle_estado_espacio] (
    [codigo_det_estado_espcio] INT           IDENTITY (1, 1) NOT NULL,
    [codigo_espacio]           VARCHAR (20)  NOT NULL,
    [estado_registro]          BIT           NOT NULL,
    [fecha_registro]           DATETIME      NOT NULL,
    [usuario_registro]         NVARCHAR (50) NOT NULL,
    [codigo_estado_espacio]    INT           NOT NULL,
    CONSTRAINT [codigo_det_estado_espcio_pk] PRIMARY KEY CLUSTERED ([codigo_det_estado_espcio] ASC),
    CONSTRAINT [det_estado_espacio_espacio_fk] FOREIGN KEY ([codigo_espacio]) REFERENCES [dbo].[espacio] ([codigo_espacio]),
    CONSTRAINT [det_estado_espacio_estado_fk] FOREIGN KEY ([codigo_estado_espacio]) REFERENCES [dbo].[estado_espacio] ([codigo_estado_espacio])
);