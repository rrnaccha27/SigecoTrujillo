CREATE PROCEDURE dbo.up_personal_canal_grupo_eliminar
(
	@p_CanalesXML	XML
)
AS
BEGIN

	;WITH ParsedXML AS
	(
	SELECT
		ParamValues.C.value('@codigo_registro', 'int') AS codigo_registro
	FROM 
		@p_CanalesXML.nodes('//canales_grupos/canal_grupo') AS ParamValues(C)
	)

	DELETE pcg 
	FROM dbo.personal_canal_grupo pcg
	INNER JOIN ParsedXML p ON pcg.codigo_registro = p.codigo_registro

END