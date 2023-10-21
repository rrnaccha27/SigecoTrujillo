CREATE TABLE [dbo].[banco] (
    [codigo_banco]     INT          IDENTITY (1, 1) NOT NULL,
    [nombre]           VARCHAR (50) NOT NULL,
    [estado_registro]  BIT          NOT NULL,
    [fecha_registra]   DATETIME     NOT NULL,
    [usuario_registra] VARCHAR (50) NOT NULL,
    [fecha_modifica]   DATETIME     NULL,
    [usuario_modifica] VARCHAR (50) NULL,
	[interbancario]    BIT          NULL DEFAULT 1,
    CONSTRAINT [PK_banco] PRIMARY KEY CLUSTERED ([codigo_banco] ASC)
);