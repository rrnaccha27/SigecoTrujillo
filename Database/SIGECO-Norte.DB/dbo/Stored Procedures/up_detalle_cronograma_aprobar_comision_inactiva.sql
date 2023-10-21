USE SIGECO
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[up_detalle_cronograma_aprobar_comision_inactiva]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].up_detalle_cronograma_aprobar_comision_inactiva
GO

CREATE PROCEDURE dbo.up_detalle_cronograma_aprobar_comision_inactiva
(
	@p_array_detalle_cronograma	ARRAY_DETALLE_CRONOGRAMA_TYPE readonly,
	@p_nivel					INT,
	@p_codigo_resultado			INT,
	@p_usuario_aprobacion		VARCHAR(50),
	@p_observacion				VARCHAR(250)
)
AS
BEGIN
	SET NOCOUNT ON

	DECLARE
		@c_fecha_proceso			DATETIME = GETDATE()
		,@c_estado_activo			BIT = 1
		,@c_liquidado				BIT = 1
		,@c_aprobacion_adm			INT = 2
		,@c_aprobacion_comercial	INT = 1
		,@c_aprobado				INT = 1

	UPDATE c
	SET
		c.liquidado = CASE WHEN @p_nivel = @c_aprobacion_adm AND @p_codigo_resultado = @c_aprobado THEN @c_liquidado ELSE c.liquidado END
		,c.usuario_aprobacion_n1 = CASE WHEN @p_nivel = @c_aprobacion_comercial THEN @p_usuario_aprobacion ELSE c.usuario_aprobacion_n1 END
		,c.fecha_aprobacion_n1 = CASE WHEN @p_nivel = @c_aprobacion_comercial THEN @c_fecha_proceso ELSE c.fecha_aprobacion_n1 END
		,c.codigo_resultado_n1 = CASE WHEN @p_nivel = @c_aprobacion_comercial THEN @p_codigo_resultado ELSE c.codigo_resultado_n1 END
		,c.observacion_n1 = CASE WHEN @p_nivel = @c_aprobacion_comercial THEN @p_observacion ELSE c.observacion_n1 END
		,c.usuario_aprobacion_n2 = CASE WHEN @p_nivel = @c_aprobacion_adm THEN @p_usuario_aprobacion ELSE c.usuario_aprobacion_n2 END
		,c.fecha_aprobacion_n2 = CASE WHEN @p_nivel = @c_aprobacion_adm THEN @c_fecha_proceso ELSE c.fecha_aprobacion_n2 END
		,c.codigo_resultado_n2 = CASE WHEN @p_nivel = @c_aprobacion_adm THEN @p_codigo_resultado ELSE c.codigo_resultado_n2 END
		,c.observacion_n2 = CASE WHEN @p_nivel = @c_aprobacion_adm THEN @p_observacion ELSE c.observacion_n2 END
		,c.usuario_modifica = @p_usuario_aprobacion
		,c.fecha_modifica = @c_fecha_proceso
	FROM
		dbo.comision_personal_inactivo c
	INNER JOIN @p_array_detalle_cronograma array
		ON array.codigo_detalle_cronograma = c.codigo_detalle_cronograma

	SET NOCOUNT OFF
END
