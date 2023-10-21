CREATE PROCEDURE dbo.up_reclamo_estado_contrato
(
	@p_codigo_empresa	INT
	,@p_nro_contrato	VARCHAR(100)
)
AS
BEGIN
	DECLARE
		@v_codigo_equivalencia				VARCHAR(4)

	SET @v_codigo_equivalencia = (SELECT TOP 1 codigo_equivalencia FROM dbo.empresa_sigeco WHERE codigo_empresa = @p_codigo_empresa)

	SELECT 
		cm.codigo_estado_proceso
		,ep.nombre as nombre_estado_proceso
		,cm.observacion
	FROM 
		dbo.contrato_migrado cm 
	INNER JOIN estado_proceso ep 
		ON ep.codigo_estado_proceso = cm.codigo_estado_proceso
	WHERE 
		cm.Codigo_empresa = @v_codigo_equivalencia
		AND cm.NumAtCard = @p_nro_contrato
END