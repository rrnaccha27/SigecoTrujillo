USE SIGECO
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[up_reporte_liquidacion_detalle_v2]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].up_reporte_liquidacion_detalle_v2
GO

CREATE PROCEDURE dbo.up_reporte_liquidacion_detalle_v2
(
	@p_codigo_planilla	int
)
AS
BEGIN

	SET NOCOUNT ON

	IF EXISTS(SELECT codigo_planilla FROM sigeco_reporte_comision_liquidacion WHERE codigo_planilla = @p_codigo_planilla)
	BEGIN
		SELECT
			codigo_planilla,fecha_inicio,numero_planilla,fecha_fin,codigo_tipo_planilla,codigo_estado_planilla,codigo_empresa,nombre_empresa,nombre_empresa_largo
			,ruc,direccion_fiscal,codigo_personal,nombre_tipo_documento,nombre_personal,nro_documento,email_personal,codigo_personal_referencial,nombre_personal_referencial
			,email_personal_referencial,nombre_envio_correo,apellido_envio_correo,nro_contrato,nro_cuota,nombre_articulo,nombre_tipo_venta,nombre_tipo_pago,igv,monto_bruto,monto_neto
			,igv_empresa,bruto_empresa,neto_empresa,descuento_empresa,canal_grupo_nombre,descuento_motivo,codigo_jardines,codigo_moneda,detraccion_empresa,neto_pagar_empresa,neto_pagar_empresa_letra,tipo_reporte, codigo_canal
		FROM sigeco_reporte_comision_liquidacion 
		WHERE 
			codigo_planilla = @p_codigo_planilla
		ORDER BY 
			codigo_empresa asc, canal_grupo_nombre asc, nombre_personal asc, nro_contrato, nombre_articulo, nro_cuota;
		select 'congelado'
	END
	ELSE
	BEGIN
		EXEC up_reporte_liquidacion_detalle @p_codigo_planilla
		select 'en vivo'
	END

	SET NOCOUNT OFF
END;