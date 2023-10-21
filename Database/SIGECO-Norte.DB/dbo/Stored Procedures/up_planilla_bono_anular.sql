USE SIGECO
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[up_planilla_bono_anular]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].up_planilla_bono_anular
GO

CREATE PROCEDURE [dbo].[up_planilla_bono_anular]
(
	@codigo_planilla int,
	@usuario_registra varchar(30)
)
AS
BEGIN

	DECLARE 
		@fecha_cierre datetime,
		@planilla_habilitado int,
		@codigo_estado_cuota_pendiente_pago int,
		@codigo_estado_cuota_en_proceso_pago int,
		@codigo_estado_planilla int,
		@c_codigo_tipo_bloqueo_bono int;

	set @fecha_cierre=GETDATE();
	set @codigo_estado_planilla=3;--indica anulado la planilla
	set @c_codigo_tipo_bloqueo_bono = 2;/*BONO*/

	select
		@planilla_habilitado= COUNT(1) 
	from 
		planilla_bono 
	where 
		codigo_planilla=@codigo_planilla 
		and codigo_estado_planilla=1;

	if(@planilla_habilitado=0)
	begin
		RAISERROR('La planilla bono no se puede anular, se encuentra cerrado o anulado',16,1); 
		return;
	end;

	update 
		planilla_bono
	set
		codigo_estado_planilla=@codigo_estado_planilla,
		fecha_anulacion=@fecha_cierre,
		usuario_anulacion=@usuario_registra,
		usuario_modifica=@usuario_registra,
		fecha_modifica=GETDATE()
	where
		codigo_planilla=@codigo_planilla;

	/*
	DESBLOQUEO DE VENDEDORES
	*/
	EXEC dbo.up_personal_bloqueo_anular @codigo_planilla, @c_codigo_tipo_bloqueo_bono, @usuario_registra

END;