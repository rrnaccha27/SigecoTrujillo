CREATE VIEW [dbo].[vwPersonal_Reclamo]
AS
SELECT        dbo.personal.codigo_personal, dbo.personal.codigo_equivalencia, dbo.personal.nombre AS Personal, dbo.cabecera_contrato.NumAtCard AS Contrato, dbo.empresa_sigeco.codigo_empresa, 
                         dbo.empresa_sigeco.codigo_equivalencia AS CodEquiEmp, dbo.empresa_sigeco.nombre AS Empresa, dbo.articulo.codigo_articulo, dbo.articulo.codigo_sku, dbo.articulo.nombre AS Articulo
FROM            dbo.detalle_contrato INNER JOIN
                         dbo.cabecera_contrato ON dbo.detalle_contrato.Codigo_empresa = dbo.cabecera_contrato.Codigo_empresa AND dbo.detalle_contrato.NumAtCard = dbo.cabecera_contrato.NumAtCard INNER JOIN
                         dbo.empresa_sigeco ON dbo.cabecera_contrato.Codigo_empresa = dbo.empresa_sigeco.codigo_equivalencia INNER JOIN
                         dbo.personal ON dbo.cabecera_contrato.Cod_Vendedor = dbo.personal.codigo_equivalencia INNER JOIN
                         dbo.articulo ON dbo.detalle_contrato.ItemCode = dbo.articulo.codigo_sku