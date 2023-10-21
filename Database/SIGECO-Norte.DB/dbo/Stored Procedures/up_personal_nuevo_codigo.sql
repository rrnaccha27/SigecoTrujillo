CREATE PROCEDURE dbo.up_personal_nuevo_codigo
(
	@p_codigo_personal		INT
	,@p_codigo_canal_grupo	INT
	,@p_usuario				VARCHAR(50)
)
AS
BEGIN

	DECLARE
		@v_codigo_personal_nuevo	INT
		,@v_codigo_equivalencia		NVARCHAR(10)
	
	BEGIN TRY
		BEGIN TRAN personal_canal_grupo

		SET @v_codigo_equivalencia = (SELECT MAX(CONVERT(NUMERIC(10), codigo_equivalencia)) FROM dbo.personal)
		SET @v_codigo_equivalencia = CASE WHEN @v_codigo_equivalencia IS NULL THEN '1' ELSE CONVERT(NVARCHAR(10), CONVERT(NUMERIC(10), @v_codigo_equivalencia) + 1) END
		SET @v_codigo_equivalencia = REPLICATE('0', 10 - LEN(@v_codigo_equivalencia)) + @v_codigo_equivalencia

		--print @v_codigo_equivalencia

		INSERT INTO dbo.personal
		(
			codigo_equivalencia
			,codigo_tipo_cuenta
			,codigo_cuenta_moneda
			,codigo_tipo_documento
			,codigo_banco
			,es_persona_juridica
			,nombre
			,apellido_paterno
			,apellido_materno
			,nro_documento
			,nro_ruc
			,telefono_fijo
			,telefono_celular
			,correo_electronico
			,nro_cuenta
			,codigo_interbancario
			,estado_registro
			,fecha_registra
			,usuario_registra
		)
		SELECT
			@v_codigo_equivalencia
			,codigo_tipo_cuenta
			,codigo_cuenta_moneda
			,codigo_tipo_documento
			,codigo_banco
			,es_persona_juridica
			,nombre
			,apellido_paterno
			,apellido_materno
			,nro_documento
			,nro_ruc
			,telefono_fijo
			,telefono_celular
			,correo_electronico
			,nro_cuenta
			,codigo_interbancario
			,estado_registro
			,GETDATE()
			,@p_usuario
		FROM	
			dbo.personal
		WHERE
			codigo_personal = @p_codigo_personal

		SET @v_codigo_personal_nuevo = @@IDENTITY

		--print convert(varchar(10), @v_codigo_personal_nuevo)

		UPDATE
			dbo.personal
		SET
			estado_registro = 0
			,fecha_modifica = GETDATE()
			,usuario_modifica = @p_usuario
		WHERE
			codigo_personal = @p_codigo_personal	
		
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
			@v_codigo_personal_nuevo
			,codigo_canal_grupo
			,codigo_canal
			,es_supervisor_canal
			,es_supervisor_grupo
			,percibe_comision
			,percibe_bono
			,estado_registro
			,GETDATE()
			,@p_usuario
		FROM
			dbo.personal_canal_grupo
		WHERE
			codigo_personal = @p_codigo_personal
			AND codigo_canal_grupo <> @p_codigo_canal_grupo
			AND estado_registro = 1
		
		UPDATE
			dbo.personal_canal_grupo
		SET
			estado_registro = 0
			,fecha_modifica = GETDATE()
			,usuario_modifica = @p_usuario
		WHERE
			codigo_personal = @p_codigo_personal
			AND estado_registro = 1
			
		SELECT @v_codigo_personal_nuevo AS codigo_personal, @v_codigo_equivalencia AS codigo_equivalencia
		COMMIT TRAN personal_canal_grupo
	END TRY
	
	BEGIN CATCH
		ROLLBACK TRAN personal_canal_grupo
	END CATCH
END