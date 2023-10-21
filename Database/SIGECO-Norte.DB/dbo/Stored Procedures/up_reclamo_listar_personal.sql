CREATE PROCEDURE dbo.up_reclamo_listar_personal
(
	@p_codigo_usuario	VARCHAR(50)
)
AS
BEGIN
	SET NOCOUNT ON

	DECLARE
		@v_codigo_personal	INT = 0
	
	DECLARE
		@t_grupos TABLE(
			codigo_grupo	int
		)

	SET @v_codigo_personal = ISNULL((SELECT TOP 1 codigo_personal FROM dbo.usuario_personal WHERE codigo_usuario = @p_codigo_usuario), 0)

	IF (@v_codigo_personal <> 0)
	BEGIN
		INSERT INTO
			@t_grupos
		SELECT
			codigo_grupo
		FROM
			vw_personal
		WHERE
			estado_persona = 1
			AND codigo_personal = @v_codigo_personal
	END

	SELECT 
		p.codigo_personal
		,p.codigo_equivalencia
		,p.nombre_personal
		,p.codigo_canal
		,p.nombre_canal
		,p.codigo_grupo
		,p.nombre_grupo 
	FROM 
		vw_personal p
	WHERE 
		p.estado_persona = 1
		and p.es_supervisor_canal = 0
		and p.es_supervisor_grupo = 0
		AND (@v_codigo_personal = 0 or (@v_codigo_personal > 0 AND p.codigo_grupo IN (select codigo_grupo from @t_grupos)))
	ORDER BY
		p.nombre_personal ASC

	SET NOCOUNT OFF
END;