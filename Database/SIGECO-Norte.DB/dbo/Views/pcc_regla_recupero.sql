CREATE VIEW dbo.pcc_regla_recupero
AS
	SELECT 
		codigo_regla_recupero,
		nro_cuota,
		vigencia_inicio,
		convert(datetime, convert(varchar(8), vigencia_fin, 112) + ' 23:59:59') AS vigencia_fin
	FROM 
		dbo.regla_recupero
	WHERE
		estado_registro = 1