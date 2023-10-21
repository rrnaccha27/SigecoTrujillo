USE SIGECO
GO

IF OBJECT_ID('dbo.periodo_trimestral', 'U') IS NOT NULL 
	DROP TABLE dbo.periodo_trimestral; 
GO

CREATE TABLE dbo.periodo_trimestral
(
	codigo_periodo int identity(1, 1), 
	nombre varchar(250),
	rango varchar(250),
	estado_registro bit,
	fecha_registra datetime,
	usuario_registra varchar(25),
	fecha_modifica datetime,
	usuario_modifica varchar(25),
	CONSTRAINT [PK_periodo_trimestral] PRIMARY KEY CLUSTERED ([codigo_periodo] ASC)
)
