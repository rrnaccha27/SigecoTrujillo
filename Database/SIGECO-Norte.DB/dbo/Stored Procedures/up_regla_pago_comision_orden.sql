CREATE PROCEDURE dbo.up_regla_pago_comision_orden
AS
BEGIN
	SELECT
		codigo_orden
		,nombre
		,orden
		,usuario_registra
	FROM
		dbo.regla_pago_comision_orden
	ORDER BY orden ASC
END