USE SIGECO
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[up_planilla_listar_checklist_bono_trimestral]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].up_planilla_listar_checklist_bono_trimestral
GO

CREATE PROCEDURE [dbo].up_planilla_listar_checklist_bono_trimestral
AS
BEGIN
	SET NOCOUNT ON

	DECLARE	
		@c_estado_cerrado		INT = 2
		,@c_estado_anulado		INT = 3
		,@c_planilla_vendedor	INT = 1
	
	DECLARE
		@v_fecha_limite	VARCHAR(10)

	SET @v_fecha_limite = ISNULL((SELECT valor FROM dbo.parametro_sistema WHERE codigo_parametro_sistema = 30), '20190801')

	SELECT 
		pl.codigo_planilla
		,isnull(pl.numero_planilla,' ') numero_planilla
		,isnull(convert(varchar, pl.fecha_anulacion, 103) + ' ' + convert(varchar, pl.fecha_anulacion, 108), '') as fecha_anulacion
		,isnull(pl.usuario_anulacion, '') as usuario_anulacion
		,convert(varchar, pl.fecha_apertura, 103) + ' ' + convert(varchar, pl.fecha_apertura, 108) as fecha_apertura
		,pl.usuario_apertura
		,convert(varchar, pl.fecha_cierre, 103) + ' ' + convert(varchar, pl.fecha_cierre, 108) as fecha_cierre
		,pl.usuario_cierre
		,pl.codigo_estado_planilla
		,ep.nombre as nombre_estado_planilla
		,pl.fecha_registra
		,pl.fecha_modifica
		,pl.usuario_registra
		,pl.usuario_modifica
		,convert(varchar, pl.anio_periodo) as anio_periodo
		,pt.nombre as periodo
		,pl.codigo_regla
		,rbt.descripcion as nombre_regla
	FROM 
		planilla_bono_trimestral pl 
	INNER JOIN estado_planilla ep 
		ON pl.codigo_estado_planilla = ep.codigo_estado_planilla 
	INNER JOIN periodo_trimestral pt 
		ON pt.codigo_periodo = pl.codigo_periodo
	INNER JOIN regla_bono_trimestral rbt 
		ON rbt.codigo_regla = pl.codigo_regla
	WHERE
		pl.codigo_estado_planilla = @c_estado_cerrado --and rtp.envio_liquidacion = 1
		AND NOT EXISTS (SELECT codigo_checklist FROM dbo.checklist_bono_trimestral c WHERE c.codigo_estado_checklist <> @c_estado_anulado AND c.codigo_planilla = pl.codigo_planilla)
		AND pl.fecha_cierre >= CONVERT(DATETIME, @v_fecha_limite)
	ORDER BY 
		pl.fecha_registra DESC;

	SET NOCOUNT OFF
END;

