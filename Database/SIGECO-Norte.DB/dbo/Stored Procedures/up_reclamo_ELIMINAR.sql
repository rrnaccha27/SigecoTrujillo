CREATE PROCEDURE [dbo].[up_reclamo_ELIMINAR]
	@codigo_reclamo                                     int
AS
BEGIN
	BEGIN TRANSACTION Eliminarreclamo
	BEGIN
		DELETE FROM reclamo
		WHERE
			codigo_reclamo = @codigo_reclamo
	END
	IF @@ERROR <> 0 ROLLBACK TRANSACTION
	ELSE COMMIT TRANSACTION
END