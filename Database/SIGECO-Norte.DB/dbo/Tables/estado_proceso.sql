CREATE TABLE [dbo].[estado_proceso] (
    [codigo_estado_proceso] INT          IDENTITY (1, 1) NOT NULL,
    [nombre]                VARCHAR (50) NOT NULL,
    [estado_registro]       BIT          NOT NULL,
    [fecha_registra]        DATETIME     NOT NULL,
    [usuario_registra]      VARCHAR (50) NOT NULL,
    [fecha_modifica]        DATETIME     NULL,
    [usuario_modifica]      VARCHAR (50) NULL,
    CONSTRAINT [PK_estado_proceso] PRIMARY KEY CLUSTERED ([codigo_estado_proceso] ASC)
);