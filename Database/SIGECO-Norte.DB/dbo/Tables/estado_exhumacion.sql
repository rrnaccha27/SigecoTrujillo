CREATE TABLE [dbo].[estado_exhumacion] (
    [codigo_estado_exhumacion] INT            IDENTITY (1, 1) NOT NULL,
    [nombre_estado_exhumacion] NVARCHAR (100) NOT NULL,
    [estado_registro]          BIT            DEFAULT ((1)) NOT NULL,
    [fecha_registra]           DATETIME       DEFAULT (getdate()) NOT NULL,
    [fecha_modifica]           DATETIME       NULL,
    [usuario_registra]         VARCHAR (20)   NOT NULL,
    [usuario_modifica]         VARCHAR (20)   NULL,
    PRIMARY KEY CLUSTERED ([codigo_estado_exhumacion] ASC)
);