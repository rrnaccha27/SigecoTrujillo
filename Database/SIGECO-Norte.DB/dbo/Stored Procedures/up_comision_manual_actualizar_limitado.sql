CREATE PROC dbo.up_comision_manual_actualizar_limitado
(
	@p_codigo_comision_manual	int
	,@p_usuario_modifica		varchar(50)
	,@p_nro_factura_vendedor	varchar(20)
)
AS
BEGIN
	UPDATE
		dbo.comision_manual
	SET
		fecha_modifica = GETDATE(),
		usuario_modifica = @p_usuario_modifica,
		nro_factura_vendedor = @p_nro_factura_vendedor
	WHERE
		codigo_comision_manual = @p_codigo_comision_manual
END;