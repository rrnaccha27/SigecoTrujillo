CREATE PROCEDURE dbo.up_contrato_analisis_empresas
(
	@p_nro_contrato	VARCHAR(100)
)
AS
BEGIN
	SET NOCOUNT ON

	SELECT
		e.codigo_empresa
		,e.nombre
	FROM
		dbo.cabecera_contrato cc
	INNER JOIN
		dbo.empresa_sigeco e
		ON e.codigo_equivalencia = cc.codigo_empresa
	WHERE cc.numatcard = @p_nro_contrato

	SET NOCOUNT OFF
END;