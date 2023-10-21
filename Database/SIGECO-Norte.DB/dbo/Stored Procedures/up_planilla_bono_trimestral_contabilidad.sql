USE SIGECO
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[up_planilla_bono_trimestral_contabilidad]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].up_planilla_bono_trimestral_contabilidad
GO

CREATE PROCEDURE [dbo].up_planilla_bono_trimestral_contabilidad
(
	@p_codigo_planilla	INT
	,@p_codigo_empresa	INT
)
AS
BEGIN

	SET NOCOUNT ON

	DECLARE 
		@v_codigo_tipo_planilla	INT
		,@v_igv DECIMAL(10, 2)

	SET @v_igv = round((SELECT TOP 1 convert(decimal(10, 2), valor) FROM dbo.parametro_sistema WHERE codigo_parametro_sistema = 9) / 100, 2) + 1
	SET @v_codigo_tipo_planilla = (SELECT TOP 1 codigo_tipo_planilla FROM dbo.planilla_bono WHERE codigo_planilla = @p_codigo_planilla)

	DECLARE @t_detalle_planilla_bono table(
		codigo_detalle int
		--,dinero_ingresado decimal(12, 2)
		--,porcentaje_pago decimal(12, 2)
		--,codigo_articulo int
		,codigo_personal int
		,codigo_canal int
		,codigo_grupo int
		,codigo_empresa int
		--,codigo_moneda int
		,codigo_tipo_venta int
		,codigo_planilla int
		,numero_contrato varchar(100)
		,con_igv decimal(12,2 )
		--,total_dpb decimal(12, 2)
		--,total_apb decimal(12, 2)
		--,total_rpb decimal(12, 2)
	)

	insert into @t_detalle_planilla_bono
	SELECT 
		dpb.codigo_planilla_detalle
		--,apb.dinero_ingresado
		--,rpb.porcentaje_pago
		--,apb.codigo_articulo
		,dpb.codigo_personal
		,dpb.codigo_canal
		,dpb.codigo_grupo
		,dpb.codigo_empresa
		,2
		,dpb.codigo_planilla
		,''
		,monto_bono
	FROM 
		planilla_bono_trimestral_detalle dpb
	WHERE 
		dpb.codigo_planilla = @p_codigo_planilla
		and dpb.codigo_empresa = @p_codigo_empresa
		AND dpb.monto_bono IS NOT NULL

	--select * from planilla_bono_trimestral


	--update @t_detalle_planilla_bono
	--set 
	--	con_igv = ((con_igv/total_apb) * total_dpb)
	--where
	--	total_rpb = total_dpb and ((total_apb - total_dpb) > 1)

	SELECT
		'1' as ORDEN,
		@p_codigo_planilla as codigo_planilla,
		@p_codigo_empresa as codigo_empresa,
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
		'2' as ORDEN
		,@p_codigo_planilla as codigo_planilla
		,@p_codigo_empresa as codigo_empresa
		,convert(varchar, dpb.codigo_detalle) as COD_COMISION
		,'1' as N_CUOTA
		,convert(varchar, round(con_igv / @v_igv, 2)) as IMP_PAGAR
		,e.codigo_equivalencia as COD_EMPRESA_G
		,dpb.numero_contrato as NUM_CONTRATO
		,convert(varchar,  case when @v_codigo_tipo_planilla = 2 AND dpb.codigo_empresa = 2 THEN 0 ELSE round(con_igv, 2) - round(con_igv / @v_igv, 2) END) as IGV
		,upper(left(tv.abreviatura, 3)) as	DES_TIPO_VENTA
		,upper(left(tv.abreviatura, 1)) as	TIPO_VENTA
		,'' as FEC_HAVILITADO
		,/*upper(tp.nombre)*/ '' as	DES_FORMA_PAGO
		,/*convert(varchar, tp.codigo_tipo_pago)*/ '' as	ID_FORMA_DE_PAGO
		,upper('bono') as	DES_TIPO_COMISION
		,'199' as	TIPO_COMISION
		,'' as	CUARTA
		,'' as IES
		,'S/.' as	TIPO_MONEDA
		,case when dpb.codigo_canal = 1 then '0373' else '0003' end as	TIPO_AGENTE_G
		,p.codigo_equivalencia as	C_AGENTE
		,g.codigo_equivalencia as	COD_GRUPO_VENTA_G
		,g.nombre as	NOMBRE_GRUPO
		,'' as	DESCRIPCION_1
		,'' as COD_BIEN
		,'' as	DESCRIPCION_2
		,'' as COD_CONCEPTO
		,'0' as	TIPO_CAMBIO
		,convert(varchar, round(con_igv / (case when @v_codigo_tipo_planilla = 2 AND dpb.codigo_empresa = 2 THEN @v_igv ELSE 1.00 END), 2)) as SALDO_A_PAGAR
		,convert(varchar, pbt.fecha_cierre, 103) as	FEC_PLANILLA
		,c.nombre as USU_PLANILLA
		,'0' as	N_OPERACION
		,ISNULL(convert(varchar, pbt.fecha_cierre, 103), '') as	FEC_CIERRE
		,'' as	DESC_TIPO_DOCUM
		,p.nro_ruc as	RUC
		,p.nro_documento as	NUM_DOC
		,ltrim(isnull(p.apellido_paterno + ' ', '') + isnull(p.apellido_materno + ' ', '') + p.nombre) as NOM_AGENTE
		,e.nombre_largo as	NOM_EMPRESA
		,e.direccion_fiscal as	DIR_EMPRESA
		,e.ruc as	RUC_EMPRESA
		,convert(varchar, pbt.fecha_apertura, 103) as	FEC_INICIO
		,convert(varchar, pbt.fecha_cierre, 103) as	FEC_TERMINO
		,'0' as	DSCTO_ESTUDIO_C
		,'0'  as DSCTO_DETRACCION
		,'0.18' as	PORC_IGV
		,'' as	UNIDAD_NEGOCIO
		,dpb.codigo_canal
		,'OPEX'
		,'GS002'
		,CASE dpb.codigo_canal WHEN 4 THEN 'CON0081' WHEN 2 THEN 'CON0083' WHEN 1 THEN 'CON0082' END
	FROM 
		@t_detalle_planilla_bono dpb
	inner join planilla_bono_trimestral pbt on pbt.codigo_planilla = dpb.codigo_planilla
	--inner join dbo.articulo a on a.codigo_articulo = dpb.codigo_articulo
	inner join personal p on dpb.codigo_personal=p.codigo_personal
	inner join canal_grupo c on c.codigo_canal_grupo=dpb.codigo_canal
	inner join canal_grupo g on g.codigo_canal_grupo=dpb.codigo_grupo
	inner join empresa_sigeco e on e.codigo_empresa=dpb.codigo_empresa
	--inner join moneda m on m.codigo_moneda=dpb.codigo_moneda
	inner join tipo_venta tv on tv.codigo_tipo_venta = dpb.codigo_tipo_venta
	--inner join planilla_bono pl on dpb.codigo_planilla = pl.codigo_planilla
	--inner join cabecera_contrato cc on cc.NumAtCard = dpb.numero_contrato and cc.Codigo_empresa = e.codigo_equivalencia
	--inner join tipo_pago tp on tp.codigo_equivalencia = cc.Cod_FormaPago 
	--left  join dbo.detalle_contrato dcont
	--	on dcont.NumAtCard = cc.NumAtCard and dcont.Codigo_empresa = cc.Codigo_empresa and dcont.ItemCode = a.codigo_sku
	WHERE 
		dpb.codigo_planilla = @p_codigo_planilla
		and dpb.codigo_empresa = @p_codigo_empresa
	ORDER BY 
		orden, codigo_canal desc, NOMBRE_GRUPO asc, NOM_AGENTE, NUM_CONTRATO, DESCRIPCION_1, N_CUOTA

	SET NOCOUNT OFF
END;

