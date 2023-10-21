CREATE TABLE [dbo].[estado_movimiento_tabla] (
    [codigo_estado_movimiento_tabla] INT           IDENTITY (1, 1) NOT NULL,
    [nombre_estado_movimiento_tabla] VARCHAR (100) NULL,
    [estado_registro]                BIT           DEFAULT ((1)) NOT NULL,
    [descripcion]                    VARCHAR (100) NULL,
    [tabla]                          INT           NOT NULL,
    [fecha_registro]                 DATETIME      DEFAULT (getdate()) NOT NULL,
    [usuario_registro]               VARCHAR (50)  NULL,
    CONSTRAINT [codigo_estado_movimiento_tabla_pk] PRIMARY KEY CLUSTERED ([codigo_estado_movimiento_tabla] ASC)
);