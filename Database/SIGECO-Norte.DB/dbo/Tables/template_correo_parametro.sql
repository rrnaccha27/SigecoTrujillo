CREATE TABLE dbo.template_correo_parametro
(
	codigo_template		VARCHAR(250)
	,indice				INT
	,parametro			VARCHAR(250)
	,usuario_registra	VARCHAR(25)
	,fecha_registra		DATETIME
	,CONSTRAINT [PK_template_correo_parametro] PRIMARY KEY CLUSTERED ([codigo_template] ASC, [indice] ASC)
)
