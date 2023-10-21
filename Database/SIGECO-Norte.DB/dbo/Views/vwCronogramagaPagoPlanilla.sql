CREATE VIEW [dbo].[vwCronogramagaPagoPlanilla]
AS
	SELECT
		n1.codigo_cronograma, n1.codigo_empresa, n5.codigo_personal, n1.codigo_personal_canal_grupo, n1.nro_contrato, n2.codigo_detalle, n2.codigo_articulo, n2.nro_cuota, n2.fecha_programada, n2.monto_bruto, n2.monto_neto,
		n2.codigo_tipo_cuota, n2.codigo_estado_cuota, n3.codigo_detalle_planilla, n3.excluido, n3.fecha_pago, n3.observacion, n4.codigo_planilla, n4.numero_planilla, n4.codigo_estado_planilla, n3.codigo_canal, 
		n3.codigo_grupo, n3.codigo_moneda, n3.codigo_tipo_venta, n3.codigo_tipo_pago,n4.estado_registro
	FROM
		dbo.cronograma_pago_comision AS n1 INNER JOIN
			dbo.detalle_cronograma AS n2 ON n1.codigo_cronograma = n2.codigo_cronograma INNER JOIN
			dbo.detalle_planilla AS n3 ON n2.codigo_detalle = n3.codigo_detalle_cronograma INNER JOIN
			dbo.planilla AS n4 ON n3.codigo_planilla = n4.codigo_planilla INNER JOIN
			dbo.personal_canal_grupo AS n5 ON n1.codigo_personal_canal_grupo = n5.codigo_registro
	WHERE
		n4.codigo_estado_planilla <> 3