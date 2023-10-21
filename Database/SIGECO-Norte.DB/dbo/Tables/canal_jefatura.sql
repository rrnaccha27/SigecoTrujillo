CREATE TABLE dbo.canal_jefatura
(
	codigo_canal		INT 
	,email				VARCHAR(500)
	,email_copia		VARCHAR(500)
	,usuario_registra	VARCHAR(25)
	,fecha_registra		DATETIME
	,CONSTRAINT [PK_canal_jefatura] PRIMARY KEY CLUSTERED ([codigo_canal] ASC)
)
GO