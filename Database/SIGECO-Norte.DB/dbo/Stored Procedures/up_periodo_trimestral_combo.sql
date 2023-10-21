USE SIGECO
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[up_periodo_trimestral_combo]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].up_periodo_trimestral_combo
GO


CREATE PROCEDURE dbo.up_periodo_trimestral_combo
AS
BEGIN
	SET NOCOUNT ON

	SELECT
		codigo_periodo
		,nombre
	FROM
		dbo.periodo_trimestral
	WHERE
		estado_registro = 1
	ORDER BY
		codigo_periodo

	SET NOCOUNT OFF
END;