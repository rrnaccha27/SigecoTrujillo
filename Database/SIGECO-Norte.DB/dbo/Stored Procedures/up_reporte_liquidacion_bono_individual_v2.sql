USE SIGECO
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[up_reporte_liquidacion_bono_individual_v2]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].up_reporte_liquidacion_bono_individual_v2
GO

CREATE PROCEDURE [dbo].[up_reporte_liquidacion_bono_individual_v2]
(
	 @p_codigo_planilla	INT
)
AS
BEGIN
	SET NOCOUNT ON

	IF EXISTS(SELECT codigo_planilla FROM sigeco_reporte_bono_liquidacion WHERE codigo_planilla = @p_codigo_planilla)
	BEGIN
		SELECT 
			codigo_planilla,concepto,porcentaje_detraccion,fecha_inicio,fecha_fin,nombre_canal,codigo_estado_planilla,nombre_empresa_largo
			,direccion_fiscal,ruc,nro_documento,nombre_tipo_documento,codigo_supervisor,apellidos_nombres_supervisor,codigo_empresa,nombre_empresa
			,codigo_grupo,nombre_grupo,monto_bruto_empresa_supervisor,monto_igv_empresa_supervisor,monto_neto_empresa_supervisor,monto_detraccion_empresa_supervisor
			,monto_neto_pagar_empresa_supervisor,neto_pagar_empresa_supervisor_letra, codigo_canal, nombre_supervisor, email_supervisor
		FROM 
			sigeco_reporte_bono_liquidacion
		WHERE 
			codigo_planilla = @p_codigo_planilla
		ORDER BY
			codigo_supervisor

		SELECT 'CONGELADO'
	END
	ELSE
	BEGIN
		exec up_reporte_liquidacion_bono_individual @p_codigo_planilla
		select 'en vivo'
	END	
	SET NOCOUNT OFF
END;