CREATE PROCEDURE [dbo].[up_planilla_descuento_desactivar]
(
     @codigo_descuento int    
	,@estado_registro bit	
	,@usuario_registra varchar(20)
	,@p_codigo_descuento int out
)
AS
BEGIN

	declare @v_estado_registro bit;
	
	select top 1
		@v_estado_registro=estado_registro
	from 
		descuento 
	where 
		codigo_descuento=@codigo_descuento;

	if @v_estado_registro=0
	begin
		RAISERROR('El descuento ya se encuentra activado.',16,1); 
		RETURN;
	end;

	update 
		descuento 
	set 
		estado_registro=@estado_registro,
		usuario_modifica=@usuario_registra,
		fecha_modifica=GETDATE()
	where 
		codigo_descuento=@codigo_descuento 
		and estado_registro=1;
	
	set @p_codigo_descuento=@codigo_descuento;

END;