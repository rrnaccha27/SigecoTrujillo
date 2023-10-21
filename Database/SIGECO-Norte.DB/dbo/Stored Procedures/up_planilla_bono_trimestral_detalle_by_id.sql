USE SIGECO
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[up_planilla_bono_trimestral_detalle_by_id]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].up_planilla_bono_trimestral_detalle_by_id
GO

CREATE PROCEDURE [dbo].up_planilla_bono_trimestral_detalle_by_id
(
	@p_codigo_planilla int
)
AS
BEGIN
	SET NOCOUNT ON
	
	SELECT 
		codigo_planilla_detalle
		,codigo_planilla
		,codigo_personal
		,codigo_canal_grupo
		,codigo_canal
		,nombre_canal
		,codigo_grupo
		,nombre_grupo
		,nombre_personal
		,codigo_supervisor
		,correo_supervisor
		,nombre_supervisor
		,monto_contratado
		,rango
		,monto_bono
	FROM
		dbo.planilla_bono_trimestral_detalle det
	WHERE
		det.codigo_planilla = @p_codigo_planilla
		AND monto_bono IS NOT NULL
	ORDER BY
		det.rango ASC
	
	SET NOCOUNT OFF
END;

