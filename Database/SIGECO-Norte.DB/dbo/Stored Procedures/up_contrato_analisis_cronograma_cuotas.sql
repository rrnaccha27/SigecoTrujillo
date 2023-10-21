CREATE PROCEDURE dbo.up_contrato_analisis_cronograma_cuotas
(
	@p_nro_contrato		VARCHAR(100)
	,@p_codigo_empresa	INT
)
AS
BEGIN
	SET NOCOUNT ON

	DECLARE
		@v_codigo_empresa	VARCHAR(4) = (SELECT TOP 1 codigo_equivalencia FROM DBO.empresa_sigeco WHERE codigo_empresa = @p_codigo_empresa )

	SELECT
		CASE WHEN cc.Num_Cuota = 0 THEN 'CUOTA INICIAL' ELSE 'CUOTAS' END AS 'tipo_cuota'
		,CONVERT(INT, cc.Num_Cuota) as cuota
		,cc.Num_Importe_Total_SinIgv as importe_sin_igv
		,cc.Num_Importe_Igv as importe_igv
		,cc.Num_Importe_Total as importe_total
		,ISNULL(CONVERT(VARCHAR, cc.Fec_Vencimiento, 103), '') AS fec_vencimiento
		,ISNULL(CONVERT(VARCHAR, cc.Fec_Pago, 103), '') AS fec_pago
		, CASE Cod_Estado WHEN 'C' THEN 'Cancelado' WHEN 'P' THEN 'Pendiente' WHEN 'R' THEN 'Refinanciado' WHEN 'A' THEN 'Anulado' ELSE Cod_Estado END as estado
	FROM
		dbo.contrato_cuota cc
	WHERE
		cc.NumAtCard = @p_nro_contrato
		AND cc.Codigo_empresa = @v_codigo_empresa
	ORDER BY 
		cc.Num_Cuota

	SET NOCOUNT OFF
END;