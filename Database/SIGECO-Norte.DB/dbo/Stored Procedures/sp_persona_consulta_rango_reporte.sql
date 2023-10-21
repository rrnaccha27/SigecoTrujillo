CREATE PROC dbo.sp_persona_consulta_rango_reporte
@estado_registro bit,
@fecha_inicio datetime,
@fecha_fin datetime
AS
BEGIN

	SELECT
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
		p.estado_registro = @estado_registro
		AND
		p.fecha_registra between @fecha_inicio AND @fecha_fin

END