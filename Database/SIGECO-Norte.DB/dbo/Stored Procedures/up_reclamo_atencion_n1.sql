CREATE PROCEDURE dbo.up_reclamo_atencion_n1
(
	@p_codigo_reclamo			INT
	,@p_codigo_estado_resultado	INT
	,@p_observacion				VARCHAR(300)
	,@p_usuario					VARCHAR(50)
)
AS
BEGIN
	SET NOCOUNT ON

	UPDATE
		dbo.reclamo
	SET
		codigo_estado_resultado_n1 = @p_codigo_estado_resultado
		,codigo_estado_reclamo = case when @p_codigo_estado_resultado = 2 then 2 else codigo_estado_reclamo end
		,observacion_n1 = @p_observacion
		,usuario_modifica_n1 = @p_usuario
		,fecha_modifica_n1 = GETDATE()
		,usuario_modifica = @p_usuario
		,fecha_modifica = GETDATE()
	WHERE 
		codigo_reclamo = @p_codigo_reclamo

	SET NOCOUNT OFF
END;