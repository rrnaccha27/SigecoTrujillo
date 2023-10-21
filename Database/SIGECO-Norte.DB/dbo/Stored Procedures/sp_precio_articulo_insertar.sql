CREATE PROCEDURE [dbo].[sp_precio_articulo_insertar]
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
	,@p_codigo_precio_articulo int out
)
AS
BEGIN

	if @codigo_precio>0
	begin
		
		update precio_articulo 
		set 
			codigo_empresa=@codigo_empresa,
			codigo_tipo_venta=@codigo_tipo_venta,
			codigo_moneda=@codigo_moneda,
			vigencia_inicio=@vigencia_inicio,
			vigencia_fin=@vigencia_fin,
			precio=@precio,
			igv=@igv,
			precio_total=@precio_total,
			cuota_inicial=@cuota_inicial,
			usuario_modifica=@usuario_registra,
			estado_registro=@estado_registro,
			fecha_modifica=GETDATE()  
		where
			codigo_precio=@codigo_precio;
		
		set @p_codigo_precio_articulo=@codigo_precio;
		/************************************************
		*  LIBERA LAS COMISIONES QUE ESTA ASOCIADO AL PRECIO ARTICULO
		*************************************************/
		if @estado_registro=0 
		begin
			update regla_calculo_comision
			set 
				estado_registro=0,
				fecha_modifica=GETDATE(),
				usuario_modifica=@usuario_registra
			where
				codigo_precio=@codigo_precio 
				and estado_registro=1;

			update comision_precio_supervisor
			set 
				estado_registro=0,
				fecha_modifica=GETDATE(),
				usuario_modifica=@usuario_registra
			where
				codigo_precio=@codigo_precio 
				and estado_registro=1;    
		end;  
		/*************************************************/
	end
	else
	begin
		insert into precio_articulo(   
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
		values(
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
		set @p_codigo_precio_articulo=@@IDENTITY;
	end;
END;