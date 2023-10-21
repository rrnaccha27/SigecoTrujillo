CREATE TABLE [dbo].[motivo_traslado] (
    [codigo_motivo_traslado] INT          IDENTITY (1, 1) NOT NULL,
    [nombre_motivo_traslado] VARCHAR (30) NOT NULL,
    [estado_registro]        BIT          NOT NULL,
    [fecha_registra]         DATETIME     NOT NULL,
    [fecha_modifica]         DATETIME     NULL,
    [usuario_registra]       VARCHAR (50) NOT NULL,
    [usuario_modifica]       VARCHAR (50) NULL,
    CONSTRAINT [Motivo_Traslado_pk] PRIMARY KEY CLUSTERED ([codigo_motivo_traslado] ASC)
);