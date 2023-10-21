USE SIGECO
GO

IF OBJECT_ID('dbo.checklist_bono_trimestral', 'U') IS NOT NULL 
	DROP TABLE dbo.checklist_bono_trimestral; 
GO

CREATE TABLE dbo.checklist_bono_trimestral
(
	codigo_checklist			int	identity(1, 1),
	numero_checklist			varchar(25),
	codigo_estado_checklist		int,
	codigo_planilla				int,
	estado_registro				bit,
	fecha_registra				datetime,
	usuario_registra			varchar(25),
	fecha_modifica				datetime,
	usuario_modifica			varchar(25),
	fecha_cierre				datetime,
	usuario_cierre				varchar(25),
	fecha_anulacion				datetime,
	usuario_anulacion			varchar(25),
	CONSTRAINT [PK_checklist_bono_trimestral] PRIMARY KEY CLUSTERED ([codigo_checklist] ASC)
)
