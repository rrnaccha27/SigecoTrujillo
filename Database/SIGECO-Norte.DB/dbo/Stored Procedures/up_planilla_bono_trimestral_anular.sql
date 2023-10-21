USE SIGECO
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[up_planilla_bono_trimestral_anular]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].up_planilla_bono_trimestral_anular
GO

CREATE PROCEDURE [dbo].up_planilla_bono_trimestral_anular
(
	@p_codigo_planilla		INT,
	@p_usuario_anulacion	VARCHAR(30)
)
AS
BEGIN
	SET NOCOUNT ON
	
	DECLARE
		@v_fecha_proceso						DATETIME = GETDATE()
		,@c_codigo_estado_cerrado				INT = 2 --INDICA CERRADA LA PLANILLA
		,@c_codigo_estado_anulado				INT = 3 --INDICA ANULADO LA PLANILLA
		,@c_codigo_tipo_bloqueo_bono_trimestral	INT = 3 --BONO TRIMESTRAL

	IF (EXISTS(SELECT codigo_planilla FROM dbo.planilla_bono_trimestral WHERE codigo_planilla=@p_codigo_planilla AND codigo_estado_planilla = @c_codigo_estado_cerrado))
	BEGIN
		RAISERROR('La planilla bono no se puede anular pues se encuentra cerrada.',16,1); 
		RETURN;
	END;

	UPDATE 
		dbo.planilla_bono_trimestral
	SET
		codigo_estado_planilla = @c_codigo_estado_anulado,
		fecha_anulacion = @v_fecha_proceso,
		usuario_anulacion = @p_usuario_anulacion,
		usuario_modifica = @p_usuario_anulacion,
		fecha_modifica = @v_fecha_proceso
	WHERE
		codigo_planilla = @p_codigo_planilla;

	/*
	DESBLOQUEO DE VENDEDORES
	*/
	EXEC dbo.up_personal_bloqueo_anular @p_codigo_planilla, @c_codigo_tipo_bloqueo_bono_trimestral, @p_usuario_anulacion

	SET NOCOUNT OFF
END;


