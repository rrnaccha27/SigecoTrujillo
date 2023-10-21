CREATE TABLE [dbo].[tipo_piso] (
    [codigo_tipo_piso] INT           IDENTITY (1, 1) NOT NULL,
    [nombre_tipo_piso] NVARCHAR (50) NOT NULL,
    [estado_registro]  BIT           NOT NULL,
    [fecha_registra]   DATETIME      NOT NULL,
    [fecha_modifica]   DATETIME      NULL,
    [usuario_registra] VARCHAR (50)  NOT NULL,
    [usuario_modifica] VARCHAR (50)  NULL,
    CONSTRAINT [codigo_tipo_piso] PRIMARY KEY CLUSTERED ([codigo_tipo_piso] ASC)
);