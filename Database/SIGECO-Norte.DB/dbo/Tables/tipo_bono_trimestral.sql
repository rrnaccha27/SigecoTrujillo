USE SIGECO
GO

IF OBJECT_ID('dbo.tipo_bono_trimestral', 'U') IS NOT NULL 
	DROP TABLE dbo.tipo_bono_trimestral; 
GO

CREATE TABLE dbo.tipo_bono_trimestral
(
	codigo_tipo_bono int identity(1, 1), 
	nombre varchar(250),
	estado_registro bit,
	fecha_registra datetime,
	usuario_registra varchar(25),
	fecha_modifica datetime,
	usuario_modifica varchar(25),
	CONSTRAINT [PK_tipo_bono_trimestral] PRIMARY KEY CLUSTERED ([codigo_tipo_bono] ASC)
)
