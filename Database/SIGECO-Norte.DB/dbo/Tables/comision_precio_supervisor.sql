CREATE TABLE [dbo].[comision_precio_supervisor] (
    [codigo_comision]                 INT             IDENTITY (1, 1) NOT NULL,
    [codigo_precio]                   INT             NOT NULL,
    [codigo_canal_grupo]              INT             NOT NULL,
    [codigo_tipo_pago]                INT             NOT NULL,
    [codigo_tipo_comision_supervisor] INT             NOT NULL,
    [valor]                           DECIMAL (10, 2) NOT NULL,
    [vigencia_inicio]                 DATETIME        NOT NULL,
    [vigencia_fin]                    DATETIME        NOT NULL,
    [estado_registro]                 BIT             NOT NULL,
    [fecha_registra]                  DATETIME        NOT NULL,
    [usuario_registra]                VARCHAR (50)    NOT NULL,
    [fecha_modifica]                  DATETIME        NULL,
    [usuario_modifica]                VARCHAR (50)    NULL,
    CONSTRAINT [PK_comision_precio_supervisor] PRIMARY KEY CLUSTERED ([codigo_comision] ASC)
);