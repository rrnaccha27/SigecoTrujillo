USE SIGECO
GO

IF OBJECT_ID('dbo.vw_planilla_bono_trimestral', 'V') IS NOT NULL 
	DROP VIEW dbo.vw_planilla_bono_trimestral;
GO

CREATE VIEW dbo.vw_planilla_bono_trimestral
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
		CONVERT(VARCHAR, pl.anio_periodo) + pt.nombre as periodo,
		rbt.descripcion as tipo_planilla,
		pl.codigo_estado_planilla,
		epl.nombre as estado_planilla,
		pl.fecha_registra,
		pl.fecha_modifica,
		pl.usuario_registra,
		pl.usuario_modifica
		--pl.codigo_regla_tipo_planilla 
	FROM 
		planilla_bono_trimestral pl 
	INNER JOIN regla_bono_trimestral rbt on pl.codigo_regla = rbt.codigo_regla
	INNER JOIN periodo_trimestral pt on pt.codigo_periodo = pl.codigo_periodo
	INNER JOIN estado_planilla epl on epl.codigo_estado_planilla = pl.codigo_estado_planilla
	LEFT JOIN usuario u1 on u1.estado_registro = 'A' and pl.usuario_apertura = u1.codigo_usuario
	LEFT JOIN persona p1 on p1.estado_registro = 1 and u1.codigo_persona = p1.codigo_persona
	LEFT JOIN usuario u2 on u2.estado_registro = 'A' and pl.usuario_cierre = u2.codigo_usuario
	LEFT JOIN persona p2 on p2.estado_registro = 1 and u2.codigo_persona = p2.codigo_persona
	LEFT JOIN usuario u3 on u3.estado_registro = 'A' and pl.usuario_anulacion = u3.codigo_usuario
	LEFT JOIN persona p3 on p3.estado_registro = 1 and u3.codigo_persona = p3.codigo_persona;
