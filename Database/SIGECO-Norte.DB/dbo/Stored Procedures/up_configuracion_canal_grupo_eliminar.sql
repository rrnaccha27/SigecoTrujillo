CREATE PROCEDURE dbo.up_configuracion_canal_grupo_eliminar
(
	@p_codigo_canal_grupo	INT
)
AS
BEGIN
	
	DELETE FROM 
		dbo.empresa_configuracion
	WHERE
		codigo_configuracion IN (SELECT codigo_configuracion FROM dbo.configuracion_canal_grupo WHERE codigo_canal_grupo = @p_codigo_canal_grupo)

	DELETE FROM 
		dbo.configuracion_canal_grupo
	WHERE
		codigo_canal_grupo = @p_codigo_canal_grupo
	
END