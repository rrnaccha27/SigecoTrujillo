CREATE FUNCTION [dbo].[fn_generar_cronograma_comision_eval_servicio_funerario]
(
	@p_nro_contrato		VARCHAR(100)
	,@p_codigo_empresa	VARCHAR(4)
)
RETURNS BIT
AS
BEGIN
	DECLARE 
		@v_esContratoServFun	BIT = 0
		,@c_tipo_articulo_sf	INT = 3
		,@c_categoria_sf		INT = 3

	IF EXISTS
	(
		SELECT
			dc.ItemCode
		FROM 
			dbo.detalle_contrato dc
		INNER JOIN dbo.articulo a
			ON dc.NumAtCard = @p_nro_contrato AND dc.Codigo_empresa = @p_codigo_empresa AND a.codigo_sku = dc.ItemCode AND a.estado_registro = 1
		WHERE
			dc.NumAtCard = @p_nro_contrato 
			AND dc.Codigo_empresa = @p_codigo_empresa
			AND (a.codigo_tipo_articulo = @c_tipo_articulo_sf OR a.codigo_unidad_negocio = @c_categoria_sf)
	)
		SET @v_esContratoServFun = 1

	RETURN @v_esContratoServFun
END