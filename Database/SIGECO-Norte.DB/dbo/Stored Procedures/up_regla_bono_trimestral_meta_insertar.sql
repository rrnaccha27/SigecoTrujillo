USE SIGECO
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[up_regla_bono_trimestral_meta_insertar]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].up_regla_bono_trimestral_meta_insertar
GO

CREATE PROCEDURE dbo.up_regla_bono_trimestral_meta_insertar
(
	@p_codigo_regla			INT,
	@p_rango_inicio			INT,
	@p_rango_fin			INT,
	@p_monto				DECIMAL(10, 2),
	@p_usuario_registra		VARCHAR(25),
	@p_codigo_regla_meta	INT OUT
)
AS
BEGIN
	SET NOCOUNT ON

	INSERT INTO dbo.regla_bono_trimestral_meta(
		codigo_regla,
		rango_inicio,
		rango_fin,
		monto,
		usuario_registra,
		estado_registro,
		fecha_registra
	)
	VALUES(
		@p_codigo_regla,
		@p_rango_inicio,
		@p_rango_fin,
		@p_monto,
		@p_usuario_registra,
		1,
		GETDATE()
	);

	SET @p_codigo_regla_meta = SCOPE_IDENTITY();
	
	SET NOCOUNT OFF
END
