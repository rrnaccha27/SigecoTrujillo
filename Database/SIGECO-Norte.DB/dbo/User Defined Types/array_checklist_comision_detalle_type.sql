USE SIGECO
GO

IF TYPE_ID('[dbo].[array_checklist_comision_detalle_type]') IS NOT NULL
	DROP TYPE [dbo.[array_checklist_comision_detalle_type];

GO

CREATE TYPE [dbo].[array_checklist_comision_detalle_type] AS TABLE (
    [codigo_checklist_detalle] INT NOT NULL);