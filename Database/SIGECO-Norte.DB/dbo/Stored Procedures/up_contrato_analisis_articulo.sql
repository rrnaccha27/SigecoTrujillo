CREATE PROCEDURE [dbo].[up_contrato_analisis_articulo]
(
	@codigo_empresa int,
	@numero_contrato varchar(200)
)
AS
BEGIN

	declare @t_articulo_cronograma table(
		codigo_moneda int,
		codigo_articulo int,
		monto_comision_personal decimal(10,2),
		monto_comision_supervisor decimal(10,2),
		monto_comision_total decimal(10,2),
		cantidad_articulo int
	);

	DECLARE 
		@v_nro_contrato VARCHAR(100)
		,@c_esHR		BIT = 0

	IF LEN(@numero_contrato) < 10
	BEGIN
		SELECT 
			@v_nro_contrato = NumAtCard
		FROM dbo.cabecera_contrato cc
		INNER JOIN dbo.empresa_sigeco e 
			ON e.codigo_empresa = @codigo_empresa
		WHERE 
			(ISNULL(REPLICATE('0', 10 - LEN(cc.NumAtCard)), '') + cc.NumAtCard) = (ISNULL(REPLICATE('0', 10 - LEN(@numero_contrato)), '') + @numero_contrato)
			AND cc.Codigo_empresa = e.codigo_equivalencia
	END
	ELSE
		SET @v_nro_contrato = @numero_contrato

	SET @c_esHR = (SELECT dbo.fn_generar_cronograma_comision_eval_hoja_resumen(@v_nro_contrato))

	/*VISTA ALTERNATIVA*/
	DECLARE @v_vista_alternativa bit = 0, @v_tiene_cronograma bit = 0, @v_codigo_empresa_e varchar(4), @v_valor_cero decimal = 0

	SELECT TOP 1
		@v_vista_alternativa = case when cm.codigo_estado_proceso = 2 then 1 else 0 end,
		@v_codigo_empresa_e = e.codigo_equivalencia
	FROM dbo.contrato_migrado cm
	INNER JOIN dbo.empresa_sigeco e 
		ON e.codigo_empresa = @codigo_empresa
	WHERE 
		cm.NumAtCard = @v_nro_contrato
		AND cm.Codigo_empresa = e.codigo_equivalencia
		--and cm.codigo_estado_proceso = 2 --and (cm.Observacion like '%equivalencia%' or cm.Observacion like '%equivalencia%')

	SELECT TOP 1
		  @v_tiene_cronograma = 1 
	FROM dbo.cronograma_pago_comision 
	WHERE 
		CASE WHEN @c_esHR = 0 THEN nro_contrato ELSE nro_contrato_adicional END = @v_nro_contrato AND codigo_empresa = @codigo_empresa

	IF ((ISNULL(@v_vista_alternativa, 0) = 1) AND (ISNULL(@v_tiene_cronograma,0) = 0))
	BEGIN
		SELECT
			@codigo_empresa as codigo_empresa, 
			@v_nro_contrato as numero_contrato, 
			'' as anulacion_vendedor, 
			'' as anulacion_supervisor, 
			isnull(a.codigo_articulo, 0) as codigo_articulo, 
			isnull(a.nombre, dc.ItemCode) as nombre_articulo, 
			0 as codigo_moneda, 
			'' as nombre_moneda, 
			CONVERT(INT, dc.Quantity) as cantidad_articulo, 
			@v_valor_cero as monto_comision_general_personal, 
			@v_valor_cero as monto_comision_general_supervisor, 
			@v_valor_cero as monto_comision_general, 
			@v_valor_cero as monto_total_comision_vendedor, 
			@v_valor_cero as monto_total_pagado_vendedor, 
			@v_valor_cero as monto_total_saldo_vendedor, 
			@v_valor_cero as monto_total_excluido_vendedor, 
			@v_valor_cero as monto_total_comision_supervisor, 
			@v_valor_cero as monto_total_pagado_supervisor, 
			@v_valor_cero as monto_total_saldo_supervisor, 
			@v_valor_cero as monto_total_excluido_supervisor,
			isnull(a.codigo_sku, dc.ItemCode) as codigo_sku,
			CONVERT(int, @c_esHR) as es_hr,
			0 as genera_comision
		FROM
			dbo.detalle_contrato dc
		LEFT JOIN dbo.articulo a
			ON a.codigo_sku = dc.ItemCode and a.estado_registro = 1
		WHERE
			dc.NumAtCard = @v_nro_contrato and dc.Codigo_empresa = @v_codigo_empresa_e
		RETURN;
	END
	/*VISTA ALTERNATIVA*/

	insert into @t_articulo_cronograma
	select 
		cpc.codigo_moneda,
		ac.codigo_articulo,
		sum(case when cpc.codigo_tipo_planilla=1 then ac.monto_comision else 0 end) monto_comision_personal,
		sum(case when cpc.codigo_tipo_planilla=2 then ac.monto_comision else 0 end)monto_comision_supervisor,
		sum(ac.monto_comision) monto_comision_total,
		--sum(ac.cantidad) cantidad_articulo
		sum(case when cpc.codigo_tipo_planilla=1 then ac.cantidad else null end) cantidad_articulo
	from cronograma_pago_comision cpc 
	inner join articulo_cronograma ac
		on cpc.codigo_cronograma=ac.codigo_cronograma
	where  cpc.codigo_empresa=@codigo_empresa and CASE WHEN @c_esHR = 0 THEN cpc.nro_contrato ELSE cpc.nro_contrato_adicional END = @v_nro_contrato
	group by ac.codigo_articulo,cpc.codigo_moneda;


	declare @t_detalle_cronograma table(
		codigo_moneda int,
		codigo_articulo int,
		cantidad_registro_vendedor int,
		cantidad_registro_supervisor int,
		cantidad_registro_anulado_vendedor int,
		cantidad_registro_anulado_supervisor int,
		monto_total_comision_vendedor decimal(10,2),
		monto_total_pagado_vendedor decimal(10,2),
		monto_total_saldo_vendedor  decimal(10,2),
		monto_total_excluido_vendedor decimal(10,2),

		monto_total_comision_supervisor decimal(10,2),
		monto_total_pagado_supervisor decimal(10,2),
		monto_total_saldo_supervisor decimal(10,2),
		monto_total_excluido_supervisor decimal(10,2),
		es_hr int,
		nro_contrato varchar(100)
	);

	insert into @t_detalle_cronograma
	select  
		cpc.codigo_moneda,
		dc.codigo_articulo,	
		count( case when cpc.codigo_tipo_planilla=1 then 1 else null end) cantidad_registro_vendedor,
		count( case when cpc.codigo_tipo_planilla=2 then 1 else null end) cantidad_registro_supervisor,
	
		count( case when cpc.codigo_tipo_planilla=1 and dc.codigo_estado_cuota=5 then 1 else null end) cantidad_registro_anulado_vendedor,
		count( case when cpc.codigo_tipo_planilla=2 and dc.codigo_estado_cuota=5 then 1 else null end) cantidad_registro_anulado_supervisor,
	
		sum((case when cpc.codigo_tipo_planilla=1 and dc.codigo_estado_cuota not in (5) then dc.monto_neto else 0 end)) as monto_total_comision_vendedor,
		sum(case when cpc.codigo_tipo_planilla=1 and (dc.codigo_estado_cuota=3 or (dc.codigo_estado_cuota=1 and dc.es_registro_manual_comision = 1)) then  dc.monto_neto else 0 end) monto_total_pagado_vendedor,
		(sum((case when cpc.codigo_tipo_planilla=1 and dc.codigo_estado_cuota not in (5) then dc.monto_neto else 0 end)) - 
			sum(case when cpc.codigo_tipo_planilla=1 and (dc.codigo_estado_cuota=3 or (dc.codigo_estado_cuota=1 and dc.es_registro_manual_comision = 1)) then  dc.monto_neto else 0 end)) monto_total_saldo_vendedor,
		sum(case when cpc.codigo_tipo_planilla=1 and dc.codigo_estado_cuota=4 then  dc.monto_neto else 0 end) monto_total_excluido_vendedor,

		sum((case when cpc.codigo_tipo_planilla=2 and dc.codigo_estado_cuota not in (5) then dc.monto_neto else 0 end)) as monto_total_comision_supervisor,
		sum(case when cpc.codigo_tipo_planilla=2 and dc.codigo_estado_cuota=3 then  dc.monto_neto else 0 end) monto_total_pagado_supervisor,
		(sum((case when cpc.codigo_tipo_planilla=2 and dc.codigo_estado_cuota not in (5) then dc.monto_neto else 0 end)) -
			sum(case when cpc.codigo_tipo_planilla=2 and dc.codigo_estado_cuota=3 then  dc.monto_neto else 0 end))monto_total_saldo_supervisor,
		sum(case when cpc.codigo_tipo_planilla=2 and dc.codigo_estado_cuota=4 then  dc.monto_neto else 0 end) monto_total_excluido_supervisor,
		CASE WHEN ISNULL(cpc.nro_contrato_adicional, '') <> '' THEN 1 ELSE 0 END AS es_hr,
		ISNULL(cpc.nro_contrato_adicional, cpc.nro_contrato) nro_contrato
	from cronograma_pago_comision cpc 
	inner join detalle_cronograma dc on cpc.codigo_cronograma=dc.codigo_cronograma
	where cpc.codigo_empresa=@codigo_empresa and CASE WHEN @c_esHR = 0 THEN cpc.nro_contrato ELSE cpc.nro_contrato_adicional END = @v_nro_contrato
	group by cpc.nro_contrato, cpc.nro_contrato_adicional, dc.codigo_articulo,cpc.codigo_moneda;

	select 
		@codigo_empresa as codigo_empresa,
		d.nro_contrato as numero_contrato,/*
		d.cantidad_registro_vendedor,
		d.cantidad_registro_supervisor,
		d.cantidad_registro_anulado_vendedor,
		d.cantidad_registro_anulado_supervisor,*/
		(case 
		   when 
				d.cantidad_registro_vendedor-d.cantidad_registro_anulado_vendedor=0 and
				d.cantidad_registro_vendedor>0 and d.cantidad_registro_anulado_vendedor>0  then 'Total'
		   when d.cantidad_registro_vendedor-d.cantidad_registro_anulado_vendedor>0 and d.cantidad_registro_anulado_vendedor>0 then 'Parcial'
			else 'No tiene ' end ) as anulacion_vendedor,
		(case 
		   when d.cantidad_registro_supervisor-d.cantidad_registro_anulado_supervisor=0
				and d.cantidad_registro_supervisor>0 and  d.cantidad_registro_anulado_supervisor>0  then 'Total'
		   when d.cantidad_registro_supervisor-d.cantidad_registro_anulado_supervisor>0 and d.cantidad_registro_anulado_supervisor>0 then 'Parcial'
			else 'No tiene ' end ) as anulacion_supervisor,

		d.codigo_articulo,
		art.nombre as nombre_articulo,
		d.codigo_moneda,
		m.nombre as nombre_moneda,
		a.cantidad_articulo,

		a.monto_comision_personal as monto_comision_general_personal,
		a.monto_comision_supervisor as monto_comision_general_supervisor,
		a.monto_comision_total as monto_comision_general,
		------------------------------------------
		d.monto_total_comision_vendedor,
		d.monto_total_pagado_vendedor,
		d.monto_total_saldo_vendedor,
		d.monto_total_excluido_vendedor,
		d.monto_total_comision_supervisor,
		d.monto_total_pagado_supervisor,
		d.monto_total_saldo_supervisor,
		d.monto_total_excluido_supervisor,
		art.codigo_sku
		,d.es_hr
		,1 as genera_comision
	from @t_articulo_cronograma a 
	inner join @t_detalle_cronograma d
		on a.codigo_articulo=d.codigo_articulo and a.codigo_moneda=d.codigo_moneda
	inner join articulo art on art.codigo_articulo=a.codigo_articulo
	inner join moneda m on m.codigo_moneda=a.codigo_moneda

	UNION

	/*Articulos que no comisionan por Contrato*/

	SELECT
		@codigo_empresa as codigo_empresa, 
		@v_nro_contrato as numero_contrato, 
		'' as anulacion_vendedor, 
		'' as anulacion_supervisor, 
		isnull(a.codigo_articulo, 0) as codigo_articulo, 
		isnull(a.nombre, dc.ItemCode) as nombre_articulo, 
		0 as codigo_moneda, 
		'' as nombre_moneda, 
		CONVERT(INT, dc.Quantity) as cantidad_articulo, 
		@v_valor_cero as monto_comision_general_personal, 
		@v_valor_cero as monto_comision_general_supervisor, 
		@v_valor_cero as monto_comision_general, 
		@v_valor_cero as monto_total_comision_vendedor, 
		@v_valor_cero as monto_total_pagado_vendedor, 
		@v_valor_cero as monto_total_saldo_vendedor, 
		@v_valor_cero as monto_total_excluido_vendedor, 
		@v_valor_cero as monto_total_comision_supervisor, 
		@v_valor_cero as monto_total_pagado_supervisor, 
		@v_valor_cero as monto_total_saldo_supervisor, 
		@v_valor_cero as monto_total_excluido_supervisor,
		isnull(a.codigo_sku, dc.ItemCode) as codigo_sku
		,0
		,0 as genera_comision
	FROM
		dbo.detalle_contrato dc
	inner JOIN dbo.articulo a
		ON a.codigo_sku = dc.ItemCode and (a.estado_registro = 0 or a.genera_comision = 0)
	WHERE
		dc.NumAtCard = @v_nro_contrato and dc.Codigo_empresa = @v_codigo_empresa_e and not exists (select top 1 codigo_articulo from @t_articulo_cronograma t where t.codigo_articulo = a.codigo_articulo )

	--UNION

	--/*Articulos que no comisionan por HR*/
	--SELECT
	--	@codigo_empresa as codigo_empresa, 
	--	@v_nro_contrato as numero_contrato, 
	--	'' as anulacion_vendedor, 
	--	'' as anulacion_supervisor, 
	--	isnull(a.codigo_articulo, 0) as codigo_articulo, 
	--	isnull(a.nombre, dc.ItemCode) as nombre_articulo, 
	--	0 as codigo_moneda, 
	--	'' as nombre_moneda, 
	--	CONVERT(INT, dc.Quantity) as cantidad_articulo, 
	--	@v_valor_cero as monto_comision_general_personal, 
	--	@v_valor_cero as monto_comision_general_supervisor, 
	--	@v_valor_cero as monto_comision_general, 
	--	@v_valor_cero as monto_total_comision_vendedor, 
	--	@v_valor_cero as monto_total_pagado_vendedor, 
	--	@v_valor_cero as monto_total_saldo_vendedor, 
	--	@v_valor_cero as monto_total_excluido_vendedor, 
	--	@v_valor_cero as monto_total_comision_supervisor, 
	--	@v_valor_cero as monto_total_pagado_supervisor, 
	--	@v_valor_cero as monto_total_saldo_supervisor, 
	--	@v_valor_cero as monto_total_excluido_supervisor,
	--	isnull(a.codigo_sku, dc.ItemCode) as codigo_sku
	--	,1
	--FROM
	--	dbo.detalle_contrato dc
	--INNER JOIN dbo.cabecera_contrato cc
	--	on cc.Codigo_empresa = dc.Codigo_empresa and cc.NumAtCard = dc.NumAtCard
	--inner JOIN dbo.articulo a
	--	ON a.codigo_sku = dc.ItemCode and (a.estado_registro = 0 or a.genera_comision = 0)
	--WHERE
	--	cc.Num_Contrato_Referencia = @v_nro_contrato and cc.Cod_Empresa_Referencia = @v_codigo_empresa_e;
	ORDER BY
		nombre_articulo;
END;