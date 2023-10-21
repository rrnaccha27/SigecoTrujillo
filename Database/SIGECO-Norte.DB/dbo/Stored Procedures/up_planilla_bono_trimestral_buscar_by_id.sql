USE SIGECO
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[up_planilla_bono_trimestral_buscar_by_id]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].up_planilla_bono_trimestral_buscar_by_id
GO

CREATE PROCEDURE [dbo].up_planilla_bono_trimestral_buscar_by_id
(
	@p_codigo_planilla int
)
AS
BEGIN
	SET NOCOUNT ON
	
	SELECT TOP 1
		pl.codigo_planilla,  
		isnull(pl.numero_planilla,' ') numero_planilla,
		pl.codigo_regla,
		rb.descripcion as nombre_regla,
		pl.codigo_periodo,
		pl.fecha_anulacion,
		dbo.fn_obtener_nombre_usuario(pl.usuario_anulacion) as usuario_anulacion,
  		pl.fecha_apertura,
  		pl.fecha_cierre,
  		dbo.fn_obtener_nombre_usuario(pl.usuario_apertura) usuario_apertura,
		dbo.fn_obtener_nombre_usuario(pl.usuario_cierre) usuario_cierre,
		pl.codigo_estado_planilla,
		ep.nombre as nombre_estado_planilla,
		pl.fecha_registra,
		pl.fecha_modifica,
		pl.usuario_registra,
		pl.usuario_modifica
	FROM 
		dbo.planilla_bono_trimestral pl 
	INNER JOIN estado_planilla ep 
		ON pl.codigo_estado_planilla=ep.codigo_estado_planilla
	INNER JOIN regla_bono_trimestral rb 
		on rb.codigo_regla = pl.codigo_regla
	WHERE 
		pl.codigo_planilla = @p_codigo_planilla;

	SET NOCOUNT OFF
END;

