USE SIGECO
GO

IF OBJECT_ID('dbo.regla_bono_trimestral_detalle', 'U') IS NOT NULL 
	DROP TABLE dbo.regla_bono_trimestral_detalle; 
GO

CREATE TABLE dbo.regla_bono_trimestral_detalle
(
	codigo_regla_detalle int identity(1,1),
	codigo_regla int,
	codigo_canal int,
	codigo_empresa varchar(400),
	codigo_tipo_venta varchar(400),
	estado_registro bit,
	usuario_registra varchar(25) not null,
	fecha_registra datetime not null,
	usuario_modifica varchar(25),
	fecha_modifica datetime,
	CONSTRAINT [PK_regla_bono_trimestral_detalle] PRIMARY KEY CLUSTERED ([codigo_regla_detalle] ASC),
	CONSTRAINT [FK_regla_bono_trimestral_detalle-regla_bono_trimestral] FOREIGN KEY ([codigo_regla]) REFERENCES dbo.regla_bono_trimestral([codigo_regla])
)
