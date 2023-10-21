CREATE PROCEDURE dbo.up_reclamo_atencion_contrato_migrado
(
	@p_codigo_reclamo		INT
	,@p_respuesta			VARCHAR(1000)
	,@p_usuario_modifica	VARCHAR(20)
)
AS
BEGIN
	
	DECLARE
		@v_codigo_empresa				INT
		,@v_codigo_empresa_equivalencia	VARCHAR(4)
		,@v_nro_contrato				VARCHAR(100)


	BEGIN TRY
		BEGIN TRAN
		UPDATE
			dbo.reclamo
		SET
			respuesta = @p_respuesta
			,codigo_estado_reclamo = 2
			,codigo_estado_resultado = 1
			,usuario_modifica = @p_usuario_modifica
			,fecha_modifica = GETDATE()
			,codigo_estado_resultado_n2 = 1
			,usuario_modifica_n2 = @p_usuario_modifica
			,fecha_modifica_n2 = GETDATE()
			,observacion_n2 = @p_respuesta
		WHERE
			codigo_reclamo = @p_codigo_reclamo

		SELECT TOP 1 @v_codigo_empresa = codigo_empresa, @v_nro_contrato = nrocontrato 
		FROM dbo.reclamo 
		WHERE codigo_reclamo = @p_codigo_reclamo

		SET @v_codigo_empresa_equivalencia = (SELECT TOP 1 codigo_equivalencia FROM dbo.empresa_sigeco WHERE codigo_empresa = @v_codigo_empresa)

		UPDATE 
			dbo.contrato_migrado
		SET
			codigo_estado_proceso = 1,
			Fec_Proceso = NULL,
			Observacion = NULL
		WHERE
			Codigo_empresa = @v_codigo_empresa_equivalencia
			AND NumAtCard = @v_nro_contrato
		COMMIT TRAN
	END TRY
	BEGIN CATCH
		ROLLBACK TRAN 
	END CATCH

END