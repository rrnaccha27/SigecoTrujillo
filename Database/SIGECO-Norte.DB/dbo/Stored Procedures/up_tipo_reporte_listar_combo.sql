IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[up_tipo_reporte_listar_combo]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[up_tipo_reporte_listar_combo]

GO
CREATE PROCEDURE [dbo].[up_tipo_reporte_listar_combo]
AS
BEGIN
	SET NOCOUNT ON

	SELECT
		codigo_tipo_reporte
		,nombre
	FROM
		dbo.tipo_reporte
	WHERE
		estado_registro = 1
	ORDER BY
		codigo_tipo_reporte
		

	SET NOCOUNT OFF
END