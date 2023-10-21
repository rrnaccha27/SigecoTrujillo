USE SIGECO
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[up_regla_bono_trimestral_listar]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].up_regla_bono_trimestral_listar
GO

CREATE PROCEDURE dbo.up_regla_bono_trimestral_listar
AS
BEGIN
	SET NOCOUNT ON

	SELECT  
		r.codigo_regla,	
		r.descripcion,
		r.codigo_tipo_bono,
		tpr.nombre as nombre_tipo_bono,
		r.estado_registro,
		(case when r.estado_registro=1 then 'Activo' else 'Inactivo' end) nombre_estado_registro,
		convert(varchar, r.fecha_registra, 103) + ' ' + convert(varchar, r.fecha_registra, 108) as fecha_registra,	
		dbo.fn_obtener_nombre_usuario(r.usuario_registra) usuario_registra,
		CONVERT(VARCHAR, r.vigencia_inicio, 103) + '  -  ' + CONVERT(VARCHAR, r.vigencia_fin, 103) as vigencia
	FROM 
		dbo.regla_bono_trimestral r 
	INNER JOIN 
		dbo.tipo_bono_trimestral tpr on r.codigo_tipo_bono = tpr.codigo_tipo_bono
	ORDER BY 
		r.estado_registro desc, r.codigo_tipo_bono, r.descripcion

	SET NOCOUNT OFF
END
