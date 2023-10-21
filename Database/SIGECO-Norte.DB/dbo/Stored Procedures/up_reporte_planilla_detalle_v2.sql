CREATE PROCEDURE [dbo].[up_reporte_planilla_detalle_v2]
(
	@p_codigo_planilla	int
	,@p_codigo_personal	int
)
AS
BEGIN

	SET NOCOUNT ON

	IF EXISTS(SELECT codigo_planilla FROM sigeco_reporte_comision_planilla WHERE codigo_planilla = @p_codigo_planilla)
	BEGIN
		--select 'del congelado'
		IF ISNULL(@p_codigo_personal, 0) = 0
			SELECT
				codigo_tipo_planilla, nombre_tipo_planilla, numero_planilla, codigo_estado_planilla, fecha_inicio, fecha_fin, codigo_moneda, moneda, articulo, nro_contrato, nro_cuota, tipo_venta, 
				tipo_pago, codigo_empresa, empresa, codigo_canal, canal, codigo_grupo, grupo, codigo_personal, personal, personal_referencial, monto_bruto, igv, monto_neto, 
				monto_bruto_empresa, igv_empresa, monto_neto_empresa, monto_bruto_canal, igv_canal, monto_neto_canal, monto_bruto_grupo, igv_grupo, monto_neto_grupo, 
				monto_bruto_personal, igv_personal, monto_neto_personal, monto_descuento, monto_neto_personal_con_descuento, monto_detraccion_personal, monto_neto_pagar_personal, 
				es_comision_manual, fecha_contrato, usuario_cm, tipo_reporte
			FROM sigeco_reporte_comision_planilla 
			WHERE 
				codigo_planilla = @p_codigo_planilla
			ORDER BY 
				codigo_empresa, nro_contrato, articulo, nro_cuota;
		ELSE
			SELECT
				codigo_tipo_planilla, nombre_tipo_planilla, numero_planilla, codigo_estado_planilla, fecha_inicio, fecha_fin, codigo_moneda, moneda, articulo, nro_contrato, nro_cuota, tipo_venta, 
				tipo_pago, codigo_empresa, empresa, codigo_canal, canal, codigo_grupo, grupo, codigo_personal, personal, personal_referencial, monto_bruto, igv, monto_neto, 
				case when row_number() over(partition by codigo_empresa, codigo_moneda order by codigo_empresa )=1    
				then 
					sum(monto_bruto)over(partition by codigo_empresa, codigo_moneda) 
				else null end monto_bruto_empresa,

				case when row_number() over(partition by codigo_empresa, codigo_moneda order by codigo_empresa )=1    
				then 
					sum(igv)over(partition by codigo_empresa, codigo_moneda)   
				else null end igv_empresa,

				case when row_number() over(partition by codigo_empresa, codigo_moneda order by codigo_empresa )=1    
				then 
					sum(monto_neto)over(partition by codigo_empresa, codigo_moneda)
				else null end monto_neto_empresa,
				--------------------------------------------------------------------------------------------------------------------------
				case when row_number() over(partition by codigo_empresa,codigo_canal, codigo_moneda order by codigo_canal )=1    
				then 
					sum(monto_bruto)over(partition by codigo_empresa,codigo_canal, codigo_moneda) 
				else null end monto_bruto_canal,
  
				case when row_number() over(partition by codigo_empresa,codigo_canal, codigo_moneda order by codigo_canal )=1    
				then 
					sum(igv)over(partition by codigo_empresa,codigo_canal, codigo_moneda)   
				else null end igv_canal,

				case when row_number() over(partition by codigo_empresa,codigo_canal, codigo_moneda order by codigo_canal )=1    
				then 
					sum(monto_neto)over(partition by codigo_empresa,codigo_canal, codigo_moneda)
				else null end monto_neto_canal,
				--------------------------------------------------------------------------------------------------------------------------
				case when row_number() over(partition by codigo_empresa,codigo_canal,codigo_grupo, codigo_moneda order by codigo_grupo )=1    
				then 
					sum(monto_bruto)over(partition by codigo_empresa,codigo_canal,codigo_grupo, codigo_moneda) 
				else null end monto_bruto_grupo,
  
				case when row_number() over(partition by codigo_empresa,codigo_canal,codigo_grupo, codigo_moneda order by codigo_grupo )=1    
				then 
					sum(igv)over(partition by codigo_empresa,codigo_canal,codigo_grupo, codigo_moneda)   
				else null end igv_grupo,

				case when row_number() over(partition by codigo_empresa,codigo_canal,codigo_grupo, codigo_moneda order by codigo_grupo )=1    
				then 
					sum(monto_neto)over(partition by codigo_empresa,codigo_canal,codigo_grupo, codigo_moneda)
				else null end monto_neto_grupo,
				monto_bruto_personal, igv_personal, monto_neto_personal, monto_descuento, monto_neto_personal_con_descuento, monto_detraccion_personal, monto_neto_pagar_personal, 
				es_comision_manual, fecha_contrato, usuario_cm, tipo_reporte
			FROM sigeco_reporte_comision_planilla 
			WHERE 
				codigo_personal = @p_codigo_personal
				AND codigo_planilla = @p_codigo_planilla
			ORDER BY 
				codigo_empresa, nro_contrato, articulo, nro_cuota;
	END
	ELSE
	BEGIN
		--select 'en vivo'
		EXEC up_reporte_planilla_detalle @p_codigo_planilla, @p_codigo_personal
	END

	SET NOCOUNT OFF
END;