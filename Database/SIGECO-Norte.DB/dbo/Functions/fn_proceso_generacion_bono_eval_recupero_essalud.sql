CREATE FUNCTION [dbo].[fn_proceso_generacion_bono_eval_recupero_essalud]
(
	@p_nro_contrato		VARCHAR(100)
	,@p_codigo_empresa	VARCHAR(4)
)
RETURNS BIT
AS
BEGIN
	DECLARE
		@v_retorno			BIT = 0
		,@v_monto_recupero	DECIMAL(12, 4) = 0.00
		,@v_importe_cuota	DECIMAL(12, 4) = 0.00
		,@v_fecha_proceso	DATETIME
	
	SET @p_nro_contrato = CASE WHEN LEN(@p_nro_contrato) < 10 THEN REPLICATE('0',10 - LEN(@p_nro_contrato)) + @p_nro_contrato ELSE @p_nro_contrato END
	
	SELECT TOP 1 
		@v_monto_recupero = CASE WHEN cc.Flg_Recupero = 1 THEN cc.Monto_Recupero ELSE 0 END 
		,@v_fecha_proceso = cc.CreateDate
	FROM 
		dbo.cabecera_contrato cc 
	WHERE 
		cc.NumAtCard = @p_nro_contrato AND cc.Codigo_empresa = @p_codigo_empresa

	IF (@v_monto_recupero > 0)
	BEGIN
		SELECT 
			@v_importe_cuota = SUM(Num_Importe_Total)
		FROM 
			dbo.contrato_cuota 
		WHERE 
			NumAtCard = @p_nro_contrato 
			AND Codigo_empresa = @p_codigo_empresa 
			AND Cod_Estado = 'C'

		IF (@v_importe_cuota = @v_monto_recupero)
			SET @v_retorno = 1-- ES UN CONTRATO CON RECUPERO
	END

	RETURN @v_retorno;
END;