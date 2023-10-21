/********************************************************************************************
                     SECCIÓN ANALISIS CONTRATO
*********************************************************************************************/
CREATE PROCEDURE [dbo].[up_contrato_analisis_cabecera]
(
	@codigo_empresa int,
	@nro_contrato varchar(20)
)
AS
BEGIN

	DECLARE 
		@v_codigo_personal int,
		@v_codigo_canal_grupo int, 
		@v_tipo int, 
		@v_codigo_supervisor int,
		@v_nombres_supervisor varchar(100),
		@v_nombres_vendedor varchar(100),
		@v_nombre_canal varchar(100),
		@v_observacion_contrato_migrado varchar(400),
		@v_nombre_estado_proceso_migrado varchar(100),
		@v_nombre_grupo varchar(100),
		@v_codigo_empresa varchar(4),
		@v_fecha_proceso varchar(25) = '',
		@v_fecha_migracion varchar(25) = '',
		@v_usuario_proceso varchar(150) = '',
		@v_codigo_personal_canal_grupo int,
		@c_esHR bit = 0,
		@v_codigo_grupo_j varchar(60) = '',
		@v_bloqueo char(1) = '',
		@v_usuario_bloqueo varchar(50) = '',
		@v_fecha_bloqueo varchar(25),
		@v_codigo_bloqueo int,
		@v_motivo_bloqueo varchar(250),
		@v_nro_contrato VARCHAR(100);

	SELECT 
		@v_nro_contrato = NumAtCard, @v_codigo_empresa = cc.Codigo_empresa, @v_codigo_grupo_j = codigo_grupo
	FROM dbo.cabecera_contrato cc
	INNER JOIN dbo.empresa_sigeco e 
		ON e.codigo_empresa = @codigo_empresa
	WHERE 
		(ISNULL(REPLICATE('0', 10 - LEN(cc.NumAtCard)), '') + cc.NumAtCard) = (ISNULL(REPLICATE('0', 10 - LEN(@nro_contrato)), '') + @nro_contrato)
		AND cc.Codigo_empresa = e.codigo_equivalencia and ISNULL(cc.Cod_Estado_Contrato, '') <> 'ANL'

	SET @c_esHR = (SELECT dbo.fn_generar_cronograma_comision_eval_hoja_resumen(@v_nro_contrato))

	select top 1
		@v_codigo_personal=p.codigo_personal,
		@v_nombres_vendedor=p.nombre + ISNULL(' ' + p.apellido_paterno, '') + ISNULL(' ' + p.apellido_materno, '')
	from cabecera_contrato cc
	inner join personal p on (cc.Cod_Vendedor=p.codigo_equivalencia)
	where cc.NumAtCard = @v_nro_contrato and cc.Codigo_empresa=@v_codigo_empresa;
	/********************************************************************************************/

	select top 1
		@v_observacion_contrato_migrado=cm.Observacion,
		@v_nombre_estado_proceso_migrado=ep.nombre
		,@v_fecha_proceso = dbo.fn_formatear_fecha_hora(cm.Fec_Proceso)
		,@v_fecha_migracion = CASE WHEN cm.Fec_Actualizacion IS NULL THEN dbo.fn_formatear_fecha_hora(cm.Fec_Creacion) ELSE dbo.fn_formatear_fecha_hora(cm.Fec_Actualizacion) END 
		,@v_usuario_proceso = dbo.fn_obtener_nombre_usuario(cm.codigo_usuario)
		,@v_bloqueo = CONVERT(CHAR(1), ISNULL(cm.bloqueo, 0))
		,@v_codigo_bloqueo = ISNULL(cm.codigo_bloqueo, 0)
	from contrato_migrado cm 
	inner join estado_proceso ep on cm.codigo_estado_proceso=ep.codigo_estado_proceso
	where cm.Codigo_empresa=@v_codigo_empresa and cm.NumAtCard = @v_nro_contrato

	SELECT top 1
		@v_usuario_bloqueo = dbo.fn_obtener_nombre_usuario(usuario_registro)
		,@v_fecha_bloqueo = dbo.fn_formatear_fecha_hora(fecha_registro)
		,@v_motivo_bloqueo = motivo
	from
		dbo.contrato_bloqueo
	where
		@v_codigo_bloqueo > 0 AND codigo_bloqueo = @v_codigo_bloqueo AND estado_registro = 1

	/*********************************************************************************************
	si codigo_canal=codigo_canal_grupo solo pertenece a un canal
	si codigo_canal<>codigo_canal_grupo Pertenece a un grupo
	*********************************************************************************************/
	SELECT TOP 1
		@v_codigo_personal_canal_grupo = codigo_personal_canal_grupo 
	FROM 
		dbo.cronograma_pago_comision 
	WHERE 
		codigo_empresa = @codigo_empresa 
		AND CASE WHEN @c_esHR = 0 THEN nro_contrato ELSE nro_contrato_adicional END = @v_nro_contrato 
		AND codigo_tipo_planilla = 1 
		AND ( (@c_esHR = 1) OR (@c_esHR = 0 AND nro_contrato_adicional IS NULL) )
	ORDER BY 
		fecha_registro DESC

	IF (ISNULL(@v_codigo_personal_canal_grupo, 0) > 0)
		select top 1
			@v_nombre_canal = ca.nombre,
			@v_nombre_grupo = gr.nombre
		from personal_canal_grupo pcg 
		inner join dbo.canal_grupo ca on pcg.codigo_canal = ca.codigo_canal_grupo
		left join dbo.canal_grupo gr on gr.codigo_canal_grupo = pcg.codigo_canal_grupo
		where pcg.codigo_registro = @v_codigo_personal_canal_grupo
	ELSE
		select top 1
			@v_nombre_canal = ca.nombre,
			@v_nombre_grupo = gr.nombre
		from personal_canal_grupo pcg 
		inner join dbo.canal_grupo ca on pcg.codigo_canal = ca.codigo_canal_grupo
		left join dbo.canal_grupo gr on gr.codigo_canal_grupo = pcg.codigo_canal_grupo
		where gr.codigo_equivalencia = @v_codigo_grupo_j and pcg.estado_registro = 1

	set @v_codigo_supervisor = dbo.fn_obtener_personal_supervisor(@v_nro_contrato, @v_codigo_empresa, 1)

	select 
		@v_nombres_supervisor=nombre + ISNULL(' ' + apellido_paterno, '') + ISNULL(' ' + apellido_materno, '')
	from
		personal
	where codigo_personal = @v_codigo_supervisor;

	select top 1
		cc.NumAtCard as nro_contrato,
		cc.CardName as apellidos_nombres_cliente,
		e.nombre as nombre_empresa,
		e.codigo_empresa,
		isnull(@v_nombre_canal,' ') as nombre_canal,
		isnull(@v_nombre_grupo,' ') as nombre_grupo,
		tv.nombre as nombre_tipo_venta,
		tp.nombre as nombre_tipo_pago,
		isnull(@v_nombres_vendedor,' ') as apellidos_nombres_vendedor,
		isnull(@v_nombres_supervisor,' ') as apellidos_nombres_supervisor
		,CASE WHEN cc.Flg_Documentacion_Completa = '01' THEN 'SI' ELSE 'NO' END AS doc_completa,
		@v_observacion_contrato_migrado as observacion_contrato_migrado,
		@v_nombre_estado_proceso_migrado as nombre_estado_proceso_migrado
		,dbo.fn_formatear_fecha_hora(cc.CreateDate) as fecha_contrato
		,@v_fecha_proceso as fecha_proceso
		,@v_fecha_migracion as fecha_migracion
		,@v_usuario_proceso as usuario_proceso
		,ISNULL(cc.Num_Contrato_Referencia, '') as nro_contrato_ref
		,ISNULL(erf.nombre, '') as nombre_empresa_ref
		,CASE WHEN ISNULL(cc.Flg_Transferencia, 0) = 0 THEN 'NO' ELSE 'SI' END as tiene_transferencia
		,ISNULL(etrf.nombre, '') as nombre_empresa_transferencia
		,ISNULL(ContratoTransferencia,'') as nro_contrato_transferencia
		,ISNULL(cc.MontoTransferencia, 0.00) as monto_transferencia
		,@v_bloqueo as bloqueo
		,@v_usuario_bloqueo as usuario_bloqueo
		,@v_fecha_bloqueo as fecha_bloqueo
		,@v_motivo_bloqueo as motivo_bloqueo
	from cabecera_contrato cc
	inner join tipo_venta tv on cc.Cod_Tipo_Venta =tv.codigo_equivalencia
	inner join tipo_pago tp on tp.codigo_equivalencia=cc.Cod_FormaPago
	inner join empresa_sigeco e on e.codigo_equivalencia=cc.Codigo_empresa
	left join empresa_sigeco erf on erf.codigo_equivalencia = cc.Cod_Empresa_Referencia
	left join empresa_sigeco etrf on etrf.codigo_equivalencia = cc.EmpresaTransferencia
	where cc.NumAtCard = @v_nro_contrato and e.codigo_empresa=@codigo_empresa;

END;

SET ANSI_NULLS ON