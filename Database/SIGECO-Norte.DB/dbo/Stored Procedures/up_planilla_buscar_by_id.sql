USE SIGECO
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[up_planilla_buscar_by_id]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].up_planilla_buscar_by_id
GO
CREATE PROCEDURE [dbo].[up_planilla_buscar_by_id]
(
	@p_codigo_planilla INT
)
AS
BEGIN	

	SELECT TOP 1
		pl.codigo_planilla,  
		pl.codigo_regla_tipo_planilla,  
		isnull(pl.numero_planilla,' ') numero_planilla,  
		pl.fecha_anulacion,
		dbo.fn_obtener_nombre_usuario(pl.usuario_anulacion) as usuario_anulacion,  
		pl.fecha_apertura,
		pl.usuario_registra,
		dbo.fn_obtener_nombre_usuario(pl.usuario_apertura) usuario_apertura,
		dbo.fn_obtener_nombre_usuario(pl.usuario_cierre) usuario_cierre,
		pl.fecha_cierre,
		pl.codigo_tipo_planilla,
		tp.nombre as nombre_tipo_planilla,  
		pl.codigo_estado_planilla,
		ep.nombre as nombre_estado_planilla,  
		pl.fecha_registra, 
		pl.fecha_modifica,
		pl.usuario_modifica ,
		pl.fecha_inicio,
		pl.fecha_fin,
		rtp.nombre as nombre_regla_tipo_planilla,
		rtp.envio_liquidacion
	FROM 
		dbo.planilla pl 
	INNER JOIN dbo.estado_planilla ep 
		ON ep.codigo_estado_planilla = pl.codigo_estado_planilla
	INNER JOIN dbo.tipo_planilla tp 
		ON tp.codigo_tipo_planilla = pl.codigo_tipo_planilla   
	INNER JOIN dbo.regla_tipo_planilla rtp
		ON rtp.codigo_regla_tipo_planilla = pl.codigo_regla_tipo_planilla
	WHERE 
		pl.codigo_planilla=@p_codigo_planilla;

END;