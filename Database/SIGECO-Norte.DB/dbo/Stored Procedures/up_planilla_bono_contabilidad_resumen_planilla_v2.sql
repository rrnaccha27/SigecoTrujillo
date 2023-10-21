CREATE PROCEDURE [dbo].[up_planilla_bono_contabilidad_resumen_planilla_v2]
(
	 @p_codigo_planilla	INT
)
AS
BEGIN
	SET NOCOUNT ON

	IF EXISTS(SELECT codigo_planilla FROM dbo.sigeco_reporte_bono_contabilidad_resumen WHERE codigo_planilla = @p_codigo_planilla)
	BEGIN
		SELECT 
			codigo_planilla,codigo_empresa,nombre_empresa,bonos
		FROM 
			dbo.sigeco_reporte_bono_contabilidad_resumen
		WHERE 
			codigo_planilla = @p_codigo_planilla
		ORDER BY 
			codigo_empresa

		SELECT 'CONGELADO'
	END
	ELSE
	BEGIN
		EXEC up_planilla_bono_contabilidad_resumen_planilla @p_codigo_planilla
		SELECT 'en vivo'
	END	
	SET NOCOUNT OFF
END;