create PROCEDURE [dbo].[up_personal_insertar]
(
	 @codigo_banco               int
	,@codigo_moneda int
	,@codigo_tipo_cuenta        int
	,@codigo_tipo_documento     int
	,@nombre                    varchar(250)
	,@apellido_paterno          varchar(250)
	,@apellido_materno          varchar(250)
	,@nro_documento             varchar(15)
	,@nro_ruc		            varchar(11)
	,@telefono_fijo             varchar(10)
	,@telefono_celular          varchar(10)
	,@correo_electronico        varchar(100)
	,@nro_cuenta                varchar(50)
	,@codigo_interbancario      varchar(50)=NULL
	,@es_persona_juridica       bit=NULL
	,@usuario_registra          varchar(50)
	,@codigo_personal			INT OUTPUT
)
AS
BEGIN

	DECLARE 
		@v_codigo_equivalencia NVARCHAR(10)
		
	SET @v_codigo_equivalencia = (SELECT MAX(CONVERT(NUMERIC(10), codigo_equivalencia)) FROM dbo.personal)
	SET @v_codigo_equivalencia = CASE WHEN @v_codigo_equivalencia IS NULL THEN '1' ELSE CONVERT(NVARCHAR(10), CONVERT(NUMERIC(10), @v_codigo_equivalencia) + 1) END
	SET @v_codigo_equivalencia = REPLICATE('0', 10 - LEN(@v_codigo_equivalencia)) + @v_codigo_equivalencia

	INSERT INTO dbo.personal(
		codigo_equivalencia
		,codigo_banco
		,codigo_tipo_documento
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
		,es_persona_juridica
		,estado_registro
		,usuario_registra
		,fecha_registra
		,codigo_cuenta_moneda
		,codigo_tipo_cuenta
	)
	VALUES(
		@v_codigo_equivalencia
		,@codigo_banco
		,@codigo_tipo_documento
		,@nombre
		,@apellido_paterno
		,@apellido_materno
		,@nro_documento
		,@nro_ruc
		,@telefono_fijo
		,@telefono_celular
		,@correo_electronico
		,@nro_cuenta
		,@codigo_interbancario
		,@es_persona_juridica
		,1
		,@usuario_registra
		,GETDATE()
		,@codigo_moneda
		,@codigo_tipo_cuenta
	)
	SET @codigo_personal = @@IDENTITY
END