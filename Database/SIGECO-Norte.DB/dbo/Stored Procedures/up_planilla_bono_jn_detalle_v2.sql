CREATE PROCEDURE [dbo].[up_planilla_bono_jn_detalle_v2]
(
	 @p_codigo_planilla	INT
)
AS
BEGIN
	SET NOCOUNT ON

	IF EXISTS(SELECT codigo_planilla FROM dbo.sigeco_reporte_bono_jn_detalle WHERE codigo_planilla = @p_codigo_planilla)
	BEGIN
		SELECT 
			codigo_planilla,codigo_canal_grupo,nombre_grupo,codigo_personal,vendedor,codigo_empresa,nombre_empresa,codigo_canal
			,nombre_canal,nombre_moneda,nro_contrato,codigo_articulo,nombre_articulo,fecha_contrato,dinero_ingresado
		FROM 
			dbo.sigeco_reporte_bono_jn_detalle
		WHERE 
			codigo_planilla = @p_codigo_planilla
		ORDER BY 
			codigo_empresa desc, fecha_contrato, nro_contrato

		SELECT 'CONGELADO'
	END
	ELSE
	BEGIN
		EXEC up_planilla_bono_jn_detalle @p_codigo_planilla
		SELECT 'en vivo'
	END	
	SET NOCOUNT OFF
END;