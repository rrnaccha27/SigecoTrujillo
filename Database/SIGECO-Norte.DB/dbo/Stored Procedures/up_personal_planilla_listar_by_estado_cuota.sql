create proc [dbo].[up_personal_planilla_listar_by_estado_cuota]
(
@codigo_planilla int,
@codigo_estado_cuota int

)
as
begin

select 
distinct
pe.codigo_personal,
pe.nro_documento,
pe.correo_electronico,
pe.nombre,
pe.apellido_materno,
pe.apellido_paterno
from detalle_planilla dp
inner join detalle_cronograma dc on dp.codigo_detalle_cronograma=dc.codigo_detalle
inner join cronograma_pago_comision cpc on dc.codigo_cronograma=cpc.codigo_cronograma
inner join personal_canal_grupo pcg on cpc.codigo_personal_canal_grupo=pcg.codigo_registro
inner join personal pe on pcg.codigo_personal=pe.codigo_personal
where dp.codigo_planilla=@codigo_planilla and dc.codigo_estado_cuota=@codigo_estado_cuota and dp.excluido=0;

end;