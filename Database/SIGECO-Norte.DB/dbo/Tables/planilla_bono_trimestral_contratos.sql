USE SIGECO
GO

IF OBJECT_ID('dbo.planilla_bono_trimestral_contratos', 'U') IS NOT NULL 
	DROP TABLE dbo.planilla_bono_trimestral_contratos; 
GO

CREATE TABLE dbo.planilla_bono_trimestral_contratos
(
	codigo_planilla int,
	codigo_personal int,
	codigo_empresa int,
	numero_contrato varchar(100),
	monto_contratado decimal(12, 2),
	estado_registro bit,
	observacion varchar(250)
	CONSTRAINT [FK_planilla_bono_trimestral_contratos] FOREIGN KEY ([codigo_planilla]) REFERENCES dbo.planilla_bono_trimestral([codigo_planilla]),
	CONSTRAINT [FK_planilla_bono_trimestral_contratos-personal] FOREIGN KEY ([codigo_personal]) REFERENCES dbo.personal([codigo_personal]),
	CONSTRAINT [FK_planilla_bono_trimestral_contratos-empresa_sigeco] FOREIGN KEY ([codigo_empresa]) REFERENCES dbo.empresa_sigeco([codigo_empresa])
)
