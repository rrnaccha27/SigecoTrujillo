CREATE PROC [dbo].up_personal_listado_comision_manual
(
	 @p_nombre	VARCHAR(50)
)
AS
BEGIN

	SELECT
		p.codigo_personal
		,p.nombre + ' ' + ISNULL(p.apellido_paterno, '') + ' ' + ISNULL(p.apellido_materno, '') AS nombres
		,pcg.codigo_canal
		,cg.nombre as nombre_canal
		,p.codigo_equivalencia
	FROM 
		dbo.personal p
	INNER JOIN
		dbo.personal_canal_grupo pcg
		ON pcg.estado_registro =1 AND pcg.codigo_personal = p.codigo_personal
	INNER JOIN
		dbo.canal_grupo cg
		ON cg.estado_registro = 1 AND cg.codigo_canal_grupo = pcg.codigo_canal
	WHERE
		p.estado_registro = 1
		AND 
		(
			LEN(@p_nombre) = 0 OR 
			(LEN(@p_nombre) > 0 AND p.nombre + ' ' + ISNULL(p.apellido_paterno, '') + ' ' + ISNULL(p.apellido_materno, '') like '%' + @p_nombre + '%')
		)
	ORDER BY
		nombres
END