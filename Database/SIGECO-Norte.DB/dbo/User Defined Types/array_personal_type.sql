USE SIGECO
GO

IF TYPE_ID('[dbo].[array_personal_type]') IS NOT NULL
	DROP TYPE [dbo.[array_personal_type];

GO

CREATE TYPE [dbo].[array_personal_type] AS TABLE (
    [codigo_personal] INT NOT NULL);