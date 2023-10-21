CREATE PROC dbo.up_comision_manual_insertar
(
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
	@p_nro_factura_vendedor		varchar(20),
	@p_comision_sin_igv			decimal(10, 2) ,
	@p_igv						decimal(10, 2) ,
	@p_comision					decimal(10, 2) ,
	@p_usuario_registra			varchar(50),
	@p_retorno					varchar(200) OUTPUT
)
AS
BEGIN
	BEGIN TRAN COMISION_MANUAL
	
	DECLARE
		@v_codigo_comision_manual	int

	INSERT INTO
		dbo.comision_manual
	(
		codigo_estado_cuota,
		codigo_estado_proceso,
		codigo_tipo_documento,
		nro_documento,
		nombre,
		apellido_paterno,
		apellido_materno,
		codigo_personal,
		codigo_canal,
		codigo_empresa,
		nro_contrato,
		codigo_articulo,
		comentario,
		codigo_tipo_venta,
		codigo_tipo_pago,
		comision_sin_igv,
		igv,
		comision,
		estado_registro,
		fecha_registra,
		usuario_registra,
		nro_factura_vendedor
	)
	VALUES
	(
		3,
		1,
		@p_codigo_tipo_documento,
		@p_nro_documento,
		@p_nombre,
		@p_apellido_paterno,
		@p_apellido_materno,
		@p_codigo_personal,
		@p_codigo_canal,
		@p_codigo_empresa,
		@p_nro_contrato,
		@p_codigo_articulo,
		@p_comentario,
		@p_codigo_tipo_venta,
		@p_codigo_tipo_pago,
		@p_comision_sin_igv,
		@p_igv,
		@p_comision,
		1,
		GETDATE(),
		@p_usuario_registra,
		@p_nro_factura_vendedor
	)
	SET @v_codigo_comision_manual = SCOPE_IDENTITY()

	EXEC up_comision_manual_procesar @v_codigo_comision_manual, @p_retorno OUTPUT

	IF LEN(@p_retorno) > 0 
		ROLLBACK TRAN COMISION_MANUAL
	ELSE
		COMMIT TRAN COMISION_MANUAL

END;