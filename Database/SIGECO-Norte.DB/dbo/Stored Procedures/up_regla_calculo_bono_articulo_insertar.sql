CREATE PROCEDURE dbo.up_regla_calculo_bono_articulo_insertar
(
	 @p_codigo_regla_calculo_bono	INT
	,@p_codigo_articulo				INT
	,@p_cantidad					INT
)
AS
BEGIN
	INSERT INTO
		dbo.regla_calculo_bono_articulo
	(
		 codigo_regla_calculo_bono
		,codigo_articulo
		,cantidad
	)
	VALUES
	(
		 @p_codigo_regla_calculo_bono
		,@p_codigo_articulo
		,@p_cantidad
	)
END;