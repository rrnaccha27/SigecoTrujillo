USE SIGECO
GO

IF OBJECT_ID('dbo.sigeco_reporte_bono_jn_liquidacion', 'U') IS NOT NULL 
	DROP TABLE dbo.sigeco_reporte_bono_jn_liquidacion; 
GO

CREATE TABLE dbo.sigeco_reporte_bono_jn_liquidacion
(
	codigo_planilla	INT
	,codigo_empresa	INT
	,nombre_empresa VARCHAR(250)
	,nombre_empresa_largo	VARCHAR(250)
	,direccion_fiscal	VARCHAR(250)
	,ruc_empresa	VARCHAR(250)
	,nombre_grupo	VARCHAR(250)
	,codigo_personal	INT
	,nombre_personal	VARCHAR(250)
	,documento_personal	VARCHAR(250)
	,monto_bono	DECIMAL(12, 2)
	,monto_sin_igv	DECIMAL(12, 2)
	,monto_igv	DECIMAL(12, 2)
	,monto_bono_letras VARCHAR(500)
	,concepto_liquidacion VARCHAR(250)
	,codigo_estado_planilla INT
	,dinero_ingresado	DECIMAL(12, 2)
)