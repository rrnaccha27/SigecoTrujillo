CREATE TABLE [dbo].[unidad_negocio] (
    [codigo_unidad_negocio] INT           IDENTITY (1, 1) NOT NULL,
    [codigo_equivalencia]   NVARCHAR (10) NULL,
    [nombre]                VARCHAR (50)  NOT NULL,
    [estado_registro]       BIT           NOT NULL,
    [fecha_registra]        DATETIME      NOT NULL,
    [usuario_registra]      VARCHAR (50)  NOT NULL,
    [fecha_modifica]        DATETIME      NULL,
    [usuario_modifica]      VARCHAR (50)  NULL,
    CONSTRAINT [PK_unidad_negocio] PRIMARY KEY CLUSTERED ([codigo_unidad_negocio] ASC)
);