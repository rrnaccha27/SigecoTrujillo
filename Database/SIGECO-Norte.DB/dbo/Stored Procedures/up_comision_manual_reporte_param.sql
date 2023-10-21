CREATE PROCEDURE dbo.up_comision_manual_reporte_param
(
	@p_fecha_inicio	VARCHAR(10)
	,@p_fecha_fin	VARCHAR(10)
	,@p_usuario	VARCHAR(50)
)
AS
BEGIN
	SET NOCOUNT ON
	SELECT
		CASE WHEN LEN(ISNULL(@p_fecha_inicio, '')) > 0 THEN 'DESDE ' + CONVERT(varchar, CONVERT(datetime, @p_fecha_inicio), 103) ELSE ' ' END 
		+
		CASE WHEN LEN(ISNULL(@p_fecha_fin, '')) > 0 THEN ' HASTA ' + CONVERT(varchar, CONVERT(datetime, @p_fecha_fin), 103)  ELSE ' ' END 
		AS fechas
		, CONVERT(varchar, GETDATE(), 103) + ' ' + LEFT(CONVERT(varchar, GETDATE(), 108), 5) AS fecha_impresion
		, dbo.fn_obtener_nombre_usuario(@p_usuario) AS usuario
	SET NOCOUNT OFF
END;