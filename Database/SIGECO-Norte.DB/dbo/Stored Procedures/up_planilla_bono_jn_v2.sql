CREATE PROCEDURE [dbo].[up_planilla_bono_jn_v2]
(
	 @p_codigo_planilla	INT
)
AS
BEGIN
	SET NOCOUNT ON

	IF EXISTS(SELECT codigo_planilla FROM dbo.sigeco_reporte_bono_jn WHERE codigo_planilla = @p_codigo_planilla)
	BEGIN
		SELECT 
			codigo_planilla,numero_planilla,codigo_tipo_planilla,codigo_estado_planilla,fecha_inicio,fecha_fin
			,fecha_registra,dinero_ingresado,bono,porcentaje,meta_100,meta_90
		FROM 
			dbo.sigeco_reporte_bono_jn
		WHERE 
			codigo_planilla = @p_codigo_planilla

		SELECT 'CONGELADO'
	END
	ELSE
	BEGIN
		EXEC up_planilla_bono_jn @p_codigo_planilla
		SELECT 'en vivo'
	END	
	SET NOCOUNT OFF
END;