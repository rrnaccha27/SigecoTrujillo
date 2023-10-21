CREATE PROCEDURE [dbo].[up_reporte_liquidacion_bono_supervisor_general_v2]
(
	 @p_codigo_planilla	INT
)
AS
BEGIN
	SET NOCOUNT ON

	IF EXISTS(SELECT codigo_planilla FROM sigeco_reporte_bono_detalle_supervisor WHERE codigo_planilla = @p_codigo_planilla)
	BEGIN
		SELECT 
			codigo_planilla,fecha_inicio,fecha_fin,nombre_canal,codigo_estado_planilla,codigo_supervisor,apellidos_nombres_supervisor,codigo_empresa
			,nombre_empresa,codigo_grupo,nombre_grupo,monto_bruto_empresa,monto_neto_empresa,monto_detraccion_empresa,monto_neto_bono_empresa,monto_ingresado_empresa
			,monto_igv_empresa,monto_neto_supervisor,monto_contratado_supervisor,monto_ingresado_supervisor,codigo_personal,apellidos_nombres_personal,numero_contrato
			,nombre_tipo_venta,monto_contratado,monto_ingresado,porcentaje_pago,monto_bono,fecha_contrato
		FROM 
			sigeco_reporte_bono_detalle_supervisor 
		WHERE 
			codigo_planilla = @p_codigo_planilla
		ORDER BY
			apellidos_nombres_supervisor
			,apellidos_nombres_personal
			,numero_contrato;

		SELECT 'CONGELADO'
	END
	ELSE
	BEGIN
		exec up_reporte_liquidacion_bono_supervisor_general @p_codigo_planilla
		select 'en vivo'
	END	
	SET NOCOUNT OFF
END;