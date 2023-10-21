CREATE TABLE [dbo].[campo_santo_sigeco] (
    [codigo_campo_santo]  INT           IDENTITY (1, 1) NOT NULL,
    [codigo_equivalencia] NVARCHAR (10) NULL,
    [nombre]              VARCHAR (50)  NOT NULL,
    [estado_registro]     BIT           NOT NULL,
    [fecha_registra]      DATETIME      NOT NULL,
    [fecha_modifica]      DATETIME      NULL,
    [usuario_registra]    VARCHAR (50)  NOT NULL,
    [usuario_modifica]    VARCHAR (50)  NULL,
    CONSTRAINT [PK_campo_santo_sigeco] PRIMARY KEY CLUSTERED ([codigo_campo_santo] ASC)
);