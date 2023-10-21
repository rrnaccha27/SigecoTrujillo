create function [dbo].[fn_obtener_personal_supervisor]
(
	@p_nro_contrato	nvarchar(100),
	@p_codigo_empresa nvarchar(20),
	@p_codigo_tipo_planilla int
)
RETURNS INT
BEGIN
	
	declare 
	    @v_codigo_personal_planilla int,
		@v_codigo_supervisor varchar(20),
		@v_codigo_personal varchar(20);

	select top 1
		@v_codigo_supervisor=Cod_Supervisor,
		@v_codigo_personal=Cod_Vendedor
	from cabecera_contrato where Codigo_empresa=@p_codigo_empresa and NumAtCard=@p_nro_contrato;
		
	----obteniendo personal de venta del supervisor
	if @p_codigo_tipo_planilla=2
	begin
		select top 1
			@v_codigo_personal_planilla=codigo_personal
		from personal where codigo_equivalencia=@v_codigo_personal;
	end;

	----obteniendo supervisor del personal
	if @p_codigo_tipo_planilla=1
	begin
		select top 1
			@v_codigo_personal_planilla=codigo_personal
		from personal where codigo_equivalencia=@v_codigo_supervisor;
	end;

	return @v_codigo_personal_planilla;

END