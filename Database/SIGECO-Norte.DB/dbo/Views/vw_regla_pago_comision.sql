USE SIGECO
GO

IF OBJECT_ID('dbo.vw_regla_pago_comision', 'V') IS NOT NULL 
	DROP VIEW dbo.vw_regla_pago_comision
GO

CREATE VIEW dbo.vw_regla_pago_comision
AS
	select rpc.codigo_regla_pago, rpc.nombre_regla_pago, e.nombre as empresa, c.nombre as camposanto, cg.nombre as canal, v.nombre as venta, p.nombre as pago
	, ta.nombre as tipo_articulo, case rpc.evaluar_plan_integral when 1 then 'Si' else 'No' end as evaluar_plan_integral, case rpc.evaluar_anexado when 1 then 'Si' else 'No' end as evaluar_anexado 
	, taa.nombre as tipo_articulo_anexado
	, case tipo_pago when 1 then '100%' when 2 then convert(varchar(3), rpc.valor_inicial_pago) + '% en CI - ' + convert(varchar(3), rpc.valor_cuota_pago) + '% en 1ra C'
	when 3 then convert(varchar(3), rpc.valor_inicial_pago) + '% de CI - ' + convert(varchar(3), rpc.valor_cuota_pago) + '% de Cuotas'
	when 4 then convert(varchar(5), rpc.valor_inicial_pago) + ' de CI - ' + convert(varchar(3), rpc.valor_cuota_pago) + '% de Cuotas' end as tipo_pago
	, convert(varchar, rpc.fecha_inicio, 103) + ' - ' +convert(varchar, rpc.fecha_fin, 103) as vigencia
	from regla_pago_comision rpc
	left join empresa_sigeco e on e.codigo_empresa = rpc.codigo_empresa
	left join campo_santo_sigeco c on c.codigo_campo_santo = rpc.codigo_campo_santo
	left join canal_grupo cg on cg.codigo_canal_grupo = rpc.codigo_canal_grupo
	left join tipo_venta v on v.codigo_tipo_venta = rpc.codigo_tipo_venta
	left join tipo_pago p on p.codigo_tipo_pago = rpc.codigo_tipo_pago
	left join tipo_articulo ta on ta.codigo_tipo_articulo = rpc.codigo_tipo_articulo
	left join tipo_articulo taa on taa.codigo_tipo_articulo = rpc.codigo_tipo_articulo_anexado