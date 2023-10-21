USE SIGECO
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[up_personal_listado_no_validados]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].up_personal_listado_no_validados
GO

CREATE PROCEDURE dbo.up_personal_listado_no_validados
AS
BEGIN
	SET NOCOUNT ON

	DECLARE
		@c_estado_planilla_cerrada INT = 2
		,@c_estado_activo			BIT = 1
		,@c_no_validado				BIT = 0
		,@c_cuenta_interbancaria	INT = 5
		,@c_persona_juridica		BIT = 1

	SELECT
		p.codigo_personal
		--,p.nombre + isnull(' ' + p.apellido_paterno, '') + isnull(' ' + p.apellido_materno, '') as nombres
		,p.codigo_equivalencia
		,vp.nombre_personal
		,td.nombre_tipo_documento + ' ' + case when p.es_persona_juridica = @c_persona_juridica then p.nro_ruc else p.nro_documento end as documento
		,b.nombre as banco
		,tc.nombre as tipo_cuenta
		,case when p.codigo_tipo_cuenta <> @c_cuenta_interbancaria then p.nro_cuenta else p.codigo_interbancario end as nro_cuenta
		,dbo.fn_formatear_fecha_hora(p.fecha_modifica) as fecha_modifica
		,ISNULL(p.usuario_modifica, '') as usuario_modifica
		,CASE WHEN 
			NOT EXISTS(select dp.codigo_planilla from detalle_planilla dp INNER JOIN dbo.planilla pl on pl.codigo_planilla = dp.codigo_planilla and pl.codigo_estado_planilla = @c_estado_planilla_cerrada and dp.codigo_personal = p.codigo_personal)
			AND
			NOT EXISTS(select dp.codigo_planilla from detalle_planilla_bono dp INNER JOIN dbo.planilla_bono pl on pl.codigo_planilla = dp.codigo_planilla and pl.codigo_estado_planilla = @c_estado_planilla_cerrada and dp.codigo_personal = p.codigo_personal)
			AND
			NOT EXISTS(select dp.codigo_planilla from planilla_bono_trimestral_detalle dp INNER JOIN dbo.planilla_bono_trimestral pl on pl.codigo_planilla = dp.codigo_planilla and pl.codigo_estado_planilla = @c_estado_planilla_cerrada and dp.codigo_personal = p.codigo_personal)
		THEN
			'NUEVO'
		ELSE
			'MODIFICADO'
		END as tipo_validacion
		,vp.nombre_canal + '\' + vp.nombre_grupo as canal_grupo
	FROM
		dbo.personal p
	INNER JOIN dbo.tipo_documento td
		ON td.codigo_tipo_documento = p.codigo_tipo_documento
	INNER JOIN dbo.banco b
		ON b.codigo_banco = p.codigo_banco
	INNER JOIN dbo.tipo_cuenta tc
		ON tc.codigo_tipo_cuenta = p.codigo_tipo_cuenta
	INNER JOIN dbo.vw_personal vp
		on vp.codigo_personal = p.codigo_personal and vp.validado = @c_no_validado
	WHERE
		p.estado_registro = @c_estado_activo
		AND p.validado = @c_no_validado
	ORDER BY
		P.fecha_modifica DESC, vp.nombre_personal

	SET NOCOUNT Off
END