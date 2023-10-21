USE SIGECO
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[up_planilla_bono_trimestral_listar]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].up_planilla_bono_trimestral_listar
GO

CREATE PROCEDURE [dbo].[up_planilla_bono_trimestral_listar]
AS
BEGIN
	SET NOCOUNT ON

	SELECT 
		pl.codigo_planilla,
		isnull(pl.numero_planilla,' ') numero_planilla,  
		convert(varchar, pl.fecha_anulacion, 103) as fecha_anulacion,
		pl.usuario_anulacion,
		pl.fecha_apertura,
		pl.usuario_apertura,
		pl.fecha_cierre,
		pl.usuario_cierre,

		pl.codigo_regla as codigo_regla_bono,
		rb.descripcion as nombre_regla_bono,

		pl.codigo_tipo_bono,
		tp.nombre as nombre_tipo_bono,
  
		pl.codigo_estado_planilla,
		ep.nombre as nombre_estado_planilla,
  
		pl.fecha_registra,
		pl.fecha_modifica,
		pl.usuario_registra,
		pl.usuario_modifica ,

		pt.nombre as periodo,
		pl.anio_periodo,
		p.valor as estilo  
	FROM 
		planilla_bono_trimestral pl 
	INNER JOIN estado_planilla ep 
		on pl.codigo_estado_planilla=ep.codigo_estado_planilla
	INNER JOIN tipo_bono_trimestral tp 
		on tp.codigo_tipo_bono=pl.codigo_tipo_bono
	INNER JOIN regla_bono_trimestral rb 
		on rb.codigo_regla = pl.codigo_regla
	INNER JOIN dbo.fn_split_parametro_sistema('21,22,23') p 
		on pl.codigo_estado_planilla = p.codigo
	INNER JOIN dbo.periodo_trimestral pt 
		ON pt.codigo_periodo = pl.codigo_periodo
	ORDER BY 
		pl.codigo_planilla desc;

	SET NOCOUNT OFF
END;
