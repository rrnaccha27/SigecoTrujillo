CREATE TABLE [dbo].[moneda] (
    [codigo_moneda]       INT           IDENTITY (1, 1) NOT NULL,
    [codigo_equivalencia] NVARCHAR (10) NULL,
    [nombre]              VARCHAR (50)  NOT NULL,
    [simbolo]             VARCHAR (5)   NOT NULL,
    [estado_registro]     BIT           NOT NULL,
    [fecha_registra]      DATETIME      NOT NULL,
    [usuario_registra]    VARCHAR (50)  NOT NULL,
    [fecha_modifica]      DATETIME      NULL,
    [usuario_modifica]    VARCHAR (50)  NULL,
    CONSTRAINT [PK_moneda] PRIMARY KEY CLUSTERED ([codigo_moneda] ASC)
);