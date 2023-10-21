CREATE PROCEDURE [dbo].[up_articulo_insertar]
(
	@codigo_articulo int 
	,@codigo_unidad_negocio int
	,@codigo_categoria int
	,@codigo_tipo_articulo int
	,@codigo_sku  VARCHAR(20)
	,@nombre VARCHAR(250)
	,@abreviatura   VARCHAR(10)
	,@genera_comision  bit 
	,@genera_bono bit 
	,@genera_bolsa_bono bit
	,@tiene_contrato_vinculante bit
	,@anio_contrato_vinculante int
	,@usuario_registra varchar(50)
	,@cantidad_unica bit
	,@p_codigo_articulo int out
)
AS
BEGIN

	DECLARE
		@cantidad_codigo_sku int;

	if @codigo_articulo>0 
	begin
		exec up_articulo_log_insertar @codigo_articulo

		update 
			articulo
		set 
			codigo_categoria=@codigo_categoria,
			nombre=@nombre,
			tiene_contrato_vinculante=@tiene_contrato_vinculante,
			anio_contrato_vinculante=@anio_contrato_vinculante,
			genera_bolsa_bono=@genera_bolsa_bono,
			abreviatura=@abreviatura,
			genera_comision=@genera_comision,
			genera_bono=@genera_bono,
			codigo_tipo_articulo=@codigo_tipo_articulo,
			cantidad_unica = @cantidad_unica,
			usuario_modifica=@usuario_registra,
			fecha_modifica=GETDATE()
		where
			codigo_articulo=@codigo_articulo;   
		set @p_codigo_articulo=@codigo_articulo;
	end
	else 
	begin
		select 
			@cantidad_codigo_sku=COUNT(1)
		from 
			articulo 
		where 
			upper(codigo_sku)=upper(@codigo_sku) and estado_registro=1;

		if(@cantidad_codigo_sku>0)
		begin
			RAISERROR('El código sku ya existe, vuelva ingresar otro código.',16,1); 
			return;
		end;  
    
		insert into articulo (
			codigo_unidad_negocio,
			codigo_categoria,
			codigo_tipo_articulo,
			codigo_sku,
			nombre,
			abreviatura,
			genera_comision,
			genera_bono,
			genera_bolsa_bono,
			tiene_contrato_vinculante,
			anio_contrato_vinculante,
			cantidad_unica,
			estado_registro,
			fecha_registra,
			usuario_registra
		)
		values(
			@codigo_unidad_negocio,
			@codigo_categoria,
			@codigo_tipo_articulo,
			@codigo_sku,
			@nombre,
			@abreviatura,
			@genera_comision,
			@genera_bono,
			@genera_bolsa_bono,
			@tiene_contrato_vinculante,
			@anio_contrato_vinculante,
			@cantidad_unica,
			1,
			GETDATE(),
			@usuario_registra
		);
		set @p_codigo_articulo=@@IDENTITY;
	end;
  
END;