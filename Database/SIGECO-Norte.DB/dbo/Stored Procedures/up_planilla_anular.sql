USE SIGECO
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[up_planilla_anular]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].up_planilla_anular
GO

CREATE PROCEDURE [dbo].[up_planilla_anular]
(
	@codigo_planilla int,
	@usuario_registra varchar(30)
)
AS
BEGIN

	declare 
		--@fecha_cierre date,
		@planilla_habilitado int = 0,
		--@codigo_estado_cuota_pendiente_pago int,
		--@codigo_estado_cuota_en_proceso_pago int,
		@codigo_estado_planilla int,
		@v_numero_planilla VARCHAR(100),
		@c_codigo_tipo_bloqueo_comision INT;


	--set @fecha_cierre=GETDATE();
	set @codigo_estado_planilla=3;--indica anulado la planilla
	set @c_codigo_tipo_bloqueo_comision = 1; /* COMISION */
	--set @codigo_estado_cuota_pendiente_pago=1;--INDICA QUE LA CUOTA SE ENCUENTRA PENDIENTE

	select 
		@planilla_habilitado = 1,
		@v_numero_planilla = numero_planilla
	from 
		planilla 
	where 
		codigo_planilla=@codigo_planilla and codigo_estado_planilla=1;

	IF (ISNULL(@planilla_habilitado, 0) = 0)
	BEGIN
		RAISERROR('La planilla no se puede anular, se encuentra cerrada o anulada.',16,1); 
		RETURN;
	END;

	update planilla
	set 
		codigo_estado_planilla=@codigo_estado_planilla,
		fecha_anulacion=GETDATE(),
		usuario_anulacion=@usuario_registra,
		usuario_modifica=@usuario_registra,
		fecha_modifica=GETDATE()
	where 
		codigo_planilla=@codigo_planilla;

	/*------------------------------------
	ACTUALIZANDO TABLA OPERACIONES CUOTA
	------------------------------------*/
	update occ
	set occ.estado_registro =0
	from detalle_planilla as dp
	inner join detalle_cronograma dc 
		on dp.codigo_detalle_cronograma=dc.codigo_detalle
	inner join operacion_cuota_comision as occ 
		on dp.codigo_detalle_cronograma = occ.codigo_detalle_cronograma 
	where 
		dp.codigo_planilla= @codigo_planilla 
		and dc.codigo_estado_cuota=2 
		and occ.estado_registro=1;

	/*-----------------------------------
	INSERTAR EN TABLA OPERACIONES CUOTA
	-----------------------------------*/
	insert into operacion_cuota_comision(
		codigo_detalle_cronograma,
		codigo_tipo_operacion_cuota,
		motivo_movimiento,
		fecha_movimiento,
		estado_registro,
		usuario_registra,
		fecha_registra
	)
	select 
		dp.codigo_detalle_cronograma,
		7,--CUOTA LIBERADO
		'POR ANULACION DE PLANILLA ' + @v_numero_planilla,
		GETDATE(),
		1,
		@usuario_registra,
		GETDATE()
	from detalle_planilla as dp
	inner join detalle_cronograma dc on dp.codigo_detalle_cronograma=dc.codigo_detalle
	where dp.codigo_planilla= @codigo_planilla and dc.codigo_estado_cuota=2;

	/*-----------------------------------------
	ACTUALIZNADO CON ESTADO PENDIENTE DE PAGO
	-----------------------------------------*/
	update dc
	set dc.codigo_estado_cuota = 1--PENDIENTE DE PAGO
	from detalle_planilla as dp
	inner join detalle_cronograma as dc 
		   on dp.codigo_detalle_cronograma = dc.codigo_detalle 
	where dp.codigo_planilla= @codigo_planilla AND dc.codigo_estado_cuota=2;

	/*--------------------------------------------------------------------------
	-- SECCIÓN REGISTRO MANUAL DE COMISIONES-LIBERANDO COMISION DE LA PLANILLA
	--------------------------------------------------------------------------*/
	merge into comision_manual cm
	using(
		select 
			dc.codigo_detalle 
		from 
			detalle_planilla dp 
		inner join 
			detalle_cronograma dc on dp.codigo_detalle_cronograma=dc.codigo_detalle
		where 
			dp.codigo_planilla=@codigo_planilla 
			and dc.es_registro_manual_comision=1 
			and dc.estado_registro=1 
		) sc on (cm.codigo_detalle_cronograma=sc.codigo_detalle and cm.en_planilla=1 )
	when matched then update 
	set 
		cm.codigo_estado_proceso=1,
		--cm.codigo_detalle_cronograma=null,
		cm.codigo_planilla=null,
		cm.en_planilla=0;

	/*---------------------
	-- ANULADO DESCUENTOS
	---------------------*/
	--update
	--	descuento
	--set
	--	estado_registro = 0
	--	,usuario_modifica = @usuario_registra
	--	,fecha_modifica = GETDATE()
	--where
	--	codigo_planilla = @codigo_planilla
	--	and estado_registro = 1

	/*
	DESBLOQUEO DE VENDEDORES
	*/
	EXEC dbo.up_personal_bloqueo_anular @codigo_planilla, @c_codigo_tipo_bloqueo_comision, @usuario_registra

END;