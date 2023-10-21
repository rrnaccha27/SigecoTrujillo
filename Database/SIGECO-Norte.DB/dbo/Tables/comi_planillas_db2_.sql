CREATE TABLE [dbo].[comi_planillas_db2_] (
    [FEC_PLANILLA] DATE            NOT NULL,
    [USU_PLANILLA] VARCHAR (10)    NOT NULL,
    [FEC_INICIO]   DATE            NULL,
    [FEC_TERMINO]  DATE            NULL,
    [FEC_CIERRE]   DATE            NULL,
    [TIPO_CAMBIO]  DECIMAL (10, 3) NULL,
    [TIPO_MONEDA]  CHAR (1)        NULL
);