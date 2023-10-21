USE SIGECO
GO

IF OBJECT_ID('dbo.checklist_bono_trimestral_detalle', 'U') IS NOT NULL 
	DROP TABLE dbo.checklist_bono_trimestral_detalle; 
GO

CREATE TABLE dbo.checklist_bono_trimestral_detalle
(
	codigo_checklist_detalle	int	identity(1, 1),
	codigo_checklist			int,
	codigo_planilla				int,
	numero_planilla				varchar(25),
	codigo_personal				int,
	codigo_empresa				int,
	nombre_empresa				varchar(25),
	codigo_grupo				int,
	nombre_grupo				varchar(250),
	validado					bit,
	estado_registro				bit,
	fecha_registra				datetime,
	usuario_registra			varchar(25),
	fecha_modifica				datetime,
	usuario_modifica			varchar(25),
	CONSTRAINT [PK_checklist_bono_trimestral_detalle] PRIMARY KEY CLUSTERED ([codigo_checklist_detalle] ASC)
)
