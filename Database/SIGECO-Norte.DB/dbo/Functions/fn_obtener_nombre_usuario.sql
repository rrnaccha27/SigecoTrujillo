create function fn_obtener_nombre_usuario(@p_codigo_usuario varchar(60))
returns varchar(150)
as
begin
declare 
   @v_codigo_persona int,
   @v_personal varchar(150);
select @v_codigo_persona=codigo_persona from usuario u where u.codigo_usuario=@p_codigo_usuario;
select 
  @v_personal=nombre_persona+' '+ISNULL(apellido_paterno, '')+' ' +ISNULL(apellido_materno, '')
from persona where codigo_persona=@v_codigo_persona;
return isnull(@v_personal,isnull(@p_codigo_usuario,''))

end;