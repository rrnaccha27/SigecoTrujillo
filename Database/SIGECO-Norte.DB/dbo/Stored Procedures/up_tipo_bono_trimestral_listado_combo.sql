USE SIGECO
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[up_tipo_bono_trimestral_listar_combo]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].up_tipo_bono_trimestral_listar_combo
GO

CREATE PROCEDURE dbo.up_tipo_bono_trimestral_listar_combo
AS
BEGIN
	SET NOCOUNT ON

	SELECT  
		codigo_tipo_bono
		,nombre
	FROM 
		dbo.tipo_bono_trimestral 
	WHERE
		estado_registro = 1
	ORDER BY 
		nombre ASC

	SET NOCOUNT OFF
END
