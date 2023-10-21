CREATE VIEW [dbo].[vwPersonal_Cronograma]
AS
SELECT        dbo.personal.codigo_personal, dbo.personal.nombre AS Personal, dbo.empresa_sigeco.codigo_empresa, dbo.empresa_sigeco.nombre AS Empresa, dbo.cronograma_pago_comision.codigo_cronograma, 
                         dbo.cronograma_pago_comision.nro_contrato AS Contrato, dbo.detalle_cronograma.codigo_detalle, dbo.detalle_cronograma.codigo_articulo, dbo.detalle_cronograma.nro_cuota, 
                         dbo.detalle_cronograma.fecha_programada, dbo.detalle_cronograma.monto_bruto, dbo.detalle_cronograma.monto_neto, dbo.detalle_cronograma.codigo_tipo_cuota, 
                         dbo.detalle_cronograma.codigo_estado_cuota, dbo.detalle_planilla.codigo_detalle_planilla, dbo.detalle_planilla.excluido, dbo.detalle_planilla.fecha_pago, dbo.detalle_planilla.observacion, 
                         dbo.planilla.codigo_planilla, dbo.planilla.numero_planilla, dbo.planilla.codigo_estado_planilla
FROM            dbo.personal_canal_grupo INNER JOIN
                         dbo.personal ON dbo.personal_canal_grupo.codigo_personal = dbo.personal.codigo_personal INNER JOIN
                         dbo.empresa_sigeco INNER JOIN
                         dbo.cronograma_pago_comision INNER JOIN
                         dbo.detalle_cronograma ON dbo.cronograma_pago_comision.codigo_cronograma = dbo.detalle_cronograma.codigo_cronograma ON 
                         dbo.empresa_sigeco.codigo_empresa = dbo.cronograma_pago_comision.codigo_empresa ON 
                         dbo.personal_canal_grupo.codigo_registro = dbo.cronograma_pago_comision.codigo_personal_canal_grupo LEFT OUTER JOIN
                         dbo.planilla INNER JOIN
                         dbo.detalle_planilla ON dbo.planilla.codigo_planilla = dbo.detalle_planilla.codigo_planilla ON dbo.detalle_cronograma.codigo_detalle = dbo.detalle_planilla.codigo_detalle_cronograma