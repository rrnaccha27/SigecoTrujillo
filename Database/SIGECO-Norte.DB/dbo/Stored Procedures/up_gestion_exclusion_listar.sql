CREATE PROCEDURE [dbo].[up_gestion_exclusion_listar]
AS
BEGIN
	select 
		dp.codigo_planilla,	
		p.numero_planilla,
		p.fecha_registra,
		p.fecha_inicio,
		p.fecha_fin,
		--------------------------
		p_dest.numero_planilla as numero_planilla_incluido,
		p_dest.fecha_registra as fecha_registra_incluido,
		p_dest.fecha_inicio as fecha_inicio_incluido,
		p_dest.fecha_fin as fecha_fin_incluido,	 
		-------------------------
		dp.nro_contrato,
		per.codigo_personal,
		per.apellido_materno,
		per.apellido_paterno,
		per.nombre as nombre_persona,
		ec.codigo_estado_cuota,
		ec.nombre as nombre_estado_cuota,
		(case when ecp.estado_exclusion=1 then 'Activo' else 'Inactivo' end) nombre_estado_exclusion ,
		------------------------------------------
		ecp.codigo_exclusion,
		ecp.codigo_detalle_cronograma,
		ecp.codigo_detalle_planilla,
		dbo.fn_obtener_nombre_usuario(ecp.usuario_exclusion) usuario_exclusion,
		ecp.fecha_exclusion ,
		ecp.motivo_exclusion,
		dbo.fn_obtener_nombre_usuario(ecp.usuario_habilita) usuario_habilita,
		ecp.fecha_habilita ,
		ecp.motivo_habilita ,
		cast(ecp.estado_exclusion as int)codigo_estado_exclusion,
		dp.nro_cuota,
		e.nombre as nombre_empresa
	from detalle_planilla dp
	inner join exclusion_cuota_planilla ecp on dp.codigo_detalle_planilla=ecp.codigo_detalle_planilla
	inner join planilla p on dp.codigo_planilla=p.codigo_planilla	
	inner join personal per on per.codigo_personal=dp.codigo_personal	
	inner join empresa_sigeco e on dp.codigo_empresa = e.codigo_empresa
	left join detalle_cronograma dc on dc.codigo_detalle=ecp.codigo_detalle_cronograma
	left join estado_cuota ec on ec.codigo_estado_cuota=dc.codigo_estado_cuota	
	left join planilla p_dest on ecp.codigo_planilla_destino=p_dest.codigo_planilla
	where 
		dp.excluido = 1
	order by
		ecp.codigo_exclusion desc;
END;