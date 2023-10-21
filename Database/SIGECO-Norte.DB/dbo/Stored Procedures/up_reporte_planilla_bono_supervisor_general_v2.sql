CREATE PROCEDURE [dbo].[up_reporte_planilla_bono_supervisor_general_v2]
(
	 @p_codigo_planilla	INT
)
AS
BEGIN
	SET NOCOUNT ON

	IF EXISTS(SELECT codigo_planilla FROM sigeco_reporte_bono_resumen WHERE codigo_planilla = @p_codigo_planilla)
	BEGIN
		SELECT 
			codigo_planilla,fecha_inicio,fecha_fin,nombre_canal,codigo_estado_planilla,codigo_grupo,nombre_grupo,codigo_personal,nombres_apellidos
			,codigo_empresa,nombre_empresa,monto_bruto_empresa,monto_igv_empresa,monto_neto_empresa,monto_ingresado_empresa,detraccion_empresa
			,monto_neto_bono_empresa,monto_bruto_grupo,monto_igv_grupo,monto_neto_grupo,monto_contrato_grupo,monto_ingresado_grupo,meta_logrado
			,porcentaje_pago
		FROM 
			sigeco_reporte_bono_resumen 
		WHERE 
			codigo_planilla = @p_codigo_planilla
		ORDER BY
			nombre_grupo, nombres_apellidos

		SELECT 'CONGELADO'
	END
	ELSE
	BEGIN
		exec up_reporte_planilla_bono_supervisor_general @p_codigo_planilla
		select 'en vivo'
	END	
	SET NOCOUNT OFF
END