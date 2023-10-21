CREATE PROCEDURE [dbo].[up_planilla_bono_jn_listar]
AS
BEGIN
	SET NOCOUNT ON
 
	DECLARE @t_canales table (nombre varchar(100), codigo int)
	DECLARE @v_nombre varchar(100) = ''
	INSERT INTO @t_canales exec up_proceso_generacion_bono_obtener_canal 1
	SELECT @v_nombre = @v_nombre + nombre + ',' from @t_canales
	SET @v_nombre = LEFT(@v_nombre, len(@v_nombre) -1 )
  
	SELECT 
		pl.codigo_planilla,
		isnull(pl.numero_planilla,' ') numero_planilla,  
		@v_nombre as nombre_canal,
		convert(varchar, pl.fecha_anulacion, 103) as fecha_anulacion,
		pl.usuario_anulacion,
  
		pl.fecha_apertura,
		pl.usuario_apertura,
		pl.fecha_cierre,
		pl.usuario_cierre,
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
		p.valor as estilo
	FROM 
		planilla_bono pl 
	INNER JOIN estado_planilla ep 
		on pl.codigo_estado_planilla=ep.codigo_estado_planilla
	INNER JOIN tipo_planilla tp 
		on tp.codigo_tipo_planilla=pl.codigo_tipo_planilla  
	INNER JOIN dbo.fn_split_parametro_sistema('21,22,23') p 
		on pl.codigo_estado_planilla = p.codigo
	WHERE
		pl.estado_registro = 1 AND pl.es_planilla_jn = 1
	ORDER BY 
		pl.codigo_planilla desc;

	SET NOCOUNT OFF
END;