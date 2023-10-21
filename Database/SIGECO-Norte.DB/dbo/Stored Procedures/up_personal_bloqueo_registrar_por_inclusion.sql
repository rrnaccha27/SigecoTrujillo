USE SIGECO
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[up_personal_bloqueo_registrar_por_inclusion]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].up_personal_bloqueo_registrar_por_inclusion
GO

CREATE PROCEDURE dbo.up_personal_bloqueo_registrar_por_inclusion
(
	@p_codigo_planilla		INT,
	@p_usuario_registra		VARCHAR(30),
	@p_contratos			XML
)
AS
BEGIN
	SET NOCOUNT ON

	DECLARE
		@c_tipo_bloqueo_comision	INT = 1
		,@c_estado_activo			BIT = 1
		,@p_fecha_proceso			DATETIME = GETDATE()
		,@c_validado				BIT = 1

	;WITH ParsedXML AS
	(
	SELECT
		ParamValues.C.value('@codigo_detalle_cronograma', 'int') AS codigo_detalle_cronograma
	FROM 
		@p_contratos.nodes('//contratos/contrato') AS ParamValues(C)
	)

	INSERT INTO
		dbo.personal_bloqueo
	(
		codigo_personal
		,codigo_planilla
		,codigo_tipo_bloqueo
		,estado_registro
		,usuario_registra
		,fecha_registra
	)
	SELECT DISTINCT
		pe.codigo_personal
		,@p_codigo_planilla
		,@c_tipo_bloqueo_comision
		,@c_estado_activo
		,@p_usuario_registra
		,@p_fecha_proceso
	FROM
		dbo.detalle_cronograma dc 
	INNER JOIN ParsedXML tmp 
		ON tmp.codigo_detalle_cronograma = dc.codigo_detalle
	INNER JOIN dbo.cronograma_pago_comision cro 
		ON cro.codigo_cronograma = dc.codigo_cronograma 
	INNER JOIN dbo.personal_canal_grupo p
		ON p.codigo_registro = cro.codigo_personal_canal_grupo
	INNER JOIN dbo.personal pe
		ON pe.codigo_personal = p.codigo_personal AND pe.validado = @c_validado
	WHERE
		NOT EXISTS(
			SELECT 
				pb.codigo_personal 
			FROM 
				dbo.personal_bloqueo pb 
			WHERE 
				pb.codigo_personal = pe.codigo_personal 
				AND pb.estado_registro = @c_estado_activo 
				AND pb.codigo_planilla = @p_codigo_planilla 
				AND pb.codigo_tipo_bloqueo = @c_tipo_bloqueo_comision
		)

	SET NOCOUNT OFF
END