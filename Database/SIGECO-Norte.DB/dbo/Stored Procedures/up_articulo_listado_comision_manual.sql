create PROC [dbo].[up_articulo_listado_comision_manual]
(
	 @p_codigo_empresa		INT
	,@p_nro_contrato		VARCHAR(20)
	,@p_nombre				VARCHAR(50)
	,@p_codigo_personal		INT
	,@p_codigo_canal		INT
	,@p_codigo_tipo_venta	INT
	,@p_codigo_tipo_pago	INT
)
AS
BEGIN

	DECLARE @t_articulos TABLE
	(
		codigo_sku VARCHAR(20)
	)

	DECLARE
		@codigo_equivalencia_empresa	NVARCHAR(4)
		,@v_codigo_campo_santo			NVARCHAR(4)
		,@v_ExisteContrato				BIT = 0
		,@v_fecha						DATETIME
		,@v_codigo_personal				VARCHAR(10)
	
	SET @v_fecha = GETDATE()

	IF ISNULL(@p_codigo_empresa, 0) <> 0 AND LEN(ISNULL(@p_nro_contrato, '')) > 0
	BEGIN
		SELECT @codigo_equivalencia_empresa = codigo_equivalencia FROM dbo.empresa_sigeco WHERE codigo_empresa = @p_codigo_empresa
		SELECT @v_codigo_personal = codigo_equivalencia FROM dbo.personal WHERE codigo_personal = @p_codigo_personal

		IF EXISTS(SELECT * FROM cabecera_contrato 
		WHERE Codigo_empresa = @codigo_equivalencia_empresa AND  NumAtCard = @p_nro_contrato AND Cod_Vendedor = @v_codigo_personal)
		BEGIN
			SET @v_ExisteContrato = 1

			INSERT INTO @t_articulos
			SELECT
				ItemCode 
			FROM 
				dbo.detalle_contrato
			WHERE 
				Codigo_empresa = @codigo_equivalencia_empresa
				AND NumAtCard = @p_nro_contrato
		END
	END

	SELECT
		a.codigo_articulo
		,a.nombre
		,a.codigo_sku
		,ISNULL(p.precio_total, 0) AS precio
		,CONVERT(decimal(10, 2),ISNULL(CASE WHEN r.codigo_tipo_comision = 1 THEN r.valor ELSE ROUND((r.valor * p.precio_total)/100, 2) END, 0)) AS monto_comision 
	FROM 
		dbo.articulo a
	INNER JOIN
		dbo.pcc_precio_articulo p
		ON p.codigo_empresa = @p_codigo_empresa AND p.codigo_tipo_venta = @p_codigo_tipo_venta AND p.codigo_articulo = a.codigo_articulo AND @v_fecha BETWEEN p.vigencia_inicio AND vigencia_fin
	LEFT JOIN
		dbo.pcc_regla_calculo_comision r
		ON r.codigo_canal = @p_codigo_canal AND r.codigo_tipo_pago = @p_codigo_tipo_pago AND r.codigo_precio = p.codigo_precio AND @v_fecha BETWEEN r.vigencia_inicio AND r.vigencia_fin
	WHERE
		a.estado_registro = 1 AND a.genera_comision = 1
		AND a.nombre like '%' + ISNULL(@p_nombre, a.nombre) + '%'
		AND (@v_ExisteContrato = 0 OR (@v_ExisteContrato = 1 AND a.codigo_sku IN (SELECT codigo_sku FROM @t_articulos)))
END