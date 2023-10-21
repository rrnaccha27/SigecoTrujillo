CREATE PROCEDURE dbo.up_regla_calculo_bono_insertar
(
	 @p_codigo_tipo_planilla		INT
	,@p_calcular_igv				BIT
	,@p_codigo_canal				INT
	,@p_codigo_grupo				INT
	,@p_vigencia_inicio				VARCHAR(8)
	,@p_vigencia_fin				VARCHAR(8)
	,@p_monto_meta					DECIMAL(10, 2)
	,@p_porcentaje_pago				DECIMAL(10, 2)
	,@p_monto_tope					DECIMAL(10, 2)
	,@p_cantidad_ventas				INT
	,@p_usuario_registra			VARCHAR(50)
	,@p_codigo_regla_calculo_bono	INT OUTPUT
)
AS
BEGIN
	
	INSERT INTO
		dbo.regla_calculo_bono
	(
		 codigo_tipo_planilla
		,codigo_canal
		,codigo_grupo
		,vigencia_inicio
		,vigencia_fin
		,monto_meta
		,porcentaje_pago
		,monto_tope
		,cantidad_ventas
		,estado_registro
		,fecha_registra
		,usuario_registra
		,calcular_igv
	)
	VALUES
	(
		 @p_codigo_tipo_planilla
		,@p_codigo_canal
		,CASE WHEN @p_codigo_grupo = 0 THEN NULL ELSE @p_codigo_grupo END
		,@p_vigencia_inicio
		,@p_vigencia_fin
		,@p_monto_meta
		,@p_porcentaje_pago
		,@p_monto_tope
		,@p_cantidad_ventas
		,1
		,GETDATE()
		,@p_usuario_registra
		,@p_calcular_igv
	)
	SET @p_codigo_regla_calculo_bono = @@IDENTITY

END;