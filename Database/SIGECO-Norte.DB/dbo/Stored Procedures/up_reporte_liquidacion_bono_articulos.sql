CREATE PROCEDURE [dbo].[up_reporte_liquidacion_bono_articulos]
(
	@p_codigo_planilla	INT
	,@p_codigo_empresa	INT
	,@p_nro_contrato	VARCHAR(100)
)
AS
BEGIN
	SET NOCOUNT ON
	
	SELECT
		@p_codigo_planilla as codigo_planilla
		,a.nombre AS nombre_articulo
		,apb.codigo_empresa
		,nro_contrato
		,monto_contratado
		,CASE WHEN apb.excluido = 0 THEN apb.dinero_ingresado ELSE 0.00 END AS dinero_ingresado
		,CONVERT(INT, dc.Quantity) AS cantidad
	FROM
		dbo.articulo_planilla_bono apb
	INNER JOIN dbo.articulo a
		ON a.codigo_articulo = apb.codigo_articulo
	INNER JOIN dbo.empresa_sigeco e 
		ON e.codigo_empresa = apb.codigo_empresa
	INNER JOIN dbo.detalle_contrato dc 
		ON dc.NumAtCard = nro_contrato and dc.Codigo_empresa = e.codigo_equivalencia and dc.ItemCode = a.codigo_sku
	WHERE	
		apb.codigo_planilla_bono = @p_codigo_planilla
		AND (@p_codigo_empresa is null or (@p_codigo_empresa is not null and apb.codigo_empresa = @p_codigo_empresa))
		AND (@p_nro_contrato is null or (@p_nro_contrato is not null and apb.nro_contrato = @p_nro_contrato))
		and apb.excluido = 0
	SET NOCOUNT OFF
END;