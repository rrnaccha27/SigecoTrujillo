USE SIGECO
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[up_planilla_bono_jn_resumen_titulos_v2]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].up_planilla_bono_jn_resumen_titulos_v2
GO

CREATE PROCEDURE [dbo].up_planilla_bono_jn_resumen_titulos_v2
(
	@p_codigo_planilla	INT
)
AS
BEGIN
	SET NOCOUNT ON

	IF EXISTS(SELECT codigo_planilla FROM dbo.sigeco_reporte_bono_jn_resumen_titulo WHERE codigo_planilla = @p_codigo_planilla)
	BEGIN
		SELECT codigo_planilla, titulo_funjar, titulo_ofsa FROM dbo.sigeco_reporte_bono_jn_resumen_titulo WHERE codigo_planilla = @p_codigo_planilla
	END
	ELSE
	BEGIN
		EXEC dbo.up_planilla_bono_jn_resumen_titulos @p_codigo_planilla
	END

	SET NOCOUNT OFF
END;