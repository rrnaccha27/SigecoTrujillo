CREATE PROCEDURE dbo.up_plan_integral_insertar
(
	@p_nombre					VARCHAR(250)
	,@p_vigencia_inicio			VARCHAR(8)
	,@p_vigencia_fin			VARCHAR(8)
	,@p_usuario_registra		VARCHAR(50)
	,@p_codigo_plan_integral	INT OUTPUT
)
AS
BEGIN
	
	INSERT INTO
		dbo.plan_integral
	(
		 nombre
		,vigencia_inicio
		,vigencia_fin
		,estado_registro
		,fecha_registra
		,usuario_registra
	)
	VALUES
	(
		 @p_nombre
		,@p_vigencia_inicio
		,@p_vigencia_fin
		,1
		,GETDATE()
		,@p_usuario_registra
	)
	SET @p_codigo_plan_integral = @@IDENTITY

END