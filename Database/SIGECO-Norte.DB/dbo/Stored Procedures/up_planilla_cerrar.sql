USE SIGECO
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[up_planilla_cerrar]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].up_planilla_cerrar
GO

CREATE PROCEDURE [dbo].[up_planilla_cerrar]
(
	@codigo_planilla int,
	@usuario_registra varchar(30)
)
AS
BEGIN
	SET NOCOUNT ON;

	declare 
		@fecha_cierre datetime,
		@v_fecha_apertura datetime,
		@planilla_habilitado int,
		@codigo_estado_cuota int,
		@cantidad_registros_a_cerrar int,
		@codigo_estado_planilla int
		,@v_numero_planilla varchar(100)
		,@c_codigo_tipo_bloqueo_comision INT;

	set @fecha_cierre=GETDATE();
	set @codigo_estado_planilla=2;--indica cerrado la planilla
	set @codigo_estado_cuota=3;--INDICA QUE LA CUOTA SE ENCUENTRA PAGADO
	set @c_codigo_tipo_bloqueo_comision = 1; /*COMISION*/

	--select top 1
	--     @v_fecha_apertura=fecha_apertura
	--from planilla where codigo_planilla=@codigo_planilla ;

	--if(convert(date, @fecha_cierre)=convert(date, @v_fecha_apertura))
	--begin
	--	RAISERROR('No se puede cerrar la planilla en la misma fecha de apertura.',16,1); 
	--	return;
	--end;
	 
	select @planilla_habilitado= COUNT(1) 
	from planilla where codigo_planilla=@codigo_planilla and codigo_estado_planilla=@codigo_estado_planilla;

	IF (ISNULL(@planilla_habilitado, 0) > 0)
	BEGIN
		RAISERROR('La planilla se encuentra cerrada.',16,1); 
		return;
	END;

	SET @v_numero_planilla = (SELECT TOP 1 numero_planilla FROM dbo.planilla WHERE codigo_planilla = @codigo_planilla)

	/************************************
	*  validar cantidad de registros a aprobar
	*************************************/
	select 
	@cantidad_registros_a_cerrar=count(*)
	from 
		detalle_planilla  p 
	inner join detalle_cronograma dc
		on p.codigo_detalle_cronograma=dc.codigo_detalle
	where 
		p.codigo_planilla=@codigo_planilla and dc.codigo_estado_cuota=2;

	if(@cantidad_registros_a_cerrar=0)
	begin
		RAISERROR('La planilla no cuenta con pagos habilitados',16,1); 
		return;
	end;
	 
	update
		planilla
	set 
		codigo_estado_planilla=@codigo_estado_planilla,
		fecha_cierre=@fecha_cierre,
		usuario_cierre=@usuario_registra,
		usuario_modifica=@usuario_registra,
		fecha_modifica=GETDATE()
	where
		codigo_planilla=@codigo_planilla;

	/**********************************************
	ACTUALIZANDO TABLA OPERACIONES CUOTA
	***********************************************/
	update
		occ
	set 
		occ.estado_registro =0
	from 
		detalle_planilla as dp
	inner join detalle_cronograma dc 
		on dp.codigo_detalle_cronograma=dc.codigo_detalle
	inner join operacion_cuota_comision as occ 
		on dp.codigo_detalle_cronograma = occ.codigo_detalle_cronograma 
	where 
		dp.codigo_planilla= @codigo_planilla 
		and dc.codigo_estado_cuota=2 
		and occ.estado_registro=1;

	/**********************************************
	INSERTAR EN TABLA OPERACIONES CUOTA
	***********************************************/

	insert into operacion_cuota_comision(
		codigo_detalle_cronograma,
		codigo_tipo_operacion_cuota,
		motivo_movimiento,
		fecha_movimiento,
		estado_registro,
		usuario_registra,
		fecha_registra)
	select 
		dp.codigo_detalle_cronograma,
		3,
		'POR CIERRE DE PLANILLA ' + @v_numero_planilla,
		GETDATE(),
		1,
		@usuario_registra,
		GETDATE()
	from detalle_planilla as dp
	inner join detalle_cronograma dc on 
		dp.codigo_detalle_cronograma=dc.codigo_detalle
	where 
		dp.codigo_planilla= @codigo_planilla 
		and dc.codigo_estado_cuota=2 ;

	/*************************************************************************************
	ACTUALIZANDO CON ESTADO PAGADO A CRONOGRAMA DETALLE Y FECHA HABILITACION SI NO TENIA
	*************************************************************************************/
	update dc
	set dc.codigo_estado_cuota = @codigo_estado_cuota
		,dc.fecha_programada=isnull(dc.fecha_programada,dp.fecha_pago)
	from detalle_planilla as dp
	inner join detalle_cronograma as dc 
		   on dp.codigo_detalle_cronograma = dc.codigo_detalle 
	where dp.codigo_planilla= @codigo_planilla and dp.excluido=0 and dc.codigo_estado_cuota=2;

	-----------------------------------------------------------------------------------------
	--  SECCIÓN REGISTRO MANUAL DE COMISIONES
	-----------------------------------------------------------------------------------------
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
	when matched then update set cm.codigo_estado_proceso=3;

	/*---------------------------------
	-- DESCUENTOS, RESTA A LOS SALDOS
	---------------------------------*/
	MERGE INTO dbo.descuento_comision dc
	USING(
		select 
			d.monto
			,d.codigo_descuento_comision
		from 
			dbo.descuento d 
		where 
			d.codigo_planilla = @codigo_planilla 
			AND d.estado_registro = 1
	) d on (dc.codigo_descuento_comision = d.codigo_descuento_comision and dc.estado_registro = 1)
	WHEN MATCHED THEN 
		UPDATE 
			SET dc.saldo = dc.saldo - d.monto;

	--update
	--	descuento_comision
	--set 
	--	saldo = saldo - ISNULL((select d.monto from descuento d where d.codigo_planilla = @codigo_planilla and d.codigo_descuento_comision = descuento_comision.codigo_descuento_comision), 0)
	--where
	--	estado_registro = 1

	/*
	CONGELAR LA INFORMACION DE PLANILLA, LIQUIDACION Y EXPORTACION TXT
	*/

	DELETE FROM dbo.sigeco_reporte_comision_planilla WHERE codigo_planilla = @codigo_planilla
	INSERT INTO dbo.sigeco_reporte_comision_planilla
	EXEC up_reporte_planilla_detalle @codigo_planilla, 0

	DELETE FROM dbo.sigeco_reporte_comision_liquidacion WHERE codigo_planilla = @codigo_planilla
	INSERT INTO dbo.sigeco_reporte_comision_liquidacion
	EXEC up_reporte_liquidacion_detalle @codigo_planilla

	DELETE FROM dbo.sigeco_reporte_comision_rrhh WHERE codigo_planilla = @codigo_planilla
	INSERT INTO dbo.sigeco_reporte_comision_rrhh
	EXEC up_detalle_planilla_txt @codigo_planilla

	DELETE FROM dbo.sigeco_reporte_comision_resumen_contabilidad WHERE codigo_planilla = @codigo_planilla
	INSERT INTO dbo.sigeco_reporte_comision_resumen_contabilidad
	EXEC up_planilla_contabilidad_resumen_planilla @codigo_planilla
	
	declare @t_temporal as table (
		id int identity(1, 1)
		,codigo_empresa int
	)
	declare @v_id int = 0, @v_total int = 0, @v_codigo_empresa int
	
	INSERT INTO @t_temporal select codigo_empresa from sigeco_reporte_comision_resumen_contabilidad where codigo_planilla = @codigo_planilla
	SELECT @v_id = MIN(id), @v_total = MAX(id) from @t_temporal
	DELETE FROM dbo.sigeco_reporte_comision_contabilidad WHERE codigo_planilla = @codigo_planilla
	
	WHILE (@v_id <= @v_total)
	BEGIN 
		SET @v_codigo_empresa = (SELECT TOP 1 codigo_empresa FROM @t_temporal WHERE id = @v_id)

		INSERT INTO dbo.sigeco_reporte_comision_contabilidad
		EXEC up_planilla_contabilidad_planilla @codigo_planilla, @v_codigo_empresa
		SET @v_id = @v_id + 1
	END

	/*
	DESBLOQUEO DE VENDEDORES
	*/
	EXEC dbo.up_personal_bloqueo_anular @codigo_planilla, @c_codigo_tipo_bloqueo_comision, @usuario_registra

	SET NOCOUNT OFF;

END;