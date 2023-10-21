USE SIGECO
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[up_planilla_bono_contabilidad_planilla_v3]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].up_planilla_bono_contabilidad_planilla_v3
GO


CREATE PROCEDURE [dbo].up_planilla_bono_contabilidad_planilla_v3
(
	@p_codigo_checklist	INT
	,@p_codigo_empresa	INT
	,@p_codigo_planilla	INT = 0
)
AS
BEGIN
	SET NOCOUNT ON

	SELECT
		cont.codigo_planilla, cont.codigo_empresa,
		ORDEN,COD_COMISION,N_CUOTA,IMP_PAGAR,COD_EMPRESA_G,NUM_CONTRATO,IGV,DES_TIPO_VENTA,TIPO_VENTA,FEC_HAVILITADO,DES_FORMA_PAGO,ID_FORMA_DE_PAGO
		,DES_TIPO_COMISION,TIPO_COMISION,CUARTA,IES,TIPO_MONEDA,TIPO_AGENTE_G,C_AGENTE,COD_GRUPO_VENTA_G,cont.NOMBRE_GRUPO,DESCRIPCION_1,COD_BIEN,DESCRIPCION_2
		,COD_CONCEPTO,TIPO_CAMBIO,SALDO_A_PAGAR,FEC_PLANILLA,USU_PLANILLA,N_OPERACION,FEC_CIERRE,DESC_TIPO_DOCUM,RUC,NUM_DOC,NOM_AGENTE,NOM_EMPRESA
		,DIR_EMPRESA,RUC_EMPRESA,FEC_INICIO,FEC_TERMINO,DSCTO_ESTUDIO_C,DSCTO_DETRACCION,PORC_IGV,UNIDAD_NEGOCIO,codigo_canal,DIMENSION_3,DIMENSION_4,DIMENSION_5
	FROM 
		checklist_bono_detalle det 
	INNER JOIN sigeco_reporte_bono_rrhh rrhh
		ON rrhh.codigo_planilla = det.codigo_planilla and rrhh.codigo_empresa = det.codigo_empresa and rrhh.codigo_personal = det.codigo_personal 
	INNER JOIN personal p
		ON p.codigo_personal = det.codigo_personal
	INNER JOIN sigeco_reporte_bono_contabilidad cont
		ON cont.codigo_planilla = rrhh.codigo_planilla and cont.codigo_empresa = rrhh.codigo_empresa and cont.C_AGENTE = p.codigo_equivalencia 
	INNER JOIN checklist_bono chk
		ON chk.codigo_checklist = det.codigo_checklist
	WHERE 
		det.codigo_checklist = @p_codigo_checklist and det.codigo_empresa = @p_codigo_empresa
		AND det.validado = 1 and rrhh.validado = 1
		AND ( @p_codigo_planilla = 0 OR (@p_codigo_planilla <> 0 and det.codigo_planilla = @p_codigo_planilla) )

	UNION

	SELECT TOP 1
		cont.codigo_planilla, cont.codigo_empresa,
		ORDEN,COD_COMISION,N_CUOTA,IMP_PAGAR,COD_EMPRESA_G,NUM_CONTRATO,IGV,DES_TIPO_VENTA,TIPO_VENTA,FEC_HAVILITADO,DES_FORMA_PAGO,ID_FORMA_DE_PAGO
		,DES_TIPO_COMISION,TIPO_COMISION,CUARTA,IES,TIPO_MONEDA,TIPO_AGENTE_G,C_AGENTE,COD_GRUPO_VENTA_G,cont.NOMBRE_GRUPO,DESCRIPCION_1,COD_BIEN,DESCRIPCION_2
		,COD_CONCEPTO,TIPO_CAMBIO,SALDO_A_PAGAR,FEC_PLANILLA,USU_PLANILLA,N_OPERACION,FEC_CIERRE,DESC_TIPO_DOCUM,RUC,NUM_DOC,NOM_AGENTE,NOM_EMPRESA
		,DIR_EMPRESA,RUC_EMPRESA,FEC_INICIO,FEC_TERMINO,DSCTO_ESTUDIO_C,DSCTO_DETRACCION,PORC_IGV,UNIDAD_NEGOCIO,codigo_canal,DIMENSION_3,DIMENSION_4,DIMENSION_5
	FROM 
		checklist_bono chk
	INNER JOIN sigeco_reporte_bono_contabilidad cont
		ON cont.codigo_planilla = chk.codigo_planilla and cont.codigo_empresa = @p_codigo_empresa and orden = 1
	WHERE 
		chk.codigo_checklist = @p_codigo_checklist
	ORDER BY 
		orden, codigo_canal desc, cont.NOMBRE_GRUPO asc, NOM_AGENTE, NUM_CONTRATO, DESCRIPCION_1, N_CUOTA

	SET NOCOUNT OFF
END;
