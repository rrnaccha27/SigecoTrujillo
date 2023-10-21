USE [SIGECO]
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[up_planilla_bono_buscar_by_id]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].up_planilla_bono_buscar_by_id
GO

CREATE PROCEDURE [dbo].[up_planilla_bono_buscar_by_id]
(
	@p_codigo_planilla int
)
AS
BEGIN
	SET NOCOUNT ON
	
	DECLARE
		@v_canales VARCHAR(100)

	SET @v_canales = (SELECT TOP 1 valor FROM dbo.parametro_sistema WHERE codigo_parametro_sistema = 28)

	SELECT TOP 1
		pl.codigo_planilla,  
		isnull(pl.codigo_canal,0) codigo_canal,
		isnull(pl.numero_planilla,' ') numero_planilla,
		pl.fecha_anulacion,
		dbo.fn_obtener_nombre_usuario(pl.usuario_anulacion) as usuario_anulacion,
  		pl.fecha_apertura,
  		pl.fecha_cierre,
  		dbo.fn_obtener_nombre_usuario(pl.usuario_apertura) usuario_apertura,
		dbo.fn_obtener_nombre_usuario(pl.usuario_cierre) usuario_cierre,
		pl.codigo_tipo_planilla,
		tp.nombre as nombre_tipo_planilla,
		pl.codigo_estado_planilla,
		ep.nombre as nombre_estado_planilla,
		pl.fecha_registra,
		pl.fecha_modifica,
		pl.usuario_registra,
		pl.usuario_modifica ,
		pl.fecha_inicio,
		pl.fecha_fin,
		@v_canales as codigo_canales,
		CONVERT(INT, pl.es_planilla_jn) es_planilla_jn,
		CONVERT(BIT, CASE WHEN pl.codigo_tipo_planilla = 1 AND pl.codigo_canal = 4 THEN 1 ELSE 0 END) AS envio_liquidacion
	FROM 
		dbo.planilla_bono pl 
	INNER JOIN estado_planilla ep 
		ON pl.codigo_estado_planilla=ep.codigo_estado_planilla
	INNER JOIN tipo_planilla tp 
		ON tp.codigo_tipo_planilla=pl.codigo_tipo_planilla
	WHERE 
		pl.codigo_planilla = @p_codigo_planilla;

	SET NOCOUNT OFF
END;
