USE SIGECO
GO

IF OBJECT_ID('dbo.sigeco_reporte_bono_trimestral_rrhh', 'U') IS NOT NULL 
	DROP TABLE dbo.sigeco_reporte_bono_trimestral_rrhh; 
GO

CREATE TABLE [dbo].[sigeco_reporte_bono_trimestral_rrhh](
	[codigo_planilla] [int] NULL,
	[numero_planilla] [varchar](10) NULL,
	[fecha_proceso] [varchar](10) NULL,
	[codigo_empresa] [int] NULL,
	[simbolo_moneda_cuenta_desembolso] [varchar](25) NULL,
	[nombre_empresa] [varchar](25) NULL,
	[numero_cuenta_desembolso] [varchar](250) NULL,
	[tipo_cuenta_desembolso] [varchar](25) NULL,
	[numero_cuenta_abono] [varchar](250) NULL,
	[tipo_cuenta_abono] [varchar](25) NULL,
	[simbolo_moneda_cuenta_abono] [varchar](25) NULL,
	[nombre_tipo_documento] [varchar](25) NULL,
	[nro_documento] [varchar](25) NULL,
	[nombre_personal] [varchar](250) NULL,
	[codigo_personal] [int] NULL,
	[importe_abono_personal] [decimal](12, 2) NULL,
	[importe_desembolso_empresa] [decimal](12, 2) NULL,
	[calcular_detraccion] [bit] NULL,
	[checksum] [varchar](250) NULL,
	[codigo_grupo] [int] NULL,
	[validado] [bit] DEFAULT(0)
) ON [PRIMARY]
GO
