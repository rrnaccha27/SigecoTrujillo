USE SIGECO
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[up_planilla_bono_anular_pago]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].up_planilla_bono_anular_pago
GO

CREATE PROCEDURE dbo.up_planilla_bono_anular_pago
(
	@p_codigo_planilla_bono	INT
	,@p_codigo_articulo		INT
	,@p_codigo_empresa		INT
	,@p_codigo_personal		INT
	,@p_nro_contrato		VARCHAR(100)
	,@p_excluido_motivo		VARCHAR(300)
	,@p_excluido_usuario	VARCHAR(50)
)
AS
BEGIN 
	SET NOCOUNT ON

	DECLARE
		@v_dinero_ingresado			DECIMAL(10, 2)
		,@v_monto_contratado		DECIMAL(10, 2)
		,@v_porcentaje_pago			DECIMAL(10, 2)
		,@v_total_monto_ingresado	DECIMAL(10, 2)
		,@v_total_monto_contratado	DECIMAL(10, 2)
		,@v_neto					DECIMAL(10, 4)
		,@v_igv						DECIMAL(10, 4)
		,@v_bruto					DECIMAL(10, 4)
		,@c_igv						DECIMAL(10, 4)		
		,@v_codigo_tipo_planilla	INT

	DECLARE
		@c_tipo_bloqueo_bono	INT = 2 /*BONO*/
		,@c_estado_inactivo		BIT = 0
		,@c_fecha_proceso		DATETIME = GETDATE()

	SET @v_codigo_tipo_planilla = (SELECT TOP 1 codigo_tipo_planilla FROM dbo.planilla_bono WHERE codigo_planilla = @p_codigo_planilla_bono)
	SET @c_igv = (SELECT TOP 1 ROUND(CONVERT(DECIMAL(12, 4), valor)/100, 4) FROM dbo.parametro_sistema WHERE codigo_parametro_sistema = 9)-- Confirmar el codigo_parametro_sistema

	SELECT TOP 1
		@v_dinero_ingresado = dinero_ingresado,
		@v_monto_contratado = monto_contratado
	FROM 
		dbo.articulo_planilla_bono 
	WHERE 	
		codigo_planilla_bono = @p_codigo_planilla_bono
		AND codigo_articulo = @p_codigo_articulo
		AND codigo_empresa = @p_codigo_empresa
		--AND codigo_personal = @p_codigo_personal
		AND nro_contrato = @p_nro_contrato

	SET @v_porcentaje_pago = (SELECT TOP 1 porcentaje_pago FROM dbo.resumen_planilla_bono WHERE codigo_planilla = @p_codigo_planilla_bono AND codigo_personal = @p_codigo_personal)

	UPDATE
		dbo.articulo_planilla_bono 
	SET
		excluido = 1
		,excluido_fecha = GETDATE()
		,excluido_motivo = @p_excluido_motivo
		,excluido_usuario = @p_excluido_usuario
	WHERE 	
		codigo_planilla_bono = @p_codigo_planilla_bono
		AND codigo_articulo = @p_codigo_articulo
		AND codigo_empresa = @p_codigo_empresa
		--AND codigo_personal = @p_codigo_personal
		AND nro_contrato = @p_nro_contrato

	INSERT INTO dbo.articulo_planilla_bono_excluido
		(codigo_planilla_bono,codigo_canal_grupo,nombre_grupo,codigo_personal,nombre_persona,codigo_empresa,nombre_empresa,codigo_canal,nombre_canal,nombre_moneda,nro_contrato,codigo_articulo,nombre_articulo,monto_bruto,monto_igv,monto_neto,excluido_motivo,excluido_usuario,excluido_fecha,dinero_ingresado,monto_contratado,porcentaje_pago)
	SELECT 
		@p_codigo_planilla_bono,
		g.codigo_canal_grupo,
		g.nombre as nombre_grupo,
		p.codigo_personal,
		p.nombre + ISNULL(' ' + p.apellido_paterno,'') + ISNULL(' ' + p.apellido_materno,'') as nombre_persona,
		e.codigo_empresa,
		e.nombre as nombre_empresa,	
		dpb.codigo_canal,
		c.nombre as nombre_canal,	
		m.nombre as nombre_moneda
		,cpb.numero_contrato as nro_contrato
		,apb.codigo_articulo
		,a.nombre as nombre_articulo
		,0.00 as monto_bruto
		,0.00 as monto_igv
		,((apb.dinero_ingresado * rpb.porcentaje_pago) / 100) as monto_neto
		,apb.excluido_motivo
		,dbo.fn_obtener_nombre_usuario(apb.excluido_usuario) as excluido_usuario
		,apb.excluido_fecha
		,apb.dinero_ingresado
		,apb.monto_contratado
		,rpb.porcentaje_pago
	FROM 
		dbo.detalle_planilla_bono dpb
	INNER JOIN dbo.empresa_sigeco e on e.codigo_empresa=dpb.codigo_empresa
	INNER JOIN dbo.moneda m on m.codigo_moneda=dpb.codigo_moneda
	LEFT JOIN dbo.resumen_planilla_bono rpb on rpb.codigo_planilla = @p_codigo_planilla_bono and rpb.codigo_personal = dpb.codigo_personal
	LEFT JOIN dbo.contrato_planilla_bono cpb on cpb.codigo_planilla = @p_codigo_planilla_bono and case when @v_codigo_tipo_planilla = 1 then cpb.codigo_personal else cpb.codigo_supervisor end = dpb.codigo_personal and cpb.codigo_empresa = dpb.codigo_empresa
	LEFT JOIN dbo.articulo_planilla_bono apb on apb.codigo_planilla_bono = @p_codigo_planilla_bono and apb.codigo_empresa = cpb.codigo_empresa and apb.nro_contrato = cpb.numero_contrato
	LEFT JOIN dbo.articulo a on a.codigo_articulo = apb.codigo_articulo
	LEFT JOIN dbo.personal p on dpb.codigo_personal=p.codigo_personal
	LEFT JOIN dbo.canal_grupo c on c.codigo_canal_grupo=dpb.codigo_canal
	LEFT JOIN dbo.canal_grupo g on g.codigo_canal_grupo=dpb.codigo_grupo
	WHERE 
		dpb.codigo_planilla = @p_codigo_planilla_bono
		AND apb.excluido = 1
		AND apb.codigo_articulo = @p_codigo_articulo
		AND apb.codigo_empresa = @p_codigo_empresa
		AND nro_contrato = @p_nro_contrato

	UPDATE
		dbo.contrato_planilla_bono 
	SET
		monto_ingresado = monto_ingresado - @v_dinero_ingresado,
		monto_contratado = monto_contratado - @v_monto_contratado
	WHERE 	
		codigo_planilla = @p_codigo_planilla_bono
		AND codigo_empresa = @p_codigo_empresa
		AND case when @v_codigo_tipo_planilla = 1 then codigo_personal else codigo_supervisor end = @p_codigo_personal
		AND numero_contrato = @p_nro_contrato
	
	UPDATE
		dbo.resumen_planilla_bono 
	SET
		monto_ingresado = monto_ingresado - @v_dinero_ingresado,
		monto_contratado = monto_contratado - @v_monto_contratado
	WHERE 	
		codigo_planilla = @p_codigo_planilla_bono
		AND codigo_personal = @p_codigo_personal

	SELECT
		@v_total_monto_ingresado = monto_ingresado - @v_dinero_ingresado,
		@v_total_monto_contratado = monto_contratado - @v_monto_contratado
	FROM
		dbo.detalle_planilla_bono 
	WHERE 	
		codigo_planilla = @p_codigo_planilla_bono
		AND codigo_personal = @p_codigo_personal
		AND codigo_empresa = @p_codigo_empresa

	SET @v_bruto = ROUND((@v_total_monto_ingresado * @v_porcentaje_pago)/100, 4)
	SET @v_neto = ROUND(@v_bruto * (1 + @c_igv), 4)
	SET @v_igv = ROUND(@v_neto - @v_bruto, 4)

	UPDATE
		dbo.detalle_planilla_bono 
	SET
		monto_ingresado = @v_total_monto_ingresado 
		,monto_contratado = @v_total_monto_contratado
		,monto_neto = ROUND(@v_neto, 2)
		,monto_igv = ROUND(@v_igv, 2)
		,monto_bruto = ROUND(@v_bruto, 2)
	WHERE 	
		codigo_planilla = @p_codigo_planilla_bono
		AND codigo_personal = @p_codigo_personal
		AND codigo_empresa = @p_codigo_empresa

	EXEC up_proceso_generacion_bono_ceros @p_codigo_planilla_bono
	EXEC up_proceso_generacion_bono_reeval @p_codigo_planilla_bono, @p_codigo_personal

	IF NOT EXISTS(
		SELECT
			dpl.codigo_personal
		FROM
			dbo.detalle_planilla_bono dpl
		WHERE
			dpl.codigo_planilla = @p_codigo_planilla_bono
			AND dpl.codigo_personal = @p_codigo_personal
	)
	BEGIN
		UPDATE
			dbo.personal_bloqueo
		SET
			estado_registro = @c_estado_inactivo
			,fecha_registra = @c_fecha_proceso
			,usuario_modifica = @p_excluido_usuario
		WHERE
			codigo_planilla = @p_codigo_planilla_bono
			AND codigo_personal = @p_codigo_personal
			AND codigo_tipo_bloqueo = @c_tipo_bloqueo_bono
	END

	SET NOCOUNT OFF
END;