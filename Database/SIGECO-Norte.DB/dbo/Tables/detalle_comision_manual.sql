CREATE TABLE [dbo].[detalle_comision_manual] (
    [id]                     UNIQUEIDENTIFIER NOT NULL,
    [codigo_comision_manual] INT              NOT NULL,
    [codigo_articulo]        INT              NOT NULL,
    [comision]               DECIMAL (12, 4)  NOT NULL
);