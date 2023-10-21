CREATE PROCEDURE dbo.up_log_contrato_sap_listado
(
	 @p_inicio			VARCHAR(8)
	,@p_fin				VARCHAR(8)
	,@p_codigo_canal	VARCHAR(100)
)
AS
BEGIN

	SELECT
		nro_contrato
		,codigo_empresa
		,nombre_empresa
		,fecha_contrato
		,fecha_migracion
		,fecha_proceso
		,estado
		,usuario
		,codigo_estado_proceso
		,codigo_empresa_sigeco
		,identificador_contrato
	FROM(
		SELECT DISTINCT
			cm.NumAtCard as nro_contrato
			,cm.Codigo_empresa as codigo_empresa
			,e.nombre as nombre_empresa
			,CONVERT(VARCHAR(10), cc.CreateDate, 103) + ' ' + CONVERT(VARCHAR(8), cc.CreateDate, 108) AS fecha_contrato
			,CONVERT(VARCHAR(10), cm.Fec_Creacion, 103) + ' ' + CONVERT(VARCHAR(8), cm.Fec_Creacion, 108) AS fecha_migracion
			,CONVERT(VARCHAR(10), cm.Fec_Proceso, 103) + ' ' + CONVERT(VARCHAR(8), cm.Fec_Proceso, 108) AS fecha_proceso
			,ep.nombre AS estado
			,dbo.fn_obtener_nombre_usuario(cm.codigo_usuario) AS usuario
			,cm.codigo_estado_proceso
			,e.codigo_empresa as codigo_empresa_sigeco
			,cm.NumAtCard + '-'  + cm.Codigo_empresa as identificador_contrato	
			,codigo_canal
			,CreateDate
		FROM
			dbo.contrato_migrado cm
		INNER JOIN 
			dbo.cabecera_contrato cc
			ON cc.Codigo_empresa = cm.Codigo_empresa AND cc.NumAtCard = cm.NumAtCard
		LEFT JOIN 
			dbo.empresa_sigeco e
			ON e.codigo_equivalencia = cm.Codigo_empresa
		LEFT JOIN dbo.estado_proceso ep
			ON ep.codigo_estado_proceso = cm.codigo_estado_proceso
		LEFT JOIN dbo.personal p
			ON p.codigo_equivalencia = cc.Cod_Supervisor
		LEFT JOIN dbo.personal_canal_grupo pcg 
			ON pcg.codigo_personal = p.codigo_personal and pcg.estado_registro = 1
		WHERE
			cc.CreateDate BETWEEN CONVERT(DATETIME, @p_inicio + ' 00:00:00') AND CONVERT(DATETIME, @p_fin + ' 23:59:59')
	) temp
	LEFT JOIN dbo.fn_SplitReglaTipoPlanilla(@p_codigo_canal) canal
		on canal.codigo = isnull(temp.codigo_canal, 99)
	WHERE
		1 = (CASE WHEN LEN(@p_codigo_canal) = 0 THEN 2 ELSE 1 END)
		AND (LEN(@p_codigo_canal) > 0 AND ISNULL(canal.codigo, 0) > 0)
	ORDER BY
		CreateDate ASC, codigo_empresa, nro_contrato;
		
END;