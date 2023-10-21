CREATE TABLE [dbo].[log_wcf_sap] (
    [objeto]              VARCHAR (50)  NULL,
    [codigo_sigeco]       INT           NULL,
    [codigo_equivalencia] VARCHAR (50)  NULL,
    [tipo_operacion]      CHAR (1)      NULL,
    [mensaje_excepcion]   VARCHAR (MAX) NULL,
    [fecha_registro]      DATETIME      DEFAULT (getdate()) NULL,
    [usuario_registro]    VARCHAR (50)  NULL
);