USE SIGECO
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[up_regla_bono_trimestral_detalle_insertar]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].up_regla_bono_trimestral_detalle_insertar
GO

CREATE PROCEDURE dbo.up_regla_bono_trimestral_detalle_insertar
(
	@p_codigo_regla			INT,
	@p_codigo_canal			INT,
	@p_codigo_empresa		VARCHAR(400),
	@p_codigo_tipo_venta	VARCHAR(400),
	@p_usuario_registra		VARCHAR(25),
	@p_codigo_regla_detalle	INT OUT
)
AS
BEGIN
	SET NOCOUNT ON

	INSERT INTO dbo.regla_bono_trimestral_detalle(
		codigo_regla,
		codigo_canal,
		codigo_empresa,
		codigo_tipo_venta,
		usuario_registra,
		estado_registro,
		fecha_registra
	)
	VALUES(
		@p_codigo_regla,
		@p_codigo_canal,
		@p_codigo_empresa,
		@p_codigo_tipo_venta,
		@p_usuario_registra,
		1,
		GETDATE()
	);

	SET @p_codigo_regla_detalle = SCOPE_IDENTITY();
	
	SET NOCOUNT OFF
END
