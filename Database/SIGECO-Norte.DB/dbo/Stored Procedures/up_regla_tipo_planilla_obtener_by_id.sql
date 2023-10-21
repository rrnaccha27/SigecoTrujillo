IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[up_regla_tipo_planilla_obtener_by_id]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[up_regla_tipo_planilla_obtener_by_id]

GO
CREATE PROCEDURE [dbo].[up_regla_tipo_planilla_obtener_by_id]
(
	@p_codigo_regla_tipo_planilla int
)
AS
BEGIN
	SET NOCOUNT ON
	
	SELECT TOP 1
		rtp.codigo_regla_tipo_planilla,	
		rtp.codigo_tipo_planilla,
		rtp.nombre ,	
		rtp.descripcion,
		rtp.estado_registro,
		rtp.fecha_registra,	
		dbo.fn_obtener_nombre_usuario(rtp.usuario_registra) AS usuario_registra,
		rtp.afecto_doc_completa,
		ISNULL(tr.codigo_tipo_reporte, 1) as codigo_tipo_reporte,
		rtp.detraccion_por_contrato,
		rtp.envio_liquidacion
	FROM 
		dbo.regla_tipo_planilla rtp
	LEFT JOIN 
		dbo.tipo_reporte tr ON tr.nomenclatura = rtp.tipo_reporte
	WHERE 
		rtp.codigo_regla_tipo_planilla = @p_codigo_regla_tipo_planilla;

	SET NOCOUNT OFF
END;