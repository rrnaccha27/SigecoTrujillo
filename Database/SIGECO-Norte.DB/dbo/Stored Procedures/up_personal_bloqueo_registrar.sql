USE SIGECO
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[up_personal_bloqueo_registrar]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].up_personal_bloqueo_registrar
GO

CREATE PROCEDURE dbo.up_personal_bloqueo_registrar
(
	@p_codigo_planilla			INT
	,@p_codigo_tipo_bloqueo		INT
	,@p_usuario_registra		VARCHAR(25)
)
AS
BEGIN

	DECLARE
		@c_estado_activo			BIT = 1
		,@c_fecha_proceso			DATETIME = GETDATE()
		,@c_bloqueo_por_comision	INT = 1
		,@c_bloqueo_por_bono		INT = 2
		,@c_bloqueo_por_bono_tri	INT = 3

	IF (@p_codigo_tipo_bloqueo = @c_bloqueo_por_comision)
	BEGIN
		INSERT INTO
			dbo.personal_bloqueo
		(
			codigo_personal,
			codigo_planilla,
			codigo_tipo_bloqueo,
			estado_registro,
			fecha_registra,
			usuario_registra
		)
		SELECT DISTINCT
			codigo_personal
			,codigo_planilla
			,@p_codigo_tipo_bloqueo
			,@c_estado_activo
			,@c_fecha_proceso
			,@p_usuario_registra
		FROM
			dbo.detalle_planilla dpl
		WHERE
			codigo_planilla = @p_codigo_planilla 
	END

	IF (@p_codigo_tipo_bloqueo = @c_bloqueo_por_bono)
	BEGIN
		INSERT INTO
			dbo.personal_bloqueo
		(
			codigo_personal,
			codigo_planilla,
			codigo_tipo_bloqueo,
			estado_registro,
			fecha_registra,
			usuario_registra
		)
		SELECT DISTINCT
			codigo_personal
			,codigo_planilla
			,@p_codigo_tipo_bloqueo
			,@c_estado_activo
			,@c_fecha_proceso
			,@p_usuario_registra
		FROM
			dbo.detalle_planilla_bono dpl
		WHERE
			codigo_planilla = @p_codigo_planilla 
	END

	IF (@p_codigo_tipo_bloqueo = @c_bloqueo_por_bono_tri)
	BEGIN
		INSERT INTO
			dbo.personal_bloqueo
		(
			codigo_personal,
			codigo_planilla,
			codigo_tipo_bloqueo,
			estado_registro,
			fecha_registra,
			usuario_registra
		)
		SELECT DISTINCT
			codigo_personal
			,codigo_planilla
			,@p_codigo_tipo_bloqueo
			,@c_estado_activo
			,@c_fecha_proceso
			,@p_usuario_registra
		FROM
			dbo.planilla_bono_trimestral_detalle dpl
		WHERE
			codigo_planilla = @p_codigo_planilla
			AND ISNULL(monto_bono, 0) > 0
	END

END