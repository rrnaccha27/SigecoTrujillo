
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[up_reporte_liquidacion_bono_individual_v2_contabilidad]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[up_reporte_liquidacion_bono_individual_v2_contabilidad]

GO
CREATE PROCEDURE [dbo].[up_reporte_liquidacion_bono_individual_v2_contabilidad]
(
	 @p_codigo_planilla	INT
)
AS
BEGIN
	SET NOCOUNT ON

	IF EXISTS(SELECT codigo_planilla FROM sigeco_reporte_bono_liquidacion WHERE codigo_planilla = @p_codigo_planilla)
	BEGIN
		SELECT 
			pb.numero_planilla,bl.codigo_empresa,bl.nombre_empresa, bl.nombre_empresa_largo,bl.ruc,bl.direccion_fiscal,bl.codigo_supervisor as codigo_personal,bl.nombre_tipo_documento,
			bl.apellidos_nombres_supervisor as nombre_personal,p.nro_ruc as nro_documento,bl.email_supervisor as email_personal,nombre_envio_correo='',apellido_envio_correo='',
			bl.nombre_grupo as canal_grupo_nombre,bl.monto_neto_pagar_empresa_supervisor as neto_pagar_empresa,tipo_reporte='_e', bl.codigo_canal
		FROM 
			sigeco_reporte_bono_liquidacion bl
			inner join planilla_bono pb on pb.codigo_planilla=bl.codigo_planilla
			inner join personal p on p.codigo_personal=bl.codigo_supervisor
		WHERE 
			bl.codigo_planilla = @p_codigo_planilla
		ORDER BY
			bl.codigo_empresa,bl.nombre_grupo,bl.apellidos_nombres_supervisor

		--SELECT 'CONGELADO'
	END
	--ELSE
	--BEGIN
	--	exec up_reporte_liquidacion_bono_individual @p_codigo_planilla
	--	select 'en vivo'
	--END	
	SET NOCOUNT OFF
END