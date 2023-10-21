CREATE PROCEDURE [dbo].[up_planilla_bono_jn_resumen_v2]
(
	 @p_codigo_planilla	INT
)
AS
BEGIN
	SET NOCOUNT ON

	IF EXISTS(SELECT codigo_planilla FROM dbo.sigeco_reporte_bono_jn_resumen WHERE codigo_planilla = @p_codigo_planilla)
	BEGIN
		SELECT 
			codigo_planilla,nombre_canal,FUNJAR,B_FUNJAR,OFSA,B_OFSA,TOTAL,B_TOTAL
		FROM 
			dbo.sigeco_reporte_bono_jn_resumen
		WHERE 
			codigo_planilla = @p_codigo_planilla
		ORDER BY 
			nombre_canal

		SELECT 'CONGELADO'
	END
	ELSE
	BEGIN
		EXEC up_planilla_bono_jn_resumen @p_codigo_planilla
		SELECT 'en vivo'
	END	
	SET NOCOUNT OFF
END;