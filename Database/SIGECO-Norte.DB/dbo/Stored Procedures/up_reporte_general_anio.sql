CREATE PROCEDURE dbo.up_reporte_general_anio
AS
BEGIN
	SELECT DISTINCT 
		YEAR(fecha_fin) AS [id]
		,YEAR(fecha_fin) AS [text] 
	FROM dbo.planilla 
	ORDER BY YEAR(fecha_fin) DESC
END;