USE SIGECO
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[up_regla_bono_trimestral_combo]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].up_regla_bono_trimestral_combo
GO

CREATE PROCEDURE dbo.up_regla_bono_trimestral_combo
AS
BEGIN
	SET NOCOUNT ON
	
	SELECT 
		codigo_regla
		,descripcion
	FROM 
		dbo.regla_bono_trimestral 
	WHERE 
		estado_registro = 1
		AND CONVERT(VARCHAR, GETDATE(), 112) BETWEEN vigencia_inicio and vigencia_fin
	ORDER BY
		descripcion ASC

	SET NOCOUNT OFF
END;

