USE SIGECO
GO

CREATE VIEW dbo.pcb_regla_calculo_bono_matriz
AS
	
	SELECT 
		codigo_regla_calculo_bono
		,porcentaje_meta
		,monto_meta
		,(porcentaje_pago / 100) as porcentaje_pago
	FROM regla_calculo_bono_matriz
