CREATE TABLE [dbo].[contrato_bloqueo] (
    [codigo_bloqueo]   INT           IDENTITY (1, 1) NOT NULL,
    [numatcard]        VARCHAR (100) NULL,
    [codigo_empresa]   VARCHAR (4)   NULL,
    [bloqueo]          BIT           NULL,
    [motivo]           VARCHAR (250) NULL,
    [usuario_registro] VARCHAR (50)  NULL,
    [fecha_registro]   DATETIME      NULL,
    [estado_registro]  BIT           NULL
);