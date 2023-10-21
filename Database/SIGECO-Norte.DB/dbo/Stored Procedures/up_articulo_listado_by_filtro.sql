CREATE PROCEDURE [dbo].[up_articulo_listado_by_filtro]
(
	@codigo_empresa		VARCHAR(20)
	,@nro_contrato		VARCHAR(20)
	,@nombre			VARCHAR(250)
)
AS
BEGIN

	DECLARE @codigo_equivalencia_empresa	NVARCHAR(4)
	SELECT @codigo_equivalencia_empresa=codigo_equivalencia FROM empresa_sigeco WHERE codigo_empresa=@codigo_empresa
		
	SELECT n1.codigo_empresa,n1.NumAtCard,n3.codigo_articulo, n3.codigo_sku, n3.nombre, n3.abreviatura 
	FROM cabecera_contrato n1
	INNER JOIN detalle_contrato n2 ON n1.Codigo_empresa=n2.Codigo_empresa AND n1.NumAtCard=n2.NumAtCard
	INNER JOIN articulo n3 ON n2.ItemCode=n3.codigo_sku
	WHERE n1.Codigo_empresa=@codigo_equivalencia_empresa
	AND n1.NumAtCard = @nro_contrato
	AND n3.nombre like '%' + @nombre + '%'

END;