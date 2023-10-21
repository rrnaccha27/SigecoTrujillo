CREATE PROCEDURE [dbo].[up_gestion_exclusion_habilitar_pago]
(
	@codigo_exclusion int,
	@codigo_planilla int,
	@usuario_registra varchar(30),
	@observacion varchar(200)
)
as
begin
	SET NOCOUNT ON
	
	declare 
		@codigo_detalle_cronograma int,
		@codigo_detalle_planilla int,
		---------------------------------
		@v_existe_registro int,
		@v_codigo_detalle_planilla_nuevo int,
		@v_codigo_estado_cuota int,
		@v_realiza_operacion int,
		@v_numero_planilla varchar(20),
		@v_mensaje varchar(150),
		@v_nombre_estado_planilla varchar(100),
		@v_codigo_estado_planilla int,
		@v_codigo_tipo_planilla int;

	/************************************/
	set @v_realiza_operacion=0;
	set @v_codigo_detalle_planilla_nuevo=0;
	/***************************************************************************************************************/
	select 
		@v_codigo_estado_planilla=p.codigo_estado_planilla,
		@v_numero_planilla=p.numero_planilla,
		@v_nombre_estado_planilla=ep.nombre,
		@v_codigo_tipo_planilla = codigo_tipo_planilla
	from 
		planilla p 
	inner join estado_planilla ep 
		on p.codigo_estado_planilla=ep.codigo_estado_planilla 
	where 
		p.codigo_planilla=@codigo_planilla;
	/*****************************************************************************************************************/
	select 
		@codigo_detalle_cronograma =codigo_detalle_cronograma,
		@codigo_detalle_planilla=codigo_detalle_planilla
	from
		exclusion_cuota_planilla 
	where
		codigo_exclusion=@codigo_exclusion;
	/***************************************************************************************************************/
	select
		@v_codigo_estado_cuota=codigo_estado_cuota 
	from 
		detalle_cronograma 
	where
		codigo_detalle=@codigo_detalle_cronograma;
	/*****************************************************************************************************************/
	if @v_codigo_estado_cuota<>4
	begin
		set @v_mensaje='El pago solo se puede habilitar en estado "Excluido", el estado actual del registro es ';
		select @v_mensaje=@v_mensaje+nombre from estado_cuota where codigo_estado_cuota=@v_codigo_estado_cuota;
		---------------------------------------------------------------------------------------------
		RAISERROR(@v_mensaje,16,1);
		return;
	end;
	/**************************************************************************
	***************************************************************************/
	if @codigo_planilla>0 and @v_codigo_estado_planilla<>1
	begin
		set @v_mensaje='Para agregar un pago a la Planilla '+@v_numero_planilla+' debe estar "Abierta", el estadoa actual de la planilla es '+@v_nombre_estado_planilla;
		RAISERROR(@v_mensaje,16,1);
		return;
	end;
	/************************************************************/
	if  @v_codigo_estado_cuota=4 and @codigo_planilla=0
	begin
		update
			detalle_cronograma 
		set
			codigo_estado_cuota=1 /*INDICA QUE EL PAGO SE ENCUENTRA PENDIENTE*/
		where
			codigo_detalle=@codigo_detalle_cronograma;
		/***********************************************/
		set @v_realiza_operacion=1;
	end;
	/*********************************************************************/
 
	if @codigo_planilla>0 and @v_codigo_estado_planilla=1 and @v_codigo_estado_cuota=4
	begin
		insert into detalle_planilla(
			codigo_planilla,
			codigo_cronograma,
			codigo_moneda,
			codigo_canal,
			codigo_grupo,
			codigo_empresa,
			codigo_personal,
			codigo_articulo,
			nro_contrato,
			codigo_tipo_venta,
			codigo_tipo_pago,
			codigo_detalle_cronograma,
			excluido,
			nro_cuota,
			fecha_pago,
			monto_bruto,
			igv,
			monto_neto,
			estado_registro,
			fecha_registra,
			usuario_registra,
			codigo_personal_referencial,
			es_transferencia
		)
		select 
			@codigo_planilla,
			dc.codigo_cronograma,
			cpc.codigo_moneda,
			pcg.codigo_canal,
			pcg.codigo_canal_grupo,
			cpc.codigo_empresa,
			pcg.codigo_personal,
			dc.codigo_articulo,
			cpc.nro_contrato,
			cpc.codigo_tipo_venta,
			cpc.codigo_tipo_pago,
			dc.codigo_detalle,
			0 as excluido,
			dc.nro_cuota,
			ISNULL(dc.fecha_programada, GETDATE()),
			dc.monto_bruto,
			dc.igv,
			dc.monto_neto,
			1 as estado_registro,
			GETDATE() as fecha_registra,
			@usuario_registra,
			dbo.fn_obtener_personal_supervisor(cpc.nro_contrato, ee.codigo_equivalencia, @v_codigo_tipo_planilla),
			dc.es_transferencia
		from cronograma_pago_comision cpc
		inner join empresa_sigeco ee 
			on ee.codigo_empresa=cpc.codigo_empresa
		inner join detalle_cronograma dc 
			on cpc.codigo_cronograma=dc.codigo_cronograma 
		inner join personal_canal_grupo pcg 
			on (pcg.codigo_registro=cpc.codigo_personal_canal_grupo)
		where 
			dc.codigo_detalle=@codigo_detalle_cronograma

		set @v_codigo_detalle_planilla_nuevo=@@IDENTITY;
		/*******************************************************************************************/
		update 
			detalle_cronograma 
		set 
			codigo_estado_cuota=2 /*INDICA QUE EL PAGO SE ENCUENTRA EN PROCESO PAGO*/
		where 
			codigo_detalle=@codigo_detalle_cronograma;
		/***********************************************/
		set @v_realiza_operacion=2;
	end;

	IF (@v_realiza_operacion > 0)
		EXEC dbo.up_operacion_cuota_comision_insertar @codigo_detalle_cronograma, 1, @observacion, @usuario_registra
 
	/**********************************************
	ACTUALIZANDO EXCLUSION
	***********************************************/	 
	update 
		exclusion_cuota_planilla
	set
		usuario_habilita=@usuario_registra,
		fecha_habilita=GETDATE(),
		motivo_habilita=isnull(@observacion,'Se habilita exclusion'),
		estado_exclusion=0,
		codigo_planilla_destino=(case when @v_codigo_detalle_planilla_nuevo=0 then null else @codigo_planilla end)
	where 
		codigo_exclusion=@codigo_exclusion	
		and estado_exclusion=1 and @v_realiza_operacion>0 ;
	
	SET NOCOUNT OFF		
end;