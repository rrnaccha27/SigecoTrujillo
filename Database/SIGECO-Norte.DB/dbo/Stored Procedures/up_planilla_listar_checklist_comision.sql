USE SIGECO
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[up_planilla_listar_checklist_comision]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].up_planilla_listar_checklist_comision
GO

CREATE PROCEDURE [dbo].up_planilla_listar_checklist_comision
AS
BEGIN
	SET NOCOUNT ON

	DECLARE
		@v_fecha_limite	VARCHAR(10)

	SET @v_fecha_limite = ISNULL((SELECT valor FROM dbo.parametro_sistema WHERE codigo_parametro_sistema = 30), '20190801')

	SELECT 
		pl.codigo_planilla,
		isnull(pl.numero_planilla,' ') numero_planilla,  
		rtp.nombre as nombre_regla_tipo_planilla,
		pl.codigo_regla_tipo_planilla,
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
		convert(varchar, pl.fecha_fin, 103) as fecha_fin
	FROM 
		planilla pl 
	INNER JOIN estado_planilla ep 
		ON pl.codigo_estado_planilla=ep.codigo_estado_planilla  
	INNER JOIN regla_tipo_planilla rtp 
		ON rtp.codigo_regla_tipo_planilla=pl.codigo_regla_tipo_planilla
	WHERE
		pl.codigo_estado_planilla = 2 and rtp.envio_liquidacion = 1
		AND NOT EXISTS (SELECT codigo_checklist FROM dbo.checklist_comision c WHERE c.codigo_estado_checklist <> 3 AND c.codigo_planilla = pl.codigo_planilla)
		AND pl.fecha_cierre >= CONVERT(DATETIME, @v_fecha_limite)
	ORDER BY 
		pl.fecha_registra DESC;

	SET NOCOUNT OFF
END;

