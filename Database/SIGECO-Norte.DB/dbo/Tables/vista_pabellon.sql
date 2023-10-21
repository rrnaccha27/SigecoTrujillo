CREATE TABLE [dbo].[vista_pabellon] (
    [codigo_vista_pabellon] INT          IDENTITY (1, 1) NOT NULL,
    [nombre_vista_pabellon] VARCHAR (50) NULL,
    [estado_registro]       BIT          NOT NULL,
    [fecha_registra]        DATETIME     NOT NULL,
    [fecha_modifica]        DATETIME     NULL,
    [usuario_registra]      VARCHAR (50) NOT NULL,
    [usuario_modifica]      VARCHAR (50) NULL,
    CONSTRAINT [codigo_vista_pabellon_pk] PRIMARY KEY CLUSTERED ([codigo_vista_pabellon] ASC)
);