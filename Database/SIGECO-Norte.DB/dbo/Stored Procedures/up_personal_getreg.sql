USE SIGECO
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[up_personal_getreg]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].up_personal_getreg
GO

CREATE PROCEDURE [dbo].[up_personal_getreg]
	@codigo_personal	int
AS
BEGIN
	SELECT TOP 1
		codigo_personal
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
		,codigo_tipo_cuenta
		,codigo_cuenta_moneda
		,usuario_registra
		,dbo.fn_formatear_fecha_hora(fecha_registra) as fecha_registra
		,ISNULL(usuario_modifica, '') as usuario_modifica
		,dbo.fn_formatear_fecha_hora(fecha_modifica) as fecha_modifica
		,CASE WHEN estado_registro = 1 THEN 'ACTIVO' WHEN estado_registro = 0 THEN 'INACTIVO' ELSE '' END as estado_registro_texto 
		,CONVERT(INT, ISNULL(validado, 0)) as validado
		,CASE WHEN validado = 1 THEN 'VALIDADO' WHEN validado = 0 THEN 'NO VALIDADO' ELSE '' END as validado_texto 
		,dbo.fn_formatear_fecha_hora(fecha_validado) as fecha_validado
		,ISNULL(usuario_validado, '') as usuario_validado
	FROM 
		dbo.personal
	WHERE
		codigo_personal = @codigo_personal
END