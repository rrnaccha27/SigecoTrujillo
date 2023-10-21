CREATE PROCEDURE dbo.up_regla_calculo_bono_actualizar
(
	 @p_codigo_regla_calculo_bono	INT
	,@p_codigo_tipo_planilla		INT
	,@p_codigo_canal				INT
	,@p_codigo_grupo				INT
	,@p_vigencia_inicio				VARCHAR(8)
	,@p_vigencia_fin				VARCHAR(8)
	,@p_monto_meta					DECIMAL(10, 2)
	,@p_porcentaje_pago				DECIMAL(10, 2)
	,@p_monto_tope					DECIMAL(10, 2)
	,@p_cantidad_ventas				INT
	,@p_usuario_modifica			VARCHAR(50)
)
AS
BEGIN
	
	UPDATE
		dbo.regla_calculo_bono
	SET
		 codigo_tipo_planilla = @p_codigo_tipo_planilla
		,codigo_canal = @p_codigo_canal
		,codigo_grupo = @p_codigo_grupo
		,vigencia_inicio = @p_vigencia_inicio
		,vigencia_fin = @p_vigencia_fin
		,monto_meta = @p_monto_meta
		,porcentaje_pago = @p_porcentaje_pago
		,monto_tope = @p_monto_tope
		,cantidad_ventas = @p_cantidad_ventas
		,fecha_modifica = GETDATE() 
		,usuario_modifica = @p_usuario_modifica
	WHERE
		codigo_regla_calculo_bono = @p_codigo_regla_calculo_bono

END;