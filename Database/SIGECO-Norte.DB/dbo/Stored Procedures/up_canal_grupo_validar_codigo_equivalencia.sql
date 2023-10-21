CREATE PROCEDURE dbo.up_canal_grupo_validar_codigo_equivalencia
(
	@p_codigo_canal_grupo	INT
	,@p_codigo_equivalencia	VARCHAR(4)
)
AS
BEGIN

	SELECT TOP 1 
		codigo_canal_grupo 
	FROM 
		dbo.canal_grupo 
	WHERE 
		codigo_canal_grupo <> @p_codigo_canal_grupo 
		AND codigo_equivalencia = @p_codigo_equivalencia 
		AND estado_registro = 1

END