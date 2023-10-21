CREATE PROCEDURE dbo.up_descuento_comision_validar_planilla
(
	@p_codigo_planilla	INT
	,@p_cantidad		INT OUTPUT
)
AS
BEGIN
	SET NOCOUNT ON

	SELECT
		@p_cantidad = COUNT(dc.codigo_descuento_comision)
	FROM
		dbo.descuento_comision dc
	WHERE 
		dc.estado_registro = 1
		AND dc.saldo > 0
		AND EXISTS(
			SELECT 
				dp.codigo_detalle_planilla 
			FROM 
				dbo.detalle_planilla dp 
			WHERE 
				dp.codigo_planilla = @p_codigo_planilla 
				AND dp.codigo_personal = dc.codigo_personal 
				AND dp.codigo_empresa = dc.codigo_empresa
				AND dp.excluido = 0
				AND dp.estado_registro = 1
		)
		AND NOT EXISTS(SELECT TOP 1 d.codigo_descuento FROM dbo.descuento d inner join dbo.planilla p on p.codigo_planilla = @p_codigo_planilla and p.codigo_planilla = d.codigo_planilla WHERE d.codigo_descuento_comision = dc.codigo_descuento_comision AND d.estado_registro = 1)

	SET NOCOUNT OFF
END;