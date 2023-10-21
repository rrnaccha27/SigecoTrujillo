CREATE PROCEDURE [dbo].[up_reclamo_LISTAR_All]
@NroContrato                                        varchar(20)
,@codigo_estado_reclamo                              int
AS
BEGIN
       SELECT 
          codigo_reclamo
          ,codigo_personal
          ,NroContrato
          ,codigo_articulo
          ,codigo_empresa
          ,Cuota
          ,Importe
          ,codigo_estado_reclamo
          ,codigo_estado_resultado
          ,Observacion
          ,Respuesta
          ,usuario_registra
          ,fecha_registra
          ,usuario_modifica
          ,fecha_modifica
       FROM reclamo WITH (NOLOCK)
       WHERE
          ISNULL(NroContrato,'') LIKE '%' + @NroContrato + '%'
          and (@codigo_estado_reclamo = 0 OR codigo_estado_reclamo = @codigo_estado_reclamo)
END