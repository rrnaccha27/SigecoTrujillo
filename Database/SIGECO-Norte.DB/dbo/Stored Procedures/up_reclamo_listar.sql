CREATE PROCEDURE [dbo].[up_reclamo_listar]
(
	@NroContrato                VARCHAR(20)
	,@PersonalVentas			VARCHAR(50)
	,@codigo_estado_reclamo     INT
	,@p_codigo_perfil			INT
	,@p_codigo_usuario			VARCHAR(50)
)
AS
BEGIN

	DECLARE
		@v_codigo_personal	INT = 0
	
	SET @v_codigo_personal = ISNULL((SELECT TOP 1 codigo_personal FROM dbo.usuario_personal WHERE codigo_usuario = @p_codigo_usuario), 0)
	
	SELECT 
		n1.codigo_reclamo
		,n1.codigo_personal
		,RTRIM(ISNULL(n2.nombre,'')) + RTRIM(ISNULL(' ' + n2.apellido_paterno,'')) + RTRIM(ISNULL(' ' + n2.apellido_materno,'')) AS PersonalVentas
		,n1.NroContrato
		,n1.codigo_articulo
		,n3.nombre AS Articulo
		,n1.codigo_empresa
		,n4.nombre AS Empresa
		,n1.Cuota
		,n1.Importe

		,n1.atencion_codigo_articulo
		,n3.nombre AS atencion_Articulo
		,n1.atencion_codigo_empresa
		,n4.nombre AS atencion_Empresa
		,n1.atencion_Cuota
		,n1.atencion_Importe

		,n1.codigo_estado_reclamo
		,n5.nombre_estado_reclamo as Estado
		,n1.codigo_estado_resultado
		,n6.nombre_estado_resultado AS Resultado
		,n1.Observacion
		,n1.Respuesta
		,n1.codigo_planilla
		,n9.numero_planilla
		,n1.usuario_registra
		,n1.fecha_registra
		,n1.usuario_modifica
		,n1.fecha_modifica
		,case when n1.es_contrato_migrado = 1 then 'Si' else 'No' end as error_contrato_migrado
		,ISNULL(n1.codigo_estado_resultado_n1, 0) as codigo_estado_resultado_n1
		,ISNULL(n1.codigo_estado_resultado_n2, 0) as codigo_estado_resultado_n2
		,ISNULL(er1.nombre_estado_resultado, 'Pendiente') as nombre_estado_resultado_n1
		,ISNULL(er2.nombre_estado_resultado, case when n1.codigo_estado_resultado_n1 = 2 then '' else 'Pendiente' end) as nombre_estado_resultado_n2
		,ISNULL(p.valor, '') as estilo
	FROM reclamo n1 WITH (NOLOCK)
	INNER JOIN personal n2 ON n1.codigo_personal=n2.codigo_personal
	INNER JOIN empresa_sigeco n4 ON n1.codigo_empresa=n4.codigo_empresa
	INNER JOIN empresa_sigeco n8 ON n1.atencion_codigo_empresa=n8.codigo_empresa
	LEFT JOIN articulo n3 ON n1.codigo_articulo=n3.codigo_articulo
	LEFT JOIN articulo n7 ON n1.atencion_codigo_articulo=n7.codigo_articulo
	LEFT JOIN estado_reclamo n5 ON n1.codigo_estado_reclamo=n5.codigo_estado_reclamo
	LEFT JOIN estado_resultado n6 ON n1.codigo_estado_resultado=n6.codigo_estado_resultado
	--LEFT JOIN detalle_planilla n10 ON n1.codigo_detalle_cronograma=n10.codigo_detalle_cronograma
	LEFT JOIN planilla n9 ON n1.codigo_planilla=n9.codigo_planilla
	LEFT JOIN estado_resultado er1 on er1.codigo_estado_resultado = n1.codigo_estado_resultado_n1
	LEFT JOIN estado_resultado er2 on er2.codigo_estado_resultado = n1.codigo_estado_resultado_n2
	LEFT JOIN dbo.fn_split_parametro_sistema('25, 26') p on p.codigo = ISNULL(er2.codigo_estado_resultado, 0)
	WHERE
		(Len(@NroContrato) = 0 OR  (Len(@NroContrato) > 0 AND n1.NroContrato LIKE '%' + @NroContrato + '%'))
		AND 
		(@codigo_estado_reclamo = 0 OR (@codigo_estado_reclamo <> 0 AND n1.codigo_estado_reclamo = @codigo_estado_reclamo))
		AND 
		(Len(@PersonalVentas) = 0 OR (Len(@PersonalVentas) > 0 AND RTRIM(ISNULL(n2.apellido_paterno,'')) + ' ' + RTRIM(ISNULL(n2.apellido_materno,'')) + ' ' + RTRIM(ISNULL(n2.nombre,'')) LIKE '%' + @PersonalVentas + '%'))
		AND
		(@v_codigo_personal = 0 OR (@v_codigo_personal > 0 AND n1.usuario_registra = @p_codigo_usuario))
	ORDER BY
		CODIGO_RECLAMO DESC;
END;