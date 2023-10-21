CREATE PROCEDURE dbo.up_tipo_comision_supervisor_listado
AS
BEGIN
	SET NOCOUNT ON
	SELECT
		codigo_tipo_comision_supervisor,
		nombre
	FROM
		dbo.tipo_comision_supervisor
	WHERE
		estado_registro = 1
	ORDER BY
		codigo_tipo_comision_supervisor ASC
	SET NOCOUNT OFF
END