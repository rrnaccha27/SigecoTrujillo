CREATE PROCEDURE dbo.up_personal_canal_grupo_cantidad
(
	@p_codigo_personal		INT
	,@p_codigo_canal_grupo	INT
	,@p_cantidad_canales	INT OUTPUT
)
AS
BEGIN
	SET @p_cantidad_canales = 0
	
	SELECT
		@p_cantidad_canales = COUNT(codigo_registro)
	FROM
		dbo.personal_canal_grupo
	WHERE
		codigo_personal = @p_codigo_personal
		AND codigo_canal_grupo <> @p_codigo_canal_grupo
		AND estado_registro = 1
END