USE SIGECO
GO

IF TYPE_ID('[dbo].[array_detalle_cronograma_type]') IS NOT NULL
	DROP TYPE [dbo.[array_detalle_cronograma_type];

GO

CREATE TYPE [dbo].[array_detalle_cronograma_type] AS TABLE (
    [codigo_detalle_cronograma] INT NOT NULL);