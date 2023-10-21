CREATE TABLE [dbo].[detalle_grupo_lapida] (
    [codigo_detalle]         INT IDENTITY (1, 1) NOT NULL,
    [codigo_grupo_lapida]    INT NOT NULL,
    [codigo_cabecera_lapida] INT NOT NULL,
    [estado_registro]        BIT NOT NULL,
    PRIMARY KEY CLUSTERED ([codigo_detalle] ASC),
    FOREIGN KEY ([codigo_cabecera_lapida]) REFERENCES [dbo].[cabecera_lapida] ([codigo_cabecera_lapida]),
    FOREIGN KEY ([codigo_grupo_lapida]) REFERENCES [dbo].[grupo_lapida] ([codigo_grupo_lapida])
);