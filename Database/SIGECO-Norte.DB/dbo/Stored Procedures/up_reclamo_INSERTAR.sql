IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[up_reclamo_INSERTAR]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[up_reclamo_INSERTAR]
GO
CREATE PROCEDURE [dbo].[up_reclamo_INSERTAR]
	@codigo_personal                                    int=NULL
	,@NroContrato                                       varchar(20)
	,@codigo_articulo                                   int=NULL
	,@codigo_empresa                                    int=NULL
	,@Cuota                                             int=NULL
	,@Importe                                           decimal(18,2)
	,@codigo_estado_reclamo                             int=NULL
	,@codigo_estado_resultado                           int=NULL
	,@Observacion                                       varchar(1000)
	,@Respuesta                                         varchar(1000)
	,@usuario_registra                                  varchar(50)
	,@fecha_registra                                    datetime=NULL
	,@es_contrato_migrado								bit
AS
BEGIN TRANSACTION Insertarreclamo
BEGIN
	DECLARE @TIPO VARCHAR(10)
	DECLARE @MSJ VARCHAR(50)

	SET @TIPO='SUCCESS'		--TIPO:ERROR,ALERT,SUCCESS
	SET @MSJ=''

	IF @TIPO='SUCCESS'
	BEGIN
       INSERT INTO reclamo(
          codigo_personal
          ,NroContrato
          ,codigo_articulo
          ,codigo_empresa
          ,Cuota
          ,Importe

		  ,atencion_codigo_articulo
          ,atencion_codigo_empresa
          ,atencion_Cuota
          ,atencion_Importe

          ,codigo_estado_reclamo
          ,codigo_estado_resultado
          ,Observacion
          ,Respuesta
          ,usuario_registra
          ,fecha_registra
		  ,es_contrato_migrado
		)
		VALUES(
          @codigo_personal
          ,@NroContrato
          ,@codigo_articulo
          ,@codigo_empresa
          ,@Cuota
          ,@Importe

		  ,@codigo_articulo
          ,@codigo_empresa
          ,@Cuota
          ,@Importe

          ,@codigo_estado_reclamo
          ,@codigo_estado_resultado
          ,@Observacion
          ,@Respuesta
          ,@usuario_registra
          ,@fecha_registra
		  ,@es_contrato_migrado
		)
		SET @MSJ='Registro exitoso'
	END
	
	-- APROBACION AUTOMATICA DE RECLAMO N1
	DECLARE @codigo_reclamo INT

	SET @codigo_reclamo = SCOPE_IDENTITY()

	UPDATE dbo.reclamo
	SET
		codigo_estado_resultado_n1 = 1
		,usuario_modifica_n1 = 'root'
		,fecha_modifica_n1 = GETDATE()
		,observacion_n1 = 'APROBACION AUTOMATICA'
	WHERE	
		codigo_reclamo = @codigo_reclamo
	--------------------------------------

	SELECT @TIPO + '|' + @MSJ AS Respuesta

END
IF @@ERROR <> 0 ROLLBACK TRANSACTION
ELSE COMMIT TRANSACTION