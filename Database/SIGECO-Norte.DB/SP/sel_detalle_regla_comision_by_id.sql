    
CREATE OR ALTER  PROCEDURE sel_detalle_regla_comision_by_id    
(  
@codigo_detalle_regla_comision int  
)  
AS    
SELECT [codigo_detalle_regla_comision]  
      ,[codigo_regla_comision]  
      ,[rango_inicio]  
      ,[rango_fin]  
      ,[comision]  
      ,c.[codigo_tipo_comision]  
   ,tc.nombre as nombre_tipo_comision  
      ,[porcentaje_pago_comision]  
      ,[existe_condicional]  
      ,[valor_condicion]  
      ,[descripcion_condicion]  
      ,c.[estado_registro]  
      ,[orden_regla]  
      ,[formula_calculo]  
  FROM [dbo].[detalle_regla_comision] c   
  LEFT JOIN tipo_comision tc ON C.codigo_tipo_comision=tc.codigo_tipo_comision  
  WHERE codigo_detalle_regla_comision=@codigo_detalle_regla_comision  
  
    
	