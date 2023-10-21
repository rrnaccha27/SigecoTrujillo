USE SIGECO
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[up_personal_bloqueo_listado_por_codigo]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].up_personal_bloqueo_listado_por_codigo
GO

CREATE PROCEDURE dbo.up_personal_bloqueo_listado_por_codigo
(
	@p_codigo_personal	INT
)
AS
BEGIN
	SET NOCOUNT ON

	DECLARE
		@c_codigo_tipo_bloqueo_comision		INT = 1
		,@c_codigo_tipo_bloqueo_bono		INT = 2
		,@c_codigo_tipo_bloqueo_bono_tri	INT = 3
		,@c_estado_activo					BIT = 1
	
	/* COMISION */
	SELECT 
		@p_codigo_personal as codigo_personal
		,tbl.descripcion
		,pl.numero_planilla
		,pl.nombre_planilla as descripcion_planilla
		,pl.estado_planilla
		,dbo.fn_formatear_fecha_hora(b.fecha_registra) as fecha_registra
		,dbo.fn_formatear_fecha_hora(b.fecha_modifica) as fecha_modifica
		,case when b.estado_registro = @c_estado_activo THEN 'ACTIVO' ELSE 'INACTIVO' END as estado_registro_texto
	FROM personal_bloqueo b
	INNER JOIN dbo.tipo_bloqueo_personal tbl
		ON tbl.codigo_tipo_bloqueo_personal = b.codigo_tipo_bloqueo
	INNER JOIN dbo.vw_planilla pl
		ON pl.codigo_planilla = b.codigo_planilla
	WHERE
		b.codigo_tipo_bloqueo = @c_codigo_tipo_bloqueo_comision
		AND codigo_personal = @p_codigo_personal
	
	/* BONO */
	UNION

	SELECT 
		@p_codigo_personal as codigo_personal
		,tbl.descripcion
		,pl.numero_planilla
		,pl.tipo_planilla as descripcion_planilla
		,pl.estado_planilla
		,dbo.fn_formatear_fecha_hora(b.fecha_registra) as fecha_registra
		,dbo.fn_formatear_fecha_hora(b.fecha_modifica) as fecha_modifica
		,case when b.estado_registro = @c_estado_activo THEN 'ACTIVO' ELSE 'INACTIVO' END as estado_registro_texto
	FROM personal_bloqueo b
	INNER JOIN dbo.tipo_bloqueo_personal tbl
		ON tbl.codigo_tipo_bloqueo_personal = b.codigo_tipo_bloqueo
	INNER JOIN dbo.vw_planilla_bono pl
		ON pl.codigo_planilla = b.codigo_planilla
	WHERE
		b.codigo_tipo_bloqueo = @c_codigo_tipo_bloqueo_bono
		AND codigo_personal = @p_codigo_personal
	

	/* BONO TRIMESTRAL */
	UNION

	SELECT 
		@p_codigo_personal as codigo_personal
		,tbl.descripcion
		,pl.numero_planilla
		,pl.tipo_planilla + ' ' +pl.periodo as descripcion_planilla
		,pl.estado_planilla
		,dbo.fn_formatear_fecha_hora(b.fecha_registra) as fecha_registra
		,dbo.fn_formatear_fecha_hora(b.fecha_modifica) as fecha_modifica
		,case when b.estado_registro = @c_estado_activo THEN 'ACTIVO' ELSE 'INACTIVO' END as estado_registro_texto
	FROM personal_bloqueo b
	INNER JOIN dbo.tipo_bloqueo_personal tbl
		ON tbl.codigo_tipo_bloqueo_personal = b.codigo_tipo_bloqueo
	INNER JOIN dbo.vw_planilla_bono_trimestral pl
		ON pl.codigo_planilla = b.codigo_planilla
	WHERE
		b.codigo_tipo_bloqueo = @c_codigo_tipo_bloqueo_bono_tri
		AND codigo_personal = @p_codigo_personal

	/* ORDER */
	ORDER BY
		fecha_registra DESC

	SET NOCOUNT OFF
END