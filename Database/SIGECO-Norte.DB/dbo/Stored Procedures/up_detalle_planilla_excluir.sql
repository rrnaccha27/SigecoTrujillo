USE SIGECO
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[up_detalle_planilla_excluir]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].up_detalle_planilla_excluir
GO

CREATE PROCEDURE [dbo].up_detalle_planilla_excluir
(
	@codigo_detalle_planilla	int,
	@observacion				varchar(200),
	@usuario_registra			varchar(30),
	@p_permanente				bit,
	@p_error					varchar(200) output,
	@p_excluyo					bit output
)
AS
BEGIN

	DECLARE 
		@p_codigo_detalle_cronograma int,
		@v_codigo_estado_cuota int,
		@v_codigo_planilla int,
		@v_cantidad_registro_excluido_cuota int, 
		@v_codigo_regla_tipo_planilla int,
		@v_codigo_personal int;

	DECLARE
		@c_estado_activo			BIT = 1
		,@c_tipo_bloqueo_comision	INT = 1
		,@c_no_excluido				BIT = 0

	SET @p_excluyo = 0
	SET @p_error = ''
	---------------------------------------------------------------------------
	select top 1
		@p_codigo_detalle_cronograma=codigo_detalle_cronograma,
		@v_codigo_planilla=codigo_planilla,
		@v_codigo_personal = codigo_personal
	from detalle_planilla 
	where codigo_detalle_planilla=@codigo_detalle_planilla;
	----------------------------------------------------------------------------
	select top 1
		@v_codigo_estado_cuota=codigo_estado_cuota 
	from detalle_cronograma 
	where codigo_detalle=@p_codigo_detalle_cronograma;
	---------------------------------------------------------------------------

	if @v_codigo_estado_cuota<>2
	begin    
		SET @p_error = ('La cuota solo se excluye en estado "En Proceso de Pago". ')
		return;
	end;

	/*****************************************************************
	VERIFICANDO QUE NO EXISTE EXCLUSIONES ACTIVAS PARA LA CUOTA
	******************************************************************/
	select
		@v_cantidad_registro_excluido_cuota=count(*) 
	from exclusion_cuota_planilla 
	where codigo_detalle_cronograma=1 and estado_exclusion=1 and estado_registro=1

	if @v_cantidad_registro_excluido_cuota>0
	begin
		SET @p_error = ('El pago no se puede excluir, se encuentra activo en mas de un registro en excluidos.');
		return;
	end;

	update detalle_planilla
	set 
		excluido=1,
		observacion=@observacion,
		fecha_modifica=GETDATE(),
		usuario_modifica=@usuario_registra
	where codigo_detalle_planilla=@codigo_detalle_planilla;

	SET @v_codigo_regla_tipo_planilla = (SELECT TOP 1 codigo_regla_tipo_planilla FROM dbo.planilla WHERE codigo_planilla = @v_codigo_planilla )

	/******************************************************************
	insertando en la tabla excluido
	*******************************************************************/
	insert into exclusion_cuota_planilla
	(
		codigo_detalle_planilla,
		codigo_detalle_cronograma,
		codigo_planilla,	
		usuario_exclusion,
		fecha_exclusion,
		motivo_exclusion,
		estado_exclusion,
		estado_registro,
		codigo_regla_tipo_planilla
	)
	values (@codigo_detalle_planilla, @p_codigo_detalle_cronograma, @v_codigo_planilla, @usuario_registra, GETDATE(), ISNULL(@observacion,'Exclusión automatico'), @p_permanente, 1, @v_codigo_regla_tipo_planilla);

	IF @p_permanente = 0
		update detalle_cronograma 
		set codigo_estado_cuota = 1 /*INDICA QUE EL PAGO SE ENCUENTRA PENDIENTE*/
		where codigo_detalle=@p_codigo_detalle_cronograma;

	ELSE
		update detalle_cronograma 
		set codigo_estado_cuota=4 /*INDICA QUE EL PAGO SE ENCUENTRA EXCLUIDO*/
		where codigo_detalle=@p_codigo_detalle_cronograma;

	EXEC dbo.up_operacion_cuota_comision_insertar @p_codigo_detalle_cronograma, 4, @observacion, @usuario_registra

	/*
	DESBLOQUEAR VENDEDOR
	*/
	IF NOT EXISTS(			
			SELECT 
				dpl.codigo_personal 
			FROM 
				dbo.detalle_planilla dpl
			WHERE 
				dpl.codigo_personal = @v_codigo_personal
				AND dpl.estado_registro = @c_estado_activo 
				AND dpl.codigo_planilla = @v_codigo_planilla 
				AND dpl.excluido = @c_no_excluido
		)
	BEGIN
			UPDATE
				dbo.personal_bloqueo 
			SET
				estado_registro = 0
				,fecha_modifica = GETDATE()
				,usuario_modifica = @usuario_registra
			WHERE 
				codigo_personal = @v_codigo_personal
				AND estado_registro = @c_estado_activo 
				AND codigo_planilla = @v_codigo_planilla 
				AND codigo_tipo_bloqueo = @c_tipo_bloqueo_comision
	END

	SET @p_excluyo = 1;
END;