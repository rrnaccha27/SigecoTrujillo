create proc [dbo].[up_usuario_autenticar]
(
@p_codigo_usuario varchar(50)
)
as
begin

select 
u.codigo_usuario,
c.clave,
u.codigo_persona,
u.codigo_perfil_usuario,
u.estado_registro 
from usuario u 
inner join clave_usuario c on u.codigo_usuario=c.codigo_usuario and u.codigo_usuario=@p_codigo_usuario and
c.estado_registro=1

end;