USE SIGECO
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[up_comision_personal_inactivo_listado]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].up_comision_personal_inactivo_listado
GO

CREATE PROCEDURE dbo.up_comision_personal_inactivo_listado
(
	 @p_fecha_inicio	VARCHAR(8)
	,@p_fecha_fin		VARCHAR(8)
	,@p_codigo_canal	VARCHAR(100)
	,@p_liquidado		INT
)
AS
BEGIN
	SET NOCOUNT ON

	SELECT 
		cpi.codigo_detalle_cronograma
		,ca.nombre AS nombre_canal
		,ISNULL(gr.nombre, '') AS nombre_grupo
		,tpl.nombre as nombre_tipo_planilla
		,p.nombre + ISNULL(' ' + p.apellido_paterno, '') + ISNULL(' ' + p.apellido_materno, '')  AS nombre_personal
		,e.nombre AS nombre_empresa
		,cpc.nro_contrato
		,tv.abreviatura AS nombre_tipo_venta
		,tp.nombre AS nombre_tipo_pago
		,a.nombre AS nombre_articulo
		,dc.nro_cuota
		,dc.monto_bruto AS monto_sin_igv
		,dc.igv AS monto_igv
		,dc.monto_neto AS monto_con_igv
		,ISNULL(occ.motivo_movimiento, '') as observacion
		,dbo.fn_formatear_fecha_hora(cpi.fecha_registra) as fecha_registra
		,dbo.fn_obtener_nombre_usuario(cpi.usuario_registra) as usuario_registra	
		,CONVERT(INT, cpi.liquidado) as liquidado
		,ISNULL(cpi.codigo_resultado_n1, 0) as codigo_resultado_n1
		,ISNULL(cpi.codigo_resultado_n2, 0) as codigo_resultado_n2
		,ISNULL(res1.nombre_estado_resultado, '') as resultado_n1
		,ISNULL(res2.nombre_estado_resultado, '') as resultado_n2
		,ISNULL(prmt1.valor, '') as estilo_n1
		,ISNULL(prmt2.valor, '') as estilo_n2
		,CASE 
			WHEN usuario_aprobacion_n1 IS NULL AND usuario_aprobacion_n2 IS NULL THEN 0
			WHEN usuario_aprobacion_n1 IS NOT NULL AND usuario_aprobacion_n2 IS NULL THEN 1
			WHEN usuario_aprobacion_n1 IS NOT NULL AND usuario_aprobacion_n2 IS NOT NULL THEN 2
		END as nivel_aprobacion
	FROM
		dbo.comision_personal_inactivo cpi
	INNER JOIN dbo.detalle_cronograma dc
		ON dc.codigo_detalle = cpi.codigo_detalle_cronograma
	INNER JOIN dbo.articulo_cronograma ac
		ON ac.codigo_articulo = dc.codigo_articulo and ac.codigo_cronograma = dc.codigo_cronograma
	INNER JOIN dbo.articulo a
		ON a.codigo_articulo = ac.codigo_articulo
	INNER JOIN dbo.cronograma_pago_comision cpc
		ON cpc.codigo_cronograma = dc.codigo_cronograma
	INNER JOIN dbo.tipo_venta tv 
		ON tv.codigo_tipo_venta = cpc.codigo_tipo_venta
	INNER JOIN dbo.tipo_pago tp
		ON tp.codigo_tipo_pago = cpc.codigo_tipo_pago
	INNER JOIN dbo.empresa_sigeco e
		ON e.codigo_empresa = cpc.codigo_empresa
	INNER JOIN dbo.tipo_planilla tpl
		ON tpl.codigo_tipo_planilla = cpc.codigo_tipo_planilla
	INNER JOIN dbo.personal_canal_grupo pcg
		ON pcg.codigo_registro = cpc.codigo_personal_canal_grupo
	INNER JOIN dbo.personal p
		ON p.codigo_personal = pcg.codigo_personal
	INNER JOIN canal_grupo ca
		ON ca.codigo_canal_grupo = pcg.codigo_canal
	LEFT JOIN canal_grupo gr
		ON gr.codigo_canal_grupo = pcg.codigo_canal_grupo
	LEFT JOIN operacion_cuota_comision occ
		ON occ.codigo_detalle_cronograma = cpi.codigo_detalle_cronograma and occ.codigo_tipo_operacion_cuota = 5
	LEFT JOIN dbo.fn_SplitReglaTipoPlanilla(@p_codigo_canal) canal
		on canal.codigo = pcg.codigo_canal
	LEFT JOIN estado_resultado res1
		ON res1.codigo_estado_resultado = cpi.codigo_resultado_n1
	LEFT JOIN estado_resultado res2
		ON res2.codigo_estado_resultado = cpi.codigo_resultado_n2
	LEFT JOIN dbo.fn_split_parametro_sistema('25, 26') prmt1
		ON prmt1.codigo = cpi.codigo_resultado_n1
	LEFT JOIN dbo.fn_split_parametro_sistema('25, 26') prmt2
		ON prmt2.codigo = cpi.codigo_resultado_n2
	WHERE
		dc.codigo_estado_cuota = 5
		--AND cpi.liquidado = 0
		and cpi.fecha_registra BETWEEN CONVERT(DATETIME, @p_fecha_inicio + ' 00:00:00') AND CONVERT(DATETIME, @p_fecha_fin + ' 23:59:59')
		AND 1 = (CASE WHEN LEN(@p_codigo_canal) = 0 THEN 2 ELSE 1 END)
		AND (LEN(@p_codigo_canal) > 0 AND ISNULL(canal.codigo, 0) > 0)
		AND (@p_liquidado = 99 OR (@p_liquidado <> 99 AND cpi.liquidado = CONVERT(BIT, @p_liquidado)) )
	ORDER BY
		ca.codigo_canal_grupo desc, gr.nombre asc, p.nombre + ISNULL(' ' + p.apellido_paterno, '') + ISNULL(' ' + p.apellido_materno, '') asc, e.codigo_empresa asc, cpc.nro_contrato asc, a.nombre asc 
	
	SET NOCOUNT OFF
END;