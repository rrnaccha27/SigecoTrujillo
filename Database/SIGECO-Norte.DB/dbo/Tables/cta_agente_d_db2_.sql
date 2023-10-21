﻿CREATE TABLE [dbo].[cta_agente_d_db2_] (
    [COD_COMISION]       INT             NOT NULL,
    [N_CUOTA]            SMALLINT        NOT NULL,
    [COD_EMPRESA_G]      CHAR (4)        NULL,
    [NUM_CONTRATO]       VARCHAR (10)    NULL,
    [TIPO_VENTA]         VARCHAR (1)     NULL,
    [ID_FORMA_DE_PAGO]   VARCHAR (1)     NULL,
    [TIPO_COMISION]      VARCHAR (4)     NULL,
    [TIPO_AGENTE_G]      VARCHAR (4)     NULL,
    [C_AGENTE]           VARCHAR (10)    NULL,
    [COD_GRUPO_VENTA_G]  VARCHAR (4)     NULL,
    [COD_BIEN]           VARCHAR (6)     NULL,
    [COD_CONCEPTO]       VARCHAR (3)     NULL,
    [IMP_PAGAR]          DECIMAL (14, 2) NULL,
    [FEC_HAVILITADO]     DATETIME        NULL,
    [IGV]                DECIMAL (14, 2) NULL,
    [CUARTA]             DECIMAL (14, 2) NULL,
    [IES]                DECIMAL (14, 2) NULL,
    [TIPO_MONEDA]        CHAR (1)        NULL,
    [TIPO_CAMBIO]        DECIMAL (10, 3) NULL,
    [SALDO_A_PAGAR]      DECIMAL (14, 2) NULL,
    [FEC_PLANILLA]       DATETIME        NULL,
    [USU_PLANILLA]       VARCHAR (10)    NULL,
    [N_OPERACION]        INT             NULL,
    [FEC_ANULACION]      DATETIME        NULL,
    [OBSERV_ANULACION]   VARCHAR (200)   NULL,
    [COD_ADENDO]         VARCHAR (10)    NULL,
    [FLAG_SUPERVISOR]    CHAR (1)        NULL,
    [COD_ANULACION]      CHAR (1)        NULL,
    [NOTA]               VARCHAR (255)   NULL,
    [CANTIDAD_CONT]      INT             NULL,
    [FLAG_ANTIGUO]       CHAR (1)        NULL,
    [NUM_ANTIGUO]        VARCHAR (10)    NULL,
    [COD_DOC]            VARCHAR (2)     NULL,
    [NUM_DOC]            VARCHAR (17)    NULL,
    [ID_INDIVIDUAL]      VARCHAR (1)     NULL,
    [DSCTO_ESTUDIO_C]    DECIMAL (14, 2) NULL,
    [DSCTO_DETRACCION]   DECIMAL (14, 2) NULL,
    [FLAG_SUSPENDIDA]    CHAR (1)        NULL,
    [C_ADICIONAL]        VARCHAR (11)    NULL,
    [D_NIVEL]            VARCHAR (30)    NULL,
    [FUNES_VIGENTE]      VARCHAR (1)     NULL,
    [TIPO_REGISTRO]      VARCHAR (20)    NULL,
    [FECHA_REGISTRO]     DATETIME        NULL,
    [ELECC_ESPACIO_COMI] VARCHAR (1)     NULL
);
GO
CREATE NONCLUSTERED INDEX [_dta_index_cta_agente_d_db2_22_1063010868__K27_K4_K3_K11_K12_13_15]
    ON [dbo].[cta_agente_d_db2_]([FLAG_SUPERVISOR] ASC, [NUM_CONTRATO] ASC, [COD_EMPRESA_G] ASC, [COD_BIEN] ASC, [COD_CONCEPTO] ASC)
    INCLUDE([IMP_PAGAR], [IGV]);
GO
CREATE NONCLUSTERED INDEX [_dta_index_cta_agente_d_db2_22_1063010868__K4_K3_K2_21]
    ON [dbo].[cta_agente_d_db2_]([NUM_CONTRATO] ASC, [COD_EMPRESA_G] ASC, [N_CUOTA] ASC)
    INCLUDE([FEC_PLANILLA]);