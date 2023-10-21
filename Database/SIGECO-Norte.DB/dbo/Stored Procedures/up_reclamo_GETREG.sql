CREATE PROCEDURE [dbo].[up_reclamo_GETREG]
(
	@codigo_reclamo	INT
)
AS
BEGIN
	SET NOCOUNT ON

    SELECT TOP 1
        n1.codigo_reclamo
        ,n1.codigo_personal
		,RTRIM(ISNULL(n2.nombre,'')) + RTRIM(ISNULL(' ' + n2.apellido_paterno,'')) + RTRIM(ISNULL(' ' + n2.apellido_materno,'')) AS PersonalVentas
        ,n1.NroContrato
        ,n1.codigo_articulo
		,ISNULL(n3.nombre, '') AS Articulo
        ,n1.codigo_empresa
		,e.nombre as nombre_empresa
        ,n1.Cuota
        ,n1.Importe

		,n1.atencion_codigo_articulo
		,ISNULL(n4.nombre, '') AS atencion_Articulo
        ,n1.atencion_codigo_empresa
        ,n1.atencion_Cuota
        ,n1.atencion_Importe

        ,n1.codigo_estado_reclamo
		,er.nombre_estado_reclamo
        ,n1.codigo_estado_resultado
        ,n1.Observacion
        ,n1.Respuesta
		,n1.codigo_planilla
		,n9.numero_planilla
        ,n1.usuario_registra
        ,n1.fecha_registra
        ,n1.usuario_modifica
        ,n1.fecha_modifica
		,dbo.fn_obtener_nombre_usuario(n1.usuario_modifica) AS UsuarioAtencion
		,dbo.fn_obtener_nombre_usuario(n1.usuario_registra) AS UsuarioRegistra 
		,convert(varchar, n1.fecha_modifica, 103) + ' ' + convert(varchar, n1.fecha_modifica, 108) AS FechaAtencion
		,convert(varchar, n1.fecha_registra, 103) + ' ' + convert(varchar, n1.fecha_registra, 108) AS FechaRegistra
		,CONVERT(INT, n1.es_contrato_migrado) AS es_contrato_migrado
		,n1.codigo_estado_resultado_n1
		,ISNULL(er1.nombre_estado_resultado, 'Pendiente') as nombre_estado_resultado_n1
		,ISNULL(n1.observacion_n1, '') as observacion_n1
		,dbo.fn_obtener_nombre_usuario(n1.usuario_modifica_n1) AS usuario_n1
		,ISNULL(convert(varchar, n1.fecha_modifica_n1, 103) + ' ' + convert(varchar, n1.fecha_modifica_n1, 108), '') AS fecha_n1
		,ISNULL(er2.nombre_estado_resultado, case when n1.codigo_estado_resultado_n1 = 2 then '' else 'Pendiente' end) as nombre_estado_resultado_n2
		,ISNULL(n1.observacion_n2, '') as observacion_n2
		,dbo.fn_obtener_nombre_usuario(n1.usuario_modifica_n2) AS usuario_n2
		,ISNULL(convert(varchar, n1.fecha_modifica_n2, 103) + ' ' + convert(varchar, n1.fecha_modifica_n2, 108), '') AS fecha_n2
    FROM dbo.reclamo n1 WITH (NOLOCK)
	INNER JOIN dbo.personal n2 ON n1.codigo_personal=n2.codigo_personal
	INNER JOIN dbo.empresa_sigeco e ON e.codigo_empresa = n1.codigo_empresa
	INNER JOIN dbo.estado_reclamo er ON er.codigo_estado_reclamo = n1.codigo_estado_reclamo
	LEFT JOIN dbo.articulo n3 ON n1.codigo_articulo=n3.codigo_articulo
	LEFT JOIN dbo.articulo n4 ON n1.atencion_codigo_articulo=n4.codigo_articulo
	LEFT JOIN dbo.planilla n9 ON n1.codigo_planilla=n9.codigo_planilla
	LEFT JOIN dbo.estado_resultado er1 ON er1.codigo_estado_resultado = n1.codigo_estado_resultado_n1
	LEFT JOIN dbo.estado_resultado er2 ON er2.codigo_estado_resultado = n1.codigo_estado_resultado_n2
    WHERE
        codigo_reclamo = @codigo_reclamo

	SET NOCOUNT OFF
END;