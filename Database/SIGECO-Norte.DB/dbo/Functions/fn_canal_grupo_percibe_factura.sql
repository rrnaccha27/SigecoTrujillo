CREATE FUNCTION dbo.fn_canal_grupo_percibe_factura
(

	 @p_codigo_canal_grupo	INT
	,@p_codigo_empresa		INT
	,@p_supervisor_personal	BIT
	,@p_comision_bono		BIT
)
RETURNS BIT
AS
BEGIN
	
	DECLARE
		@v_retorno							BIT = 0
		,@v_codigo_configuracion			INT
		,@v_codigo_empresa_configuracion	INT

	SET @v_codigo_configuracion = ISNULL((SELECT TOP 1 codigo_configuracion FROM dbo.configuracion_canal_grupo WHERE codigo_canal_grupo = @p_codigo_canal_grupo AND supervisor_personal = @p_supervisor_personal AND comision_bono = @p_comision_bono AND percibe = 1), 0)

	IF (@v_codigo_configuracion = 0)
		RETURN @v_retorno

	SET @v_codigo_empresa_configuracion = ISNULL((SELECT TOP 1 codigo_empresa_configuracion FROM dbo.empresa_configuracion WHERE codigo_configuracion = @v_codigo_configuracion AND planilla_factura = 0 AND codigo_empresa = @p_codigo_empresa ), 0)

	IF (@v_codigo_empresa_configuracion = 0)
		RETURN @v_retorno

	SET @v_retorno = 1

	RETURN @v_retorno
END