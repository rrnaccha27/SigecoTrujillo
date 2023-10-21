CREATE PROCEDURE dbo.up_regla_calculo_bono_matriz_eliminar
(
	 @p_codigo_regla_calculo_bono	INT
)
AS
BEGIN
	DELETE FROM
		dbo.regla_calculo_bono_matriz
	WHERE
		codigo_regla_calculo_bono = @p_codigo_regla_calculo_bono
END;