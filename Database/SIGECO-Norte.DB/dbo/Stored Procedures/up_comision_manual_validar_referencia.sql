CREATE PROC dbo.up_comision_manual_validar_referencia
(
	@p_codigo_comision_manual	int ,
	@p_mensaje					varchar(250) OUTPUT
)
AS
BEGIN
	SET NOCOUNT ON
	SET @p_mensaje = ''

	IF EXISTS(SELECT * FROM dbo.comision_manual
	WHERE codigo_comision_manual = @p_codigo_comision_manual
	AND (NOT codigo_detalle_cronograma IS NULL OR en_planilla = 1))
	BEGIN
		SET @p_mensaje = 'Se encuentra referenciado a un contrato/planilla.'
		RETURN
	END

	IF EXISTS(SELECT * FROM dbo.comision_manual
	WHERE codigo_comision_manual = @p_codigo_comision_manual
	AND codigo_estado_proceso = 3)
	BEGIN
		SET @p_mensaje = 'Se encuentra procesado en planilla cerrada.' 
		RETURN
	END
END