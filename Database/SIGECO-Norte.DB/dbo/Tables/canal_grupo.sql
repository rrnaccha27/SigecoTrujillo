CREATE TABLE [dbo].[canal_grupo] (
    [codigo_canal_grupo]  INT           IDENTITY (1, 1) NOT NULL,
    [es_canal_grupo]      BIT           NOT NULL,
    [codigo_equivalencia] VARCHAR (4)   NULL,
    [nombre]              VARCHAR (250) NOT NULL,
    [administra_grupos]   BIT           NOT NULL,
    [codigo_padre]        INT           NULL,
    [estado_registro]     BIT           NOT NULL,
    [fecha_registra]      DATETIME      NOT NULL,
    [usuario_registra]    VARCHAR (50)  NOT NULL,
    [fecha_modifica]      DATETIME      NULL,
    [usuario_modifica]    VARCHAR (50)  NULL,
    CONSTRAINT [PK_canal_grupo] PRIMARY KEY CLUSTERED ([codigo_canal_grupo] ASC)
);