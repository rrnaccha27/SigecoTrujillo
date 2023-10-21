CREATE PROCEDURE dbo.up_personal_listado_canal_grupo
(
	@p_codigo_canal_grupo	INT
)
AS
BEGIN
	SELECT
		pcg.codigo_registro
		,p.codigo_personal
		,p.nombre + ' ' + ISNULL(p.apellido_paterno, '') + ' ' + ISNULL(p.apellido_materno, '') AS nombre
		,CONVERT(VARCHAR(10), pcg.fecha_registra, 103) AS fecha_registra
		,CASE WHEN ISNULL(u.codigo_persona, 0) = 0 THEN pcg.usuario_registra ELSE p_sigees.nombre_persona + ' ' + ISNULL(p_sigees.apellido_paterno, '') + ' ' + ISNULL(p_sigees.apellido_materno, '') END AS usuario_registra
		,CASE WHEN pcg.es_supervisor_canal = 1 THEN pcg.es_supervisor_canal ELSE pcg.es_supervisor_grupo END  AS es_supervisor
	FROM
		dbo.personal p
	INNER JOIN 
		dbo.personal_canal_grupo pcg
		ON p.estado_registro = 1 AND pcg.estado_registro = 1 AND pcg.codigo_personal = p.codigo_personal 
	LEFT JOIN
		dbo.usuario u
		ON u.codigo_usuario = pcg.usuario_registra
	LEFT JOIN
		dbo.persona p_sigees
		ON p_sigees.codigo_persona = u.codigo_persona
	WHERE
		pcg.codigo_canal_grupo = @p_codigo_canal_grupo

	ORDER BY es_supervisor DESC, nombre asc
END