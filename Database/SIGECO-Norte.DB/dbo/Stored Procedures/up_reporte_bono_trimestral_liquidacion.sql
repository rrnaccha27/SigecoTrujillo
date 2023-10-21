USE SIGECO
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[up_reporte_bono_trimestral_liquidacion]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].up_reporte_bono_trimestral_liquidacion
GO

CREATE PROCEDURE [dbo].up_reporte_bono_trimestral_liquidacion
(
	 @p_codigo_planilla	INT
)
AS
BEGIN
	SET NOCOUNT ON

	DECLARE
		@v_codigo_estado_planilla	INT

	SET @v_codigo_estado_planilla = (SELECT TOP 1 codigo_estado_planilla FROM dbo.planilla_bono_trimestral WHERE codigo_planilla = @p_codigo_planilla)

	SELECT
		codigo_empresa
		,nombre_empresa
		,nombre_empresa_largo
		,direccion_fiscal_empresa
		,ruc_empresa
		,codigo_canal_grupo
		,codigo_canal
		,nombre_canal
		,codigo_grupo
		,nombre_grupo
		,codigo_personal
		,nombre_personal
		,documento_personal
		,codigo_personal_j
		,codigo_supervisor
		,correo_supervisor
		,nombre_supervisor
		,monto_contratado
		,rango
		,monto_bono
		,monto_sin_igv
		,monto_igv
		,monto_bono_letras
		,concepto_liquidacion
		,@v_codigo_estado_planilla as codigo_estado_planilla
	FROM
		dbo.planilla_bono_trimestral_detalle
	WHERE
		monto_bono IS NOT NULL
		AND codigo_planilla = @p_codigo_planilla
	ORDER BY
		nombre_empresa, nombre_canal, nombre_grupo, rango ASC

	SET NOCOUNT OFF
END;

