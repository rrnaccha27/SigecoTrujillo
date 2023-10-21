CREATE PROCEDURE dbo.up_plan_integral_detalle_listar
(
	 @p_codigo_plan_integral	INT
)
AS
BEGIN

	SELECT
		 codigo_plan_integral_detalle
		,codigo_campo_santo
		,codigo_tipo_articulo
		,codigo_tipo_articulo_2
		,estado_registro
		,CASE WHEN estado_registro = 1 THEN 'Activo' ELSE 'Inactivo' END AS estado_registro_nombre
	FROM
		dbo.plan_integral_detalle
	WHERE
		codigo_plan_integral = @p_codigo_plan_integral

END