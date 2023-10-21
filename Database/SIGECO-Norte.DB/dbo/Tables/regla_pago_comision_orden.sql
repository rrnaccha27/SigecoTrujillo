CREATE TABLE [dbo].[regla_pago_comision_orden] (
    [codigo_orden]     INT          NOT NULL,
    [nombre]           VARCHAR (20) NOT NULL,
    [orden]            INT          NOT NULL,
    [nombre_campo]     VARCHAR (50) NULL,
    [fecha_registra]   DATETIME     NOT NULL,
    [usuario_registra] VARCHAR (50) NOT NULL,
    CONSTRAINT [PK_regla_pago_comision_orden] PRIMARY KEY CLUSTERED ([codigo_orden] ASC)
);