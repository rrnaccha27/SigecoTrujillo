CREATE PROCEDURE dbo.up_regla_calculo_bono_matriz_insertar
(
	 @p_codigo_regla_calculo_bono	INT
	,@p_porcentaje_meta				DECIMAL(10, 2)
	,@p_monto_meta					DECIMAL(10, 2)
	,@p_porcentaje_pago				DECIMAL(10, 2)
)
AS
BEGIN
	INSERT INTO
		dbo.regla_calculo_bono_matriz
	(
		 codigo_regla_calculo_bono
		,porcentaje_meta
		,monto_meta
		,porcentaje_pago
	)
	VALUES
	(
		 @p_codigo_regla_calculo_bono
		,@p_porcentaje_meta
		,@p_monto_meta
		,@p_porcentaje_pago
	)
END;