CREATE PROCEDURE dbo.up_plan_integral_desactivar
(
	 @p_codigo_plan_integral	INT
	,@p_usuario_modifica		VARCHAR(50)
)
AS
BEGIN

	BEGIN TRY

		BEGIN TRAN plan_integral

		UPDATE
			dbo.plan_integral_detalle
		SET
			estado_registro = 0
			,fecha_modifica = GETDATE() 
			,usuario_modifica = @p_usuario_modifica
		WHERE
			codigo_plan_integral = @p_codigo_plan_integral
			AND estado_registro = 1

		UPDATE
			dbo.plan_integral
		SET
			estado_registro = 0
			,fecha_modifica = GETDATE() 
			,usuario_modifica = @p_usuario_modifica
		WHERE
			codigo_plan_integral = @p_codigo_plan_integral

		COMMIT TRAN plan_integral

	END TRY
	BEGIN CATCH
		ROLLBACK TRAN plan_integral
	END CATCH

END