CREATE PROCEDURE up_detalle_regla_tipo_planilla_insertar
(
	@codigo_regla_tipo_planilla int,
	@codigo_canal int,
	@codigo_empresa  nvarchar(400),
	@codigo_tipo_venta  nvarchar(400),
	@codigo_campo_santo  nvarchar(400),
	@usuario_registra varchar(50),
	@p_codigo_detalle__regla_tipo_planilla int out
)
AS
BEGIN
	INSERT INTO detalle_regla_tipo_planilla(
		codigo_regla_tipo_planilla,
		codigo_canal,
		codigo_empresa,
		codigo_tipo_venta,
		codigo_campo_santo,
		usuario_registra,
		estado_registro,
		fecha_registra
	)
	VALUES(
		@codigo_regla_tipo_planilla,
		@codigo_canal,
		@codigo_empresa,
		@codigo_tipo_venta,
		@codigo_campo_santo,
		@usuario_registra,
		1,
		GETDATE()
	);

	set @p_codigo_detalle__regla_tipo_planilla=@@IDENTITY;
END;