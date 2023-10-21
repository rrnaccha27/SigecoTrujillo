IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[up_planilla_buscar_validacion_comision]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[up_planilla_buscar_validacion_comision]

GO
CREATE PROCEDURE [dbo].[up_planilla_buscar_validacion_comision]
(
	@p_codigo_tipo_planilla		INT
	,@p_codigo_empresa			INT
	,@p_codigo_canal			INT
	,@p_codigo_tipo_venta		INT
	,@p_codigo_articulo			INT
)
AS
BEGIN
	SET NOCOUNT ON

	SELECT TOP 1 
		p.codigo_planilla
		,p.nombre_planilla
		,p.estado_planilla
		,convert(varchar, fecha_inicio, 103) as fecha_inicio
		,convert(varchar, fecha_fin, 103) as fecha_fin
	FROM 
		dbo.vw_planilla p
	INNER JOIN dbo.detalle_planilla dp 
		ON dp.codigo_planilla = p.codigo_planilla and dp.excluido = 0 and dp.estado_registro = 1
	WHERE 
		p.codigo_estado_planilla <> 3 
		AND p.codigo_tipo_planilla = @p_codigo_tipo_planilla
		AND dp.codigo_empresa = @p_codigo_empresa
		AND dp.codigo_canal = @p_codigo_canal
		AND dp.codigo_tipo_venta = @p_codigo_tipo_venta
		AND dp.codigo_articulo = @p_codigo_articulo
	ORDER BY 
		p.codigo_planilla DESC

	SET NOCOUNT OFF
END;
