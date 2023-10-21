CREATE TABLE [dbo].[detalle_exhumacion] (
    [codigo_detalle_exhumacion]      INT            IDENTITY (1, 1) NOT NULL,
    [descripcion_proceso_exhumacion] NVARCHAR (200) NULL,
    [estado_registro]                BIT            DEFAULT ((1)) NOT NULL,
    [fecha_registra]                 DATETIME       DEFAULT (getdate()) NOT NULL,
    [fecha_modifica]                 DATETIME       NULL,
    [usuario_registra]               VARCHAR (20)   NOT NULL,
    [usuario_modifica]               VARCHAR (20)   NULL,
    [codigo_exhumacion]              INT            NOT NULL,
    [codigo_estado_exhumacion]       INT            NOT NULL,
    PRIMARY KEY CLUSTERED ([codigo_detalle_exhumacion] ASC),
    CONSTRAINT [detalle_exhumacion_estado_exhumacion_fk] FOREIGN KEY ([codigo_estado_exhumacion]) REFERENCES [dbo].[estado_exhumacion] ([codigo_estado_exhumacion]),
    CONSTRAINT [detalle_exhumacion_exhumacion_fallecido_fk] FOREIGN KEY ([codigo_exhumacion]) REFERENCES [dbo].[exhumacion_fallecido] ([codigo_exhumacion])
);