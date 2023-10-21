CREATE FUNCTION dbo.fn_SplitReporteGeneral
(
 @Str           NVARCHAR(MAX)
)
RETURNS  @ResultOutput table(codigo int)
AS
BEGIN
	declare @Splitted_string nvarchar(4000),@Separator char(1),@Pos int, @NextPos int;

	set @Separator=',';
	set @Str = @Str + @Separator
	set @Pos = CHARINDEX(@Separator, @Str)

	while (@pos <> 0)
	begin
		set @Splitted_string = SUBSTRING(@Str, 1, @Pos - 1)
		set @Str = SUBSTRING(@Str, @pos + 1, LEN(@Str))
		set @pos = CHARINDEX(@Separator, @Str)

		insert into @ResultOutput
		values
		(
			@Splitted_string
		)
	end
	return
END;