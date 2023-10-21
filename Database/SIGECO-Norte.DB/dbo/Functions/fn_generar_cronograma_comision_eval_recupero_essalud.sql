CREATE FUNCTION [dbo].[fn_generar_cronograma_comision_eval_recupero_essalud]
(
	@p_nro_contrato		VARCHAR(100)
	,@p_codigo_empresa	VARCHAR(4)
)
RETURNS BIT
AS
BEGIN
	DECLARE
		@v_retorno			BIT = 0
		,@v_monto_recupero	DECIMAL(12, 4)
		,@v_importe_cuota	DECIMAL(12, 4)
		,@v_fecha_proceso	DATETIME
	
	SELECT TOP 1 
		@v_monto_recupero = CASE WHEN cc.Flg_Recupero = 1 THEN cc.Monto_Recupero ELSE 0 END 
		,@v_fecha_proceso = cc.CreateDate
	FROM 
		dbo.cabecera_contrato cc 
	WHERE 
		cc.NumAtCard = @p_nro_contrato AND cc.Codigo_empresa = @p_codigo_empresa

	IF (@v_monto_recupero > 0)
	BEGIN
		DECLARE @v_nro_cuota INT

		SELECT TOP 1
			@v_nro_cuota = nro_cuota
		FROM
			dbo.pcc_regla_recupero
		WHERE 
			@v_fecha_proceso BETWEEN vigencia_inicio AND vigencia_fin
 
		IF (@v_nro_cuota IS NOT NULL)
		BEGIN
			SET @v_importe_cuota = ISNULL((SELECT TOP 1 Num_Importe_Total FROM dbo.contrato_cuota WHERE Num_Cuota = @v_nro_cuota AND NumAtCard = @p_nro_contrato and Codigo_empresa = @p_codigo_empresa AND Cod_Estado IN ('C', 'P') ORDER BY Fec_Vencimiento ASC), 0)
			IF EXISTS (SELECT NumAtcard FROM dbo.contrato_cuota WHERE NumAtCard = @p_nro_contrato and Codigo_empresa = @p_codigo_empresa AND Cod_Estado IN ('C', 'P') GROUP BY NumAtCard HAVING MAX(Num_Cuota) = @v_nro_cuota)
				IF (@v_importe_cuota = @v_monto_recupero)
					SET @v_retorno = 1-- ES UN CONTRATO CON RECUPERO
		END
	END

	RETURN @v_retorno;
END;