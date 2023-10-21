﻿CREATE PROCEDURE [dbo].[up_detalle_planilla_by_id]
(
	@codigo_planilla int,
	@codigo_tipo_venta int,
	@codigo_personal int
)
AS
BEGIN
	SET NOCOUNT ON

	set @codigo_tipo_venta=case when @codigo_tipo_venta=0 then null else @codigo_tipo_venta end;
	set @codigo_personal=case when @codigo_personal=0 then null else @codigo_personal end;

	declare 
		@v_codigo_estado_cuota_pagada int;
	
	set @v_codigo_estado_cuota_pagada=3;

	select 
		dp.codigo_cronograma,
		dp.codigo_planilla,
		dp.observacion,
		dp.codigo_detalle_planilla,
		dp.codigo_detalle_cronograma,
		a.nombre as nombre_articulo,
		dp.fecha_pago,
		dp.nro_cuota,
		dp.monto_bruto,
		dp.igv,
		dp.monto_neto,
		dp.nro_contrato,
		tv.codigo_tipo_venta,
		tv.nombre as nombre_tipo_venta,
		tp.codigo_tipo_pago,
		tp.nombre as nombre_tipo_pago,
		p.apellido_paterno,
		p.apellido_materno,
		p.nombre as nombre_persona,
		isnull(cg.nombre,' ') as nombre_grupo_canal,
		c.nombre as nombre_canal,
		emp.codigo_empresa,
		emp.nombre as nombre_empresa,
		(case when dc.es_registro_manual_comision=0 then dc.codigo_estado_cuota else @v_codigo_estado_cuota_pagada end) as codigo_estado_cuota,
		(case when dc.es_registro_manual_comision=0 then ec.nombre else 'Pagada' end) as nombre_estado_cuota,
		dc.estado_registro,
		dc.es_registro_manual_comision,
		CONVERT(INT, dp.es_transferencia) AS es_transferencia
	from 
		detalle_planilla dp 
	inner join empresa_sigeco emp on emp.codigo_empresa=dp.codigo_empresa
	inner join tipo_venta tv on tv.codigo_tipo_venta=dp.codigo_tipo_venta
	inner join tipo_pago tp on tp.codigo_tipo_pago=dp.codigo_tipo_pago
	inner join articulo a on dp.codigo_articulo=a.codigo_articulo
	inner join personal p on p.codigo_personal=dp.codigo_personal
	inner join canal_grupo c on c.codigo_canal_grupo=dp.codigo_canal
	left join detalle_cronograma dc on dp.codigo_detalle_cronograma=dc.codigo_detalle
	left join estado_cuota ec on dc.codigo_estado_cuota=ec.codigo_estado_cuota
	left join canal_grupo cg on cg.codigo_canal_grupo=dp.codigo_grupo
	where 
		dp.codigo_planilla=@codigo_planilla and dp.excluido=0 and--  and dc.codigo_estado_cuota in(2,3) and
		dp.codigo_tipo_venta=ISNULL(@codigo_tipo_venta,dp.codigo_tipo_venta)and
		dp.codigo_personal=ISNULL(@codigo_personal,dp.codigo_personal)
	order by 
		dp.codigo_empresa asc, dp.codigo_canal desc, nombre_grupo_canal asc, nombre_persona asc, nro_contrato asc, nombre_articulo asc, nro_cuota asc;

	SET NOCOUNT OFF
END;