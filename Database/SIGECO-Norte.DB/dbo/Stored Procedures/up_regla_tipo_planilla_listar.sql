CREATE PROC up_regla_tipo_planilla_listar
AS
BEGIN
	SELECT  
		r.codigo_regla_tipo_planilla,	
		r.nombre ,
		tp.nombre as nombre_tipo_planilla,
		r.descripcion, 
		(case when r.estado_registro=1 then 'Activo' else 'Inactivo' end) nombre_estado_registro,
		r.fecha_registra,	
		dbo.fn_obtener_nombre_usuario(r.usuario_registra) usuario_registra,
		case when afecto_doc_completa = 1 then 'Si' else 'No' end as afecto_doc_completa
	FROM 
		dbo.regla_tipo_planilla r 
	INNER JOIN 
		tipo_planilla tp on r.codigo_tipo_planilla=tp.codigo_tipo_planilla
	ORDER BY 
		r.estado_registro desc, r.nombre
END;