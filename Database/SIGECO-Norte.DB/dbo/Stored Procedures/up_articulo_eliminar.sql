CREATE PROCEDURE dbo.up_articulo_eliminar
(
	@codigo_articulo int,
	@usuario_registra varchar(20)
)
AS
BEGIN

	EXEC dbo.up_articulo_log_insertar @codigo_articulo
	/*************************************************************/
	update 
		articulo 
	set 
		estado_registro=0,
		usuario_modifica=@usuario_registra,
		fecha_modifica=GETDATE()
	where 
		codigo_articulo=@codigo_articulo 
		and estado_registro=1;
	/************************************************************/  
	/*
	update 
		precio_articulo 
	set 
		estado_registro=0,
		usuario_modifica=@usuario_registra,
		fecha_modifica=GETDATE()
	where 
		codigo_articulo=@codigo_articulo 
		and estado_registro=1;
  
	update 
		rcc 
	set 
		rcc.estado_registro=0,
		rcc.usuario_modifica=@usuario_registra,
		rcc.fecha_modifica=GETDATE()
	from 
		precio_articulo pa
	inner join regla_calculo_comision rcc 
		on pa.codigo_precio=rcc.codigo_precio
	where 
		pa.codigo_articulo=@codigo_articulo;  
	*/
END;