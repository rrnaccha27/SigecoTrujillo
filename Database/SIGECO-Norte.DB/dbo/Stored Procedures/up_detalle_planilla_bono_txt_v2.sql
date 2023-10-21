CREATE PROCEDURE [dbo].[up_detalle_planilla_bono_txt_v2]
(
	 @p_codigo_planilla	INT
)
AS
BEGIN
	SET NOCOUNT ON

	IF EXISTS(SELECT codigo_planilla FROM sigeco_reporte_bono_rrhh WHERE codigo_planilla = @p_codigo_planilla)
	BEGIN
		SELECT 
			codigo_planilla,numero_planilla,fecha_proceso,codigo_empresa,simbolo_moneda_cuenta_desembolso,nombre_empresa,numero_cuenta_desembolso
			,tipo_cuenta_desembolso,numero_cuenta_abono,tipo_cuenta_abono,simbolo_moneda_cuenta_abono,nombre_tipo_documento,nro_documento
			,nombre_personal,codigo_personal,importe_abono_personal,importe_desembolso_empresa,calcular_detraccion,[checksum], codigo_grupo
		FROM 
			sigeco_reporte_bono_rrhh
		WHERE 
			codigo_planilla = @p_codigo_planilla
		ORDER BY 
			nombre_personal

		SELECT 'CONGELADO'
	END
	ELSE
	BEGIN
		exec up_detalle_planilla_bono_txt @p_codigo_planilla
		select 'en vivo'
	END	
	SET NOCOUNT OFF
END;