IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[up_descuento_comision_generar_descuento]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[up_descuento_comision_generar_descuento]
GO
CREATE PROCEDURE [dbo].[up_descuento_comision_generar_descuento]
(
	@p_codigo_planilla		INT
	,@p_usuario_registra	VARCHAR(50)
	,@p_cantidad			INT OUTPUT 
)
AS
BEGIN
	SET NOCOUNT ON
	
	SET @p_cantidad = ISNULL((SELECT COUNT(codigo_descuento) FROM dbo.descuento WHERE estado_registro = 1), 0)

	INSERT INTO 
		dbo.descuento
	(
		codigo_planilla
		,codigo_empresa
		,codigo_personal
		,motivo
		,monto
		,estado_registro
		,fecha_registra
		,usuario_registra
		,codigo_descuento_comision
	)
	SELECT
		@p_codigo_planilla
		,max(dc.codigo_empresa)
		,max(dc.codigo_personal)
		,min(dc.motivo)
		,case when SUM(dp.monto_neto) > min(dc.saldo) then min(dc.saldo) else SUM(dp.monto_neto) end 
		,1
		,getdate()
		,@p_usuario_registra
		,max(dc.codigo_descuento_comision)
	FROM
		dbo.descuento_comision dc
	INNER JOIN dbo.detalle_planilla dp 
		ON dp.codigo_planilla = @p_codigo_planilla 
		AND dp.codigo_personal = dc.codigo_personal 
		AND dp.codigo_empresa = dc.codigo_empresa
		AND dp.excluido = 0
		AND dp.estado_registro = 1
	WHERE 
		dc.estado_registro = 1
		AND dc.saldo > 0
		AND NOT EXISTS(SELECT TOP 1 d.codigo_descuento FROM dbo.descuento d inner join dbo.planilla p on p.codigo_planilla = @p_codigo_planilla and p.codigo_planilla = d.codigo_planilla WHERE d.codigo_descuento_comision = dc.codigo_descuento_comision AND d.estado_registro = 1)
	GROUP BY dp.codigo_personal, dp.codigo_empresa

	SET @p_cantidad = ISNULL((SELECT COUNT(codigo_descuento) FROM dbo.descuento WHERE estado_registro = 1), 0) - @p_cantidad

	SET NOCOUNT OFF
END;