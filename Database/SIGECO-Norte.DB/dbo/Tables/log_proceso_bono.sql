CREATE TABLE [dbo].[log_proceso_bono] (
    [codigo_planilla]           INT           NULL,
    [codigo_tipo_planilla]      INT           NULL,
    [nro_contrato]              VARCHAR (100) NULL,
    [codigo_empresa]            INT           NULL,
    [codigo_canal]              INT           NULL,
    [codigo_estado]             INT           NULL,
    [observacion]               VARCHAR (200) NULL,
    [codigo_grupo]              INT           NULL,
    [codigo_regla_calculo_bono] INT           NULL
);