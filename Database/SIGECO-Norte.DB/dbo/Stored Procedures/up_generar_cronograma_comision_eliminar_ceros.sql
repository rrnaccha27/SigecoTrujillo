CREATE PROCEDURE dbo.up_generar_cronograma_comision_eliminar_ceros 
(
	@p_Codigo_empresa		VARCHAR(4)
	,@p_NumAtCard			VARCHAR(100)
)
AS
BEGIN
	SET NOCOUNT ON
	DECLARE
		 @v_codigo_empresa	INT
	DECLARE
		@t_cronogramas TABLE(
			codigo_cronograma	INT
		)

	SET @v_codigo_empresa = (SELECT TOP 1 codigo_empresa FROM dbo.empresa_sigeco WHERE codigo_equivalencia = @p_Codigo_empresa)
	
	INSERT INTO 
		@t_cronogramas
	SELECT 
		codigo_cronograma
	FROM 
		dbo.cronograma_pago_comision 
	WHERE 
		nro_contrato = @p_NumAtCard
		AND codigo_empresa = @v_codigo_empresa

	DELETE FROM
		dbo.detalle_cronograma
	WHERE
		codigo_cronograma IN (SELECT codigo_cronograma FROM @t_cronogramas)
		AND monto_neto = 0.00

	DELETE FROM
		dbo.articulo_cronograma
	WHERE
		codigo_cronograma IN (SELECT codigo_cronograma FROM @t_cronogramas)
		AND monto_comision = 0.00

	SET NOCOUNT OFF
END;