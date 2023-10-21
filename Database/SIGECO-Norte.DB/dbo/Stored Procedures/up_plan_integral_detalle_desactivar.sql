CREATE PROCEDURE dbo.up_plan_integral_detalle_desactivar
(
	 @p_codigo_plan_integral_detalle	INT
	,@p_usuario_modifica				VARCHAR(50)
)
AS
BEGIN
	UPDATE
		dbo.plan_integral_detalle
	SET
		estado_registro = 0
		,fecha_modifica = GETDATE() 
		,usuario_modifica = @p_usuario_modifica
	WHERE
		codigo_plan_integral_detalle = @p_codigo_plan_integral_detalle
END