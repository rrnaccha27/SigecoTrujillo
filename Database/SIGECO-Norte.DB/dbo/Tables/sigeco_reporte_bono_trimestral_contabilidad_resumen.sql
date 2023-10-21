USE SIGECO
GO

IF OBJECT_ID('dbo.sigeco_reporte_bono_trimestral_contabilidad_resumen', 'U') IS NOT NULL 
	DROP TABLE dbo.sigeco_reporte_bono_trimestral_contabilidad_resumen; 
GO

CREATE TABLE [dbo].[sigeco_reporte_bono_trimestral_contabilidad_resumen](
	[codigo_planilla] [int] NULL,
	[codigo_empresa] [int] NULL,
	[nombre_empresa] [varchar](25) NULL,
	[bonos] [int] NULL
) ON [PRIMARY]
