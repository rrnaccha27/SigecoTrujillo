CREATE FUNCTION [dbo].[GetCanalGrupoDes]
(
	@codigo_canal_grupo	int,
	@Clase	char(1)=null
)
RETURNS varchar(250)
AS
BEGIN
	declare @Descripcion varchar(250)
	DECLARE @Canal VARCHAR(250), @Grupo VARCHAR(250)
	DECLARE @codigo_padre	INT
	DECLARE @es_canal_grupo	BIT
		
	SET @Canal=''
	SET @Grupo=''
	SET @codigo_padre=0
	
	SELECT @es_canal_grupo=es_canal_grupo,@codigo_padre=ISNULL(codigo_padre,0) FROM canal_grupo
	WHERE codigo_canal_grupo = @codigo_canal_grupo

	IF @codigo_padre<=0
	BEGIN
		SELECT @Canal=ISNULL(nombre,'') FROM canal_grupo
		WHERE codigo_canal_grupo = @codigo_canal_grupo
	END
	ELSE
	BEGIN
		SELECT @Canal=ISNULL(nombre,'') FROM canal_grupo
		WHERE codigo_canal_grupo = @codigo_padre

		SELECT @Grupo=ISNULL(nombre,'') FROM canal_grupo
		WHERE codigo_canal_grupo = @codigo_canal_grupo
	END

	IF @Clase='C'
		SET @Descripcion=@Canal
	ELSE
		SET @Descripcion=@Grupo
	
	return isnull(@Descripcion,'')
END