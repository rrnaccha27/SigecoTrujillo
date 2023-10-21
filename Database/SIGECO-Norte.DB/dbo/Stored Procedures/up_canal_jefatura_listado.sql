USE SIGECO
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[up_canal_jefatura_listado]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].up_canal_jefatura_listado
GO
CREATE PROC [dbo].up_canal_jefatura_listado
AS
BEGIN
	SET NOCOUNT ON

	SELECT
		j.codigo_canal, j.email, j.email_copia, ISNULL(c.nombre, '') as nombre_canal
	FROM
		dbo.canal_jefatura j
	LEFT JOIN dbo.canal_grupo c
		ON c.codigo_canal_grupo = j.codigo_canal AND c.es_canal_grupo = 1

	SET NOCOUNT OFF
END