USE SIGECO
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[up_personal_bloqueo_validacion_por_codigo]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].up_personal_bloqueo_validacion_por_codigo
GO

CREATE PROCEDURE dbo.up_personal_bloqueo_validacion_por_codigo
(
	@p_codigo_personal	INT
)
AS
BEGIN
	SET NOCOUNT ON

	DECLARE
		@c_estado_activo	BIT = 1
	
	SELECT 
		COUNT(codigo_personal) as cantidad
	FROM personal_bloqueo b
	WHERE
		b.estado_registro = @c_estado_activo
		AND codigo_personal = @p_codigo_personal
	
	SET NOCOUNT OFF
END