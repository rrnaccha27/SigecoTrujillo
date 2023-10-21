CREATE PROC up_planilla_anular_pago_comision
(
	@codigo_detalle_cronograma int,
	@usuario_registra varchar(30),
	@motivo varchar(200)
)
AS
BEGIN

	declare @v_codigo_estado_cuota int;

	select 
		@v_codigo_estado_cuota=codigo_estado_cuota
	from detalle_cronograma 
	where codigo_detalle=@codigo_detalle_cronograma;

	if (@v_codigo_estado_cuota=1 or @v_codigo_estado_cuota=2 or  @v_codigo_estado_cuota=4)
	begin
		set @motivo=isnull(@motivo,'ANULANDO CUOTA DE LA COMISION');
	end
	else
	begin
		RAISERROR('La cuota de la comisión no se encuentra habilitado para anular.',16,1); 
		return; 
	end;
 
	/************************************************************************
	SI ESTA EN EXCLUSIÓN DEBE ACTUALIZAR EL MOTIVO
	*************************************************************************/
	if @v_codigo_estado_cuota=4
	begin    
		update
			exclusion_cuota_planilla 
		set 
			estado_registro=0,
			fecha_habilita=GETDATE(),
			motivo_habilita=@motivo,
			usuario_habilita=@usuario_registra
		where 
			codigo_detalle_cronograma=@codigo_detalle_cronograma 
			and estado_registro=1;
	end;

	update 
		detalle_cronograma
	set 
		codigo_estado_cuota = 5 /* Estado de cuota ANULADO*/
	where 
		codigo_detalle=@codigo_detalle_cronograma ;

	EXEC dbo.up_operacion_cuota_comision_insertar @codigo_detalle_cronograma, 5, @motivo, @usuario_registra

end