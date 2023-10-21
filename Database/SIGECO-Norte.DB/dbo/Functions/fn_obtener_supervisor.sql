create function dbo.fn_obtener_supervisor
(
	@p_codigo_registro	int
)
RETURNS INT
BEGIN
	DECLARE 
		@v_codigo_canal_grupo	INT
		,@v_codigo_registro		INT

	SELECT TOP 1 @v_codigo_canal_grupo = codigo_canal_grupo
	FROM dbo.personal_canal_grupo
	WHERE codigo_registro = @p_codigo_registro

	SELECT TOP 1 @v_codigo_registro = pcg.codigo_registro 
	FROM dbo.personal_canal_grupo pcg 
	WHERE pcg.codigo_canal_grupo = @v_codigo_canal_grupo AND (pcg.es_supervisor_canal = 1 OR pcg.es_supervisor_grupo = 1)

	RETURN ISNULL(@v_codigo_registro, @p_codigo_registro)
END