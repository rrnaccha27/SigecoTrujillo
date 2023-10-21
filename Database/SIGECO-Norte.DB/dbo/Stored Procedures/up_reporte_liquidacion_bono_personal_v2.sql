CREATE PROCEDURE [dbo].[up_reporte_liquidacion_bono_personal_v2]
(
	 @p_codigo_planilla	INT
)
AS
BEGIN
	SET NOCOUNT ON

	IF EXISTS(SELECT codigo_planilla FROM sigeco_reporte_bono_detalle_vendedor WHERE codigo_planilla = @p_codigo_planilla)
	BEGIN
		SELECT 
			codigo_planilla,fecha_inicio,fecha_fin,nombre_canal,codigo_estado_planilla,codigo_personal,apellidos_nombres,monto_contratado_personal
			,monto_ingresado_personal,monto_bono_total_persona,codigo_empresa,nombre_empresa,nombre_empresa_largo,monto_contratado_empresa,monto_ingresado_empresa
			,monto_bruto_empresa,monto_igv_empresa,detraccion_empresa,monto_neto_empresa,codigo_grupo,nombre_grupo,numero_contrato,nombre_tipo_venta,monto_contratado
			,monto_ingresado,porcentaje_pago,importe_bono_detalle,fecha_contrato,tipo_pago,num_ventas
		FROM 
			sigeco_reporte_bono_detalle_vendedor 
		WHERE 
			codigo_planilla = @p_codigo_planilla
		ORDER BY
			nombre_grupo asc
			,apellidos_nombres
			,numero_contrato ;

		SELECT 'CONGELADO'
	END
	ELSE
	BEGIN
		exec up_reporte_liquidacion_bono_personal @p_codigo_planilla
		select 'en vivo'
	END	
	SET NOCOUNT OFF
END