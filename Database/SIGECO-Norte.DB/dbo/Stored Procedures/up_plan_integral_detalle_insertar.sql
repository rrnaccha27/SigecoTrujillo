CREATE PROCEDURE dbo.up_plan_integral_detalle_insertar
(
	@p_codigo_plan_integral				INT 
	,@p_codigo_campo_santo				INT
	,@p_codigo_tipo_articulo			INT
	,@p_codigo_tipo_articulo_2			INT
	,@p_usuario_registra				VARCHAR(50)
	,@p_codigo_plan_integral_detalle	INT OUTPUT
)
AS
BEGIN
	
	INSERT INTO
		dbo.plan_integral_detalle
	(
		 codigo_plan_integral
		,codigo_campo_santo
		,codigo_tipo_articulo
		,codigo_tipo_articulo_2
		,estado_registro
		,fecha_registra
		,usuario_registra
	)
	VALUES
	(
		 @p_codigo_plan_integral
		,@p_codigo_campo_santo
		,@p_codigo_tipo_articulo
		,@p_codigo_tipo_articulo_2
		,1
		,GETDATE()
		,@p_usuario_registra
	)
	SET @p_codigo_plan_integral_detalle = @@IDENTITY

END