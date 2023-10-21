IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[up_planilla_buscar_ultimo_cerrado]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[up_planilla_buscar_ultimo_cerrado]

GO
CREATE PROCEDURE [dbo].[up_planilla_buscar_ultimo_cerrado]
AS
BEGIN
	SET NOCOUNT ON

	SELECT TOP 1 
		codigo_planilla
		,nombre_planilla
		,estado_planilla
		,convert(varchar, fecha_inicio, 103) as fecha_inicio
		,convert(varchar, fecha_fin, 103) as fecha_fin
	FROM 
		dbo.vw_planilla 
	WHERE 
		codigo_estado_planilla = 2 
	ORDER BY 
		codigo_planilla DESC

	SET NOCOUNT OFF
END;