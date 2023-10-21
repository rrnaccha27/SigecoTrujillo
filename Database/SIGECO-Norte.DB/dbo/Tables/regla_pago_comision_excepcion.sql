CREATE TABLE [dbo].[regla_pago_comision_excepcion] (
    [codigo_regla]       INT          IDENTITY (1, 1) NOT NULL,
    [codigo_campo_santo] INT          NOT NULL,
    [codigo_empresa]     INT          NOT NULL,
    [codigo_canal_grupo] INT          NOT NULL,
    [codigo_articulo]    INT          NOT NULL,
    [nombre]             VARCHAR (50) NOT NULL,
    [cuotas]             INT          NOT NULL,
    [vigencia_inicio]    DATETIME     NOT NULL,
    [vigencia_fin]       DATETIME     NOT NULL,
    [estado_registro]    BIT          NOT NULL,
    [fecha_registra]     DATETIME     NOT NULL,
    [fecha_modifica]     DATETIME     NULL,
    [usuario_registra]   VARCHAR (50) NOT NULL,
    [usuario_modifica]   VARCHAR (50) NULL,
    [valor_promocion]    INT          DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_regla_pago_comision_excepcion] PRIMARY KEY CLUSTERED ([codigo_regla] ASC),
    CONSTRAINT [FK_regla_pago_comision_excepcion_codigo_articulo_articulo_codigo_articulo] FOREIGN KEY ([codigo_articulo]) REFERENCES [dbo].[articulo] ([codigo_articulo])
);