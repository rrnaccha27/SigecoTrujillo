CREATE TABLE [dbo].[tipo_venta] (
    [codigo_tipo_venta]   INT           IDENTITY (1, 1) NOT NULL,
    [codigo_equivalencia] NVARCHAR (10) NULL,
    [nombre]              VARCHAR (50)  NOT NULL,
    [abreviatura]         VARCHAR (10)  NOT NULL,
    [estado_registro]     BIT           NOT NULL,
    [fecha_registra]      DATETIME      NOT NULL,
    [usuario_registra]    VARCHAR (50)  NOT NULL,
    [fecha_modifica]      DATETIME      NULL,
    [usuario_modifica]    VARCHAR (50)  NULL,
    CONSTRAINT [PK_tipo_venta] PRIMARY KEY CLUSTERED ([codigo_tipo_venta] ASC)
);