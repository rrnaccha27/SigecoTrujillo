USE SIGECO
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[up_regla_bono_trimestral_insertar]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].up_regla_bono_trimestral_insertar
GO

CREATE PROCEDURE dbo.up_regla_bono_trimestral_insertar
(
	@p_descripcion						VARCHAR(250),
	@p_codigo_tipo_bono					INT,
	@p_vigencia_inicio					VARCHAR(8),
	@p_vigencia_fin						VARCHAR(8),
	@p_usuario_registra					VARCHAR(25),
	@p_codigo_regla						INT OUT
)
AS
BEGIN
	SET NOCOUNT ON

	DECLARE 
		@v_cantidad_registro int;

	IF EXISTS(SELECT codigo_regla FROM dbo.regla_bono_trimestral WHERE estado_registro = 1 and UPPER(descripcion) = UPPER(@p_descripcion))
	BEGIN
		RAISERROR('El nombre de la regla ya existe, vuelva ingresar otro nombre.',16,1); 
		RETURN;
	END;  

	IF EXISTS(SELECT codigo_regla FROM dbo.regla_bono_trimestral WHERE estado_registro = 1 and codigo_tipo_bono = @p_codigo_tipo_bono)
	BEGIN
		RAISERROR('El tipo de bono de la regla ya existe, vuelva seleccionar otro tipo.',16,1); 
		RETURN;
	END;  

	IF EXISTS(SELECT codigo_regla FROM dbo.regla_bono_trimestral WHERE estado_registro = 1 and CONVERT(datetime, @p_vigencia_inicio) BETWEEN vigencia_inicio and vigencia_fin and codigo_tipo_bono = @p_codigo_tipo_bono)
	BEGIN
		RAISERROR('El inicio de vigencia coincide con otra regla, vuelva seleccionar otro.',16,1); 
		RETURN;
	END;  

	IF EXISTS(SELECT codigo_regla FROM dbo.regla_bono_trimestral WHERE estado_registro = 1 and CONVERT(datetime, @p_vigencia_fin) BETWEEN vigencia_inicio and vigencia_fin and codigo_tipo_bono = @p_codigo_tipo_bono)
	BEGIN
		RAISERROR('El fin de vigencia coincide con otra regla, vuelva seleccionar otro.',16,1); 
		RETURN;
	END;  

	IF (@p_vigencia_fin < CONVERT(VARCHAR, GETDATE(), 112))
	BEGIN
		RAISERROR('El fin de vigencia no puede ser menor a hoy, vuelva seleccionar otro.',16,1); 
		RETURN;
	END;  

	INSERT INTO dbo.regla_bono_trimestral(
		descripcion, codigo_tipo_bono, vigencia_inicio, vigencia_fin, usuario_registra, estado_registro, fecha_registra
	)
	VALUES(
		@p_descripcion, @p_codigo_tipo_bono, @p_vigencia_inicio, @p_vigencia_fin, @p_usuario_registra, 1, GETDATE()
	);

	SET @p_codigo_regla=SCOPE_IDENTITY();

	SET NOCOUNT Off
END
