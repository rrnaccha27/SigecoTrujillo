CREATE PROCEDURE [dbo].up_detalle_cronograma_deshabilitar
(
	@p_XmlDetalleCronograma		XML,
	@p_motivo					VARCHAR(200),
	@p_usuario_registra			VARCHAR(30)
)
AS
BEGIN

	DECLARE	
		@v_codigo_detalle_cronograma	INT
		,@v_indice						INT = 1
		,@v_total						INT = 0
		,@c_codigo_tipo_operacion		INT = 8 --DESHABILITADO
	
	DECLARE		
		@t_detalle_cronograma TABLE(
			indice						INT IDENTITY(1, 1)
			,codigo_detalle_cronograma	INT
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
		dc 
	SET
		fecha_programada = null
	FROM
		dbo.detalle_cronograma dc
	INNER JOIN @t_detalle_cronograma a ON dc.codigo_detalle = a.codigo_detalle_cronograma

	WHILE (@v_indice <= @v_total)
	BEGIN

		SET @v_codigo_detalle_cronograma = (SELECT TOP 1 codigo_detalle_cronograma FROM @t_detalle_cronograma WHERE indice = @v_indice)

		EXEC dbo.up_operacion_cuota_comision_insertar @v_codigo_detalle_cronograma, @c_codigo_tipo_operacion, @p_motivo, @p_usuario_registra

		SET @v_indice = @v_indice + 1
	END

END;