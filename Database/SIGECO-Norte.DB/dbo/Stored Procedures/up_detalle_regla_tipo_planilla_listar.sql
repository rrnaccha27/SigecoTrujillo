CREATE PROCEDURE up_detalle_regla_tipo_planilla_listar
(
	@p_codigo_regla_tipo_planilla int
)
AS
BEGIN
	SELECT  
		codigo_regla_tipo_planilla,	
		codigo_detalle_regla_tipo_planilla,
		codigo_canal,
		codigo_tipo_venta ,
		codigo_empresa,	
		codigo_campo_santo,
		(case when estado_registro=1 then 'Activo' else 'Inactivo' end) nombre_estado_registro,
		fecha_registra,	
		dbo.fn_obtener_nombre_usuario(usuario_registra) usuario_registra
	FROM 
		dbo.detalle_regla_tipo_planilla 
	WHERE 
		codigo_regla_tipo_planilla=@p_codigo_regla_tipo_planilla;
END;