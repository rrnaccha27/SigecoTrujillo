CREATE procedure dbo.up_regla_recupero_insertar
(
	 @p_nombre					VARCHAR(250)
	,@p_nro_cuota				INT
	,@p_vigencia_inicio			date
	,@p_vigencia_fin			date
	,@p_usuario_registra		VARCHAR(50)
	,@p_codigo_regla_recupero	INT OUTPUT
)
AS
BEGIN
	declare @v_existe_registro int;

	select 
		@v_existe_registro=count(*) 	 
		from regla_recupero 
		where estado_registro=1 and 

		 (
			@p_vigencia_inicio between vigencia_inicio AND vigencia_fin
			OR @p_vigencia_fin between vigencia_inicio AND vigencia_fin
			OR vigencia_inicio between @p_vigencia_inicio AND @p_vigencia_fin
			OR vigencia_fin between @p_vigencia_inicio AND @p_vigencia_fin)



	if @v_existe_registro>0
	 begin
		raiserror('Rango de fechas ingresadas ya existe.',16,1);
		return;
	 end;



	INSERT INTO
		dbo.regla_recupero
	(
		 nombre
		,nro_cuota
		,vigencia_inicio
		,vigencia_fin
		,estado_registro
		,fecha_registra
		,usuario_registra
	)
	VALUES
	(
		 @p_nombre
		,@p_nro_cuota
		,@p_vigencia_inicio
		,@p_vigencia_fin
		,1
		,GETDATE()
		,@p_usuario_registra
	)
	SET @p_codigo_regla_recupero = @@IDENTITY

END