USE SIGECO
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[up_regla_bono_trimestral_obtener_by_id]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].up_regla_bono_trimestral_obtener_by_id
GO

CREATE PROCEDURE dbo.up_regla_bono_trimestral_obtener_by_id
(
	@p_codigo_regla int
)
AS
BEGIN
	SET NOCOUNT ON

	SELECT TOP 1
		r.codigo_regla,	
		r.codigo_tipo_bono,
		tpr.nombre as nombre_tipo_bono,
		r.descripcion,	
		r.estado_registro,
		(case when r.estado_registro = 1 then 'Activo' else 'Inactivo' end) nombre_estado_registro,
		convert(varchar, r.fecha_registra, 103) + ' ' + convert(varchar, r.fecha_registra, 108) as fecha_registra,	
		dbo.fn_obtener_nombre_usuario(r.usuario_registra) usuario_registra,
		convert(varchar, r.fecha_modifica, 103) + ' ' + convert(varchar, r.fecha_modifica, 108) as fecha_modifica,
		dbo.fn_obtener_nombre_usuario(r.usuario_modifica) usuario_modifica,
		CONVERT(VARCHAR, r.vigencia_inicio, 112) AS vigencia_inicio,
		CONVERT(VARCHAR, r.vigencia_fin, 112) AS vigencia_fin
	FROM 
		dbo.regla_bono_trimestral r
	INNER JOIN dbo.tipo_bono_trimestral tpr
		ON tpr.codigo_tipo_bono = r.codigo_tipo_bono
	WHERE 
		r.codigo_regla = @p_codigo_regla;

	SET NOCOUNT ON
END
