CREATE VIEW dbo.pcc_regla_pago_comision_excepcion
AS
	SELECT 
		codigo_campo_santo,
		codigo_empresa,
		codigo_canal_grupo,
		codigo_articulo,
		cuotas,
		valor_promocion, 
		vigencia_inicio,
		convert(datetime, convert(varchar(8), vigencia_fin, 112) + ' 23:59:59') AS vigencia_fin
	FROM 
		dbo.regla_pago_comision_excepcion
	WHERE
		estado_registro = 1