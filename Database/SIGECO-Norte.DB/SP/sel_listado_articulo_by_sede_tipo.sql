CREATE OR ALTER PROCEDURE [dbo].[sel_listado_articulo_by_sede_tipo]    
(    
 @p_tipo_articulo int  ,  
 @p_sede int  
)    
AS    
BEGIN    
 SELECT     
  DISTINCT     
   a.codigo_articulo,    
   max(a.codigo_sku) as codigo_sku,     
   a.nombre,     
   a.abreviatura
 FROM articulo a
 WHERE      
   a.codigo_tipo_articulo=@p_tipo_articulo
  and a.codigo_sede=@p_sede  
 GROUP BY a.codigo_articulo, a.nombre, a.abreviatura
 ORDER BY a.nombre ASC    
END;    
    
    
    