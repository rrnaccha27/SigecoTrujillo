CREATE VIEW pcb_regla_calculo_bono
AS
	SELECT
		codigo_regla_calculo_bono,
		codigo_tipo_planilla,
		codigo_canal,
		codigo_grupo,
		monto_meta,
		ROUND(porcentaje_pago / 100, 4) as porcentaje_pago,
		monto_tope,
		cantidad_ventas,
		vigencia_inicio,
		convert(datetime, convert(varchar(8), vigencia_fin, 112) + ' 23:59:59') AS vigencia_fin
		,calcular_igv
		,es_jn
	FROM 
		dbo.regla_calculo_bono
	WHERE
		estado_registro = 1