CREATE TABLE [dbo].[menu] (
    [codigo_menu]       INT           IDENTITY (1, 1) NOT NULL,
    [codigo_menu_padre] INT           NULL,
    [nombre_menu]       VARCHAR (50)  NOT NULL,
    [ruta_menu]         VARCHAR (100) NULL,
    [estado_registro]   BIT           NOT NULL,
    [orden]             INT           NOT NULL,
    [fecha_registra]    DATETIME      NOT NULL,
    [fecha_modifica]    DATETIME      NULL,
    [usuario_registra]  VARCHAR (50)  NOT NULL,
    [usuario_modifica]  VARCHAR (50)  NULL,
    CONSTRAINT [codigo_menu] PRIMARY KEY CLUSTERED ([codigo_menu] ASC)
);