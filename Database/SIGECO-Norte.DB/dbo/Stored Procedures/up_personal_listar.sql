USE SIGECO
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[up_personal_listar]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].up_personal_listar
GO

CREATE PROCEDURE [dbo].[up_personal_listar]
	@p_codigo_canal		INT
	,@p_codigo_grupo	INT
	,@p_estado_registro	int
	,@p_nombre			VARCHAR(50)
AS
BEGIN
	declare @v_estado bit;

    set @v_estado=case when @p_estado_registro=-1 then null else cast(@p_estado_registro as bit) end; 

	--SELECT TOP (@v_Top)
	SELECT
		p.codigo_personal
		,td.nombre_tipo_documento AS nombre_tipo_documento
		,p.nombre
		,ISNULL(p.apellido_paterno,'')+' '+ISNULL(p.apellido_materno,'') AS apellidos
		,CASE WHEN p.es_persona_juridica = 0 THEN p.nro_documento ELSE p.nro_RUC END AS nro_documento
		,p.estado_registro
		,pcg.es_supervisor_canal
		,pcg.es_supervisor_grupo
		,dbo.GetCanalGrupoDes(pcg.codigo_canal_grupo,'C') AS nombre_canal
		,dbo.GetCanalGrupoDes(pcg.codigo_canal_grupo,'G') AS nombre_grupo
		,p.codigo_equivalencia
		,ISNULL(p.nombre,'') + ISNULL(' ' + p.apellido_paterno, '') + ISNULL(' ' + p.apellido_materno, '') as nombre_completo
		,CASE WHEN validado = 1 THEN 'SI' WHEN validado = 0 THEN 'NO' ELSE '' END as validado_texto
	FROM 
		dbo.personal p WITH (NOLOCK)
	INNER JOIN 
		dbo.tipo_documento td 
		ON p.codigo_tipo_documento=td.codigo_tipo_documento
	LEFT JOIN dbo.personal_canal_grupo pcg 
		ON p.codigo_personal = pcg.codigo_personal and pcg.estado_registro = 1
	LEFT JOIN dbo.canal_grupo cg
		ON pcg.codigo_canal_grupo = cg.codigo_canal_grupo
	WHERE
		p.estado_registro =isnull(@v_estado,p.estado_registro)
		AND
		(
			(@p_codigo_canal = -1 AND @p_codigo_grupo = -1) 
			OR 
			((@p_codigo_canal <> -1 AND @p_codigo_grupo = -1) AND ( (cg.codigo_padre = @p_codigo_canal) OR (cg.codigo_canal_grupo = @p_codigo_canal)) )
			OR
			((@p_codigo_canal <> -1 AND @p_codigo_grupo <> -1) AND cg.codigo_canal_grupo = @p_codigo_grupo)
		)
		AND
		(	
			LEN(@p_nombre) = 0 OR (LEN(@p_nombre) > 0 AND p.nombre +' '+ ISNULL(p.apellido_paterno, '') +' '+ ISNULL(p.apellido_materno, '') like '%' + @p_nombre + '%')
		)

END