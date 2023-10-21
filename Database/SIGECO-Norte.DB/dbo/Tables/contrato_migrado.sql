CREATE TABLE [dbo].[contrato_migrado] (
    [Id]                    INT            IDENTITY (1, 1) NOT NULL,
    [Codigo_empresa]        NVARCHAR (4)   NOT NULL,
    [NumAtCard]             NVARCHAR (100) NOT NULL,
    [Fec_Creacion]          DATETIME       CONSTRAINT [DF__contrato___Fec_C__603D47BB] DEFAULT (getdate()) NULL,
    [Fec_Actualizacion]     DATETIME       NULL,
    [Fec_Proceso]           DATETIME       NULL,
    [codigo_estado_proceso] INT            NOT NULL,
    [Observacion]           VARCHAR (200)  NULL,
    [codigo_usuario]        VARCHAR (50)   NULL,
    [DocEntry]              INT            NULL,
    [bloqueo]               BIT            NULL,
    [codigo_bloqueo]        INT            NULL,
    CONSTRAINT [PK_contrato_migrado] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_contrato_migrado_codigo_estado_proceso_estado_proceso_codigo_estado_proceso] FOREIGN KEY ([codigo_estado_proceso]) REFERENCES [dbo].[estado_proceso] ([codigo_estado_proceso])
);