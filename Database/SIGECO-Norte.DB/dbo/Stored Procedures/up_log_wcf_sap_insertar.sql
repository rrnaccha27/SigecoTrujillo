CREATE PROCEDURE dbo.up_log_wcf_sap_insertar
(
	@p_objeto				VARCHAR(50)
	,@p_codigo_sigeco		INT
	,@p_codigo_equivalencia	VARCHAR(50)
	,@p_tipo_operacion		CHAR(1)
	,@p_mensaje_excepcion	VARCHAR(MAX)
	,@p_usuario_registro	VARCHAR(50)
)
AS
BEGIN
	
	INSERT INTO	dbo.log_wcf_sap
	(
		objeto
		,codigo_sigeco
		,codigo_equivalencia
		,tipo_operacion
		,mensaje_excepcion
		,usuario_registro
	)
	VALUES
	(
		@p_objeto
		,@p_codigo_sigeco
		,@p_codigo_equivalencia
		,@p_tipo_operacion
		,@p_mensaje_excepcion
		,@p_usuario_registro
	)

END