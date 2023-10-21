CREATE PROCEDURE dbo.up_proceso_generacion_bono_ceros
(
	@p_codigo_planilla_bono	INT 
)
AS
BEGIN
	SET NOCOUNT ON

	delete from dbo.contrato_planilla_bono where (monto_contratado between -99.95 and 0.95) and (monto_ingresado  between -99.95 and 0.95) and codigo_planilla = @p_codigo_planilla_bono
	delete from dbo.articulo_planilla_bono where (monto_contratado between -99.95 and 0.95) and (dinero_ingresado between -99.95 and 0.95) and codigo_planilla_bono = @p_codigo_planilla_bono

	SET NOCOUNT OFF

END