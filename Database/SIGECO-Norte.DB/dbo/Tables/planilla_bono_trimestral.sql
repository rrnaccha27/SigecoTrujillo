USE SIGECO
GO

IF OBJECT_ID('dbo.planilla_bono_trimestral', 'U') IS NOT NULL 
	DROP TABLE dbo.planilla_bono_trimestral; 
GO

CREATE TABLE dbo.planilla_bono_trimestral
(
	codigo_planilla int identity(1, 1),
	numero_planilla varchar(10),
	codigo_regla int,
	codigo_tipo_bono int,
	codigo_periodo int,
	anio_periodo int,
	codigo_estado_planilla int,

	usuario_apertura varchar(25),
	fecha_apertura datetime,
	usuario_cierre varchar(25),
	fecha_cierre datetime,
	usuario_anulacion varchar(25),
	fecha_anulacion datetime,

	fecha_registra datetime,
	usuario_registra varchar(25),
	fecha_modifica datetime,
	usuario_modifica varchar(25),

	CONSTRAINT [PK_planilla_bono_trimestral] PRIMARY KEY CLUSTERED ([codigo_planilla] ASC),
	CONSTRAINT [FK_planilla_bono_trimestral-tipo_bono_trimestral] FOREIGN KEY ([codigo_tipo_bono]) REFERENCES dbo.tipo_bono_trimestral([codigo_tipo_bono]),
	CONSTRAINT [FK_planilla_bono_trimestral-periodo_trimestral] FOREIGN KEY ([codigo_periodo]) REFERENCES dbo.periodo_trimestral([codigo_periodo]),
	CONSTRAINT [FK_planilla_bono_trimestral-regla_bono_trimestral] FOREIGN KEY ([codigo_regla]) REFERENCES dbo.regla_bono_trimestral([codigo_regla])
)
