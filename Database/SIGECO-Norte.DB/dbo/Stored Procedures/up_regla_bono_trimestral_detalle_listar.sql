USE SIGECO
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[up_regla_bono_trimestral_detalle_listar]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].up_regla_bono_trimestral_detalle_listar
GO

CREATE PROCEDURE up_regla_bono_trimestral_detalle_listar
(
	@p_codigo_regla	INT
)
AS
BEGIN
	SET NOCOUNT ON

	SELECT  
		codigo_regla,	
		codigo_regla_detalle,
		codigo_canal,
		codigo_tipo_venta ,
		codigo_empresa,	
		estado_registro,
		(case when estado_registro=1 then 'Activo' else 'Inactivo' end) nombre_estado_registro,
		convert(varchar, fecha_registra, 103) + ' ' + convert(varchar, fecha_registra, 108) as fecha_registra,
		dbo.fn_obtener_nombre_usuario(usuario_registra) usuario_registra
	FROM 
		dbo.regla_bono_trimestral_detalle 
	WHERE 
		codigo_regla = @p_codigo_regla;
	
	SET NOCOUNT ON
END
