USE SIGECO
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[up_planilla_bono_cerrar]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].up_planilla_bono_cerrar
GO

CREATE PROCEDURE [dbo].[up_planilla_bono_cerrar]
(
	@codigo_planilla int,
	@usuario_registra varchar(30)
)
AS
BEGIN
	SET NOCOUNT ON

	DECLARE 
		@fecha_cierre datetime,
		@fecha_apertura datetime,
		@planilla_habilitado int,
		@codigo_estado_cuota int,
		@cantidad_registros_a_cerrar int,
		@codigo_estado_planilla int,
		@c_codigo_tipo_bloqueo_bono int;

	set @fecha_cierre=GETDATE();
	set @codigo_estado_planilla=2;--indica cerrado la planilla
	set @c_codigo_tipo_bloqueo_bono = 2;/*BONO*/

	select 
		@planilla_habilitado= COUNT(1) 
	from
		planilla_bono 
	where 
		codigo_planilla=@codigo_planilla 
		and codigo_estado_planilla=@codigo_estado_planilla;

	if(@planilla_habilitado>0)
	begin
		RAISERROR('La planilla de bono se encuentra cerrada.',16,1); 
		return;
	end;

	--select @fecha_apertura= fecha_apertura 
	--from planilla_bono where codigo_planilla=@codigo_planilla;

	--if( convert(date, @fecha_cierre) = convert(date, @fecha_apertura) )
	--begin
	--	RAISERROR('No se puede cerrar la planilla de bono  en la misma fecha.',16,1); 
	--	return;
	--end;
	 
	/************************************
	*  validar cantidad de registros a aprobar
	*************************************/
	select 
		@cantidad_registros_a_cerrar=count(*)
	from
		detalle_planilla_bono p 
	inner join planilla_bono dc
		on p.codigo_planilla=dc.codigo_planilla
	where 
		p.codigo_planilla=@codigo_planilla 
		and dc.codigo_estado_planilla=2;

	/*
	if(@cantidad_registros_a_cerrar=0)
	begin
		RAISERROR('La planilla de bono no cuenta con pagos habilitados',16,1); 
		return;
	end;
	*/

	update
		planilla_bono
	set
		codigo_estado_planilla=@codigo_estado_planilla,
		fecha_cierre=@fecha_cierre,
		usuario_cierre=@usuario_registra,
		usuario_modifica=@usuario_registra,
		fecha_modifica=GETDATE()
	where
		codigo_planilla=@codigo_planilla;

	/* SECCION DE CONGELAR DATA PARA REPORTES */

	DECLARE 
		@v_es_planilla_vendedor	bit
		,@v_es_planilla_jn	bit
		,@v_id int
		,@v_total int
		,@v_codigo_empresa int

	DECLARE @t_temp AS TABLE(
		id				int identity(1,1)
		,codigo_empresa int
	)

	SELECT TOP 1 
		@v_es_planilla_jn = ISNULL(es_planilla_jn, 0)
		,@v_es_planilla_vendedor = case when codigo_tipo_planilla = 1 then 1 else 0 end
	FROM
		dbo.planilla_bono
	WHERE
		codigo_planilla = @codigo_planilla

	IF (@v_es_planilla_vendedor = 1)
	BEGIN
		DELETE FROM dbo.sigeco_reporte_bono_resumen WHERE codigo_planilla = @codigo_planilla
		INSERT INTO dbo.sigeco_reporte_bono_resumen
		EXEC dbo.up_repote_planilla_bono_personal @codigo_planilla 
		
		DELETE FROM dbo.sigeco_reporte_bono_porcentajes WHERE codigo_planilla = @codigo_planilla
		INSERT INTO dbo.sigeco_reporte_bono_porcentajes
		EXEC dbo.up_reporte_liquidacion_bono_personal_porcentajes @codigo_planilla

		DELETE FROM dbo.sigeco_reporte_bono_detalle_vendedor WHERE codigo_planilla = @codigo_planilla
		INSERT INTO dbo.sigeco_reporte_bono_detalle_vendedor
		EXEC dbo.up_reporte_liquidacion_bono_personal @codigo_planilla

		DELETE FROM dbo.sigeco_reporte_bono_articulos_vendedor WHERE codigo_planilla = @codigo_planilla
		INSERT INTO dbo.sigeco_reporte_bono_articulos_vendedor
		EXEC dbo.up_reporte_liquidacion_bono_articulos @codigo_planilla, NULL, NULL

		DELETE FROM dbo.sigeco_reporte_bono_liquidacion WHERE codigo_planilla = @codigo_planilla
		INSERT INTO dbo.sigeco_reporte_bono_liquidacion
		EXEC dbo.up_reporte_liquidacion_bono_individual @codigo_planilla

		DELETE FROM dbo.sigeco_reporte_bono_rrhh WHERE codigo_planilla = @codigo_planilla
		INSERT INTO dbo.sigeco_reporte_bono_rrhh
		EXEC dbo.up_detalle_planilla_bono_txt @codigo_planilla

		DELETE FROM dbo.sigeco_reporte_bono_contabilidad_resumen WHERE codigo_planilla = @codigo_planilla
		INSERT INTO dbo.sigeco_reporte_bono_contabilidad_resumen
		EXEC up_planilla_bono_contabilidad_resumen_planilla @codigo_planilla

		DELETE FROM dbo.sigeco_reporte_bono_contabilidad WHERE codigo_planilla = @codigo_planilla
		INSERT INTO @t_temp SELECT codigo_empresa FROM dbo.sigeco_reporte_bono_contabilidad_resumen WHERE codigo_planilla = @codigo_planilla
		SELECT @v_id = MIN(id), @v_total = MAX(id) FROM @t_temp

		WHILE (@v_id <= @v_total)
		BEGIN
			SELECT TOP 1 @v_codigo_empresa = codigo_empresa FROM @t_temp WHERE id = @v_id
			
			INSERT INTO dbo.sigeco_reporte_bono_contabilidad
			EXEC dbo.up_planilla_bono_contabilidad_planilla @codigo_planilla, @v_codigo_empresa
			
			SET @v_id = @v_id + 1
		END

	END
	ELSE
	BEGIN
		IF (@v_es_planilla_jn = 0)--Es supervisoer pero no JN
		BEGIN
			DELETE FROM dbo.sigeco_reporte_bono_resumen WHERE codigo_planilla = @codigo_planilla
			INSERT INTO dbo.sigeco_reporte_bono_resumen
			EXEC dbo.up_reporte_planilla_bono_supervisor_general @codigo_planilla

			DELETE FROM dbo.sigeco_reporte_bono_detalle_supervisor WHERE codigo_planilla = @codigo_planilla
			INSERT INTO dbo.sigeco_reporte_bono_detalle_supervisor
			EXEC dbo.up_reporte_liquidacion_bono_supervisor_general @codigo_planilla

			DELETE FROM dbo.sigeco_reporte_bono_liquidacion WHERE codigo_planilla = @codigo_planilla
			INSERT INTO dbo.sigeco_reporte_bono_liquidacion
			EXEC dbo.up_reporte_liquidacion_bono_individual @codigo_planilla

			DELETE FROM dbo.sigeco_reporte_bono_rrhh WHERE codigo_planilla = @codigo_planilla
			INSERT INTO dbo.sigeco_reporte_bono_rrhh
			EXEC dbo.up_detalle_planilla_bono_txt @codigo_planilla

			DELETE FROM dbo.sigeco_reporte_bono_contabilidad_resumen WHERE codigo_planilla = @codigo_planilla
			INSERT INTO sigeco_reporte_bono_contabilidad_resumen
			EXEC up_planilla_bono_contabilidad_resumen_planilla @codigo_planilla

			DELETE FROM dbo.sigeco_reporte_bono_contabilidad WHERE codigo_planilla = @codigo_planilla
			INSERT INTO @t_temp SELECT codigo_empresa FROM dbo.sigeco_reporte_bono_contabilidad_resumen WHERE codigo_planilla = @codigo_planilla
			SELECT @v_id = MIN(id), @v_total = MAX(id) FROM @t_temp

			WHILE (@v_id <= @v_total)
			BEGIN
				SELECT TOP 1 @v_codigo_empresa = codigo_empresa FROM @t_temp WHERE id = @v_id
			
				INSERT INTO dbo.sigeco_reporte_bono_contabilidad
				EXEC dbo.up_planilla_bono_contabilidad_planilla @codigo_planilla, @v_codigo_empresa
			
				SET @v_id = @v_id + 1
			END
		END
		ELSE
		BEGIN
			DELETE FROM dbo.sigeco_reporte_bono_jn_detalle WHERE codigo_planilla = @codigo_planilla
			INSERT INTO dbo.sigeco_reporte_bono_jn_detalle
			EXEC dbo.up_planilla_bono_jn_detalle @codigo_planilla

			DELETE FROM dbo.sigeco_reporte_bono_jn_resumen WHERE codigo_planilla = @codigo_planilla
			INSERT INTO dbo.sigeco_reporte_bono_jn_resumen
			EXEC dbo.up_planilla_bono_jn_resumen @codigo_planilla

			DELETE FROM dbo.sigeco_reporte_bono_jn_resumen_titulo WHERE codigo_planilla = @codigo_planilla
			INSERT INTO dbo.sigeco_reporte_bono_jn_resumen_titulo
			EXEC dbo.up_planilla_bono_jn_resumen_titulos @codigo_planilla

			DELETE FROM dbo.sigeco_reporte_bono_jn WHERE codigo_planilla = @codigo_planilla
			INSERT INTO dbo.sigeco_reporte_bono_jn
			EXEC dbo.up_planilla_bono_jn @codigo_planilla

			DELETE FROM dbo.sigeco_reporte_bono_jn_contabilidad WHERE codigo_planilla = @codigo_planilla
			INSERT INTO dbo.sigeco_reporte_bono_jn_contabilidad
			EXEC dbo.up_planilla_bono_jn_contabilidad @codigo_planilla

			DELETE FROM dbo.sigeco_reporte_bono_jn_liquidacion WHERE codigo_planilla = @codigo_planilla
			INSERT INTO dbo.sigeco_reporte_bono_jn_liquidacion
			EXEC dbo.up_planilla_bono_jn_liquidacion @codigo_planilla

		END
	END

	/*
	DESBLOQUEO DE VENDEDORES
	*/
	EXEC dbo.up_personal_bloqueo_anular @codigo_planilla, @c_codigo_tipo_bloqueo_bono, @usuario_registra


	SET NOCOUNT OFF

END;