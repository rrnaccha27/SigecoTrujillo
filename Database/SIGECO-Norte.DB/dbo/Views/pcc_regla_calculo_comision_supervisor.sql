CREATE VIEW dbo.pcc_regla_calculo_comision_supervisor
AS
	SELECT 
		codigo_campo_santo,
		codigo_empresa,
		codigo_canal_grupo,
		tipo_supervisor,
		ROUND(valor_pago/100, 4) AS valor_pago,
		incluye_igv,
		vigencia_inicio,
		CONVERT(DATETIME, CONVERT(VARCHAR(8), vigencia_fin, 112) + ' 23:59:59') AS vigencia_fin
	FROM
		dbo.regla_calculo_comision_supervisor
	WHERE
		estado_registro = 1