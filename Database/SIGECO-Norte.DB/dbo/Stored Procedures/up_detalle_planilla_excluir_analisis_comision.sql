CREATE PROCEDURE [dbo].up_detalle_planilla_excluir_analisis_comision
(
	@p_XmlDetalleCronograma		XML,
	@p_motivo					VARCHAR(200),
	@p_usuario_registra			VARCHAR(30),
	@p_permanente				BIT
)
AS
BEGIN

	DECLARE	
		@v_observacion				VARCHAR(200)
		,@v_excluyo					BIT
		,@v_codigo_detalle_planilla	INT
		,@v_indice					INT = 1
		,@v_total					INT = 0
	
	DECLARE		
		@t_detalle_cronograma TABLE(
			indice						INT IDENTITY(1, 1)
			,codigo_detalle_cronograma	INT
			,codigo_detalle_planilla	INT
		);

	;WITH ParsedXML AS
	(
	SELECT
		ParamValues.C.value('@codigo_detalle_cronograma', 'int') AS codigo_detalle_cronograma
	FROM 
		@p_XmlDetalleCronograma.nodes('//cronograma/detalle') AS ParamValues(C)
	)
	INSERT INTO 
		@t_detalle_cronograma
		(
			codigo_detalle_cronograma
		)
	SELECT
		codigo_detalle_cronograma
	FROM
		ParsedXml p 

	SET @v_total = (SELECT COUNT(indice) FROM @t_detalle_cronograma)

	UPDATE 
		@t_detalle_cronograma 
	SET
		codigo_detalle_planilla = dp.codigo_detalle_planilla
	FROM
		dbo.detalle_planilla dp
	INNER JOIN dbo.planilla p
		on dp.codigo_planilla = p.codigo_planilla and p.codigo_estado_planilla = 1
	INNER JOIN @t_detalle_cronograma a ON dp.codigo_detalle_cronograma = a.codigo_detalle_cronograma
	WHERE
		dp.excluido = 0
		AND dp.estado_registro = 1

	WHILE (@v_indice <= @v_total)
	BEGIN

		SET @v_codigo_detalle_planilla = (SELECT TOP 1 codigo_detalle_planilla FROM @t_detalle_cronograma WHERE indice = @v_indice)

		EXEC dbo.up_detalle_planilla_excluir @v_codigo_detalle_planilla, @p_motivo, @p_usuario_registra , @p_permanente, @v_observacion output, @v_excluyo output

		IF (@v_excluyo = 0)
		BEGIN
			RAISERROR(@v_observacion, 16, 1);
			RETURN;
		END

		SET @v_indice = @v_indice + 1
	END

END;