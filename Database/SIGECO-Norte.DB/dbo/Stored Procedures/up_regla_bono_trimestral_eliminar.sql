USE SIGECO
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[up_regla_bono_trimestral_eliminar]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].up_regla_bono_trimestral_eliminar
GO

CREATE PROCEDURE dbo.up_regla_bono_trimestral_eliminar
(
	@p_codigo_regla	INT,
	@p_usuario_registra				VARCHAR(50)
)
AS
BEGIN
	SET NOCOUNT ON

	UPDATE 
		dbo.regla_bono_trimestral_meta
	SET
		estado_registro = 0,
		usuario_modifica = @p_usuario_registra,
		fecha_modifica = GETDATE()	
	 WHERE 
		codigo_regla = @p_codigo_regla
		AND estado_registro = 1

	UPDATE 
		dbo.regla_bono_trimestral_detalle
	SET
		estado_registro = 0,
		usuario_modifica = @p_usuario_registra,
		fecha_modifica = GETDATE()	
	 WHERE 
		codigo_regla = @p_codigo_regla
		AND estado_registro = 1

	UPDATE 
		dbo.regla_bono_trimestral 
	SET
		estado_registro = 0,
		usuario_modifica = @p_usuario_registra,
		fecha_modifica = GETDATE()	
	 WHERE 
		codigo_regla = @p_codigo_regla
		AND estado_registro = 1

	SET NOCOUNT OFF
END;
