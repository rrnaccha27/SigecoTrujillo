CREATE PROCEDURE dbo.up_regla_calculo_bono_unique
(
	 @p_codigo_regla_calculo_bono	INT
)
AS
BEGIN

	SELECT
		 TOP 1
		 codigo_regla_calculo_bono
		,codigo_tipo_planilla
		,codigo_canal
		,codigo_grupo
		,CONVERT(VARCHAR(8), vigencia_inicio, 112) as vigencia_inicio
		,CONVERT(VARCHAR(8), vigencia_fin, 112) as vigencia_fin
		,monto_meta
		,porcentaje_pago
		,monto_tope
		,cantidad_ventas
		,estado_registro
		,calcular_igv
	FROM
		dbo.regla_calculo_bono rcb
	WHERE
		rcb.estado_registro = 1
		AND codigo_regla_calculo_bono = @p_codigo_regla_calculo_bono

END;