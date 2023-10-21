CREATE TABLE dbo.template_correo
(
	codigo_template		INT
	,nombre				VARCHAR(250)
	,usuario_registra	VARCHAR(25)
	,fecha_registra		DATETIME
	,CONSTRAINT [PK_template_correo] PRIMARY KEY CLUSTERED ([codigo_template] ASC)
)
