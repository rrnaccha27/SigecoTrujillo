CREATE FUNCTION dbo.fn_split_parametro_sistema
(
	@p_cadena	VARCHAR(MAX)
)
RETURNS @t_resultado TABLE(codigo int, valor varchar(100))
AS
BEGIN

	DECLARE 
		@v_cadena_separada	varchar(max)
		,@Pos				int
		,@NextPos			int
		,@v_separador		CHAR(1) = ','
		,@v_separadordata	CHAR(1) = '|'
	
	DECLARE 
		@t_temporal TABLE(codigo int)

	set @p_cadena = @p_cadena + @v_separador
	set @Pos = CHARINDEX(@v_separador, @p_cadena)

	while (@pos <> 0)
	begin
		set @v_cadena_separada = SUBSTRING(@p_cadena, 1, @Pos - 1)
		set @p_cadena = SUBSTRING(@p_cadena, @pos + 1, LEN(@p_cadena))
		set @pos = CHARINDEX(@v_separador, @p_cadena)
		insert into @t_temporal
		values
		(
			@v_cadena_separada
		)
	end

	insert into @t_resultado
	select
		SUBSTRING(valor, 1, (CHARINDEX(@v_separadordata, valor)) - 1)
		,SUBSTRING(valor, (CHARINDEX(@v_separadordata, valor)) + 1, LEN(valor))
	from
		parametro_sistema p
	inner join @t_temporal t
		on t.codigo = p.codigo_parametro_sistema

	RETURN
END;