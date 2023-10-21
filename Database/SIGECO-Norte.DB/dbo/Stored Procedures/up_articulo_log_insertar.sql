CREATE PROCEDURE dbo.up_articulo_log_insertar
(
	@p_codigo_articulo	INT
)
AS
BEGIN
	SET NOCOUNT ON

	INSERT INTO dbo.articulo_log(
		[fecha_log]
		,[codigo_articulo]
		,[codigo_unidad_negocio]
		,[codigo_categoria]
		,[codigo_sku]
		,[nombre]
		,[abreviatura]
		,[genera_comision]
		,[genera_bono]
		,[genera_bolsa_bono]
		,[anio_contrato_vinculante]
		,[tiene_contrato_vinculante]
		,[estado_registro]
		,[fecha_registra]
		,[usuario_registra]
		,[fecha_modifica]
		,[usuario_modifica]
		,[codigo_tipo_articulo]
		,[cantidad_unica]
	)
	SELECT TOP 1
		GETDATE()
		,[codigo_articulo]
		,[codigo_unidad_negocio]
		,[codigo_categoria]
		,[codigo_sku]
		,[nombre]
		,[abreviatura]
		,[genera_comision]
		,[genera_bono]
		,[genera_bolsa_bono]
		,[anio_contrato_vinculante]
		,[tiene_contrato_vinculante]
		,[estado_registro]
		,[fecha_registra]
		,[usuario_registra]
		,[fecha_modifica]
		,[usuario_modifica]
		,[codigo_tipo_articulo]
		,[cantidad_unica]
	FROM
		dbo.articulo
	WHERE
		codigo_articulo = @p_codigo_articulo

	SET NOCOUNT OFF
END;