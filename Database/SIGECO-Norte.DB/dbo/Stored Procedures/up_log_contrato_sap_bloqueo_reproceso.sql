CREATE PROCEDURE dbo.up_log_contrato_sap_bloqueo_reproceso
(
	 @p_nro_contrato				VARCHAR(100)
	,@p_codigo_empresa			INT
	,@p_bloqueo					INT
	,@p_codigo_usuario_bloqueo	VARCHAR(30)
	,@p_motivo					VARCHAR(250)
)
AS
BEGIN
	DECLARE 
		@v_codigo_empresa	VARCHAR(4)
		,@v_codigo_bloqueo	INT

	SET @v_codigo_empresa = (SELECT TOP 1 codigo_equivalencia FROM dbo.empresa_sigeco WHERE codigo_empresa = @p_codigo_empresa)

	UPDATE 
		dbo.contrato_bloqueo
	SET
		estado_registro = 0
	WHERE
		numatcard = @p_nro_contrato
		AND codigo_empresa = @v_codigo_empresa
	
	INSERT INTO dbo.contrato_bloqueo(numatcard, codigo_empresa, bloqueo, motivo, usuario_registro, fecha_registro, estado_registro)
	VALUES(@p_nro_contrato, @v_codigo_empresa, @p_bloqueo,@p_motivo, @p_codigo_usuario_bloqueo, GETDATE(), 1)
	SET @v_codigo_bloqueo = SCOPE_IDENTITY()

	UPDATE contrato_migrado
	SET
		bloqueo = @p_bloqueo
		,codigo_bloqueo = @v_codigo_bloqueo
	WHERE
		Codigo_empresa = @v_codigo_empresa
		AND NumAtCard = @p_nro_contrato

END;