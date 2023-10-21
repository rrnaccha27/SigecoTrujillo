CREATE PROCEDURE dbo.up_contrato_analisis_tipo_planilla
(
	@p_codigo_empresa	INT
	,@p_nro_contrato	VARCHAR(100)
)
AS
BEGIN 
	SET NOCOUNT ON
	
	DECLARE 
		@c_esHR	bit = (SELECT dbo.fn_generar_cronograma_comision_eval_hoja_resumen(@p_nro_contrato))

	SELECT DISTINCT
		cpc.codigo_tipo_planilla
		,tp.nombre 
	FROM
		dbo.cronograma_pago_comision cpc
	INNER JOIN dbo.tipo_planilla tp
		ON tp.codigo_tipo_planilla = cpc.codigo_tipo_planilla
	WHERE
		cpc.codigo_empresa = @p_codigo_empresa
		AND CASE WHEN @c_esHR = 0 THEN cpc.nro_contrato else cpc.nro_contrato_adicional END = @p_nro_contrato
	ORDER BY
		tp.nombre
	
	SET NOCOUNT OFF
END;