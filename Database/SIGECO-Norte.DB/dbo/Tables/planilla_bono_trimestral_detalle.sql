USE SIGECO
GO

IF OBJECT_ID('dbo.planilla_bono_trimestral_detalle', 'U') IS NOT NULL 
	DROP TABLE dbo.planilla_bono_trimestral_detalle; 
GO

CREATE TABLE dbo.planilla_bono_trimestral_detalle
(
	codigo_planilla_detalle int identity(1, 1),
	codigo_planilla int,
	codigo_empresa int,
	nombre_empresa varchar(50),
	nombre_empresa_largo varchar(250),
	direccion_fiscal_empresa varchar(250),
	ruc_empresa varchar(25),
	codigo_canal_grupo int,
	codigo_canal int,
	nombre_canal varchar(250),
	codigo_grupo int,
	nombre_grupo varchar(250),
	codigo_personal int,
	nombre_personal varchar(500),
	documento_personal varchar(50),
	codigo_personal_j varchar(50),
	codigo_supervisor int,
	correo_supervisor varchar(250),
	nombre_supervisor varchar(500),
	monto_contratado decimal(12, 2),
	unidad_venta int,
	rango int,
	monto_bono decimal(12, 2),
	monto_sin_igv decimal(12, 2),
	monto_igv decimal(12, 2),
	monto_bono_letras varchar(500),
	concepto_liquidacion varchar(250),
	monto_bono_grupo decimal(12, 2),
	monto_bono_grupo_sin_igv decimal(12, 2),
	monto_bono_grupo_igv decimal(12, 2),
	monto_bono_canal decimal(12, 2),
	monto_bono_canal_sin_igv decimal(12, 2),
	monto_bono_canal_igv decimal(12, 2),
	monto_bono_empresa decimal(12, 2),
	monto_bono_empresa_sin_igv decimal(12, 2),
	monto_bono_empresa_igv decimal(12, 2),
	CONSTRAINT [PK_planilla_bono_trimestral_detalle] PRIMARY KEY CLUSTERED ([codigo_planilla_detalle] ASC),
	CONSTRAINT [FK_planilla_bono_trimestral_detalle-planilla_bono_trimestral] FOREIGN KEY ([codigo_planilla]) REFERENCES dbo.planilla_bono_trimestral([codigo_planilla])
)
