CREATE PROCEDURE dbo.up_regla_calculo_bono_articulo_listado
(
	 @p_codigo_regla_calculo_bono	INT
)
AS
BEGIN
	SELECT
		 rcb.codigo_articulo
		,a.nombre 
		,rcb.cantidad
	FROM
		dbo.regla_calculo_bono_articulo rcb
	INNER JOIN
		dbo.articulo a
		ON a.codigo_articulo = rcb.codigo_articulo AND a.estado_registro = 1
	WHERE
		 rcb.codigo_regla_calculo_bono = @p_codigo_regla_calculo_bono
END;