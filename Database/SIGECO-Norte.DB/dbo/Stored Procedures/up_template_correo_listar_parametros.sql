USE SIGECO
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[up_template_correo_listar_parametros]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].up_template_correo_listar_parametros
GO
CREATE PROCEDURE dbo.up_template_correo_listar_parametros
(
	@p_codigo_template	INT
)
AS
BEGIN
	SET NOCOUNT ON

	SELECT
		t.codigo_template
		,t.nombre
		,p.indice
		,p.parametro
	FROM
		dbo.template_correo t
	INNER JOIN dbo.template_correo_parametro p
		ON p.codigo_template = t.codigo_template
	WHERE
		t.codigo_template = @p_codigo_template
	ORDER BY
		p.indice ASC

	SET NOCOUNT OFF
END