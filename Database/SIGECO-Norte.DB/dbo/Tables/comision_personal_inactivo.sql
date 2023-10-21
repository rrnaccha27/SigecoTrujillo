CREATE TABLE [dbo].[comision_personal_inactivo] (
    [codigo_detalle_cronograma] INT          NOT NULL,
    [liquidado]                 BIT          NOT NULL,
    [fecha_registra]            DATETIME     NOT NULL,
    [usuario_registra]          VARCHAR (50) NOT NULL,
    [fecha_modifica]            DATETIME     NULL,
    [usuario_modifica]          VARCHAR (50) NULL,
    CONSTRAINT [PK_comision_personal_inactivo] PRIMARY KEY CLUSTERED ([codigo_detalle_cronograma] ASC)
);