CREATE PROC dbo.up_comision_manual_actualizar
(
	@p_codigo_comision_manual	int ,
	@p_codigo_tipo_documento	int ,
	@p_nro_documento			varchar(15) ,
	@p_nombre					varchar(50) ,
	@p_apellido_paterno			varchar(50) ,
	@p_apellido_materno			varchar(50) ,
	@p_codigo_personal			int	,
	@p_codigo_canal				int ,
	@p_codigo_empresa			int	,
	@p_nro_contrato				varchar(100) ,
	@p_codigo_articulo			int ,
	@p_comentario				varchar(100) ,
	@p_codigo_tipo_venta		int ,
	@p_codigo_tipo_pago			int ,
	@p_comision_sin_igv			decimal(10, 2) ,
	@p_igv						decimal(10, 2) ,
	@p_comision					decimal(10, 2) ,
	@p_usuario_modifica			varchar(50),
	@p_nro_factura_vendedor		varchar(20),
	@p_retorno					varchar(200) OUTPUT
)
AS
BEGIN
	BEGIN TRAN COMISION_MANUAL

	UPDATE
		dbo.comision_manual
	SET
		codigo_tipo_documento = @p_codigo_tipo_documento,
		nro_documento = @p_nro_documento,
		nombre = @p_nombre,
		apellido_paterno = @p_apellido_paterno,
		apellido_materno = @p_apellido_materno,
		codigo_personal = @p_codigo_personal,
		codigo_canal = @p_codigo_canal,
		codigo_empresa = @p_codigo_empresa,
		nro_contrato = @p_nro_contrato,
		codigo_articulo = @p_codigo_articulo,
		comentario = @p_comentario,
		codigo_tipo_venta = @p_codigo_tipo_venta,
		codigo_tipo_pago = @p_codigo_tipo_pago,
		comision_sin_igv = @p_comision_sin_igv,
		igv = @p_igv,
		comision = @p_comision,
		fecha_modifica = GETDATE(),
		usuario_modifica = @p_usuario_modifica,
		nro_factura_vendedor = @p_nro_factura_vendedor
	WHERE
		codigo_comision_manual = @p_codigo_comision_manual

	EXEC up_comision_manual_procesar @p_codigo_comision_manual, @p_retorno OUTPUT

	IF LEN(@p_retorno) > 0 
		ROLLBACK TRAN COMISION_MANUAL
	ELSE
		COMMIT TRAN COMISION_MANUAL

END;