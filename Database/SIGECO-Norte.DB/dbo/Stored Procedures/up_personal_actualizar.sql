USE SIGECO
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[up_personal_actualizar]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].up_personal_actualizar
GO

CREATE PROCEDURE [dbo].[up_personal_actualizar]
(
	@codigo_personal		INT
	,@codigo_moneda int
	,@codigo_tipo_cuenta    int
	,@codigo_banco			INT
	,@codigo_tipo_documento	INT
	,@nombre				VARCHAR(250)
	,@apellido_paterno		VARCHAR(250)
	,@apellido_materno		VARCHAR(250)
	,@nro_documento			VARCHAR(15)
	,@nro_ruc				VARCHAR(11)
	,@telefono_fijo			VARCHAR(10)
	,@telefono_celular		VARCHAR(10)
	,@correo_electronico	VARCHAR(100)
	,@nro_cuenta			VARCHAR(50)
	,@codigo_interbancario	VARCHAR(50)
	,@es_persona_juridica	BIT=NULL
	,@usuario_modifica		VARCHAR(50)
)
AS
BEGIN

	DECLARE
		@v_remover_validacion BIT = 0

	DECLARE
		@r_codigo_banco INT
		,@r_codigo_moneda INT
		,@r_codigo_tipo_cuenta INT
		,@r_codigo_tipo_documento INT
		,@r_nro_cuenta VARCHAR(50)
		,@r_codigo_interbancario VARCHAR(50)
		,@r_nro_documento VARCHAR(15)
		,@r_nro_ruc VARCHAR(11)
		,@r_validado BIT
			
	SELECT TOP 1
		 @r_codigo_banco = codigo_banco
		,@r_codigo_moneda = codigo_cuenta_moneda
		,@r_codigo_tipo_cuenta = codigo_tipo_cuenta
		,@r_codigo_tipo_documento = codigo_tipo_documento
		,@r_nro_cuenta = nro_cuenta
		,@r_codigo_interbancario = codigo_interbancario
		,@r_nro_documento = nro_documento
		,@r_nro_ruc = nro_ruc
		,@r_validado  = validado
	FROM
		dbo.personal
	WHERE
		codigo_personal = @codigo_personal

	IF (
		 @r_codigo_banco <> @codigo_banco
		OR @r_codigo_moneda <> @codigo_moneda
		OR @r_codigo_tipo_cuenta <> @codigo_tipo_cuenta
		OR @r_codigo_tipo_documento <> @codigo_tipo_documento
		OR @r_nro_cuenta <> @nro_cuenta
		OR @r_codigo_interbancario <> @codigo_interbancario
		OR @r_nro_documento <> @nro_documento
		OR @r_nro_ruc <> @nro_ruc
	)
	BEGIN
		SET @v_remover_validacion = 1
	END

	UPDATE 
		dbo.personal
	SET
		codigo_banco = @codigo_banco
		,codigo_cuenta_moneda=@codigo_moneda
		,codigo_tipo_cuenta=@codigo_tipo_cuenta
		,codigo_tipo_documento = @codigo_tipo_documento
		,nombre = @nombre
		,apellido_paterno = @apellido_paterno
		,apellido_materno = @apellido_materno
		,nro_documento = @nro_documento
		,nro_ruc = @nro_ruc 
		,telefono_fijo = @telefono_fijo
		,telefono_celular = @telefono_celular
		,correo_electronico = @correo_electronico
		,nro_cuenta = @nro_cuenta
		,codigo_interbancario = @codigo_interbancario
		,es_persona_juridica = @es_persona_juridica 
		,estado_registro = 1
		,usuario_modifica = @usuario_modifica
		,fecha_modifica = GETDATE()
	WHERE
		codigo_personal = @codigo_personal

	IF (@r_validado = 1 AND @v_remover_validacion = 1)
	BEGIN

		UPDATE dbo.personal
		SET
			validado = 0
			,fecha_validado = NULL
			,usuario_validado = NULL
		WHERE
			codigo_personal = @codigo_personal

		UPDATE dbo.personal_historial_validacion
		SET 
			estado_registro = 0
			,fecha_modifica = GETDATE()
			,usuario_modifica = @usuario_modifica
		WHERE
			codigo_personal = @codigo_personal
			AND estado_registro = 1
	END


END;