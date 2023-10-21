CREATE VIEW dbo.pcc_regla_pago_comision
AS
	SELECT 
		codigo_regla_pago,
		codigo_empresa,
		codigo_canal_grupo,
		codigo_tipo_venta,
		codigo_tipo_pago,
		codigo_articulo,
		codigo_tipo_articulo,
		evaluar_plan_integral,
		evaluar_anexado,
		codigo_tipo_articulo_anexado,
		codigo_campo_santo,
		tipo_pago AS tipo_pago_comision,
		ROUND(CONVERT(DECIMAL(12, 4), valor_inicial_pago), 4) as valor_inicial_pago,
		ROUND(CONVERT(DECIMAL(12, 4), valor_cuota_pago), 4) as valor_cuota_pago,
		fecha_inicio,
		fecha_inicio AS vigencia_inicio,
		CONVERT(DATETIME, CONVERT(VARCHAR(8), fecha_fin, 112) + ' 23:59:59') AS vigencia_fin,
		orden
	FROM
		dbo.regla_pago_comision
	WHERE
		estado_registro = 1