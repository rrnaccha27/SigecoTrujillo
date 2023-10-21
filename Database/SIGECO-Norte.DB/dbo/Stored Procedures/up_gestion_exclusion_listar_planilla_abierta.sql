CREATE PROCEDURE dbo.up_gestion_exclusion_listar_planilla_abierta
AS
BEGIN

	select 
		p.numero_planilla,
		p.fecha_inicio,
		p.fecha_fin,
		p.codigo_planilla,
		--p.codigo_canal,
		p.codigo_tipo_planilla,
		p.codigo_regla_tipo_planilla,
		rtp.nombre as nombre_regla_tipo_planilla
	from
		planilla  p 
	inner join regla_tipo_planilla rtp 
		on rtp.codigo_regla_tipo_planilla = p.codigo_regla_tipo_planilla
	where 
		p.codigo_estado_planilla=1 
		and p.codigo_regla_tipo_planilla in (select ex.codigo_regla_tipo_planilla from exclusion_cuota_planilla ex where ex.estado_exclusion = 1);

END;