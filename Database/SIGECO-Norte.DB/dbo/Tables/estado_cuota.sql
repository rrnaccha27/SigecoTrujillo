CREATE TABLE [dbo].[estado_cuota] (
    [codigo_estado_cuota] INT          IDENTITY (1, 1) NOT NULL,
    [nombre]              VARCHAR (50) NOT NULL,
    [estado_registro]     BIT          NOT NULL,
    [fecha_registra]      DATETIME     NOT NULL,
    [usuario_registra]    VARCHAR (50) NOT NULL,
    [fecha_modifica]      DATETIME     NULL,
    [usuario_modifica]    VARCHAR (50) NULL,
    CONSTRAINT [PK_estado_cuota] PRIMARY KEY CLUSTERED ([codigo_estado_cuota] ASC)
);