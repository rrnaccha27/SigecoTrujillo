USE SIGECO
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[up_reporte_finanzas]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].up_reporte_finanzas
GO

CREATE PROCEDURE dbo.up_reporte_finanzas
(
	@p_tipo						INT
	,@p_codigo_canal			VARCHAR(100)
	,@p_codigo_tipo_planilla	VARCHAR(100)
	,@p_codigo_tipo_reporte		INT
	,@p_periodo					VARCHAR(50)
	,@p_anio					INT
	,@p_resumen_detalle			INT 
)
AS
BEGIN
	SET NOCOUNT ON

	--declare 
	--	@p_codigo_canal varchar(100) = '2'
	--	,@p_codigo_tipo_planilla varchar(100) = '1,2' -- 1 vendedor - 2 supervisor
	--	,@p_codigo_tipo_reporte int = 2 -- 1 pagado - 2 generado
	--	,@p_periodo varchar(50) = '5'
	--	,@p_anio int = 2019
	--	,@p_tipo int = 1 -- 1 comision - 2 bono
	--	,@p_resumen_detalle int = 2 -- 1 resumen - 2 detalle

	DECLARE @t_data_bruto TABLE(
		periodo int
		,nombre_empresa varchar(50)
		,nombre_canal varchar(50)
		--,nombre_grupo varchar(50)
		,nombre_tipo_venta varchar(50)
		,nombre_unidad_negocio varchar(100)
		,monto_factura decimal(12, 2)
		,monto_planilla decimal(12, 2)
		,numero_contrato varchar(100)
		,nombre_personal varchar(250)
		,tipo_planilla int
	)

	DECLARE @t_periodo TABLE(
		periodo INT
		,nombre_periodo VARCHAR(20)
	)

	DECLARE @v_igv DECIMAL(10, 2)
	SET @v_igv = (SELECT 1 + CONVERT(NUMERIC,valor)/100 FROM dbo.parametro_sistema WHERE codigo_parametro_sistema = 9);

	INSERT INTO @t_periodo VALUES(1, 'ENERO');INSERT INTO @t_periodo VALUES(2, 'FEBRERO');INSERT INTO @t_periodo VALUES(3, 'MARZO');INSERT INTO @t_periodo VALUES(4, 'ABRIL');
	INSERT INTO @t_periodo VALUES(5, 'MAYO');INSERT INTO @t_periodo VALUES(6, 'JUNIO');INSERT INTO @t_periodo VALUES(7, 'JULIO');INSERT INTO @t_periodo VALUES(8, 'AGOSTO');
	INSERT INTO @t_periodo VALUES(9, 'SETIEMBRE');INSERT INTO @t_periodo VALUES(10, 'OCTUBRE');INSERT INTO @t_periodo VALUES(11, 'NOVIEMBRE');INSERT INTO @t_periodo VALUES(12, 'DICIEMBRE');

	IF @p_codigo_tipo_reporte = 1/*PAGADO*/ AND @p_tipo = 1/*COMISION*/
	BEGIN
		INSERT INTO @t_data_bruto
		SELECT 
			p.codigo, e.nombre as nombre_empresa, canal.nombre as canal, /*grupo.nombre as grupo, */tv.abreviatura as nombre_tipo_venta, un.nombre as nombre_unidad_negocio,
			case 
				when dpl.codigo_canal = 1 and dpl.codigo_empresa = 1 then --funes ofsa
					dpl.monto_bruto
				when dpl.codigo_canal = 2 and dpl.codigo_empresa = 2 and pl.codigo_tipo_planilla = 2 then --gestores funjar
					dpl.monto_bruto
				when dpl.codigo_canal = 2 and pl.codigo_tipo_planilla = 1 then --vend agencia
					dpl.monto_bruto
				when pl.codigo_tipo_planilla = 1 and dpl.codigo_canal = 4 then --vend representantes
					dpl.monto_bruto
				when pl.codigo_tipo_planilla = 2 and dpl.codigo_canal = 4 and dpl.codigo_empresa  = 1 then --sup representantes ofsa
					dpl.monto_bruto
				else
					0
				end
			as monto_factura
			,
			case 
				when dpl.codigo_canal = 1 and dpl.codigo_empresa = 2 then --funes funjar
					dpl.monto_bruto
				when dpl.codigo_canal = 2 and dpl.codigo_empresa = 1 and pl.codigo_tipo_planilla = 2 then --gestores ofsa
					dpl.monto_bruto
				when pl.codigo_tipo_planilla = 2 and dpl.codigo_canal = 4 and dpl.codigo_empresa  = 2 then --sup representantes funjar
					dpl.monto_bruto
				else
					0
				end
			as monto_planilla
			,dpl.nro_contrato
			,pe.nombre + isnull(' ' + pe.apellido_paterno, '') + isnull(' ' + pe.apellido_materno, '') as nombre_personal
			,pl.codigo_tipo_planilla
		FROM 
			dbo.planilla pl 
		inner join dbo.fn_SplitReporteGeneral(@p_codigo_tipo_planilla) tpl on tpl.codigo = pl.codigo_tipo_planilla
		inner join dbo.detalle_planilla dpl on pl.codigo_estado_planilla = 2 and dpl.codigo_planilla = pl.codigo_planilla
		inner join dbo.fn_SplitReporteGeneral(@p_codigo_canal) c on c.codigo = dpl.codigo_canal
		inner join dbo.canal_grupo canal on canal.codigo_canal_grupo = dpl.codigo_canal and canal.es_canal_grupo = 1
		--inner join dbo.canal_grupo grupo on grupo.codigo_canal_grupo = dpl.codigo_grupo and grupo.es_canal_grupo = 0
		inner join dbo.articulo a on dpl.codigo_articulo = a.codigo_articulo
		inner join dbo.unidad_negocio un on a.codigo_unidad_negocio = un.codigo_unidad_negocio
		inner join dbo.empresa_sigeco e on dpl.codigo_empresa = e.codigo_empresa
		inner join dbo.tipo_venta tv on dpl.codigo_tipo_venta = tv.codigo_tipo_venta
		inner join dbo.fn_SplitReporteGeneral(@p_periodo) p on p.codigo = month(pl.fecha_fin)
		inner join personal pe on pe.codigo_personal = dpl.codigo_personal
		WHERE 
			dpl.excluido = 0
			and year(pl.fecha_fin) = @p_anio 
	END

	IF @p_codigo_tipo_reporte = 2/*GENERADO*/ AND @p_tipo = 1 /*COMISION*/
	BEGIN
		INSERT INTO @t_data_bruto	
		SELECT 
			p.codigo, e.nombre as nombre_empresa, canal.nombre as canal, /*grupo.nombre as grupo,*/ tv.abreviatura as nombre_tipo_venta, un.nombre as nombre_unidad_negocio, 
			--cpc.nro_contrato,e.nombre,
			case 
				when pcg.codigo_canal = 1 and cpc.codigo_empresa = 1 then --funes ofsa
					dcr.monto_bruto
				when pcg.codigo_canal = 2 and cpc.codigo_empresa = 2 and cpc.codigo_tipo_planilla = 2 then --gestores funjar
					dcr.monto_bruto
				when pcg.codigo_canal = 2 and cpc.codigo_tipo_planilla = 1 then --vend agencia
					dcr.monto_bruto
				when cpc.codigo_tipo_planilla = 1 and pcg.codigo_canal = 4 then --vend representantes
					dcr.monto_bruto
				when cpc.codigo_tipo_planilla = 2 and pcg.codigo_canal = 4 and cpc.codigo_empresa  = 1 then --sup representantes ofsa
					dcr.monto_bruto
				else
					0
				end
			as monto_factura
			,
			case 
				when pcg.codigo_canal = 1 and cpc.codigo_empresa = 2 then --funes funjar
					dcr.monto_bruto
				when pcg.codigo_canal = 2 and cpc.codigo_empresa = 1 and cpc.codigo_tipo_planilla = 2 then --gestores ofsa
					dcr.monto_bruto
				when cpc.codigo_tipo_planilla = 2 and pcg.codigo_canal = 4 and cpc.codigo_empresa  = 2 then --sup representantes funjar
					dcr.monto_bruto
				else
					0
				end
			as monto_planilla
			,cpc.nro_contrato
			,pe.nombre + isnull(' ' + pe.apellido_paterno, '') + isnull(' ' + pe.apellido_materno, '') as nombre_personal
			,cpc.codigo_tipo_planilla
		FROM 
			dbo.cronograma_pago_comision cpc
		inner join dbo.fn_SplitReporteGeneral(@p_codigo_tipo_planilla) tpl on tpl.codigo = cpc.codigo_tipo_planilla
		inner join dbo.empresa_sigeco e on e.codigo_empresa = cpc.codigo_empresa
		inner join dbo.cabecera_contrato cc on cc.NumAtCard = cpc.nro_contrato and cc.Codigo_empresa = e.codigo_equivalencia
		inner join dbo.personal_canal_grupo pcg on pcg.codigo_registro = cpc.codigo_personal_canal_grupo
		inner join dbo.personal pe on pe.codigo_personal = pcg.codigo_personal
		inner join dbo.fn_SplitReporteGeneral(@p_codigo_canal) c on c.codigo = pcg.codigo_canal
		inner join dbo.canal_grupo canal on canal.codigo_canal_grupo = pcg.codigo_canal and canal.es_canal_grupo = 1
		--inner join dbo.canal_grupo grupo on grupo.codigo_canal_grupo = pcg.codigo_canal_grupo and grupo.es_canal_grupo = 0
		inner join dbo.detalle_cronograma dcr on dcr.codigo_cronograma = cpc.codigo_cronograma
		inner join dbo.articulo a on dcr.codigo_articulo = a.codigo_articulo
		inner join dbo.unidad_negocio un on a.codigo_unidad_negocio = un.codigo_unidad_negocio
		inner join dbo.tipo_venta tv on tv.codigo_tipo_venta = cpc.codigo_tipo_venta
		inner join dbo.fn_SplitReporteGeneral(@p_periodo) p on p.codigo = month(cc.CreateDate)
		WHERE 
			cc.Cod_Estado_Contrato = ''
			and year(cc.CreateDate) = @p_anio 
	END

	IF @p_tipo = 2 /*BONO*/
	BEGIN
		INSERT INTO @t_data_bruto
		SELECT 
			p.codigo, e.nombre as nombre_empresa, canal.nombre as canal, tv.abreviatura as nombre_tipo_venta, un.nombre as nombre_unidad_negocio, 
			case 
				when pl.codigo_tipo_planilla = 2 and dpl.codigo_canal = 1 and dpl.codigo_empresa = 2 then --sup funes funjar
					round((apb.dinero_ingresado * (rpb.porcentaje_pago / 100)), 2)
				when pl.codigo_tipo_planilla = 2 and dpl.codigo_canal = 2 and dpl.codigo_empresa = 2 then --sup gestores funjar
					round((apb.dinero_ingresado * (rpb.porcentaje_pago / 100)), 2)
				when pl.codigo_tipo_planilla = 1 and dpl.codigo_canal = 4 then --vend representantes
					round((apb.dinero_ingresado * (rpb.porcentaje_pago / 100)), 2)
				when pl.codigo_tipo_planilla = 2 and dpl.codigo_canal = 4 and dpl.codigo_empresa  = 1 then --sup representantes ofsa
					round((apb.dinero_ingresado * (rpb.porcentaje_pago / 100)), 2)
				else
					0
				end
			as monto_factura
			,
			case 
				when pl.codigo_tipo_planilla = 2 and dpl.codigo_canal = 1 and dpl.codigo_empresa = 1 then --sup funes ofsa
					round((apb.dinero_ingresado * (rpb.porcentaje_pago / 100)), 2)
				when pl.codigo_tipo_planilla = 2  and dpl.codigo_canal = 2 and dpl.codigo_empresa = 1 then --sup gestores ofsa
					round((apb.dinero_ingresado * (rpb.porcentaje_pago / 100)), 2)
				when pl.codigo_tipo_planilla = 2 and dpl.codigo_canal = 4 and dpl.codigo_empresa  = 2 then --sup representantes funjar
					round((apb.dinero_ingresado * (rpb.porcentaje_pago / 100)), 2)
				else
					0
				end
			as monto_planilla
			,'' as nro_contrato
			,'' as nombre_personal
			,0 as tipo_planilla
		FROM 
			detalle_planilla_bono dpl
		inner join planilla_bono pl on dpl.codigo_planilla = pl.codigo_planilla and pl.codigo_estado_planilla = 2
		inner join dbo.fn_SplitReporteGeneral(@p_codigo_tipo_planilla) tpl on tpl.codigo = pl.codigo_tipo_planilla
		inner join dbo.resumen_planilla_bono rpb on rpb.codigo_planilla = dpl.codigo_planilla and rpb.codigo_personal = dpl.codigo_personal
		inner join dbo.contrato_planilla_bono cpb on cpb.codigo_planilla = rpb.codigo_planilla and case when pl.codigo_tipo_planilla = 1 then cpb.codigo_personal else cpb.codigo_supervisor end = dpl.codigo_personal and cpb.codigo_empresa = dpl.codigo_empresa
		inner join dbo.articulo_planilla_bono apb on apb.codigo_planilla_bono = cpb.codigo_planilla and apb.codigo_empresa = cpb.codigo_empresa and apb.nro_contrato = cpb.numero_contrato /*and apb.codigo_personal = cpb.codigo_personal */
		inner join dbo.fn_SplitReporteGeneral(@p_codigo_canal) c1 on c1.codigo = dpl.codigo_canal
		inner join dbo.fn_SplitReporteGeneral(@p_periodo) p on p.codigo = month(pl.fecha_fin)
		INNER JOIN articulo a ON a.codigo_articulo = apb.codigo_articulo 
		INNER JOIN unidad_negocio un ON un.codigo_unidad_negocio = a.codigo_unidad_negocio
		INNER JOIN empresa_sigeco e ON e.codigo_empresa = apb.codigo_empresa	
		inner join canal_grupo canal on canal.codigo_canal_grupo = dpl.codigo_canal and canal.es_canal_grupo = 1
		inner join canal_grupo grupo on grupo.codigo_canal_grupo = dpl.codigo_grupo and grupo.es_canal_grupo = 0 and cpb.codigo_grupo = dpl.codigo_grupo
		inner join tipo_venta tv on tv.codigo_tipo_venta = cpb.codigo_tipo_venta
		WHERE
			((apb.dinero_ingresado * rpb.porcentaje_pago) / 100) > 0
			AND apb.excluido = 0
			AND year(pl.fecha_fin) = @p_anio 

		UPDATE @t_data_bruto SET monto_factura = ROUND((monto_factura / @v_igv), 2), monto_planilla = ROUND((monto_planilla / @v_igv), 2)
	END

	IF (@p_resumen_detalle = 1)
	BEGIN
		SELECT
			ROW_NUMBER() OVER(ORDER BY periodo) AS Id,
			@p_tipo as tipo, 
			(SELECT TOP 1 nombre_periodo FROM @t_periodo WHERE periodo = d.periodo) as nombre_periodo, 
			nombre_empresa,
			nombre_canal, 
			--nombre_grupo, 
			UPPER(nombre_tipo_venta) as nombre_tipo_venta,
			nombre_unidad_negocio,
			SUM(monto_factura) AS monto_factura, 
			SUM(monto_planilla) AS monto_planilla,
			@p_resumen_detalle AS resumen_detalle,
			@p_codigo_tipo_reporte AS tipo_reporte
		FROM 
			@t_data_bruto d
		GROUP BY 
			periodo, nombre_empresa, nombre_canal, /*nombre_grupo,*/ nombre_tipo_venta, nombre_unidad_negocio
		ORDER BY 
			periodo, nombre_empresa desc, nombre_canal, /*nombre_grupo,*/ nombre_tipo_venta, nombre_unidad_negocio
	END
	ELSE
	BEGIN
		SELECT
			ROW_NUMBER() OVER(ORDER BY periodo) AS Id,
			@p_tipo as tipo, 
			(SELECT TOP 1 nombre_periodo FROM @t_periodo WHERE periodo = d.periodo) as nombre_periodo, 
			nombre_empresa,
			nombre_canal, 
			--nombre_grupo, 
			UPPER(nombre_tipo_venta) as nombre_tipo_venta,
			nombre_unidad_negocio,
			SUM(monto_factura) AS monto_factura, 
			SUM(monto_planilla) AS monto_planilla 
			,numero_contrato
			,min(nombre_personal) as nombre_personal
			,case when tipo_planilla = 1 then 'V' else 'S' end as tipo_planilla
			,@p_resumen_detalle AS resumen_detalle
			,@p_codigo_tipo_reporte AS tipo_reporte
		FROM 
			@t_data_bruto d
		GROUP BY 
			periodo, nombre_empresa, nombre_canal, /*nombre_grupo,*/ nombre_tipo_venta, nombre_unidad_negocio,numero_contrato, tipo_planilla
		ORDER BY 
			periodo, nombre_empresa desc, nombre_canal, /*nombre_grupo,*/ nombre_tipo_venta, nombre_unidad_negocio,numero_contrato, tipo_planilla
	END

	SET NOCOUNT OFF
END;