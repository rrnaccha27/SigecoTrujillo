CREATE OR ALTER PROC INS_DETALLE_REGLA_COMISION(
           @codigo_regla_comision int ,
           @rango_inicio decimal(10,2),
           @rango_fin decimal(10,2),
           @comision decimal(10,2),
           @codigo_tipo_comision int,
           @porcentaje_pago_comision decimal(10,2),
           @existe_condicional bit,
           @valor_condicion decimal(10,2),
           @descripcion_condicion nvarchar(500),
           @estado_registro bit,
           @orden_regla int,
           @formula_calculo nvarchar(1000),
		   @usuario_registra nvarchar(50),
		   @codigo_detalle_regla_comision int out
)
AS
INSERT INTO [dbo].[detalle_regla_comision]
           ([codigo_regla_comision]
           ,[rango_inicio]
           ,[rango_fin]
           ,[comision]
           ,[codigo_tipo_comision]
           ,[porcentaje_pago_comision]
           ,[existe_condicional]
           ,[valor_condicion]
           ,[descripcion_condicion]
           ,[estado_registro]
           ,[orden_regla]
           ,[formula_calculo])
     VALUES
           (
		   @codigo_regla_comision,
           @rango_inicio, 
           @rango_fin, 
           @comision, 
           @codigo_tipo_comision,
           @porcentaje_pago_comision,
           @existe_condicional,
           @valor_condicion, 
           @descripcion_condicion,
           @estado_registro, 
           @orden_regla,
           @formula_calculo
		   )

SET @codigo_detalle_regla_comision=@@IDENTITY;


