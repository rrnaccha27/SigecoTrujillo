﻿CREATE TABLE [dbo].[sigeco_reporte_bono_detalle_vendedor] (
    [codigo_planilla]           INT             NULL,
    [fecha_inicio]              VARCHAR (10)    NULL,
    [fecha_fin]                 VARCHAR (10)    NULL,
    [nombre_canal]              VARCHAR (250)   NULL,
    [codigo_estado_planilla]    INT             NULL,
    [codigo_personal]           INT             NULL,
    [apellidos_nombres]         VARCHAR (250)   NULL,
    [monto_contratado_personal] DECIMAL (12, 2) NULL,
    [monto_ingresado_personal]  DECIMAL (12, 2) NULL,
    [monto_bono_total_persona]  DECIMAL (12, 2) NULL,
    [codigo_empresa]            INT             NULL,
    [nombre_empresa]            VARCHAR (25)    NULL,
    [nombre_empresa_largo]      VARCHAR (250)   NULL,
    [monto_contratado_empresa]  DECIMAL (12, 2) NULL,
    [monto_ingresado_empresa]   DECIMAL (12, 2) NULL,
    [monto_bruto_empresa]       DECIMAL (12, 2) NULL,
    [monto_igv_empresa]         DECIMAL (12, 2) NULL,
    [detraccion_empresa]        DECIMAL (12, 2) NULL,
    [monto_neto_empresa]        DECIMAL (12, 2) NULL,
    [codigo_grupo]              INT             NULL,
    [nombre_grupo]              VARCHAR (250)   NULL,
    [numero_contrato]           VARCHAR (100)   NULL,
    [nombre_tipo_venta]         VARCHAR (25)    NULL,
    [monto_contratado]          DECIMAL (12, 2) NULL,
    [monto_ingresado]           DECIMAL (12, 2) NULL,
    [porcentaje_pago]           DECIMAL (12, 2) NULL,
    [importe_bono_detalle]      DECIMAL (12, 2) NULL,
    [fecha_contrato]            VARCHAR (10)    NULL,
    [tipo_pago]                 VARCHAR (25)    NULL,
    [num_ventas]                INT             NULL
);