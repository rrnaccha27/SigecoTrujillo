USE SIGECO
GO

IF OBJECT_ID('dbo.vw_planilla_bono', 'V') IS NOT NULL 
	DROP VIEW dbo.vw_planilla_bono;
GO

CREATE VIEW dbo.vw_planilla_bono
AS
	SELECT 
		pl.codigo_planilla,
		pl.numero_planilla,
		--rtp.nombre as nombre_planilla,
		pl.fecha_anulacion,
		--pl.usuario_anulacion,
		p3.nombre_persona + isnull(' ' + p3.apellido_paterno, '') + isnull(' ' + p3.apellido_materno,'') as usuario_anulacion,
		pl.fecha_apertura,
		--pl.usuario_apertura,
		p1.nombre_persona + isnull(' ' + p1.apellido_paterno, '') + isnull(' ' + p1.apellido_materno,'') as usuario_apertura,
		pl.fecha_cierre,
		--pl.usuario_cierre,
		p2.nombre_persona + isnull(' ' + p2.apellido_paterno, '') + isnull(' ' + p2.apellido_materno,'') as usuario_cierre,
		pl.fecha_inicio,
		pl.fecha_fin,
		pl.codigo_tipo_planilla,
		tpl.nombre as tipo_planilla,
		pl.codigo_estado_planilla,
		epl.nombre as estado_planilla,
		pl.estado_registro,
		pl.fecha_registra,
		pl.fecha_modifica,
		pl.usuario_registra,
		pl.usuario_modifica,
		pl.es_planilla_jn
	FROM 
		planilla_bono pl 
	--INNER JOIN regla_tipo_planilla rtp on pl.codigo_regla_tipo_planilla = rtp.codigo_regla_tipo_planilla
	INNER JOIN tipo_planilla tpl on tpl.codigo_tipo_planilla = pl.codigo_tipo_planilla
	INNER JOIN estado_planilla epl on epl.codigo_estado_planilla = pl.codigo_estado_planilla
	LEFT JOIN usuario u1 on u1.estado_registro = 'A' and pl.usuario_apertura = u1.codigo_usuario
	LEFT JOIN persona p1 on p1.estado_registro = 1 and u1.codigo_persona = p1.codigo_persona
	LEFT JOIN usuario u2 on u2.estado_registro = 'A' and pl.usuario_cierre = u2.codigo_usuario
	LEFT JOIN persona p2 on p2.estado_registro = 1 and u2.codigo_persona = p2.codigo_persona
	LEFT JOIN usuario u3 on u3.estado_registro = 'A' and pl.usuario_anulacion = u3.codigo_usuario
	LEFT JOIN persona p3 on p3.estado_registro = 1 and u3.codigo_persona = p3.codigo_persona;