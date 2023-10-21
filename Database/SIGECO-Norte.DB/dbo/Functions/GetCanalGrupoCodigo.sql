CREATE FUNCTION [dbo].[GetCanalGrupoCodigo]
(
	@codigo_canal_grupo	int=null
	,@Clase	char(1)=null
)
RETURNS INT
AS
BEGIN
	declare @Codigo INT
	DECLARE @CanalCod INT, @GrupoCod INT
	DECLARE @codigo_padre	INT
	DECLARE @es_canal_grupo	BIT
		
	SET @CanalCod=null
	SET @GrupoCod=null
	SET @codigo_padre=-1
	
	SELECT @es_canal_grupo=es_canal_grupo,@codigo_padre=ISNULL(codigo_padre,null) FROM canal_grupo
	WHERE codigo_canal_grupo = @codigo_canal_grupo

	--IF @codigo_padre<=0
	--BEGIN
		IF @codigo_padre is null OR @codigo_padre<=0
		BEGIN
			SET @CanalCod = @codigo_canal_grupo
		END
		ELSE
		BEGIN
			SET @CanalCod = @codigo_padre
			SET @GrupoCod = @codigo_canal_grupo

		END
	--END

	IF @Clase='C'
		SET @Codigo=@CanalCod
	ELSE
		SET @Codigo=@GrupoCod
	
	return @Codigo
END