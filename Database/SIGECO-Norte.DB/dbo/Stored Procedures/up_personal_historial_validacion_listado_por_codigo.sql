USE SIGECO
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[up_personal_historial_validacion_listado_por_codigo]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].up_personal_historial_validacion_listado_por_codigo
GO

CREATE PROCEDURE dbo.up_personal_historial_validacion_listado_por_codigo
(
	@p_codigo_personal	INT
)
AS
BEGIN
	
	SELECT
		id,
		'VALIDADO' as texto_registra
		,dbo.fn_formatear_fecha_hora(fecha_registra) as fecha_registra
		,ISNULL(usuario_registra, '') as usuario_registra
		,CASE WHEN estado_registro = 0 THEN 'NO VALIDADO' ELSE '' END as texto_modifica
		,dbo.fn_formatear_fecha_hora(fecha_modifica) as fecha_modifica
		,ISNULL(usuario_modifica, '') as usuario_modifica
	FROM 
		dbo.personal_historial_validacion 
	WHERE 
		codigo_personal = @p_codigo_personal
	ORDER BY
		fecha_registra desc, fecha_modifica desc
END