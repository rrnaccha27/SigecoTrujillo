CREATE PROCEDURE dbo.up_personal_validacion_supervisor
(
	@p_codigo_personal		INT
	,@p_codigo_canal_grupo	INT
)
AS
BEGIN
	DECLARE 
		@v_codigo_supervisor int,
		@v_nombre_supervisor varchar(300),
		@v_nombre_canal_grupo varchar(100)
	

	SELECT TOP 1
		@v_codigo_supervisor = codigo_personal
	FROM
		dbo.personal_canal_grupo
	WHERE
		codigo_personal <> @p_codigo_personal
		AND codigo_canal_grupo = @p_codigo_canal_grupo
		AND (es_supervisor_canal = 1 OR es_supervisor_grupo = 1)
		AND estado_registro = 1

	SET @v_codigo_supervisor = ISNULL(@v_codigo_supervisor, 0)

	IF (@v_codigo_supervisor > 0)
	BEGIN
		SELECT TOP 1
			@v_nombre_supervisor = nombre + ' ' + ISNULL(apellido_paterno, '') + ' ' + ISNULL(apellido_materno, '')
		FROM
			dbo.personal
		WHERE
			codigo_personal = @v_codigo_supervisor

		SELECT TOP 1
			@v_nombre_canal_grupo = nombre
		FROM
			dbo.canal_grupo
		WHERE
			codigo_canal_grupo = @p_codigo_canal_grupo

		SELECT
			@v_nombre_supervisor AS nombre_supervisor
			,@v_nombre_canal_grupo AS nombre_canal_grupo
	END
END