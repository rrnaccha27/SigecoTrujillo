CREATE TABLE [dbo].[regla_calculo_comision_supervisor] (
    [codigo_regla]       INT             IDENTITY (1, 1) NOT NULL,
    [nombre]             VARCHAR (50)    NOT NULL,
    [codigo_campo_santo] INT             NOT NULL,
    [codigo_empresa]     INT             NOT NULL,
    [codigo_canal_grupo] INT             NOT NULL,
    [tipo_supervisor]    INT             NOT NULL,
    [valor_pago]         DECIMAL (10, 2) NOT NULL,
    [incluye_igv]        BIT             NOT NULL,
    [vigencia_inicio]    DATETIME        NOT NULL,
    [vigencia_fin]       DATETIME        NOT NULL,
    [estado_registro]    BIT             NOT NULL,
    [fecha_registra]     DATETIME        NOT NULL,
    [usuario_registra]   VARCHAR (50)    NOT NULL,
    [fecha_modifica]     DATETIME        NULL,
    [usuario_modifica]   VARCHAR (50)    NULL,
    CONSTRAINT [PK_regla_calculo_comision_supervisor] PRIMARY KEY CLUSTERED ([codigo_regla] ASC)
);