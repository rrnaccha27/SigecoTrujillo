CREATE PROCEDURE [dbo].[up_planilla_contabilidad_planilla]
(
	@p_codigo_planilla	INT
	,@p_codigo_empresa	INT
)
AS
BEGIN
	SET NOCOUNT ON

	DECLARE @descuento TABLE(
		codigo_empresa int,
		codigo_personal int,
		monto decimal(10,2)
	);

	DECLARE @detalle_planilla TABLE(
		codigo_detalle_planilla int
		,nro_cuota int
		,monto_bruto decimal(12, 2)
		,nro_contrato varchar(100)
		,igv decimal(12, 3)
		,fecha_pago datetime
		,codigo_tipo_pago int
		,codigo_canal int
		,monto_neto decimal(12, 2)
		,codigo_empresa int
		,codigo_detalle_cronograma int
		,codigo_tipo_venta int
		,codigo_moneda int
		,codigo_personal int
		,codigo_grupo int
		,codigo_articulo int
		,codigo_planilla int
		,excluido bit
		,total decimal(12, 2)
		,descuento decimal(12, 2)
	)

	INSERT INTO @descuento
	SELECT 
		d.codigo_empresa,
		d.codigo_personal,
		sum(isnull(d.monto,0))
	FROM descuento d 
	WHERE d.codigo_planilla = @p_codigo_planilla AND estado_registro = 1 AND d.codigo_empresa = @p_codigo_empresa
	GROUP BY d.codigo_empresa, d.codigo_personal

	INSERT INTO @detalle_planilla
	SELECT 
		codigo_detalle_planilla ,nro_cuota ,monto_bruto ,nro_contrato ,igv ,fecha_pago ,codigo_tipo_pago 
		,codigo_canal ,monto_neto ,codigo_empresa ,codigo_detalle_cronograma ,codigo_tipo_venta 
		,codigo_moneda ,codigo_personal ,codigo_grupo ,codigo_articulo ,codigo_planilla ,excluido 
		,0
		,ISNULL((SELECT monto FROM @descuento WHERE codigo_personal = dp.codigo_personal), 0)
	FROM
		dbo.detalle_planilla dp
	WHERE
		dp.codigo_planilla = @p_codigo_planilla
		and dp.codigo_empresa = @p_codigo_empresa
		and dp.excluido = 0

	--calculando el importe total de aquellos q se le calculara el descuento
	UPDATE d
	SET 
		total = (SELECT sum(monto_neto) FROM @detalle_planilla x WHERE x.codigo_personal = d.codigo_personal GROUP BY codigo_personal)
	FROM @detalle_planilla d
	WHERE
		descuento > 0

	--calculando el nuevo neto a pagar
	UPDATE d
	SET 
		monto_neto = monto_neto - round((((monto_neto * 100) / total) * descuento) / 100, 2)
	FROM @detalle_planilla d
	WHERE
		descuento > 0

	--calculando los importes sin igv e igv
	UPDATE d
	SET 
		monto_bruto = round(monto_neto / 1.18, 2)
		,igv = monto_neto - round(monto_neto / 1.18, 2)
	FROM @detalle_planilla d
	WHERE
		descuento > 0

	SELECT
		@p_codigo_planilla as codigo_planilla,
		@p_codigo_empresa as codigo_empresa,
		'1' as ORDEN,
		'COD_COMISION' as COD_COMISION,
		'N_CUOTA' as N_CUOTA,
		'IMP_PAGAR' as IMP_PAGAR,
		'COD_EMPRESA_G' as COD_EMPRESA_G,
		'NUM_CONTRATO' as NUM_CONTRATO,
		'IGV' as IGV,
		'DES_TIPO_VENTA' as DES_TIPO_VENTA,
		'TIPO_VENTA' as TIPO_VENTA,
		'FEC_HAVILITADO' as FEC_HAVILITADO,
		'DES_FORMA_PAGO' as DES_FORMA_PAGO,
		'ID_FORMA_DE_PAGO' as ID_FORMA_DE_PAGO,
		'DES_TIPO_COMISION' as DES_TIPO_COMISION,
		'TIPO_COMISION' as TIPO_COMISION,
		'CUARTA' as CUARTA,
		'IES' as IES,
		'TIPO_MONEDA' as TIPO_MONEDA,
		'TIPO_AGENTE_G' as TIPO_AGENTE_G,
		'C_AGENTE' as C_AGENTE,
		'COD_GRUPO_VENTA_G' as COD_GRUPO_VENTA_G,
		'NOMBRE_GRUPO' as NOMBRE_GRUPO,
		'DESCRIPCION' as DESCRIPCION_1,
		'COD_BIEN' as COD_BIEN,
		'DESCRIPCION' as DESCRIPCION_2,
		'COD_CONCEPTO' as COD_CONCEPTO,
		'TIPO_CAMBIO' as TIPO_CAMBIO,
		'SALDO_A_PAGAR' as SALDO_A_PAGAR,
		'FEC_PLANILLA' as FEC_PLANILLA,
		'USU_PLANILLA' as USU_PLANILLA,
		'N_OPERACION' as N_OPERACION,
		'FEC_CIERRE' as FEC_CIERRE,
		'DESC_TIPO_DOCUM' as DESC_TIPO_DOCUM,
		'RUC' as RUC,
		'NUM_DOC' as NUM_DOC,
		'NOM_AGENTE' as NOM_AGENTE,
		'NOM_EMPRESA' as NOM_EMPRESA,
		'DIR_EMPRESA' as DIR_EMPRESA,
		'RUC_EMPRESA' as RUC_EMPRESA,
		'FEC_INICIO' as FEC_INICIO,
		'FEC_TERMINO' as FEC_TERMINO,
		'DSCTO_ESTUDIO_C' as DSCTO_ESTUDIO_C,
		'DSCTO_DETRACCION' as DSCTO_DETRACCION,
		'PORC_IGV' as PORC_IGV,
		'UNIDAD_NEGOCIO' as UNIDAD_NEGOCIO,
		0 as codigo_canal
		,'DIMENSION_3' as DIMENSION_3
		,'DIMENSION_4' as DIMENSION_4
		,'DIMENSION_5' as DIMENSION_5
	UNION	

	SELECT
		@p_codigo_planilla as codigo_planilla,
		@p_codigo_empresa as codigo_empresa,
		'2' as ORDEN,
		convert(varchar, dp.codigo_detalle_planilla) as COD_COMISION,
		convert(varchar, dp.nro_cuota) as	N_CUOTA,
		convert(varchar, dp.monto_bruto) as	IMP_PAGAR,
		e.codigo_equivalencia as	COD_EMPRESA_G,
		dp.nro_contrato as	NUM_CONTRATO,
		convert(varchar,dp.igv) as	IGV,
		upper(left(tv.abreviatura, 3)) as	DES_TIPO_VENTA,
		upper(left(tv.abreviatura, 1)) as	TIPO_VENTA,
		convert(varchar, dp.fecha_pago, 103)	FEC_HAVILITADO,
		upper(tp.nombre) as	DES_FORMA_PAGO,
		convert(varchar, dp.codigo_tipo_pago) as	ID_FORMA_DE_PAGO,
		upper('comision') as	DES_TIPO_COMISION,
		'199' as	TIPO_COMISION,
		'' as	CUARTA,
		'' as IES,
		upper(left(m.simbolo, 1)) as	TIPO_MONEDA,
		case when dp.codigo_canal = 1 then '0373' else '0003' end as	TIPO_AGENTE_G,
		p.codigo_equivalencia as	C_AGENTE,
		cg.codigo_equivalencia as	COD_GRUPO_VENTA_G,
		cg.nombre as	NOMBRE_GRUPO,
		a.nombre as	DESCRIPCION_1,
		dcont.COD_BIEN as	COD_BIEN,
		a.nombre as	DESCRIPCION_2,
		dcont.COD_CONCEPTO as	COD_CONCEPTO,
		'0' as	TIPO_CAMBIO,
		convert(varchar, dp.monto_neto) as	SALDO_A_PAGAR,
		convert(varchar, pl.fecha_fin, 103) as	FEC_PLANILLA,
		rtp.nombre as	USU_PLANILLA,
		'0' as	N_OPERACION,
		convert(varchar, pl.fecha_cierre, 103) as	FEC_CIERRE,
		'' as	DESC_TIPO_DOCUM,
		p.nro_ruc as	RUC,
		p.nro_documento as	NUM_DOC,
		ltrim(isnull(p.apellido_paterno + ' ', '') + isnull(p.apellido_materno + ' ', '') + p.nombre) as NOM_AGENTE,
		e.nombre_largo as	NOM_EMPRESA,
		e.direccion_fiscal as	DIR_EMPRESA,
		e.ruc as	RUC_EMPRESA,
		convert(varchar, pl.fecha_inicio, 103) as	FEC_INICIO,
		convert(varchar, pl.fecha_fin, 103) as	FEC_TERMINO,
		'0' as	DSCTO_ESTUDIO_C,
		convert(varchar, round(dp.monto_neto/10, 0)) as	DSCTO_DETRACCION,
		'0.18' as	PORC_IGV,
		right('00000' + convert(varchar, a.codigo_unidad_negocio),4) as	UNIDAD_NEGOCIO,
		dp.codigo_canal
		,'OPEX'
		,'GS002'
		,CASE DP.codigo_canal WHEN 4 THEN 'CON0081' WHEN 2 THEN 'CON0083' WHEN 1 THEN 'CON0082' END
	FROM
		@detalle_planilla dp
	INNER JOIN empresa_sigeco e
		on e.codigo_empresa = dp.codigo_empresa
	INNER JOIN dbo.detalle_cronograma dc
		ON dc.codigo_detalle = dp.codigo_detalle_cronograma and dc.es_registro_manual_comision = 0 and dc.codigo_estado_cuota = 3
	INNER JOIN dbo.tipo_venta tv
		on tv.codigo_tipo_venta = dp.codigo_tipo_venta
	INNER JOIN dbo.tipo_pago tp
		on tp.codigo_tipo_pago = dp.codigo_tipo_pago
	INNER JOIN dbo.moneda m 
		on m.codigo_moneda = dp.codigo_moneda
	INNER JOIN dbo.personal p
		on p.codigo_personal = dp.codigo_personal
	INNER JOIN dbo.canal_grupo cg 
		on cg.codigo_canal_grupo = dp.codigo_grupo
	INNER JOIN dbo.articulo a
		on a.codigo_articulo = dp.codigo_articulo
	INNER JOIN dbo.planilla pl
		on pl.codigo_planilla = dp.codigo_planilla
	INNER JOIN dbo.regla_tipo_planilla rtp
		on rtp.codigo_regla_tipo_planilla = pl.codigo_regla_tipo_planilla
	LEFT JOIN dbo.detalle_contrato dcont
		on dcont.NumAtCard = dp.nro_contrato and dcont.Codigo_empresa = e.codigo_equivalencia and dcont.ItemCode = a.codigo_sku
	WHERE 
		dc.es_registro_manual_comision = 0
		and dc.codigo_estado_cuota = 3
	ORDER BY 
		orden, codigo_canal desc, NOMBRE_GRUPO asc, NOM_AGENTE, NUM_CONTRATO, DESCRIPCION_1, N_CUOTA

	SET NOCOUNT OFF
END;