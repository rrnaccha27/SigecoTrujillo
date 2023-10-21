create proc [dbo].[up_personal_planilla_listar]
(
	@codigo_planilla int,
	@nro_documento varchar(20),
	@nombre varchar(50),
	@apellido_paterno varchar(50),
	@apellido_materno varchar(50)
)
as
begin
	select distinct
		pe.codigo_personal,
		pe.nro_documento,
		pe.nombre,
		pe.apellido_materno,
		pe.apellido_paterno,
		pe.nombre + isnull(' ' + pe.apellido_paterno,'') + isnull(' ' + pe.apellido_materno,'') as nombre_completo,
		pe.codigo_equivalencia
	from detalle_planilla dp
	inner join personal pe on dp.codigo_personal=pe.codigo_personal
	where dp.codigo_planilla=@codigo_planilla
	order by
		nombre_completo;
end;