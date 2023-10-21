CREATE PROCEDURE [dbo].[up_descuento_by_planilla]
(
	@codigo_planilla int
)
AS
BEGIN

	select distinct
		p.codigo_personal,
		dp.codigo_planilla,		
		e.nombre as nombre_empresa,		
		p.nombre,
		p.apellido_paterno,
		p.apellido_materno,
		cg.nombre as nombre_grupo,
		d.monto,
		d.motivo,
		d.codigo_descuento,
		d.estado_registro,
		d.codigo_empresa,
		dp.codigo_canal
	from descuento d
	inner join detalle_planilla dp on 
		d.codigo_planilla=dp.codigo_planilla and
		d.codigo_empresa=dp.codigo_empresa and 
		d.codigo_personal=dp.codigo_personal
	inner join personal p on d.codigo_personal=p.codigo_personal
	inner join empresa_sigeco  e on d.codigo_empresa=e.codigo_empresa
	left join   canal_grupo cg on dp.codigo_grupo=cg.codigo_canal_grupo
	where 
		dp.codigo_planilla=@codigo_planilla
	order by 
		d.codigo_empresa asc, dp.codigo_canal desc, nombre_grupo asc, d.estado_registro desc;

END;