﻿CREATE TABLE [dbo].[Supervisor$] (
    [codigo_sku]          NVARCHAR (255) NULL,
    [articulo]            NVARCHAR (255) NULL,
    [codigo_precio]       FLOAT (53)     NULL,
    [tipo_venta]          NVARCHAR (255) NULL,
    [precio]              FLOAT (53)     NULL,
    [nuevo_precio]        FLOAT (53)     NULL,
    [pre_vig_ini]         DATETIME       NULL,
    [pre_vig_fin]         DATETIME       NULL,
    [codigo_comision]     FLOAT (53)     NULL,
    [canal]               NVARCHAR (255) NULL,
    [pago]                NVARCHAR (255) NULL,
    [comision]            NVARCHAR (255) NULL,
    [valor]               FLOAT (53)     NULL,
    [nuevo_valor]         FLOAT (53)     NULL,
    [com_vig_ini]         DATETIME       NULL,
    [com_vig_fin]         DATETIME       NULL,
    [nuevo_codigo_precio] INT            NULL
);