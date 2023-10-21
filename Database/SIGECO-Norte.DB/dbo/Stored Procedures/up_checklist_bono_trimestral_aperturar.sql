USE SIGECO
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[up_checklist_bono_trimestral_aperturar]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].up_checklist_bono_trimestral_aperturar
GO

CREATE PROCEDURE [dbo].up_checklist_bono_trimestral_aperturar
(
	@p_codigo_planilla		INT
	,@p_usuario_registra	VARCHAR(25)
)
AS
BEGIN
	SET NOCOUNT ON

	DECLARE
		@v_codigo_checklist				INT
		,@v_numero_checklist			VARCHAR(20)
		,@v_dias						INT
	
	DECLARE
		@c_codigo_estado_checklist_abierto	INT = 1
		,@c_codigo_estado_checklist_cerrado	INT = 2
		,@c_estado_registro					BIT = 1
		,@c_fecha							DATETIME = GETDATE()
		,@c_no_validado						BIT = 0


	SET @v_numero_checklist = CONVERT(VARCHAR, YEAR(@c_fecha)) + '-' + RIGHT('000' + CONVERT(VARCHAR, ISNULL((SELECT COUNT(codigo_checklist) FROM dbo.checklist_bono_trimestral WHERE YEAR(fecha_registra) = YEAR(@c_fecha)), 0) + 1), 3)
	SET @v_dias = ISNULL((SELECT TOP 1 CONVERT(INT, valor) FROM dbo.parametro_sistema WHERE codigo_parametro_sistema = 31), 60)	

	INSERT INTO dbo.checklist_bono_trimestral(
		numero_checklist
		,codigo_estado_checklist
		,codigo_planilla
		,estado_registro
		,fecha_registra
		,usuario_registra
	)
	VALUES(
		@v_numero_checklist
		,@c_codigo_estado_checklist_abierto
		,@p_codigo_planilla
		,@c_estado_registro
		,@c_fecha
		,@p_usuario_registra
	)
	
	SET @v_codigo_checklist = SCOPE_IDENTITY()

	/*AQUELLOS POR DEFECTO DE LA PLANILLA*/
	INSERT INTO dbo.checklist_bono_trimestral_detalle(
		codigo_checklist
		,codigo_planilla
		,numero_planilla
		,codigo_personal
		,codigo_empresa
		,nombre_empresa
		,codigo_grupo
		,nombre_grupo
		,validado
		,estado_registro
		,fecha_registra
		,usuario_registra
	)
	SELECT
		@v_codigo_checklist
		,pl.codigo_planilla
		,pl.numero_planilla
		,codigo_personal
		,codigo_empresa
		,nombre_empresa
		,codigo_grupo
		,nombre_grupo
		,@c_no_validado
		,@c_estado_registro
		,@c_fecha
		,@p_usuario_registra
	FROM
		dbo.planilla_bono_trimestral_detalle r
	INNER JOIN dbo.planilla_bono_trimestral pl 
		ON pl.codigo_planilla = r.codigo_planilla
	WHERE
		r.codigo_planilla = @p_codigo_planilla
		AND r.monto_bono IS NOT NULL
		--AND r.validado = 0

	/*AQUELLOS PENDIENTES DE VALIDAR DE OTRO CHECKLIST*/
	INSERT INTO dbo.checklist_bono_trimestral_detalle(
		codigo_checklist
		,codigo_planilla
		,numero_planilla
		,codigo_personal
		,codigo_empresa
		,nombre_empresa
		,codigo_grupo
		,nombre_grupo
		,validado
		,estado_registro
		,fecha_registra
		,usuario_registra
	)
	SELECT DISTINCT
		@v_codigo_checklist
		,ch_d.codigo_planilla
		,ch_d.numero_planilla
		,ch_d.codigo_personal
		,ch_d.codigo_empresa
		,ch_d.nombre_empresa
		,ch_d.codigo_grupo
		,ch_d.nombre_grupo
		,@c_no_validado
		,@c_estado_registro
		,@c_fecha
		,@p_usuario_registra
	FROM
		checklist_bono_trimestral_detalle ch_d 
	INNER JOIN checklist_bono_trimestral ch
		ON ch.codigo_checklist = ch_d.codigo_checklist AND ch_d.validado = @c_no_validado
	INNER JOIN planilla_bono_trimestral_detalle rh
		ON rh.monto_bono IS NOT NULL AND rh.codigo_planilla = ch_d.codigo_planilla AND rh.codigo_empresa = ch_d.codigo_empresa AND rh.codigo_personal = ch_d.codigo_personal --AND rh.validado = @c_no_validado
	WHERE
		ch.codigo_estado_checklist = @c_codigo_estado_checklist_cerrado
		AND EXISTS(SELECT codigo_planilla FROM dbo.planilla_bono_trimestral pl WHERE pl.codigo_planilla = rh.codigo_planilla AND DATEDIFF(d, pl.fecha_cierre, @c_fecha) <= @v_dias) 

	SET NOCOUNT OFF
END