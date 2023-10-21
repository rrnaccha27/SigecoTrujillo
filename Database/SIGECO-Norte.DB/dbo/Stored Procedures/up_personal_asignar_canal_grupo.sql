CREATE PROCEDURE dbo.up_personal_asignar_canal_grupo
(
	@p_codigo_canal_grupo			INT
	,@p_codigo_personal_supervisor	INT
	,@p_es_canal_grupo				INT
	,@p_usuario_modifica			VARCHAR(50)
	,@p_PersonalXML					XML
)
AS
BEGIN
BEGIN TRAN

DECLARE
	@v_codigo_padre	INT
	
	DELETE FROM dbo.personal_canal_grupo WHERE codigo_canal_grupo = @p_codigo_canal_grupo

	IF @p_es_canal_grupo = 0
		SET @v_codigo_padre = (SELECT TOP 1 codigo_padre FROM dbo.canal_grupo WHERE codigo_canal_grupo = @p_codigo_canal_grupo)
	ELSE
		SET @v_codigo_padre = @p_codigo_canal_grupo

	;WITH ParsedXML AS
	(
	SELECT
		ParamValues.C.value('@codigo_personal', 'int') AS codigo_personal
		,ParamValues.C.value('@percibe_comision', 'bit') AS percibe_comision
		,ParamValues.C.value('@percibe_bono', 'bit') AS percibe_bono
	FROM 
		@p_PersonalXML.nodes('//personal/persona') AS ParamValues(C)
	)
	INSERT INTO 
		dbo.personal_canal_grupo
		(
			codigo_personal
			,codigo_canal_grupo
			,codigo_canal
			,es_supervisor_canal
			,es_supervisor_grupo
			,percibe_comision
			,percibe_bono
			,estado_registro
			,fecha_registra
			,usuario_registra
		)
	SELECT
		p.codigo_personal
		,@p_codigo_canal_grupo
		,@v_codigo_padre
		,0
		,0
		,p.percibe_comision
		,p.percibe_bono
		,1
		,GETDATE()
		,@p_usuario_modifica
	FROM
		ParsedXml p 

	UPDATE 
		dbo.personal_canal_grupo
	SET
		es_supervisor_canal = CASE WHEN @p_es_canal_grupo = 1 THEN 1 ELSE 0 END
		,es_supervisor_grupo = CASE WHEN @p_es_canal_grupo = 0 THEN 1 ELSE 0 END
	WHERE
		codigo_personal = @p_codigo_personal_supervisor
		AND codigo_canal_grupo = @p_codigo_canal_grupo

COMMIT TRAN
END