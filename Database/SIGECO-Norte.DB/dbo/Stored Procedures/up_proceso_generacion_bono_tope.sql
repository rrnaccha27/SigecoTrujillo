CREATE PROCEDURE [dbo].[up_proceso_generacion_bono_tope]
(
	 @p_codigo_planilla			int
	,@p_codigo_tipo_planilla	int
)
AS
BEGIN
	SET NOCOUNT ON

	declare @t_tope table(
		indice int identity(1, 1)
		,codigo_personal int
		,codigo_moneda int
		,codigo_canal int
		,codigo_grupo int
		,monto_bruto decimal(12, 4)
		,descontar bit
	)

	declare 
		@v_monto_tope decimal(10, 2)
		,@v_indice	int
		,@v_total_registros	int
		,@v_codigo_personal	int
		,@v_codigo_canal	int
		,@v_codigo_grupo	int
		,@v_monto_bruto	decimal(10, 2)

	insert into @t_tope
	select  
		codigo_personal
		,codigo_moneda
		,codigo_canal
		,codigo_grupo
		,sum(monto_bruto) as monto_bruto
		,0 as descontar
	from detalle_planilla_bono
	where codigo_planilla = @p_codigo_planilla
	group by codigo_personal, codigo_moneda, codigo_canal, codigo_grupo
		
	SET @v_indice = 1
	SET @v_total_registros = (SELECT COUNT(indice) FROM @t_tope)

	WHILE (@v_indice <= @v_total_registros)
	BEGIN
		SELECT
			@v_codigo_personal = codigo_personal
			,@v_codigo_canal = codigo_canal
			,@v_codigo_grupo = codigo_grupo
			,@v_monto_bruto = monto_bruto
		FROM
			@t_tope
		WHERE
			indice = @v_indice

		select top 1
			@v_monto_tope = monto_tope
		from 
			dbo.pcb_regla_calculo_bono
		where 
			codigo_canal = @v_codigo_canal
			and codigo_tipo_planilla = @p_codigo_tipo_planilla
			and GETDATE() between vigencia_inicio and vigencia_fin
			and ((codigo_grupo IS NULL) OR (codigo_grupo IS NOT NULL AND codigo_grupo = @v_codigo_grupo))
		order by codigo_canal, codigo_grupo desc

		IF (@v_monto_bruto > @v_monto_tope)
		BEGIN
			update detalle_planilla_bono
			set
				monto_bruto = @v_monto_tope / (@v_monto_bruto / monto_bruto)
			where 
				codigo_planilla =  @p_codigo_planilla
				and codigo_personal = @v_codigo_personal

			update detalle_planilla_bono
			set
				monto_neto = monto_bruto * 1.18,
				monto_igv = round((monto_bruto * 1.18), 2) - monto_bruto
			where 
				codigo_planilla =  @p_codigo_planilla
				and codigo_personal = @v_codigo_personal
		END

		SET @v_indice = @v_indice + 1 
	END
	SET NOCOUNT OFF
END;