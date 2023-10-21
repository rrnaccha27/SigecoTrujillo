
  
CREATE OR ALTER PROCEDURE sel_listado_meta_regla_comision_by_id  
(
@codigo_regla_comision int
)
AS  
SELECT [codigo_meta_regla_comision]
      ,[codigo_regla_comision]
      ,[tope_unidad]
      ,[tope_unidad_comisionable]
      ,[estado_registro]
      ,[tope_unidad_fin]
  FROM [dbo].[meta_regla_comision] c   
  WHERE codigo_regla_comision=@codigo_regla_comision

  
  
