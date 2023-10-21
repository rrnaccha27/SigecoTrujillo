CREATE PROCEDURE dbo.up_reclamo_obtener_pendientes
(
	@p_usuario	VARCHAR(50)
)
AS
BEGIN
	SET NOCOUNT ON
	DECLARE
		@c_permiso_atender_N1	INT = 26
		,@c_permiso_atender_N2	INT = 27
		,@c_permito_total		INT = 1

	DECLARE 
		 @v_cantidad_pendientes		INT = 0
		 ,@v_codigo_perfil_usuario	INT
		 ,@v_nivel					INT = 0
	
	SET @v_codigo_perfil_usuario = (SELECT TOP 1 codigo_perfil_usuario FROM dbo.usuario WHERE codigo_usuario = @p_usuario)

	IF EXISTS
		(SELECT 
			* 
		FROM 
			item_tipo_acceso
		WHERE 
			estado_registro = 1
			AND codigo_perfil_usuario = @v_codigo_perfil_usuario
			AND codigo_tipo_acceso_item = @c_permiso_atender_N1)
	BEGIN
		SET @v_nivel = 1
	END

	IF EXISTS
		(SELECT 
			* 
		FROM 
			item_tipo_acceso
		WHERE 
			estado_registro = 1
			AND codigo_perfil_usuario = @v_codigo_perfil_usuario
			AND codigo_tipo_acceso_item = @c_permiso_atender_N2)
	BEGIN
		SET @v_nivel = 2
	END

	IF EXISTS
		(SELECT 
			* 
		FROM 
			item_tipo_acceso
		WHERE 
			estado_registro = 1
			AND codigo_perfil_usuario = @v_codigo_perfil_usuario
			AND codigo_tipo_acceso_item = @c_permito_total)
	BEGIN
		SET @v_nivel = 0
	END

	SELECT 
		@v_cantidad_pendientes = COUNT(codigo_reclamo)
	FROM
		dbo.reclamo
	WHERE
		(@v_nivel = 1 AND ISNULL(codigo_estado_resultado_n1, 0) = 0)
		OR 
		(@v_nivel = 2  AND ISNULL(codigo_estado_resultado_n1, 0) = 1 AND codigo_estado_resultado_n2 IS NULL)

	SELECT 
		ISNULL(@v_cantidad_pendientes, 0) AS [cantidad_pendientes]

	SET NOCOUNT OFF
END;