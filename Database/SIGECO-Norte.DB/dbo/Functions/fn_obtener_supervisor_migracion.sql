CREATE function [dbo].[fn_obtener_supervisor_migracion]
(
	@p_codigo_registro	INT
	,@p_num_contrato	VARCHAR(100)
)
RETURNS INT
BEGIN
	DECLARE 
		@v_codigo_canal_grupo	INT
		,@v_codigo_registro		INT
		,@v_C_SUPERVISOR		VARCHAR(10)
		,@v_codigo_personal		INT

	SELECT TOP 1 @v_C_SUPERVISOR = C_SUPERVISOR
	FROM dbo.CONTRATO_DB2
	WHERE 	
		NUM_CONTRATO = @p_num_contrato

	SELECT TOP 1 @v_codigo_personal = codigo_personal
	FROM dbo.personal
	WHERE
		codigo_equivalencia = @v_C_SUPERVISOR
	
	SELECT TOP 1 @v_codigo_registro = pcg.codigo_registro 
	FROM dbo.personal_canal_grupo pcg 
	WHERE pcg.codigo_personal = @v_codigo_personal AND (pcg.es_supervisor_canal = 1 OR pcg.es_supervisor_grupo = 1)

	IF (@v_codigo_registro IS NULL)
	BEGIN
		SELECT TOP 1 @v_codigo_canal_grupo = codigo_canal_grupo
		FROM dbo.personal_canal_grupo
		WHERE codigo_registro = @p_codigo_registro

		SELECT TOP 1 @v_codigo_registro = pcg.codigo_registro 
		FROM dbo.personal_canal_grupo pcg 
		WHERE pcg.codigo_canal_grupo = @v_codigo_canal_grupo AND (pcg.es_supervisor_canal = 1 OR pcg.es_supervisor_grupo = 1)
	END

	RETURN ISNULL(@v_codigo_registro, @p_codigo_registro)
END