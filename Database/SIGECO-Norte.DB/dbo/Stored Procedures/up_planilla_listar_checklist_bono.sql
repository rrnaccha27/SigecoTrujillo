USE SIGECO
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[up_planilla_listar_checklist_bono]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].up_planilla_listar_checklist_bono
GO

CREATE PROCEDURE [dbo].up_planilla_listar_checklist_bono
AS
BEGIN
	SET NOCOUNT ON

	DECLARE	
		@c_estado_cerrado		INT = 2
		,@c_estado_anulado		INT = 3
		,@c_planilla_vendedor	INT = 1
	
	DECLARE
		@v_fecha_limite	VARCHAR(10)

	SET @v_fecha_limite = ISNULL((SELECT valor FROM dbo.parametro_sistema WHERE codigo_parametro_sistema = 30), '20190801')

	SELECT 
		pl.codigo_planilla,
		isnull(pl.numero_planilla,' ') numero_planilla,  
		isnull(convert(varchar, pl.fecha_anulacion, 103) + ' ' + convert(varchar, pl.fecha_anulacion, 108), '') as fecha_anulacion,
		isnull(pl.usuario_anulacion, '') as usuario_anulacion,
		convert(varchar, pl.fecha_apertura, 103) + ' ' + convert(varchar, pl.fecha_apertura, 108) as fecha_apertura,
		pl.usuario_apertura,
		convert(varchar, pl.fecha_cierre, 103) + ' ' + convert(varchar, pl.fecha_cierre, 108) as fecha_cierre,
		pl.usuario_cierre,
		pl.codigo_tipo_planilla,
		pl.codigo_estado_planilla,
		ep.nombre as nombre_estado_planilla,
		pl.fecha_registra,
		pl.fecha_modifica,
		pl.usuario_registra,
		pl.usuario_modifica,
		convert(varchar, pl.fecha_inicio, 103) as fecha_inicio,
		convert(varchar, pl.fecha_fin, 103) as fecha_fin,
		canal.nombre as nombre_canal,
		UPPER(tp.nombre) as nombre_tipo_planilla
	FROM 
		planilla_bono pl 
	INNER JOIN estado_planilla ep 
		ON pl.codigo_estado_planilla=ep.codigo_estado_planilla  
	INNER JOIN dbo.canal_grupo canal
		ON canal.es_canal_grupo = 1 AND canal.codigo_canal_grupo = pl.codigo_canal
	INNER JOIN dbo.tipo_planilla tp
		ON tp.codigo_tipo_planilla = pl.codigo_tipo_planilla
	WHERE
		pl.codigo_estado_planilla = @c_estado_cerrado --and rtp.envio_liquidacion = 1
		AND pl.codigo_tipo_planilla = @c_planilla_vendedor
		AND NOT EXISTS (SELECT codigo_checklist FROM dbo.checklist_bono c WHERE c.codigo_estado_checklist <> @c_estado_anulado AND c.codigo_planilla = pl.codigo_planilla)
		AND pl.fecha_cierre >= CONVERT(DATETIME, @v_fecha_limite)
	ORDER BY 
		pl.fecha_registra DESC;

	SET NOCOUNT OFF
END;

