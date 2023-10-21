IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[up_regla_tipo_planilla_insertar]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[up_regla_tipo_planilla_insertar]

GO
CREATE PROCEDURE [dbo].[up_regla_tipo_planilla_insertar]
(
	@p_nombre						VARCHAR(200),
	@p_codigo_tipo_planilla			INT,
	@p_usuario_registra				VARCHAR(50),
	@p_afecto_doc_completa			BIT,
	@p_codigo_tipo_reporte			INT,
	@p_detraccion_por_contrato		BIT,
	@p_envio_liquidacion			BIT,
	@p_codigo_regla_tipo_planilla	INT OUT
)
AS
BEGIN
	SET NOCOUNT OFF

	DECLARE 
		@v_cantidad_registro	INT
		,@v_nomenclatura		VARCHAR(3);

	SELECT TOP 1
		@v_cantidad_registro=count(*)
	FROM 
		regla_tipo_planilla 
	WHERE
		estado_registro=1 
		and upper(nombre)  =upper(@p_nombre);

	IF(@v_cantidad_registro>0)
	BEGIN
		RAISERROR('El nombre de la regla ya existe, vuelva ingresar otro nombre.',16,1); 
		RETURN;
	END;

	SET @v_nomenclatura = (SELECT TOP 1 nomenclatura FROM dbo.tipo_reporte WHERE codigo_tipo_reporte = @p_codigo_tipo_reporte)
	SET @v_nomenclatura = ISNULL(@v_nomenclatura, '')

	INSERT INTO dbo.regla_tipo_planilla(
		nombre,codigo_tipo_planilla, usuario_registra,estado_registro,fecha_registra, afecto_doc_completa, tipo_reporte, detraccion_por_contrato, envio_liquidacion
	)
	VALUES(
		@p_nombre, @p_codigo_tipo_planilla, @p_usuario_registra, 1, GETDATE(), @p_afecto_doc_completa, @v_nomenclatura, @p_detraccion_por_contrato, @p_envio_liquidacion
	);

	SET @p_codigo_regla_tipo_planilla = SCOPE_IDENTITY();

	SET NOCOUNT OFF
END;