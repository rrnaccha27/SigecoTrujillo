CREATE PROCEDURE [dbo].[up_reclamo_actualizar]
	@codigo_reclamo                                 int
	,@codigo_personal                               int=NULL
	,@NroContrato                                   varchar(20)
	,@codigo_articulo                               int=NULL
	,@codigo_empresa                                int=NULL
	,@Cuota                                         int=NULL
	,@Importe                                       decimal(18,2)

	,@atencion_codigo_articulo                      int=NULL
	,@atencion_codigo_empresa                       int=NULL
	,@atencion_Cuota                                int=NULL
	,@atencion_Importe                              decimal(18,2)

	,@codigo_estado_reclamo                         int=NULL
	,@codigo_estado_resultado                       int=NULL
	,@Observacion                                   varchar(1000)
	,@Respuesta                                     varchar(1000)
	,@usuario_modifica                              varchar(50)
	,@fecha_modifica                                datetime=NULL
	,@TipoAfectaPlanilla							VARCHAR(20)=NULL
AS
BEGIN

	UPDATE 
		reclamo
    SET
		codigo_personal = @codigo_personal
		,NroContrato = @NroContrato
		,codigo_articulo = @codigo_articulo
		,codigo_empresa = @codigo_empresa
		,Cuota = @Cuota
		,Importe = @Importe

		,atencion_codigo_articulo = @atencion_codigo_articulo
		,atencion_codigo_empresa = @atencion_codigo_empresa
		,atencion_Cuota = @atencion_Cuota
		,atencion_Importe = @atencion_Importe

		,codigo_estado_reclamo = 2	--2:Atendido--@codigo_estado_reclamo
		,codigo_estado_resultado = @codigo_estado_resultado
		,Observacion = @Observacion
		,Respuesta = @Respuesta
		--,codigo_planilla = @codigo_planilla
		,usuario_modifica = @usuario_modifica
		,fecha_modifica = @fecha_modifica
		--,codigo_detalle_cronograma=@codigo_detalle_cronograma
		,codigo_estado_resultado_n2 = @codigo_estado_resultado
		,usuario_modifica_n2 = @usuario_modifica
		,fecha_modifica_n2 = @fecha_modifica
		,observacion_n2 = @Respuesta
	WHERE
		codigo_reclamo = @codigo_reclamo
END;