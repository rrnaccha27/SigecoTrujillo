      
 /*    
 test: dbo.sel_listado_regla_comision      
 */    
    
CREATE OR ALTER    PROC dbo.sel_listado_regla_comision     
(      
 @codigo_sede int,      
 @codigo_canal_grupo int,      
 @codigo_tipo_venta int,      
 @codigo_tipo_articulo int   ,
 @codigo_tipo_planilla int
)      
AS      
    
 SELECT       
  r.codigo_regla_comision ,    
  r.nombre_regla_comision,    
  tv.nombre as nombre_tipo_venta,    
  ta.nombre  as nombre_tipo_articulo,    
  a.nombre as nombre_articulo,    
  cg.nombre as nombre_canal_grupo,   
  tp.nombre as nombre_tipo_planilla,
  
  CASE WHEN r.estado_registro = 1 THEN 'Activo' ELSE 'Inactivo' END AS estado_registro_nombre ,    
  r.estado_registro    
 FROM       
  dbo.regla_comision r      
 --LEFT JOIN dbo.empresa_sigeco e ON e.codigo_empresa = r.codigo_sede       
 LEFT JOIN dbo.canal_grupo cg ON cg.codigo_canal_grupo = r.codigo_canal_grupo       
 LEFT JOIN dbo.tipo_venta tv ON tv.codigo_tipo_venta = r.codigo_tipo_venta        
 LEFT JOIN tipo_articulo ta ON ta.codigo_tipo_articulo=r.codigo_tipo_articulo    
 LEFT JOIN dbo.articulo a ON a.codigo_articulo = r.codigo_articulo      
 LEFT JOIN dbo.tipo_planilla tp ON r.codigo_tipo_planilla = tp.codigo_tipo_planilla
 WHERE       
     r.codigo_sede=@codigo_sede  
  --(@codigo_empresa IS NULL OR (r.codigo_sede = @codigo_empresa))      
 AND (@codigo_canal_grupo IS NULL OR (r.codigo_canal_grupo = @codigo_canal_grupo))      
 AND (@codigo_tipo_planilla IS NULL OR (r.codigo_tipo_planilla = @codigo_tipo_planilla))    
 AND (@codigo_tipo_venta IS NULL OR (r.codigo_tipo_venta = @codigo_tipo_venta))      
 AND (@codigo_tipo_articulo IS NULL OR (r.codigo_tipo_articulo = @codigo_tipo_articulo))      
 ORDER BY r.nombre_regla_comision    