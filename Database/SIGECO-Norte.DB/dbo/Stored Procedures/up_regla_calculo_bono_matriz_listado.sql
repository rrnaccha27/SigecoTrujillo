CREATE PROCEDURE dbo.up_regla_calculo_bono_matriz_listado
(
	 @p_codigo_regla_calculo_bono	INT
)
AS
BEGIN
	SELECT
		 CONVERT(INT, ROW_NUMBER() OVER (ORDER BY porcentaje_meta)) AS codigo_registro
		,CONVERT(VARCHAR(10), porcentaje_meta) AS porcentaje_meta
		,monto_meta
		,CONVERT(VARCHAR(10), porcentaje_pago) AS porcentaje_pago
	FROM
		dbo.regla_calculo_bono_matriz
	WHERE
		 codigo_regla_calculo_bono = @p_codigo_regla_calculo_bono
	ORDER BY
		porcentaje_meta DESC
END;