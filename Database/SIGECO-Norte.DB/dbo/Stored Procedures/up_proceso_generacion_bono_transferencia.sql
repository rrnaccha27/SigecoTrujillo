CREATE PROCEDURE dbo.up_proceso_generacion_bono_transferencia
(
	@p_codigo_planilla			INT 
	,@p_codigo_tipo_planilla	INT
	,@p_codigo_empresa			VARCHAR(4)
	,@p_nro_contrato			VARCHAR(100)
	,@p_monto_ingresado			DECIMAL(12, 4) OUTPUT
)
AS
BEGIN
	SET NOEXEC OFF
	SET NOCOUNT ON

	DECLARE
		@v_Num_Contrato_Referencia			VARCHAR(100) 
		,@v_Cod_Empresa_Referencia			VARCHAR(4)
		,@p_monto_ingresado_pagado_ref		DECIMAL(12, 4)
		,@v_codigo_empresa_sigeco_ref		INT
	
	DECLARE
		@c_codigo_estado_planilla_cerrada	INT = 2

	SELECT
		TOP 1
		@v_Num_Contrato_Referencia = Num_Contrato_Referencia
		,@v_Cod_Empresa_Referencia = Cod_Empresa_Referencia
	FROM
		dbo.cabecera_contrato
	WHERE
		Codigo_empresa = @p_codigo_empresa
		AND NumAtCard = @p_nro_contrato	
		AND Flg_Transferencia = 1

	IF (@v_Num_Contrato_Referencia IS NULL OR @v_Num_Contrato_Referencia = '0' OR LEN(ISNULL(@v_Num_Contrato_Referencia, '')) = 0)
		SET NOEXEC ON

	IF (@v_Cod_Empresa_Referencia IS NULL OR @v_Cod_Empresa_Referencia = '0' OR LEN(ISNULL(@v_Cod_Empresa_Referencia, '')) = 0)
		SET NOEXEC ON

	SET @v_codigo_empresa_sigeco_ref = (SELECT TOP 1 codigo_empresa FROM dbo.empresa_sigeco WHERE codigo_equivalencia = @v_Cod_Empresa_Referencia)

	IF (@v_codigo_empresa_sigeco_ref IS NULL)
		SET NOEXEC ON

	SELECT
		@p_monto_ingresado_pagado_ref = cpb.monto_ingresado
	FROM
		dbo.contrato_planilla_bono cpb
	INNER JOIN dbo.planilla_bono pb 
		ON pb.codigo_planilla = cpb.codigo_planilla
	WHERE
		pb.codigo_tipo_planilla = @p_codigo_tipo_planilla
		AND codigo_estado_planilla = @c_codigo_estado_planilla_cerrada --Planilla Cerrara
		AND cpb.codigo_empresa = @v_Cod_Empresa_Referencia
		AND cpb.numero_contrato = @v_Num_Contrato_Referencia

	SET @p_monto_ingresado_pagado_ref = ISNULL(@p_monto_ingresado_pagado_ref, 0)

	IF (@p_monto_ingresado_pagado_ref = 0)
		SET NOEXEC ON
	
	IF (@p_monto_ingresado < @p_monto_ingresado_pagado_ref)
	BEGIN
		--SET @p_observacion = 'El monto de comision por transferencia es menor al monto del contrato referenciado.'
		SET @p_monto_ingresado = 0
		SET NOEXEC ON
	END

	SET @p_monto_ingresado = @p_monto_ingresado - @p_monto_ingresado_pagado_ref

	SET NOEXEC OFF
	SET NOCOUNT OFF

END