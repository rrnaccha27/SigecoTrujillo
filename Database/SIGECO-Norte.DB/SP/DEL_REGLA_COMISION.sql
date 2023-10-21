CREATE OR ALTER PROC dbo.DEL_REGLA_COMISION  
(  
@estado_registro bit,  
@usuario_registra varchar(50),  
@codigo_regla_comision int  
)  
AS  
BEGIN  
UPDATE regla_comision  
SET   
estado_registro = @estado_registro
WHERE codigo_regla_comision = @codigo_regla_comision  
  
END  
  
  