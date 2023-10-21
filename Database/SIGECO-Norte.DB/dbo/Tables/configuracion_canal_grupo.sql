CREATE TABLE [dbo].[configuracion_canal_grupo] (
    [codigo_configuracion] INT IDENTITY (1, 1) NOT NULL,
    [codigo_canal_grupo]   INT NOT NULL,
    [supervisor_personal]  BIT NOT NULL,
    [comision_bono]        BIT NOT NULL,
    [percibe]              BIT NOT NULL,
    CONSTRAINT [PK_configuracion_canal_grupo] PRIMARY KEY CLUSTERED ([codigo_configuracion] ASC),
    CONSTRAINT [FK_configuracion_canal_grupo_codigo_canal_grupo_canal_grupo_codigo_canal_grupo] FOREIGN KEY ([codigo_canal_grupo]) REFERENCES [dbo].[canal_grupo] ([codigo_canal_grupo])
);