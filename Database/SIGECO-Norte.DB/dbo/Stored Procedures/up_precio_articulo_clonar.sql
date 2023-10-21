CREATE PROCEDURE [dbo].[up_precio_articulo_clonar]
(
	@codigo_precio INT 
	,@codigo_articulo INT 
	,@codigo_empresa INT 
	,@codigo_tipo_venta INT 
	,@codigo_moneda INT 
	,@precio DECIMAL(10, 2) 
	,@igv DECIMAL(10, 2) 
	,@precio_total DECIMAL(10, 2) 
	,@cuota_inicial DECIMAL(10, 2) 
	,@estado_registro bit
	,@vigencia_inicio DATETIME 
	,@vigencia_fin DATETIME
	,@usuario_registra varchar(50)
	,@clonarcomisiones int
	,@p_codigo_precio_articulo int out
)
AS
BEGIN

	INSERT INTO 
		dbo.precio_articulo(   
			codigo_articulo 
			,codigo_empresa
			,codigo_tipo_venta
			,codigo_moneda
			,precio
			,igv
			,precio_total
			,cuota_inicial
			,vigencia_inicio
			,vigencia_fin	
			,usuario_registra
			,fecha_registra
			,estado_registro
		)
	VALUES(
		@codigo_articulo 
		,@codigo_empresa
		,@codigo_tipo_venta
		,@codigo_moneda
		,@precio
		,@igv
		,@precio_total
		,@cuota_inicial
		,@vigencia_inicio
		,@vigencia_fin	
		,@usuario_registra
		,GETDATE()
		,1
	);

	SET @p_codigo_precio_articulo = @@IDENTITY;

	INSERT INTO dbo.regla_calculo_comision(
		codigo_precio, 
		codigo_canal ,
		codigo_tipo_pago,  
		codigo_tipo_comision,
		valor,
		vigencia_inicio,
		vigencia_fin,
		usuario_registra,
		estado_registro,
		fecha_registra
	)
	SELECT 
		@p_codigo_precio_articulo, 
		codigo_canal,
		codigo_tipo_pago,  
		codigo_tipo_comision,
		valor,
		case when vigencia_inicio >= @vigencia_inicio THEN vigencia_inicio ELSE @vigencia_inicio END as vigencia_inicio,
		case when vigencia_fin <= @vigencia_fin THEN vigencia_fin ELSE @vigencia_fin END as vigencia_fin,
		@usuario_registra,
		estado_registro,
		GETDATE()
	FROM 
		dbo.regla_calculo_comision
	WHERE 
		codigo_precio = @codigo_precio 
		AND estado_registro = 1
		AND @clonarcomisiones = 1;

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
	SELECT
		codigo_precio , 
		codigo_canal_grupo  ,
		codigo_tipo_pago,  
		codigo_tipo_comision_supervisor ,
		valor ,
		case when vigencia_inicio >= @vigencia_inicio THEN vigencia_inicio ELSE @vigencia_inicio END as vigencia_inicio,
		case when vigencia_fin <= @vigencia_fin THEN vigencia_fin ELSE @vigencia_fin END as vigencia_fin,
		@usuario_registra,
		estado_registro,
		GETDATE()
	FROM
		dbo.comision_precio_supervisor
	WHERE
		codigo_precio = @codigo_precio 
		AND estado_registro = 1
		AND @clonarcomisiones = 1;   

	/*************************************************/
	--SE DESHABILITA EL PRECIO ORIGINAL
	UPDATE
		dbo.precio_articulo 
	SET 
		usuario_modifica = @usuario_registra,
		estado_registro = 0,
		fecha_modifica = GETDATE()
		--,vigencia_fin = CONVERT(DATE, GETDATE())
	WHERE 
		codigo_precio=@codigo_precio;

	--SE DESHABILITTA LAS COMISIONES DE VENDEDOR
	UPDATE
		dbo.regla_calculo_comision 
	SET 
		estado_registro = 0,
		fecha_modifica = GETDATE(),
		usuario_modifica = @usuario_registra
		--,vigencia_fin = CONVERT(DATE, GETDATE())
	WHERE 
		codigo_precio = @codigo_precio 
		AND estado_registro = 1;    

	--SE DESHABILITTA LAS COMISIONES DE SUPERVISOR
	UPDATE
		dbo.comision_precio_supervisor 
	SET 
		estado_registro = 0,
		fecha_modifica = GETDATE(),
		usuario_modifica = @usuario_registra
		--,vigencia_fin = CONVERT(DATE, GETDATE())
	WHERE 
		codigo_precio = @codigo_precio 
		AND estado_registro = 1;    
	/*************************************************/

END;