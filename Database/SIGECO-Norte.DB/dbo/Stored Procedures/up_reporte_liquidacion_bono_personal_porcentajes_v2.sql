CREATE PROCEDURE [dbo].[up_reporte_liquidacion_bono_personal_porcentajes_v2]
(
	 @p_codigo_planilla	INT
)
AS
BEGIN
	SET NOCOUNT ON

	IF EXISTS(SELECT codigo_planilla FROM sigeco_reporte_bono_porcentajes WHERE codigo_planilla = @p_codigo_planilla)
	BEGIN
		SELECT 
			codigo_planilla,mensaje_1,mensaje_2,mensaje_3
		FROM 
			sigeco_reporte_bono_porcentajes 
		WHERE 
			codigo_planilla = @p_codigo_planilla

		SELECT 'CONGELADO'
	END
	ELSE
	BEGIN
		exec up_reporte_liquidacion_bono_personal_porcentajes @p_codigo_planilla
		select 'en vivo'
	END	
	SET NOCOUNT OFF
END