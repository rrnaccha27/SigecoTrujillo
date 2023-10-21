USE SIGECO
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[up_planilla_bono_trimestral_cerrar]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].up_planilla_bono_trimestral_cerrar
GO

CREATE PROCEDURE [dbo].up_planilla_bono_trimestral_cerrar
(
	@p_codigo_planilla	INT,
	@p_usuario_cerrar	VARCHAR(30)
)
AS
BEGIN
	SET NOCOUNT ON
	
	DECLARE
		@v_fecha_proceso						DATETIME = GETDATE()
		,@c_codigo_estado_cerrado				INT = 2 --INDICA CERRADA LA PLANILLA
		,@c_codigo_estado_anulado				INT = 3 --INDICA ANULADO LA PLANILLA
		,@c_codigo_tipo_bloqueo_bono_trimestral	INT = 3 --BONO TRIMESTRAL

	IF (EXISTS(SELECT codigo_planilla FROM dbo.planilla_bono_trimestral WHERE codigo_planilla=@p_codigo_planilla AND codigo_estado_planilla = @c_codigo_estado_anulado))
	BEGIN
		RAISERROR('La planilla bono no se puede cerrar pues se encuentra anulada.',16,1); 
		RETURN;
	END;

	UPDATE 
		dbo.planilla_bono_trimestral
	SET
		codigo_estado_planilla = @c_codigo_estado_cerrado,
		fecha_cierre = @v_fecha_proceso,
		usuario_cierre = @p_usuario_cerrar,
		usuario_modifica = @p_usuario_cerrar,
		fecha_modifica = @v_fecha_proceso
	WHERE
		codigo_planilla = @p_codigo_planilla;

	/*
	TODO: procesos para congelar las liquidaciones
	*/

	-- TXT RRHH
	DELETE FROM dbo.sigeco_reporte_bono_trimestral_rrhh WHERE codigo_planilla = @p_codigo_planilla
	INSERT INTO dbo.sigeco_reporte_bono_trimestral_rrhh
	EXEC dbo.up_planilla_bono_trimestral_detalle_txt_rrhh @p_codigo_planilla

	-- TXT CONTABILIDAD
	DECLARE @t_temp AS TABLE(
		id				INT IDENTITY(1, 1)
		,codigo_empresa	INT
	)

	DECLARE
		@v_id				INT
		,@v_total			INT
		,@v_codigo_empresa	INT

	DELETE FROM dbo.sigeco_reporte_bono_trimestral_contabilidad_resumen WHERE codigo_planilla = @p_codigo_planilla
	INSERT INTO dbo.sigeco_reporte_bono_trimestral_contabilidad_resumen
	EXEC up_planilla_bono_trimestral_contabilidad_resumen @p_codigo_planilla

	DELETE FROM dbo.sigeco_reporte_bono_trimestral_contabilidad WHERE codigo_planilla = @p_codigo_planilla
	INSERT INTO @t_temp SELECT codigo_empresa FROM dbo.sigeco_reporte_bono_trimestral_contabilidad_resumen WHERE codigo_planilla = @p_codigo_planilla
	SELECT @v_id = MIN(id), @v_total = MAX(id) FROM @t_temp

	WHILE (@v_id <= @v_total)
	BEGIN
		SELECT TOP 1 @v_codigo_empresa = codigo_empresa FROM @t_temp WHERE id = @v_id
			
		INSERT INTO dbo.sigeco_reporte_bono_trimestral_contabilidad
		EXEC dbo.up_planilla_bono_trimestral_contabilidad @p_codigo_planilla, @v_codigo_empresa
			
		SET @v_id = @v_id + 1
	END

	/*
	DESBLOQUEO DE VENDEDORES
	*/
	EXEC dbo.up_personal_bloqueo_anular @p_codigo_planilla, @c_codigo_tipo_bloqueo_bono_trimestral, @p_usuario_cerrar

	SET NOCOUNT OFF
END;


