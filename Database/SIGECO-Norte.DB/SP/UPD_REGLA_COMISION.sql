    
CREATE OR ALTER  PROC dbo.UPD_REGLA_COMISION    
(    
@codigo_regla_comision int,  
@nombre_regla_comision varchar(50),  
@codigo_tipo_venta int,  
@codigo_tipo_articulo int,  
@tope_minimo_contrato decimal(10,2),  
@tope_unidad decimal(10,2),  
@meta_general decimal(10,2),  
@codigo_canal_grupo int,  
@codigo_tipo_planilla int,  
@codigo_articulo int,  
@usuario_registra varchar(50)
 
)    
AS    
BEGIN    
  

  UPDATE [dbo].[regla_comision]  
  SET 
            [nombre_regla_comision]  =@nombre_regla_comision
           ,[codigo_tipo_venta]  	 =@codigo_tipo_venta
           ,[codigo_tipo_articulo]   =@codigo_tipo_articulo
           ,[tope_minimo_contrato]   =@tope_minimo_contrato
           ,[tope_unidad]  			 =@tope_unidad
           ,[meta_general]           = @meta_general
           ,[codigo_canal_grupo]  	 =@codigo_canal_grupo
           ,[codigo_tipo_planilla]   =@codigo_tipo_planilla
           ,[codigo_articulo]  		 =@codigo_articulo           
	WHERE codigo_regla_comision=@codigo_regla_comision;

  
 
       
END    