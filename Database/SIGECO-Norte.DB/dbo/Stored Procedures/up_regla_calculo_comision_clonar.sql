CREATE PROCEDURE dbo.up_regla_calculo_comision_clonar
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
AS
BEGIN

	UPDATE 
		regla_calculo_comision
	SET 
		usuario_modifica=@usuario_registra,
		fecha_modifica=GETDATE(),
		estado_registro = 0
	WHERE
		codigo_regla=@codigo_regla;

	INSERT INTO regla_calculo_comision (		
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
	VALUES(
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
   
	SET @p_codigo_regla=@@IDENTITY;
 
END;