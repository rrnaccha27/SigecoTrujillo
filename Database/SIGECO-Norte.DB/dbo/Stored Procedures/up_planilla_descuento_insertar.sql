CREATE PROCEDURE [dbo].[up_planilla_descuento_insertar]
(
    @codigo_descuento int
    ,@codigo_planilla int
	,@codigo_personal  int
	,@codigo_empresa int
	,@motivo varchar(300)
	,@monto decimal(10,2)
	,@estado_registro bit	
	,@usuario_registra varchar(20)
	,@p_codigo_descuento int out
)
AS
BEGIN

	declare @v_existe_descuento int,@v_nombre_empresa varchar(200),
	@v_mensaje varchar(250),
	@v_total_comision decimal(10,2);

	select top 1
		@v_nombre_empresa=nombre 
	from empresa_sigeco 
	where codigo_empresa=@codigo_empresa;

	select 
		@v_existe_descuento=COUNT(1) 
	from descuento de 
	where 
		de.codigo_planilla=@codigo_planilla and
		de.codigo_personal=@codigo_personal and 
		de.estado_registro=1 and 
		de.codigo_empresa=@codigo_empresa;

	if @v_existe_descuento>0
	begin
		set @v_mensaje='El personal seleccionado tiene descuento vigente en la empresa '+@v_nombre_empresa;
		RAISERROR(@v_mensaje,16,1); 
		return;
	end;

	select 
		@v_total_comision= SUM(dp.monto_neto)
	from detalle_planilla dp 
	inner join detalle_cronograma dc on dp.codigo_detalle_cronograma=dc.codigo_detalle
	inner join cronograma_pago_comision cpc on dp.codigo_cronograma=cpc.codigo_cronograma
	inner join personal_canal_grupo pcg on pcg.codigo_registro=cpc.codigo_personal_canal_grupo
	where 
		dp.codigo_planilla=@codigo_planilla  and 
		pcg.codigo_personal=@codigo_personal and 
		cpc.codigo_empresa=@codigo_empresa and 
		dc.codigo_estado_cuota=2 and 
		dp.excluido=0;

	if @monto>@v_total_comision
	begin
		SET @v_mensaje='El descuento ('+cast(@monto as varchar(20))+') supera a comisión total('+cast(@v_total_comision as varchar(20))+') del personal';
		RAISERROR(@v_mensaje,16,1); 
		RETURN;
	end;

	insert into dbo.descuento
	(	
		codigo_planilla 
		,codigo_personal 
		,codigo_empresa
		,motivo 
		,monto
		,estado_registro
		,fecha_registra 
		,usuario_registra
		)
		values(
		 @codigo_planilla
		,@codigo_personal
		,@codigo_empresa
		,@motivo
		,@monto
		,@estado_registro
		,GETDATE()
		,@usuario_registra
	);
	
	set @p_codigo_descuento=@@IDENTITY;

END;