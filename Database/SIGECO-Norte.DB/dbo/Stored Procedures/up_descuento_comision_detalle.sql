CREATE PROCEDURE dbo.up_descuento_comision_detalle
(
	@p_codigo_descuento_comision	INT
)
AS
BEGIN
	SET NOCOUNT ON

	SELECT TOP 1
		d.codigo_descuento_comision
		,p.nombre + ISNULL(' ' + p.apellido_paterno, '') + ISNULL(' ' + p.apellido_materno, '') AS nombre_personal
		,e.nombre AS nombre_empresa
		,d.monto
		,d.saldo
		,CASE WHEN d.estado_registro = 0 THEN 'Inactivo' ELSE 'Activo' END AS nombre_estado_registro
		,CONVERT(VARCHAR, d.fecha_registra, 103) + ' ' + CONVERT(VARCHAR, d.fecha_registra, 108)  AS fecha_registra
		,dbo.fn_obtener_nombre_usuario(d.usuario_registra) AS usuario_registra
		,CASE WHEN d.estado_registro = 0 THEN CONVERT(VARCHAR, d.fecha_modifica, 103) + ' ' + CONVERT(VARCHAR, d.fecha_modifica, 108) ELSE '' END AS fecha_desactiva
		,CASE WHEN d.estado_registro = 0 THEN dbo.fn_obtener_nombre_usuario(d.usuario_modifica) ELSE '' END AS usuario_desactiva
		,d.motivo
	FROM 
		dbo.descuento_comision d
	INNER JOIN  
		dbo.personal p ON p.codigo_personal = d.codigo_personal
	INNER JOIN  
		dbo.empresa_sigeco e ON e.codigo_empresa = d.codigo_empresa
	WHERE 
		d.codigo_descuento_comision = @p_codigo_descuento_comision
	
	SET NOCOUNT OFF	
END;