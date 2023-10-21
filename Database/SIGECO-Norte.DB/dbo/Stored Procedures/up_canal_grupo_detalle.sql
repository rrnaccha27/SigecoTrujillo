CREATE PROCEDURE dbo.up_canal_grupo_detalle
(
	@p_codigo_canal_grupo	INT
)
AS
BEGIN

DECLARE 
	 @v_config_1	INT
	,@v_config_2	INT
	,@v_config_3	INT
	,@v_config_4	INT
	
	,@v_s_percibe_comision	INT
	,@v_s_percibe_bono		INT
	,@v_p_percibe_comision	INT
	,@v_p_percibe_bono		INT

	,@v_s_c_empresa_factura		VARCHAR(100)
	,@v_s_c_empresa_planilla	VARCHAR(100)
	,@v_s_b_empresa_factura		VARCHAR(100)
	,@v_s_b_empresa_planilla	VARCHAR(100)

	,@v_p_c_empresa_factura		VARCHAR(100)
	,@v_p_c_empresa_planilla	VARCHAR(100)
	,@v_p_b_empresa_factura		VARCHAR(100)
	,@v_p_b_empresa_planilla	VARCHAR(100)


	SELECT TOP 1 @v_config_1 = codigo_configuracion, @v_s_percibe_comision = percibe FROM configuracion_canal_grupo WHERE codigo_canal_grupo = @p_codigo_canal_grupo AND supervisor_personal = 1 AND comision_bono = 1
	SELECT TOP 1 @v_config_2 = codigo_configuracion, @v_s_percibe_bono = percibe FROM configuracion_canal_grupo WHERE codigo_canal_grupo = @p_codigo_canal_grupo AND supervisor_personal = 1 AND comision_bono = 0
	SELECT TOP 1 @v_config_3 = codigo_configuracion, @v_p_percibe_comision = percibe FROM configuracion_canal_grupo WHERE codigo_canal_grupo = @p_codigo_canal_grupo AND supervisor_personal = 0 AND comision_bono = 1
	SELECT TOP 1 @v_config_4 = codigo_configuracion, @v_p_percibe_bono = percibe FROM configuracion_canal_grupo WHERE codigo_canal_grupo = @p_codigo_canal_grupo AND supervisor_personal = 0 AND comision_bono = 0

	SELECT @v_s_c_empresa_factura = '', @v_s_c_empresa_planilla = '', @v_s_b_empresa_factura = '', @v_s_b_empresa_planilla = ''
	SELECT @v_p_c_empresa_factura = '', @v_p_c_empresa_planilla = '', @v_p_b_empresa_factura = '', @v_p_b_empresa_planilla = ''

	IF (@v_s_percibe_comision = 1)
	BEGIN
		SELECT @v_s_c_empresa_planilla = convert(varchar, codigo_empresa) + '.' + @v_s_c_empresa_planilla FROM dbo.empresa_configuracion where  codigo_configuracion = @v_config_1 and planilla_factura = 1
		SET @v_s_c_empresa_planilla = ISNULL(@v_s_c_empresa_planilla, '')
		SET @v_s_c_empresa_planilla = CASE WHEN LEN(@v_s_c_empresa_planilla) > 0 THEN SUBSTRING(@v_s_c_empresa_planilla, 1, LEN(@v_s_c_empresa_planilla) -1) ELSE '' END

		SELECT @v_s_c_empresa_factura = convert(varchar, codigo_empresa)  + '.' + @v_s_c_empresa_factura FROM dbo.empresa_configuracion where  codigo_configuracion = @v_config_1 and planilla_factura = 0
		SET @v_s_c_empresa_factura = ISNULL(@v_s_c_empresa_factura, '')
		SET @v_s_c_empresa_factura = CASE WHEN LEN(@v_s_c_empresa_factura) > 0 THEN SUBSTRING(@v_s_c_empresa_factura, 1, LEN(@v_s_c_empresa_factura) -1) ELSE '' END
	END

	IF (@v_s_percibe_bono = 1)
	BEGIN
		SELECT @v_s_b_empresa_planilla = convert(varchar, codigo_empresa) + '.' + @v_s_b_empresa_planilla FROM dbo.empresa_configuracion where  codigo_configuracion = @v_config_2 and planilla_factura = 1
		SET @v_s_b_empresa_planilla = ISNULL(@v_s_b_empresa_planilla, '')
		SET @v_s_b_empresa_planilla = CASE WHEN LEN(@v_s_b_empresa_planilla) > 0 THEN SUBSTRING(@v_s_b_empresa_planilla, 1, LEN(@v_s_b_empresa_planilla) -1) ELSE '' END

		SELECT @v_s_b_empresa_factura = convert(varchar, codigo_empresa) + '.' + @v_s_b_empresa_factura FROM dbo.empresa_configuracion where  codigo_configuracion = @v_config_2 and planilla_factura = 0
		SET @v_s_b_empresa_factura = ISNULL(@v_s_b_empresa_factura, '')
		SET @v_s_b_empresa_factura = CASE WHEN LEN(@v_s_b_empresa_factura) > 0 THEN SUBSTRING(@v_s_b_empresa_factura, 1, LEN(@v_s_b_empresa_factura) -1) ELSE '' END
	END

	IF (@v_p_percibe_comision = 1)
	BEGIN
		SELECT @v_p_c_empresa_planilla = convert(varchar, codigo_empresa) + '.' + @v_p_c_empresa_planilla FROM dbo.empresa_configuracion where  codigo_configuracion = @v_config_3 and planilla_factura = 1
		SET @v_p_c_empresa_planilla = ISNULL(@v_p_c_empresa_planilla, '')
		SET @v_p_c_empresa_planilla = CASE WHEN LEN(@v_p_c_empresa_planilla) > 0 THEN SUBSTRING(@v_p_c_empresa_planilla, 1, LEN(@v_p_c_empresa_planilla) -1) ELSE '' END

		SELECT @v_p_c_empresa_factura = convert(varchar, codigo_empresa) + '.' + @v_p_c_empresa_factura FROM dbo.empresa_configuracion where  codigo_configuracion = @v_config_3 and planilla_factura = 0
		SET @v_p_c_empresa_factura = ISNULL(@v_p_c_empresa_factura, '')
		SET @v_p_c_empresa_factura = CASE WHEN LEN(@v_p_c_empresa_factura) > 0 THEN SUBSTRING(@v_p_c_empresa_factura, 1, LEN(@v_p_c_empresa_factura) -1) ELSE '' END
	END

	IF (@v_p_percibe_bono = 1)
	BEGIN
		SELECT @v_p_b_empresa_planilla = convert(varchar, codigo_empresa) + '.' + @v_p_b_empresa_planilla FROM dbo.empresa_configuracion where  codigo_configuracion = @v_config_4 and planilla_factura = 1
		SET @v_p_b_empresa_planilla = ISNULL(@v_p_b_empresa_planilla, '')
		SET @v_p_b_empresa_planilla = CASE WHEN LEN(@v_p_b_empresa_planilla) > 0 THEN SUBSTRING(@v_p_b_empresa_planilla, 1, LEN(@v_p_b_empresa_planilla) -1) ELSE '' END

		SELECT @v_p_b_empresa_factura = convert(varchar, codigo_empresa) + '.' + @v_p_b_empresa_factura FROM dbo.empresa_configuracion where  codigo_configuracion = @v_config_4 and planilla_factura = 0
		SET @v_p_b_empresa_factura = ISNULL(@v_p_b_empresa_factura, '')
		SET @v_p_b_empresa_factura = CASE WHEN LEN(@v_p_b_empresa_factura) > 0 THEN SUBSTRING(@v_p_b_empresa_factura, 1, LEN(@v_p_b_empresa_factura) -1) ELSE '' END
	END

	SELECT 
			codigo_canal_grupo
			, ISNULL(codigo_equivalencia, '') AS codigo_equivalencia
			, nombre
			, administra_grupos
			, @v_s_percibe_comision AS s_percibe_comision
			, @v_s_percibe_bono AS s_percibe_bono
			, @v_p_percibe_comision AS p_percibe_comision
			, @v_p_percibe_bono AS p_percibe_bono
			, @v_s_c_empresa_planilla AS s_c_empresa_planilla
			, @v_s_c_empresa_factura AS s_c_empresa_factura
			, @v_s_b_empresa_planilla AS s_b_empresa_planilla
			, @v_s_b_empresa_factura AS s_b_empresa_factura
			, @v_p_c_empresa_planilla AS p_c_empresa_planilla
			, @v_p_c_empresa_factura AS p_c_empresa_factura
			, @v_p_b_empresa_planilla AS p_b_empresa_planilla
			, @v_p_b_empresa_factura AS p_b_empresa_factura
			, estado_registro
	FROM
		dbo.canal_grupo
	WHERE
		codigo_canal_grupo = @p_codigo_canal_grupo

END