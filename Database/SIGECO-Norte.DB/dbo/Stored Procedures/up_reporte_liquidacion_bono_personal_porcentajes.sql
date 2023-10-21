CREATE PROCEDURE [dbo].[up_reporte_liquidacion_bono_personal_porcentajes]
(
	@p_codigo_planilla	int
)
AS
BEGIN
	SET NOCOUNT ON

	DECLARE
		@v_fecha_apertura				DATETIME
		,@v_codigo_tipo_planilla		INT
		,@v_codigo_regla_calculo_bono	INT
	
	SELECT TOP 1 
		@v_fecha_apertura = fecha_apertura 
		,@v_codigo_tipo_planilla = codigo_tipo_planilla
	FROM 
		dbo.planilla_bono 
	WHERE 
		codigo_planilla = @p_codigo_planilla
	
	SET @v_codigo_regla_calculo_bono = (SELECT TOP 1 codigo_regla_calculo_bono FROM dbo.regla_calculo_bono WHERE codigo_tipo_planilla = @v_codigo_tipo_planilla AND @v_fecha_apertura BETWEEN vigencia_inicio AND vigencia_fin ORDER BY estado_registro DESC)

	DECLARE 
		@v_mensaje_1 varchar(500)
		,@v_mensaje_2 varchar(500)
		,@v_mensaje_3 varchar(500)
		,@v_valor_maximo decimal(10, 2)= 0

	select 
		@v_mensaje_1 = 'META POR MONTO CONTRATADO => S/ ' + CONVERT(VARCHAR, FORMAT(monto_meta, '#,0.00')) + ' SIN IGV.', 
		@v_mensaje_3 = 'EL TOPE MAXIMO DE BONO SERA DE S/ ' + CONVERT(VARCHAR, FORMAT(monto_tope, '#,0.00')) + ' SIN IGV.' + CASE WHEN ISNULL(cantidad_ventas,0) >0 THEN ' SE LLEGA AL BONO A PARTIR DE ' + CONVERT(VARCHAR, cantidad_ventas) + ' VENTAS.' ELSE '' END,
		@v_mensaje_2 = '100% => ' + CONVERT(VARCHAR, porcentaje_pago) + '%',
		@v_valor_maximo = 100
	from 
		dbo.regla_calculo_bono
	where
		codigo_regla_calculo_bono = @v_codigo_regla_calculo_bono

	DECLARE @t_porcentajes table(
		id int identity(1, 1)
		,porcentaje_meta decimal(10,2)
		,texto varchar(500)
	)

	insert into @t_porcentajes
	select 
		porcentaje_meta, '- ' + CONVERT(VARCHAR, porcentaje_meta) + '%  => ' + CONVERT(VARCHAR, porcentaje_pago) + '%' 
	from 
		dbo.regla_calculo_bono_matriz
	where
		codigo_regla_calculo_bono = @v_codigo_regla_calculo_bono 
	order by
		porcentaje_meta desc

	declare 
		@v_indice int = 1
		,@v_total int = (select max(id) from @t_porcentajes)
		,@v_texto varchar(500)

	while (@v_indice <= @v_total)
	begin
		set @v_valor_maximo = @v_valor_maximo - 1
		set @v_mensaje_2 = @v_mensaje_2 + '       ' +convert(varchar, @v_valor_maximo) + '% '
	
		select 
			@v_valor_maximo = porcentaje_meta 
			,@v_texto = texto 
		from
			@t_porcentajes
		where id = @v_indice

		set @v_mensaje_2 = @v_mensaje_2 + @v_texto

		set @v_indice = @v_indice + 1
	end

	select top 1 
		@v_mensaje_2 = @v_mensaje_2 + '      MENOS ' + CONVERT(VARCHAR, porcentaje_meta) + '%  NO HAY BONO.' 
	from
		dbo.regla_calculo_bono_matriz
	where
		codigo_regla_calculo_bono = 3 
	order by
		porcentaje_meta asc

	select @p_codigo_planilla as codigo_planilla, @v_mensaje_1 as mensaje_1, @v_mensaje_2 as mensaje_2, @v_mensaje_3 as mensaje_3

	SET NOCOUNT OFF
END;