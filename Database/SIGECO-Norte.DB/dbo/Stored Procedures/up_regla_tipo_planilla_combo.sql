CREATE PROCEDURE up_regla_tipo_planilla_combo
AS
BEGIN
	SELECT 
		codigo_regla_tipo_planilla
		,nombre 
	FROM 
		regla_tipo_planilla 
	WHERE 
		estado_registro = 1
	ORDER BY
		nombre ASC
END;