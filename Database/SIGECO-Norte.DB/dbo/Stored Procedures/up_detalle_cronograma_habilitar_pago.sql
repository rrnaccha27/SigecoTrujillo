create procedure [dbo].[up_detalle_cronograma_habilitar_pago]
(
@codigo_detalle_cronograma int,
@usuario_registra varchar(30),
@observacion varchar(200)
)
as
begin
declare @v_existe_registro int,@v_codigo_estado_cuota int,@v_mensaje varchar(150);
select 
	@v_codigo_estado_cuota=codigo_estado_cuota 
from detalle_cronograma 
where codigo_detalle=@codigo_detalle_cronograma ;

set @v_mensaje='El pago solo se puedo habilitar en estado "Excluido", el estado actual del registro es ';
select @v_mensaje=@v_mensaje+nombre from estado_cuota where codigo_estado_cuota=@v_codigo_estado_cuota;
 
 if @v_codigo_estado_cuota<>4
 begin
  RAISERROR(@v_mensaje,16,1);
 return;
 end;
 
	 update detalle_cronograma 
	 set codigo_estado_cuota=1 /*INDICA QUE EL PAGO SE ENCUENTRA PENDIENTE*/
	 where codigo_detalle=@codigo_detalle_cronograma;

	 update operacion_cuota_comision 
	 set estado_registro=1
	 where codigo_detalle_cronograma=@codigo_detalle_cronograma 
	 and estado_registro=1;
 /********************************************************************************/
	insert into operacion_cuota_comision(
	codigo_detalle_cronograma,
	codigo_tipo_operacion_cuota,
	motivo_movimiento,
	fecha_movimiento,
	estado_registro,
	usuario_registra,
	fecha_registra
	)
	values(
	@codigo_detalle_cronograma,
	1,
	isnull(@observacion,'Se libera el pago'),
	GETDATE(),
	1,
	@usuario_registra,
	GETDATE()
	);
 
end;