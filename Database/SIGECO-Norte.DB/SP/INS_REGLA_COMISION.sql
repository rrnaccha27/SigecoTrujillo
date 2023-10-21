  
CREATE OR ALTER PROC dbo.INS_REGLA_COMISION  
(  

@nombre_regla_comision varchar(50),
@codigo_tipo_venta int,
@codigo_tipo_articulo int,
@tope_minimo_contrato decimal(10,2),
@tope_unidad decimal(10,2),
@meta_general decimal(10,2),
@codigo_canal_grupo int,

@codigo_tipo_planilla int,
@codigo_articulo int,
@codigo_sede int,
@usuario_registra varchar(50),  
@p_codigo_regla_pago int out  
)  
AS  
BEGIN  
DECLARE @codigo_regla_comision int;

SELECT @codigo_regla_comision=ISNULL(MAX(codigo_regla_comision),0)+1 FROM [dbo].[regla_comision]

INSERT INTO [dbo].[regla_comision]
           (codigo_regla_comision
           ,[nombre_regla_comision]
           ,[codigo_tipo_venta]
           ,[codigo_tipo_articulo]
           ,[tope_minimo_contrato]
           ,[tope_unidad]
           ,[meta_general]
           ,[estado_registro]
           ,[codigo_canal_grupo]
           ,[codigo_tipo_planilla]
           ,[codigo_articulo]
           ,[codigo_sede])
     VALUES
           (
			@codigo_regla_comision,
			@nombre_regla_comision,
			@codigo_tipo_venta ,
			@codigo_tipo_articulo,
			@tope_minimo_contrato,
			@tope_unidad,
			@meta_general,
			1,
			@codigo_canal_grupo,
			@codigo_tipo_planilla,
			@codigo_articulo,
			@codigo_sede
		   );

 SET @p_codigo_regla_pago=@codigo_regla_comision;
		   
END  
  