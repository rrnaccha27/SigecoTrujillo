CREATE PROCEDURE dbo.sp_personal_consulta_interface_reporte
	@p_codigo_canal		INT
	,@p_codigo_grupo	INT
	,@p_estado_registro	BIT
AS
BEGIN
	DECLARE @v_Top	INT
	SET @v_Top = CASE WHEN @p_codigo_canal = -1 AND @p_codigo_grupo = -1 THEN 100 ELSE 100000 END

	SELECT TOP (@v_Top)
		p.codigo_personal
		,p.codigo_equivalencia
		,p.nombre
		,p.apellido_paterno
		,p.apellido_materno
		,p.telefono_fijo
		,p.telefono_celular
		,p.correo_electronico
		,p.nro_cuenta
		,p.codigo_interbancario
		,td.nombre_tipo_documento AS nombre_tipo_documento
		,b.nombre nombre_banco
		,CASE WHEN p.es_persona_juridica = 0 THEN p.nro_documento ELSE p.nro_documento END AS nro_documento
		,p.estado_registro
		,pcg.es_supervisor_canal
		,pcg.es_supervisor_grupo
		,dbo.GetCanalGrupoDes(pcg.codigo_canal_grupo,'C') AS nombre_canal
		,dbo.GetCanalGrupoDes(pcg.codigo_canal_grupo,'G') AS nombre_grupo
	FROM 
		dbo.personal p WITH (NOLOCK)
	INNER JOIN 
		dbo.tipo_documento td 
		ON p.codigo_tipo_documento=td.codigo_tipo_documento
	INNER JOIN dbo.banco b
		ON p.codigo_banco = b.codigo_banco
	LEFT JOIN dbo.personal_canal_grupo pcg 
		ON p.codigo_personal = pcg.codigo_personal
	LEFT JOIN dbo.canal_grupo cg
		ON pcg.codigo_canal_grupo = cg.codigo_canal_grupo
	WHERE
		p.estado_registro = @p_estado_registro
		AND
		(
			(@p_codigo_canal = -1 AND @p_codigo_grupo = -1) 
			OR 
			((@p_codigo_canal <> -1 AND @p_codigo_grupo = -1) AND ( (cg.codigo_padre = @p_codigo_canal) OR (cg.codigo_canal_grupo = @p_codigo_canal)) )
			OR
			((@p_codigo_canal <> -1 AND @p_codigo_grupo <> -1) AND cg.codigo_canal_grupo = @p_codigo_grupo)
		)

END