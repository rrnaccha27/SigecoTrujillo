CREATE PROCEDURE dbo.up_descuento_comision_desactivar
(
	@p_codigo_descuento_comision	INT
	,@p_usuario_modifica			VARCHAR(50)
)
AS
BEGIN
	SET NOCOUNT ON

	DECLARE 
		@v_validacion INT = 0
	
	SELECT
		@v_validacion = COUNT(dc.codigo_descuento_comision)		
	FROM
		dbo.descuento_comision dc
	INNER JOIN 
		dbo.descuento d ON d.estado_registro = 1 AND d.codigo_descuento_comision = dc.codigo_descuento_comision
	INNER JOIN
		dbo.planilla p ON p.codigo_planilla = d.codigo_planilla and p.codigo_estado_planilla <> 3
	WHERE
		dc.estado_registro = 1
		AND dc.codigo_descuento_comision = @p_codigo_descuento_comision

	IF (@v_validacion > 0)
	BEGIN
		RAISERROR('El descuento ya tiene aplicaciones en planilla. Ver en Detalle.', 16, 1); 
		RETURN;
	END

	UPDATE
		dbo.descuento_comision
	SET
		estado_registro = 0
		,usuario_modifica = @p_usuario_modifica
		,fecha_modifica = GETDATE()
	WHERE
		codigo_descuento_comision = @p_codigo_descuento_comision

	SET NOCOUNT OFF
END;