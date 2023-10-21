CREATE PROCEDURE dbo.up_log_proceso_bono_listado
(
	 @p_fecha_inicio	VARCHAR(8)
	,@p_fecha_fin		VARCHAR(8)
)
AS
BEGIN

	DECLARE @t_canales table (nombre varchar(100), codigo int)
	DECLARE @v_nombre varchar(100) = ''
	INSERT INTO @t_canales exec up_proceso_generacion_bono_obtener_canal 1
	SELECT @v_nombre = @v_nombre + nombre + ',' from @t_canales
	SET @v_nombre = LEFT(@v_nombre, len(@v_nombre) -1 )

	SELECT 
		CONVERT(VARCHAR(100),l.id) AS id
		,ISNULL(l.codigo_planilla, -1) as codigo_planilla
		,ISNULL(pb.numero_planilla, '') as nro_planilla
		,CASE WHEN ISNULL(pb.es_planilla_jn, 0) = 0 THEN ISNULL(cg.nombre, '') ELSE ISNULL(@v_nombre, '') END AS canal
		,ISNULL(tp.nombre, '') as tipo_planilla
		,CONVERT(VARCHAR(10), l.fecha_inicio, 103) AS fecha_inicio
		,CONVERT(VARCHAR(10), l.fecha_fin, 103)  AS fecha_fin
		,ISNULL(p.nombre_persona, '') + ISNULL(' ' + p.apellido_paterno, '') + ISNULL(' ' + p.apellido_materno, '') AS usuario
		,CONVERT(VARCHAR(10), l.fecha_registra, 103) + ' ' + CONVERT(VARCHAR(8), l.fecha_registra, 108) AS fecha_registra
	FROM
		dbo.log_proceso_bono_cabecera l
	LEFT JOIN 
		dbo.planilla_bono pb
		ON pb.codigo_planilla = l.codigo_planilla
	LEFT JOIN dbo.canal_grupo cg
		ON cg.codigo_canal_grupo = l.codigo_canal
	LEFT JOIN dbo.tipo_planilla tp
		ON tp.codigo_tipo_planilla = l.codigo_tipo_planilla
	LEFT JOIN dbo.usuario u 
		ON u.codigo_usuario = l.usuario_registra
	LEFT JOIN dbo.persona p
		ON p.codigo_persona = u.codigo_persona
	WHERE
		l.fecha_registra BETWEEN CONVERT(DATETIME, @p_fecha_inicio + ' 00:00:00') AND CONVERT(DATETIME, @p_fecha_fin + ' 23:59:59')
	ORDER BY
		l.fecha_registra DESC
END