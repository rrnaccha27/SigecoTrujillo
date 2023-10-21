CREATE PROCEDURE dbo.up_personal_canal_grupo_replicacion
(
	@p_codigo_personal		INT,
	@p_codigo_canal_grupo	INT
)
AS
BEGIN

	DECLARE
		@v_cantidad_canal_grupo	INT
		,@v_es_supervisor		BIT
		,@v_codigo_personal		INT
		,@v_codigo_supervisor	VARCHAR(10)
		,@v_nombre_supervisor	VARCHAR(250)
		,@v_apellido_paterno	VARCHAR(250)
		,@v_apellido_materno	VARCHAR(250)
		,@v_codigo_canal_grupo	INT
		,@v_codigo_grupo		VARCHAR(10)
		,@v_nombre_grupo		VARCHAR(250)
		,@v_codigo_canal		VARCHAR(10)
		,@v_nombre_canal		VARCHAR(250)

	SELECT 
		@v_cantidad_canal_grupo = COUNT(codigo_canal_grupo)
	FROM
		dbo.personal_canal_grupo pcg
	WHERE
		pcg.codigo_personal = @p_codigo_personal
		AND (pcg.es_supervisor_canal = 1 OR pcg.es_supervisor_grupo = 1)
	
	SET @v_es_supervisor = CASE WHEN ISNULL(@v_cantidad_canal_grupo, 0) = 0 THEN 0 ELSE 1 END 

	SELECT
		@v_codigo_supervisor = ''
		,@v_nombre_supervisor = ''
		,@v_apellido_paterno = ''
		,@v_apellido_materno = ''

	IF (@v_es_supervisor = 0)
	BEGIN
	
		SELECT 
			@v_codigo_canal_grupo = codigo_canal_grupo
		FROM
			dbo.personal_canal_grupo pcg
		WHERE
			pcg.codigo_personal = @p_codigo_personal
			
		SELECT 
			@v_codigo_personal = codigo_personal
		FROM
			dbo.personal_canal_grupo pcg
		WHERE
			pcg.codigo_canal_grupo = @v_codigo_canal_grupo
			AND (pcg.es_supervisor_canal = 1 OR pcg.es_supervisor_grupo = 1)	
	
		SELECT
			@v_codigo_supervisor = codigo_equivalencia
			,@v_nombre_supervisor = nombre
			,@v_apellido_paterno = apellido_paterno
			,@v_apellido_materno = apellido_materno
		FROM
			dbo.personal
		WHERE
			codigo_personal = @v_codigo_personal
	
	END

	SELECT
		@v_codigo_grupo = CASE WHEN cg1.es_canal_grupo = 1 THEN '' ELSE cg1.codigo_equivalencia END
		,@v_nombre_grupo = CASE WHEN cg1.es_canal_grupo = 1 THEN '' ELSE cg1.nombre END
		,@v_codigo_canal = CONVERT(VARCHAR(10), cg2.codigo_canal_grupo)
		,@v_nombre_canal = cg2.nombre 
	FROM
		dbo.personal_canal_grupo pcg
	INNER JOIN dbo.canal_grupo cg1
		ON cg1.codigo_canal_grupo = pcg.codigo_canal_grupo
	INNER JOIN dbo.canal_grupo cg2
		ON	cg2.codigo_canal_grupo = pcg.codigo_canal
	WHERE
		pcg.codigo_canal_grupo = @p_codigo_canal_grupo
		AND pcg.codigo_personal = @p_codigo_personal

	SELECT
		codigo_equivalencia
		,nombre
		,apellido_paterno
		,apellido_materno
		,CASE WHEN @v_es_supervisor = 1 THEN 'Y' ELSE 'N' END AS flag_supervisor
		,@v_codigo_grupo AS codigo_grupo
		,@v_nombre_grupo AS nombre_grupo
		,@v_codigo_canal AS codigo_canal
		,@v_nombre_canal AS nombre_canal
		,@v_codigo_supervisor AS codigo_supervisor
		,@v_nombre_supervisor AS nombre_supervisor
		,@v_apellido_paterno AS apellido_paterno_supervisor
		,@v_apellido_materno AS apellido_materno_supervisor
	FROM
		dbo.personal p
	WHERE
		codigo_personal = @p_codigo_personal

END