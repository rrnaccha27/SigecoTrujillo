CREATE PROCEDURE [dbo].up_generar_cronograma_comision
AS
BEGIN

	DECLARE 
		 @p_resultado			BIT
		,@p_observacion			VARCHAR(200)
		,@p_codigo_proceso		UNIQUEIDENTIFIER
		,@p_NumAtCard			NVARCHAR(100)
		,@p_Codigo_empresa		NVARCHAR(4)
		,@p_codigo_cronograma	INT
		,@p_genera_comision		BIT

	DECLARE
		 @v_esContratoAdicional	BIT = 0
		,@v_NumAtCard_ref		NVARCHAR(100)
		,@v_Codigo_empresa_ref	INT

	IF (SELECT CURSOR_STATUS('global','contrato_cursor')) >= -1
	BEGIN
		IF (SELECT CURSOR_STATUS('global','contrato_cursor')) > -1
		BEGIN
			CLOSE contrato_cursor
		END
		DEALLOCATE contrato_cursor
	END

	--Obtenemos los Contratos No Procesados
	DECLARE contrato_cursor CURSOR FAST_FORWARD FORWARD_ONLY FOR   
	SELECT 
		Codigo_empresa, NumAtCard
	FROM
		dbo.contrato_migrado
	WHERE 
		codigo_estado_proceso = 1

	OPEN contrato_cursor
	FETCH NEXT FROM contrato_cursor
	INTO @p_Codigo_empresa, @p_NumAtCard

	WHILE @@FETCH_STATUS = 0  
	BEGIN 

		SET @v_esContratoAdicional = dbo.fn_generar_cronograma_comision_eval_hoja_resumen(@p_NumAtCard)

		SET @p_genera_comision = 0
		EXEC up_generar_cronograma_validar_contrato @p_Codigo_empresa, @p_NumAtCard, @p_genera_comision output, @p_observacion output

		-- Validacion Contrato
		IF (@p_genera_comision = 1) 
		BEGIN

			SET @p_resultado = 0
			SET @p_observacion = ''
			SET @p_codigo_cronograma = NULL

			EXEC up_generar_cronograma_comision_vendedor @p_Codigo_empresa, @p_NumAtCard, @p_resultado output, @p_observacion output, @p_codigo_cronograma output, @p_codigo_proceso output

			--select @p_resultado as Resultado, @p_observacion AS Observacion
			--select * from log_proceso_contrato where id = @p_codigo_proceso

			IF (@p_resultado = 1)
			BEGIN
				
				IF (@v_esContratoAdicional = 1)
				BEGIN
					SELECT
						@v_NumAtCard_ref = num_contrato_referencia
						,@v_Codigo_empresa_ref = e.codigo_empresa
					FROM
						dbo.cabecera_contrato cc
					INNER JOIN dbo.empresa_sigeco e ON e.codigo_equivalencia = cc.Codigo_empresa
					WHERE	
						cc.NumAtCard = @p_NumAtCard
						AND cc.Codigo_empresa = @p_Codigo_empresa

					UPDATE dbo.cronograma_pago_comision 
					SET
						nro_contrato_adicional = nro_contrato
						,nro_contrato = @v_NumAtCard_ref						
					WHERE 
						codigo_cronograma = @p_codigo_cronograma
				END
				
				IF (ISNULL(@p_codigo_cronograma, 0) > 0)
					EXEC up_generar_cronograma_comision_supervisor @p_Codigo_empresa, @p_NumAtCard, @p_codigo_cronograma, @p_observacion output
				
				UPDATE dbo.contrato_migrado 
				SET
					 codigo_estado_proceso = 3
					,Fec_Proceso = GETDATE()
					,Observacion = @p_observacion
				WHERE 
					NumAtCard = @p_NumAtCard
					AND Codigo_empresa = @p_Codigo_empresa
			END
			ELSE
			BEGIN
				UPDATE dbo.contrato_migrado 
				SET
					codigo_estado_proceso = 2,
					Fec_Proceso = GETDATE(),
					Observacion = @p_observacion
				WHERE 
					NumAtCard = @p_NumAtCard
					AND Codigo_empresa = @p_Codigo_empresa
			END

			EXEC up_generar_cronograma_comision_eliminar_ceros @p_Codigo_empresa, @p_NumAtCard
			DELETE FROM dbo.regla_pago_comision_temporal WHERE id = @p_codigo_proceso
		END -- Validacion Contrato
		ELSE
			UPDATE dbo.contrato_migrado 
			SET
				codigo_estado_proceso = 2,
				Fec_Proceso = GETDATE(),
				Observacion = @p_observacion
			WHERE 
				NumAtCard = @p_NumAtCard
				AND Codigo_empresa = @p_Codigo_empresa			
		
		FETCH NEXT FROM contrato_cursor
		INTO @p_Codigo_empresa, @p_NumAtCard

	END   
	CLOSE contrato_cursor;  
	DEALLOCATE contrato_cursor; 

END -- SP