CREATE TABLE [dbo].[personal_canal_grupo] (
    [codigo_registro]     INT          IDENTITY (1, 1) NOT NULL,
    [codigo_personal]     INT          NOT NULL,
    [codigo_canal_grupo]  INT          NOT NULL,
    [codigo_canal]        INT          NOT NULL,
    [es_supervisor_canal] BIT          NOT NULL,
    [es_supervisor_grupo] BIT          NOT NULL,
    [percibe_comision]    BIT          NOT NULL,
    [percibe_bono]        BIT          NOT NULL,
    [estado_registro]     BIT          NOT NULL,
    [fecha_registra]      DATETIME     NOT NULL,
    [usuario_registra]    VARCHAR (50) NOT NULL,
    [fecha_modifica]      DATETIME     NULL,
    [usuario_modifica]    VARCHAR (50) NULL,
    CONSTRAINT [PK_personal_canal_grupo] PRIMARY KEY CLUSTERED ([codigo_registro] ASC),
    CONSTRAINT [personal_canal_grupo_codigo_canal_grupo_canal_grupo_codigo_canal_grupo] FOREIGN KEY ([codigo_canal_grupo]) REFERENCES [dbo].[canal_grupo] ([codigo_canal_grupo]),
    CONSTRAINT [personal_canal_grupo_codigo_personal_personal_codigo_personal] FOREIGN KEY ([codigo_personal]) REFERENCES [dbo].[personal] ([codigo_personal])
);