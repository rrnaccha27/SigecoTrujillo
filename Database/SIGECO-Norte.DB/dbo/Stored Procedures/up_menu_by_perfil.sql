create proc [dbo].[up_menu_by_perfil]
(
@p_codigo_perfil int
)
as
begin

 select
 m.codigo_menu,
 isnull(m.codigo_menu_padre,0) codigo_menu_padre,
 m.nombre_menu,
 m.ruta_menu,
 m.estado_registro,
 m.orden, 
 1 as tipo_orden
from permiso_menu p
inner join menu m on m.codigo_menu=p.codigo_menu and p.estado_registro=1 and m.estado_registro=1
where p.codigo_perfil_usuario=@p_codigo_perfil

union all

select 
 m.codigo_menu,
 isnull(m.codigo_menu_padre,0) codigo_menu_padre,
 m.nombre_menu,
 m.ruta_menu,
 m.estado_registro,
 m.orden, 
 2 as tipo_orden
from menu m where m.estado_registro=1

end;