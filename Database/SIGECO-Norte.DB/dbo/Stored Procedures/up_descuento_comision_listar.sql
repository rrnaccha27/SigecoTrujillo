CREATE PROCEDURE dbo.up_descuento_comision_listar
AS
BEGIN
	SET NOCOUNT ON

	SELECT
		d.codigo_descuento_comision
		,p.nombre + ISNULL(' ' + p.apellido_paterno, '') + ISNULL(' ' + p.apellido_materno, '') AS nombre_personal
		,e.nombre AS nombre_empresa
		,d.monto
		,d.saldo
		,CONVERT(INT, d.estado_registro) AS estado_registro
		,CASE WHEN d.estado_registro = 0 THEN 'Inactivo' ELSE 'Activo' END AS nombre_estado_registro
		,CONVERT(VARCHAR, d.fecha_registra, 103) AS fecha_registra
		,dbo.fn_obtener_nombre_usuario(d.usuario_registra) as usuario_registra
		,d.motivo
	FROM 
		dbo.descuento_comision d
	INNER JOIN  
		dbo.personal p ON p.codigo_personal = d.codigo_personal
	INNER JOIN  
		dbo.empresa_sigeco e ON e.codigo_empresa = d.codigo_empresa
	ORDER BY
		d.fecha_registra DESC
	
	SET NOCOUNT OFF	
END;