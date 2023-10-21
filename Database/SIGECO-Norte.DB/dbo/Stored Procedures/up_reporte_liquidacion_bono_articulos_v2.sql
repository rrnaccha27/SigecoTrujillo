CREATE PROCEDURE [dbo].[up_reporte_liquidacion_bono_articulos_v2]
(
	 @p_codigo_planilla	INT
	--,@p_codigo_empresa	INT
	--,@p_nro_contrato	VARCHAR(100)
)
AS
BEGIN
	SET NOCOUNT ON

	IF EXISTS(SELECT codigo_planilla FROM sigeco_reporte_bono_articulos_vendedor WHERE codigo_planilla = @p_codigo_planilla)
	BEGIN
		SELECT 
			codigo_planilla,nombre_articulo,codigo_empresa,nro_contrato,monto_contratado,dinero_ingresado,cantidad
		FROM 
			sigeco_reporte_bono_articulos_vendedor 
		WHERE 
			codigo_planilla = @p_codigo_planilla
			--AND codigo_empresa = @p_codigo_empresa
			--AND nro_contrato = @p_nro_contrato
		ORDER BY 
			nombre_articulo
		SELECT 'CONGELADO'
	END
	ELSE
	BEGIN
		exec up_reporte_liquidacion_bono_articulos @p_codigo_planilla, NULL, NULL
		select 'en vivo'
	END	
	SET NOCOUNT OFF
END;