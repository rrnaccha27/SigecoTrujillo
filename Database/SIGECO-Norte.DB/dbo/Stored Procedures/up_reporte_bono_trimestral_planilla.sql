USE SIGECO
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[up_reporte_bono_trimestral_planilla]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].up_reporte_bono_trimestral_planilla
GO

CREATE PROCEDURE dbo.up_reporte_bono_trimestral_planilla
(
	@p_codigo_planilla	INT
)
AS
BEGIN
	SET NOCOUNT On
	DECLARE
		@v_codigo_estado_planilla	INT,
		@v_numero_planilla			VARCHAR(25)

	SELECT TOP 1 
		@v_codigo_estado_planilla = codigo_estado_planilla,
		@v_numero_planilla = numero_planilla
	FROM 
		dbo.planilla_bono_trimestral 
	WHERE 
		codigo_planilla = @p_codigo_planilla

	SELECT 
		con.codigo_empresa
		,ee.nombre as nombre_empresa
		,det.codigo_canal
		,det.nombre_canal
		,det.codigo_grupo
		,det.nombre_grupo
		,det.codigo_personal
		,det.nombre_personal
		,con.numero_contrato
		,con.monto_contratado as mc_contrato
		,det.monto_contratado as mc_personal
		,det.rango
		,det.monto_sin_igv
		,det.monto_igv
		,det.monto_bono
		,det.concepto_liquidacion
		,@v_codigo_estado_planilla codigo_estado_planilla
		,det.monto_bono_grupo
		,det.monto_bono_grupo_sin_igv
		,det.monto_bono_grupo_igv
		,det.monto_bono_canal
		,det.monto_bono_canal_sin_igv
		,det.monto_bono_canal_igv
		,det.monto_bono_empresa
		,det.monto_bono_empresa_sin_igv
		,det.monto_bono_empresa_igv
		,@v_numero_planilla as numero_planilla
	FROM 
		dbo.planilla_bono_trimestral_contratos con
	INNER JOIN dbo.planilla_bono_trimestral_detalle det 
		on det.codigo_personal = con.codigo_personal and /*det.codigo_empresa = con.codigo_empresa and*/ det.codigo_planilla = con.codigo_planilla
	INNER JOIN dbo.empresa_sigeco ee
		on ee.codigo_empresa = con.codigo_empresa
	WHERE
		det.monto_bono IS NOT NULL
		AND det.codigo_planilla = @p_codigo_planilla
	ORDER BY
		det.codigo_empresa, det.codigo_canal, det.nombre_grupo, det.rango, det.nombre_personal, con.codigo_empresa, con.numero_contrato

	SET NOCOUNT OFF
END