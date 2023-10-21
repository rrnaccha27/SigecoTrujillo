CREATE PROC dbo.up_log_contrato_sap_habilitar_reproceso
(
	 @p_contratosXML	XML
	,@p_codigo_usuario	VARCHAR(50)
)
AS
BEGIN
	SET NOCOUNT ON

	DECLARE		
		@t_contratos TABLE(
			codigo_empresa		VARCHAR(4)
			,nro_contrato		VARCHAR(10)
			,contrato_bloqueado	bit
		);
	DECLARE @v_mensaje VARCHAR(100)
	;WITH ParsedXML AS
	(
	SELECT
		ParamValues.C.value('@codigo_empresa', 'varchar(4)') AS codigo_empresa
		,ParamValues.C.value('@nro_contrato', 'varchar(100)') AS nro_contrato
	FROM 
		@p_contratosXML.nodes('//contratos/contrato') AS ParamValues(C)
	)
	INSERT INTO 
		@t_contratos
		(
			codigo_empresa
			,nro_contrato
			,contrato_bloqueado
		)
	SELECT
		codigo_empresa
		,nro_contrato
		,ISNULL(CASE WHEN EXISTS(select top 1 id from contrato_migrado cm where cm.Codigo_empresa= p.codigo_empresa and cm.NumAtCard = p.nro_contrato and cm.bloqueo = 1) THEN 1 ELSE 0 END, 0)
	FROM
		ParsedXml p 
	--select * from @t_contratos
	UPDATE
		dbo.contrato_migrado
	SET
		codigo_estado_proceso = 1
		,Observacion = NULL
		,Fec_Proceso = NULL
		,codigo_usuario = @p_codigo_usuario
	FROM
		dbo.contrato_migrado cm
	INNER JOIN @t_contratos c
		ON c.codigo_empresa = cm.Codigo_empresa AND c.nro_contrato = cm.NumAtCard
	WHERE 
		c.contrato_bloqueado = 0
	
	IF EXISTS(SELECT TOP 1 nro_contrato FROM @t_contratos WHERE contrato_bloqueado = 1)
		SELECT 'Algunos contratos no se procesaran por estar bloqueados.' as mensaje
	--ELSE
	--	SET @v_mensaje = ''

	--SELECT @v_mensaje as mensaje

	SET NOCOUNT OFF
END