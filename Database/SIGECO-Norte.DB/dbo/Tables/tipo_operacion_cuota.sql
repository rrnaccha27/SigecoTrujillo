CREATE TABLE [dbo].[tipo_operacion_cuota] (
    [codigo_tipo_operacion_cuota] INT           IDENTITY (1, 1) NOT NULL,
    [nombre]                      VARCHAR (200) NOT NULL,
    [estado_registro]             BIT           NOT NULL,
    [fecha_registra]              DATETIME      NOT NULL,
    [usuario_registra]            NVARCHAR (20) NOT NULL,
    CONSTRAINT [PK_tipo_operacion_cuota] PRIMARY KEY CLUSTERED ([codigo_tipo_operacion_cuota] ASC)
);