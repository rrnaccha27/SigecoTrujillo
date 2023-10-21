  
 /*
 test: dbo.sel_regla_comision_by_id 51  
 */

CREATE OR ALTER PROC dbo.sel_regla_comision_by_id
(  
 @codigo_regla_comision int
)  
AS  

 SELECT   
  r.codigo_regla_comision ,
  r.nombre_regla_comision,
  r.codigo_tipo_venta,
  tv.nombre as nombre_tipo_venta,
  r.codigo_tipo_articulo,
  ta.nombre  as nombre_tipo_articulo,
  r.tope_minimo_contrato,
  r.tope_unidad,
  r.meta_general,
  r.codigo_canal_grupo,
  r.codigo_tipo_planilla,
  r.codigo_sede,
  r.codigo_articulo,
  a.nombre as nombre_articulo,
  cg.nombre as nombre_canal_grupo,
  CASE WHEN r.estado_registro = 1 THEN 'Activo' ELSE 'Inactivo' END AS estado_registro_nombre ,
  r.estado_registro
 FROM   
  dbo.regla_comision r    
 LEFT JOIN dbo.canal_grupo cg ON cg.codigo_canal_grupo = r.codigo_canal_grupo   
 LEFT JOIN dbo.tipo_venta tv ON tv.codigo_tipo_venta = r.codigo_tipo_venta    
 LEFT JOIN tipo_articulo ta ON ta.codigo_tipo_articulo=r.codigo_tipo_articulo
 LEFT JOIN dbo.articulo a ON a.codigo_articulo = r.codigo_articulo  
 WHERE   
     r.codigo_regla_comision=@codigo_regla_comision
 


