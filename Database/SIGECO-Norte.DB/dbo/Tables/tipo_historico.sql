CREATE TABLE [dbo].[tipo_historico] (
    [codigo_tipo_historico] INT          IDENTITY (1, 1) NOT NULL,
    [nombre]                VARCHAR (50) NOT NULL,
    [estado_registro]       BIT          NOT NULL,
    [fecha_registra]        DATETIME     NOT NULL,
    [usuario_registra]      VARCHAR (50) NOT NULL,
    [fecha_modifica]        DATETIME     NULL,
    [usuario_modifica]      VARCHAR (50) NULL,
    CONSTRAINT [PK_tipo_historico] PRIMARY KEY CLUSTERED ([codigo_tipo_historico] ASC)
);