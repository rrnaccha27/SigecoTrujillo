IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[up_reporte_liquidacion_detalle_v2_Contabilidad]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[up_reporte_liquidacion_detalle_v2_Contabilidad]

GO
CREATE PROCEDURE [dbo].[up_reporte_liquidacion_detalle_v2_Contabilidad]
(
	@p_codigo_planilla	int
)
AS
BEGIN

	SET NOCOUNT ON

	IF EXISTS(SELECT codigo_planilla FROM sigeco_reporte_comision_liquidacion WHERE codigo_planilla = @p_codigo_planilla)
	BEGIN
		SELECT
			numero_planilla,codigo_empresa,nombre_empresa,nombre_empresa_largo
			,ruc,direccion_fiscal,codigo_personal,nombre_tipo_documento,nombre_personal,nro_documento,email_personal
			,nombre_envio_correo,apellido_envio_correo
			,canal_grupo_nombre,neto_pagar_empresa,tipo_reporte='_e', codigo_canal
			/*
			pb.numero_planilla,bl.codigo_empresa,bl.nombre_empresa, bl.nombre_empresa_largo,bl.ruc,bl.direccion_fiscal,bl.codigo_supervisor as codigo_personal,bl.nombre_tipo_documento,
			bl.apellidos_nombres_supervisor as nombre_personal,p.nro_ruc as nro_documento,bl.email_supervisor as email_personal,nombre_envio_correo='',apellido_envio_correo='',
			bl.nombre_grupo as canal_grupo_nombre,bl.monto_neto_pagar_empresa_supervisor as neto_pagar_empresa,tipo_reporte='_e', bl.codigo_canal
			*/
		FROM sigeco_reporte_comision_liquidacion 
		WHERE 
			codigo_planilla = @p_codigo_planilla
		GROUP BY numero_planilla,codigo_empresa,nombre_empresa,nombre_empresa_largo
			,ruc,direccion_fiscal,codigo_personal,nombre_tipo_documento,nombre_personal,nro_documento,email_personal
			,nombre_envio_correo,apellido_envio_correo
			,canal_grupo_nombre,neto_pagar_empresa,/*tipo_reporte='_e',*/ codigo_canal
		ORDER BY 
			codigo_empresa asc, canal_grupo_nombre asc, nombre_personal asc--, nro_contrato, nombre_articulo, nro_cuota;
		--select 'congelado'
	END
	--ELSE
	--BEGIN
	--	EXEC up_reporte_liquidacion_detalle @p_codigo_planilla
	--	select 'en vivo'
	--END

	SET NOCOUNT OFF
END