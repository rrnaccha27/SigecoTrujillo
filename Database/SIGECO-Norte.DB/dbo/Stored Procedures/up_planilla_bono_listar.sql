USE SIGECO
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[up_planilla_bono_listar]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].up_planilla_bono_listar
GO

CREATE PROCEDURE [dbo].[up_planilla_bono_listar]
AS
BEGIN
	SET NOCOUNT ON

 	DECLARE @v_fecha_checklist DATETIME
	SET @v_fecha_checklist = CONVERT(DATETIME, (SELECT valor FROM dbo.parametro_sistema WHERE codigo_parametro_sistema = 30))
 
	SELECT 
		pl.codigo_planilla,
		isnull(pl.numero_planilla,' ') numero_planilla,  
		isnull(cg.nombre,'') as nombre_canal,
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
		p.valor as estilo,
		CONVERT(BIT, 
			CASE WHEN pl.fecha_apertura <= @v_fecha_checklist
				THEN 0 
				ELSE
					--CASE WHEN EXISTS(SELECT chk.codigo_planilla from checklist_bono chk where chk.codigo_planilla = pl.codigo_planilla and chk.codigo_estado_checklist <> 3)
					CASE WHEN pl.codigo_tipo_planilla = 1
					THEN 1
					ELSE 0
				END
			END
		) as envio_liquidacion
	FROM 
		planilla_bono pl 
	INNER JOIN estado_planilla ep 
		on pl.codigo_estado_planilla=ep.codigo_estado_planilla
	INNER JOIN tipo_planilla tp 
		on tp.codigo_tipo_planilla=pl.codigo_tipo_planilla  
	INNER JOIN dbo.fn_split_parametro_sistema('21,22,23') p 
		on pl.codigo_estado_planilla = p.codigo
	LEFT JOIN canal_grupo cg 
		on pl.codigo_canal=cg.codigo_canal_grupo
	WHERE
		pl.estado_registro = 1 AND pl.es_planilla_jn = 0
	ORDER BY 
		pl.codigo_planilla desc;

	SET NOCOUNT OFF
END;