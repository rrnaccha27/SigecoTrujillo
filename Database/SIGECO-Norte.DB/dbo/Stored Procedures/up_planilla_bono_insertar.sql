USE SIGECO
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[up_planilla_bono_insertar]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].up_planilla_bono_insertar
GO

CREATE PROCEDURE [dbo].[up_planilla_bono_insertar]
(
	@p_codigo_tipo_planilla int,
	@p_codigo_canal int,
	@fecha_inicio datetime,
	@fecha_fin datetime,
	@usuario_registra varchar(30),
	@p_es_planilla_jn bit = 0,
	@codigo_planilla int out,
	@total_registro_procesado int out
)
AS
BEGIN

	DECLARE 
		@n int,
		@i int,
		@numero_planilla varchar(50),
		@fecha_apertura datetime,
		@codigo_estado_cuota int,
		@codigo_estado_planilla int,
		@cantidad_planilla_mes int,
		@c_codigo_tipo_bloqueo_bono	int;
	
	set  @fecha_apertura=GETDATE();
	set @codigo_estado_planilla=1;
	set @codigo_estado_cuota=2;--INDICA QUE LA CUOTA SE ENCUENTRA EN PROCESO DE PAGO
	set @c_codigo_tipo_bloqueo_bono = 2;/*BONO*/
	select @numero_planilla=cast( YEAR(GETDATE()) AS VARCHAR)+'-'+REPLACE(STR(MONTH(GETDATE()), 2), SPACE(1), '0') 

	select @cantidad_planilla_mes=isnull(count(1),0)+1 
	from
		dbo.planilla_bono
	where 
		estado_registro = 1 and year(fecha_registra)=YEAR(GETDATE()) and month(fecha_registra)=month(GETDATE());

	set @numero_planilla = @numero_planilla + '-' + CAST(@cantidad_planilla_mes as varchar);

	--if EXISTS(
	--	SELECT TOP 1 codigo_planilla 
	--	FROM dbo.planilla_bono 
	--	WHERE
	--		estado_registro = 1 AND codigo_estado_planilla IN (1, 2) AND codigo_tipo_planilla=@p_codigo_tipo_planilla AND es_planilla_jn = @p_es_planilla_jn
	--		AND
	--		(@fecha_inicio BETWEEN fecha_inicio AND fecha_fin 
	--		OR @fecha_fin BETWEEN fecha_inicio AND fecha_fin
	--		OR fecha_inicio BETWEEN @fecha_inicio AND @fecha_fin 
	--		OR fecha_fin BETWEEN @fecha_inicio AND @fecha_fin) 
	--	)
	--begin
	--   RAISERROR('Para el rango fecha establecido ya existe planilla de bono vigente',16,1); 
	--   return;
	--end;

	insert into planilla_bono(
		numero_planilla,
		codigo_canal,
		fecha_inicio,
		fecha_fin,
		fecha_apertura,
		usuario_apertura,
		codigo_tipo_planilla,
		codigo_estado_planilla,
		estado_registro,
		fecha_registra,
		usuario_registra,
		es_planilla_jn
	)
	values(
		@numero_planilla,
		@p_codigo_canal,
		@fecha_inicio,
		@fecha_fin,
		@fecha_apertura,
		@usuario_registra,
		@p_codigo_tipo_planilla,
		@codigo_estado_planilla,
		1,
		GETDATE(),
		@usuario_registra,
		@p_es_planilla_jn
	);
	set @codigo_planilla=@@IDENTITY;
	
	/*************************************************************************/
	declare @v_id uniqueidentifier 
	set @v_id = NEWID()

	IF @p_es_planilla_jn = 0
	BEGIN
		exec [dbo].[up_proceso_generacion_bono] @codigo_planilla, @p_codigo_tipo_planilla, @p_codigo_canal, @fecha_inicio, @fecha_fin, @usuario_registra, @v_id;

		SELECT 
			@total_registro_procesado=count(*)
		FROM 
			detalle_planilla_bono dpb
		inner join dbo.resumen_planilla_bono rpb on rpb.codigo_planilla = @codigo_planilla and rpb.codigo_personal = dpb.codigo_personal
		inner join dbo.contrato_planilla_bono cpb on cpb.codigo_planilla = @codigo_planilla and case when @p_codigo_tipo_planilla = 1 then cpb.codigo_personal else cpb.codigo_supervisor end = dpb.codigo_personal and cpb.codigo_empresa = dpb.codigo_empresa
		inner join dbo.articulo_planilla_bono apb on apb.codigo_planilla_bono = @codigo_planilla and apb.codigo_empresa = cpb.codigo_empresa and apb.nro_contrato = cpb.numero_contrato
		WHERE 
			dpb.codigo_planilla = @codigo_planilla
		AND ((apb.dinero_ingresado * rpb.porcentaje_pago) / 100) > 0
	END
	ELSE
	BEGIN
		exec [dbo].[up_proceso_generacion_bono_jn] @codigo_planilla, @p_codigo_tipo_planilla, @p_codigo_canal, @fecha_inicio, @fecha_fin, @usuario_registra, @v_id;

		SELECT 
			@total_registro_procesado=count(*)
		FROM 
			dbo.contrato_planilla_bono cpb 
		inner join dbo.articulo_planilla_bono apb on apb.codigo_planilla_bono = @codigo_planilla and apb.codigo_empresa = cpb.codigo_empresa and apb.nro_contrato = cpb.numero_contrato
		inner join dbo.resumen_planilla_bono rpb on rpb.codigo_planilla = @codigo_planilla
		WHERE 
			cpb.codigo_planilla = @codigo_planilla
		AND ((apb.dinero_ingresado * rpb.porcentaje_pago) / 100) > 0

		IF (@total_registro_procesado>0)
		BEGIN
			INSERT INTO dbo.sigeco_reporte_bono_jn_resumen
			EXEC [dbo].up_planilla_bono_jn_resumen @codigo_planilla
		END
	END
	/**************************************************************************************************************/

	IF (@total_registro_procesado<=0)
	BEGIN
		UPDATE dbo.planilla_bono SET estado_registro = 0 WHERE codigo_planilla = @codigo_planilla;
		--RAISERROR('Para el rango de fecha establecido no existe pagos habilitados.',16,1); 
		RETURN;
	END
	ELSE
	BEGIN
		/*
		BLOQUEO DE VENDEDORES
		*/
		EXEC dbo.up_personal_bloqueo_registrar @codigo_planilla, @c_codigo_tipo_bloqueo_bono, @usuario_registra
	END
	 
END;