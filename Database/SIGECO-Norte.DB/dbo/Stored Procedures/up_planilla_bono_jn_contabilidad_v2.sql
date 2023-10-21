USE SIGECO
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[up_planilla_bono_jn_contabilidad_v2]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].up_planilla_bono_jn_contabilidad_v2
GO

CREATE PROCEDURE [dbo].up_planilla_bono_jn_contabilidad_v2
(
	 @p_codigo_planilla	INT
)
AS
BEGIN
	SET NOCOUNT ON

	IF EXISTS(SELECT codigo_planilla FROM dbo.sigeco_reporte_bono_jn_contabilidad WHERE codigo_planilla = @p_codigo_planilla)
	BEGIN
		SELECT 
			codigo_planilla,empresa,bono,unidad_negocio
		FROM 
			dbo.sigeco_reporte_bono_jn_contabilidad
		WHERE 
			codigo_planilla = @p_codigo_planilla

		SELECT 'CONGELADO'
	END
	ELSE
	BEGIN
		EXEC up_planilla_bono_jn_contabilidad @p_codigo_planilla
		SELECT 'en vivo'
	END	
	SET NOCOUNT OFF
END;