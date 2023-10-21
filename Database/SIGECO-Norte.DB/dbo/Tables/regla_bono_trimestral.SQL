USE SIGECO
GO

IF OBJECT_ID('dbo.regla_bono_trimestral', 'U') IS NOT NULL 
	DROP TABLE dbo.regla_bono_trimestral; 
GO

CREATE TABLE dbo.regla_bono_trimestral
(
	codigo_regla int identity(1, 1) not null,
	descripcion  varchar(250) not null,
	codigo_tipo_bono int not null,
	vigencia_inicio datetime not null,
	vigencia_fin datetime not null,
	estado_registro bit not null,
	usuario_registra varchar(25) not null,
	fecha_registra datetime not null,
	usuario_modifica varchar(25),
	fecha_modifica datetime,
	CONSTRAINT [PK_regla_bono_trimestral] PRIMARY KEY CLUSTERED ([codigo_regla] ASC),
	CONSTRAINT [FK_regla_bono_trimestral-tipo_bono_trimestral] FOREIGN KEY ([codigo_tipo_bono]) REFERENCES dbo.tipo_bono_trimestral([codigo_tipo_bono])
)
