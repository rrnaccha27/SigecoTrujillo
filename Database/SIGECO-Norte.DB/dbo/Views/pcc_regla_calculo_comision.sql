CREATE VIEW dbo.pcc_regla_calculo_comision
AS
	SELECT 
		codigo_precio,
		codigo_canal,
		codigo_tipo_pago,
		codigo_tipo_comision,
		valor, 
		vigencia_inicio,
		convert(datetime, convert(varchar(8), vigencia_fin, 112) + ' 23:59:59') AS vigencia_fin
	FROM 
		dbo.regla_calculo_comision
	WHERE
		estado_registro = 1