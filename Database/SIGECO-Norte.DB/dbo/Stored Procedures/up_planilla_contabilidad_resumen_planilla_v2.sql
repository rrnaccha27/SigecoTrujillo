CREATE PROCEDURE [dbo].[up_planilla_contabilidad_resumen_planilla_v2]
(
	@p_codigo_planilla	INT
)
AS
BEGIN
	SET NOCOUNT ON

	IF EXISTS(SELECT codigo_planilla FROM sigeco_reporte_comision_resumen_contabilidad WHERE codigo_planilla = @p_codigo_planilla)
	BEGIN
		SELECT
			codigo_planilla,codigo_empresa,nombre_empresa,comisiones
		FROM 
			sigeco_reporte_comision_resumen_contabilidad 
		WHERE 
			codigo_planilla = @p_codigo_planilla
		ORDER BY 
			codigo_empresa
		select 'congelado'
	END
	ELSE
	BEGIN
		EXEC up_planilla_contabilidad_resumen_planilla @p_codigo_planilla
		select 'en vivo'
	END

	SET NOCOUNT OFF
END;