CREATE OR ALTER PROC UPD_DETALLE_REGLA_COMISION(
           @codigo_detalle_regla_comision INT,
           @codigo_regla_comision int ,
           @rango_inicio decimal(10,2),
           @rango_fin decimal(10,2),
           @comision decimal(10,2),
           @codigo_tipo_comision int,
           @porcentaje_pago_comision decimal(10,2),
           @existe_condicional bit,
           @valor_condicion decimal(10,2),
           @descripcion_condicion nvarchar(500),           
           @orden_regla int,
           @formula_calculo nvarchar(1000),
		   @usuario_registra nvarchar(50)
		   
)
AS
UPDATE [dbo].[detalle_regla_comision]
           SET
		    
            [rango_inicio]=@rango_inicio
           ,[rango_fin]=@rango_fin
           ,[comision]=@comision
           ,[codigo_tipo_comision]=@codigo_tipo_comision
           ,[porcentaje_pago_comision]=@porcentaje_pago_comision
           ,[existe_condicional]=@existe_condicional
           ,[valor_condicion]=@valor_condicion
           ,[descripcion_condicion]=@descripcion_condicion           
           ,[orden_regla]=@orden_regla
           ,[formula_calculo]=@formula_calculo
		   WHERE codigo_detalle_regla_comision=@codigo_detalle_regla_comision




