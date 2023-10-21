CREATE PROCEDURE [dbo].[up_planilla_contabilidad_planilla_v2]
(
	@p_codigo_planilla	INT
	,@p_codigo_empresa	INT
)
AS
BEGIN
	SET NOCOUNT ON

	IF EXISTS(SELECT codigo_planilla FROM sigeco_reporte_comision_contabilidad WHERE codigo_planilla = @p_codigo_planilla AND codigo_empresa = @p_codigo_empresa)
	BEGIN
		SELECT
			codigo_planilla,codigo_empresa
			,ORDEN,COD_COMISION,N_CUOTA,IMP_PAGAR,COD_EMPRESA_G,NUM_CONTRATO,IGV,DES_TIPO_VENTA,TIPO_VENTA,FEC_HAVILITADO,DES_FORMA_PAGO,ID_FORMA_DE_PAGO
			,DES_TIPO_COMISION,TIPO_COMISION,CUARTA,IES,TIPO_MONEDA,TIPO_AGENTE_G,C_AGENTE,COD_GRUPO_VENTA_G,NOMBRE_GRUPO,DESCRIPCION_1,COD_BIEN,DESCRIPCION_2
			,COD_CONCEPTO,TIPO_CAMBIO,SALDO_A_PAGAR,FEC_PLANILLA,USU_PLANILLA,N_OPERACION,FEC_CIERRE,DESC_TIPO_DOCUM,RUC,NUM_DOC,NOM_AGENTE,NOM_EMPRESA
			,DIR_EMPRESA,RUC_EMPRESA,FEC_INICIO,FEC_TERMINO,DSCTO_ESTUDIO_C,DSCTO_DETRACCION,PORC_IGV,UNIDAD_NEGOCIO,codigo_canal,DIMENSION_3,DIMENSION_4,DIMENSION_5
		FROM 
			sigeco_reporte_comision_contabilidad 
		WHERE 
			codigo_planilla = @p_codigo_planilla
			AND codigo_empresa = @p_codigo_empresa
		ORDER BY 
			orden, codigo_canal desc, NOMBRE_GRUPO asc, NOM_AGENTE, NUM_CONTRATO, DESCRIPCION_1, N_CUOTA
		select 'congelado'
	END
	ELSE
	BEGIN
		EXEC up_planilla_contabilidad_planilla @p_codigo_planilla, @p_codigo_empresa
		select 'en vivo'
	END

	SET NOCOUNT OFF
END;