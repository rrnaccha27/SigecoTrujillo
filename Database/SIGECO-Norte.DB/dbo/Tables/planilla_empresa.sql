CREATE TABLE [dbo].[planilla_empresa] (
    [codigo_planilla_empresa] INT IDENTITY (1, 1) NOT NULL,
    [codigo_empresa]          INT NOT NULL,
    [codigo_planilla]         INT NOT NULL,
    [estado_registro]         BIT DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_planilla_empresa] PRIMARY KEY CLUSTERED ([codigo_planilla_empresa] ASC)
);