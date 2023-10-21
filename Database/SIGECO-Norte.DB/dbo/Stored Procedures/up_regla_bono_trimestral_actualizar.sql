USE SIGECO
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[up_regla_bono_trimestral_actualizar]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].up_regla_bono_trimestral_actualizar
GO

CREATE PROCEDURE dbo.up_regla_bono_trimestral_actualizar
(
	@p_codigo_regla			INT
	,@p_vigencia_fin		VARCHAR(8)
	,@p_usuario_modifica	VARCHAR(25)
)
AS
BEGIN
	SET NOCOUNT ON

	IF (@p_vigencia_fin < CONVERT(VARCHAR, GETDATE(), 112))
	BEGIN
		RAISERROR('No se puede modificar la vigencia a una fecha menor a hoy.',16,1); 
		RETURN;
	END;  

	UPDATE 
		dbo.regla_bono_trimestral
	SET 
		vigencia_fin = @p_vigencia_fin
		,usuario_modifica= @p_usuario_modifica
		,fecha_modifica = GETDATE()
	WHERE
		codigo_regla = @p_codigo_regla

	SET NOCOUNT Off
END
