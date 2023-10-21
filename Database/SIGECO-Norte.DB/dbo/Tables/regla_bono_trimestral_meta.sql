USE SIGECO
GO

IF OBJECT_ID('dbo.regla_bono_trimestral_meta', 'U') IS NOT NULL 
	DROP TABLE dbo.regla_bono_trimestral_meta; 
GO

create table dbo.regla_bono_trimestral_meta
(
	codigo_meta			int identity(1, 1),
	codigo_regla		int,
	rango_inicio		int,
	rango_fin			int,
	monto				decimal(10, 2),
	estado_registro		bit,
	usuario_registra	varchar(25),
	fecha_registra		datetime,
	usuario_modifica	varchar(25),
	fecha_modifica		datetime,
	CONSTRAINT [PK_regla_bono_trimestral_meta] PRIMARY KEY CLUSTERED ([codigo_meta] ASC),
	CONSTRAINT [FK_regla_bono_trimestral_meta-regla_bono_trimestral] FOREIGN KEY ([codigo_regla]) REFERENCES dbo.regla_bono_trimestral([codigo_regla])
)
