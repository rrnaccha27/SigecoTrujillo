CREATE procedure [dbo].[sp_regla_calculo_comision_insertar]
(
	@codigo_regla INT 
	,@codigo_precio INT 
	,@codigo_canal INT 
	,@codigo_tipo_pago INT 
	,@codigo_tipo_comision INT
	,@valor DECIMAL(10, 2)
	,@vigencia_inicio DATETIME
	,@vigencia_fin DATETIME
	,@usuario_registra varchar(50)
	,@estado_registro bit
	,@p_codigo_regla int out	
	)
as
begin

	if @codigo_regla>0
	begin
		update regla_calculo_comision
		set codigo_canal=@codigo_canal,
		codigo_tipo_pago=@codigo_tipo_pago,
		codigo_tipo_comision=@codigo_tipo_comision,
		valor=@valor,
		estado_registro=@estado_registro,
		vigencia_inicio=@vigencia_inicio,
		vigencia_fin=@vigencia_fin,
		usuario_modifica=@usuario_registra,
		fecha_modifica=GETDATE()
		where codigo_regla=@codigo_regla;
		set @p_codigo_regla=@codigo_regla;
	end
	else
	begin
		insert into regla_calculo_comision (		
		codigo_precio , 
		codigo_canal  ,
		codigo_tipo_pago,  
		codigo_tipo_comision ,
		valor ,
		vigencia_inicio ,
		vigencia_fin ,
		usuario_registra,
		estado_registro,
		fecha_registra
		)
		values(
		
		@codigo_precio , 
		@codigo_canal  ,
		@codigo_tipo_pago,  
		@codigo_tipo_comision ,
		@valor ,
		@vigencia_inicio ,
		@vigencia_fin ,
		@usuario_registra,
		1,
		GETDATE()     
		);
   
		set @p_codigo_regla=@@IDENTITY;
	end;
 
end;