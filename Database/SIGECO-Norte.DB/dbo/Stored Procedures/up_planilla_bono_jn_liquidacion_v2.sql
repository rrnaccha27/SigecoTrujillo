USE SIGECO
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[up_planilla_bono_jn_liquidacion_v2]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].up_planilla_bono_jn_liquidacion_v2
GO

CREATE PROCEDURE [dbo].up_planilla_bono_jn_liquidacion_v2
(
	 @p_codigo_planilla	INT
)
AS
BEGIN
	SET NOCOUNT ON

	IF EXISTS(SELECT TOP 1 codigo_planilla FROM dbo.sigeco_reporte_bono_jn_liquidacion WHERE codigo_planilla = @p_codigo_planilla)
	BEGIN
		SELECT
			codigo_planilla
			,codigo_empresa
			,nombre_empresa
			,nombre_empresa_largo
			,direccion_fiscal
			,ruc_empresa
			,nombre_grupo
			,codigo_personal
			,nombre_personal
			,documento_personal
			,monto_bono
			,monto_sin_igv
			,monto_igv
			,monto_bono_letras
			,concepto_liquidacion
			,codigo_estado_planilla
			,dinero_ingresado
		FROM
			dbo.sigeco_reporte_bono_jn_liquidacion
		WHERE
			codigo_planilla = @p_codigo_planilla
		ORDER BY
			codigo_empresa ASC
	END
	ELSE
	BEGIN
		EXEC up_planilla_bono_jn_liquidacion @p_codigo_planilla
	END


	SET NOCOUNT OFF
END;

