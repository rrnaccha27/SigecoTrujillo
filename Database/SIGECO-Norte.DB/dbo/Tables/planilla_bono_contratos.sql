CREATE TABLE [dbo].[planilla_bono_contratos] (
    [id]                UNIQUEIDENTIFIER NOT NULL,
    [codigo_empresa_o]  VARCHAR (4)      NULL,
    [codigo_empresa]    INT              NOT NULL,
    [nro_contrato]      VARCHAR (100)    NOT NULL,
    [codigo_camposanto] INT              NULL,
    [codigo_tipo_venta] INT              NOT NULL,
    [codigo_canal]      INT              DEFAULT ((0)) NULL
);