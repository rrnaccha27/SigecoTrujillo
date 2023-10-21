CREATE PROCEDURE [dbo].up_comision_precio_supervisor_insertar
(
	@p_codigo_comision INT 
	,@p_codigo_precio INT 
	,@p_codigo_canal_grupo INT 
	,@p_codigo_tipo_pago INT 
	,@p_codigo_tipo_comision_supervisor INT
	,@p_valor DECIMAL(10, 2)
	,@p_vigencia_inicio DATETIME
	,@p_vigencia_fin DATETIME
	,@p_usuario_registra varchar(50)
	,@p_estado_registro bit
	,@p_codigo_comision_out int out	
	)
AS
BEGIN

	IF @p_codigo_comision>0
	BEGIN
		UPDATE 
			dbo.comision_precio_supervisor
		SET 
			codigo_canal_grupo = @p_codigo_canal_grupo,
			codigo_tipo_pago=@p_codigo_tipo_pago,
			codigo_tipo_comision_supervisor=@p_codigo_tipo_comision_supervisor,
			valor=@p_valor,
			estado_registro=@p_estado_registro,
			vigencia_inicio=@p_vigencia_inicio,
			vigencia_fin=@p_vigencia_fin,
			usuario_modifica=@p_usuario_registra,
			fecha_modifica=GETDATE()
		WHERE
			codigo_comision=@p_codigo_comision;
		SET @p_codigo_comision_out = @p_codigo_comision;
	END
	ELSE
	BEGIN
		INSERT INTO dbo.comision_precio_supervisor(		
			codigo_precio , 
			codigo_canal_grupo  ,
			codigo_tipo_pago,  
			codigo_tipo_comision_supervisor ,
			valor ,
			vigencia_inicio ,
			vigencia_fin ,
			usuario_registra,
			estado_registro,
			fecha_registra
		)
		VALUES(
			@p_codigo_precio , 
			@p_codigo_canal_grupo  ,
			@p_codigo_tipo_pago,  
			@p_codigo_tipo_comision_supervisor ,
			@p_valor ,
			@p_vigencia_inicio ,
			@p_vigencia_fin ,
			@p_usuario_registra,
			1,
			GETDATE()     
		);
		SET @p_codigo_comision_out = @@IDENTITY;
	END;
 
END;