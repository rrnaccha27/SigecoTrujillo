CREATE PROCEDURE dbo.up_log_contrato_sap_fechas
AS
BEGIN
	DECLARE 
		@v_fechahoy DATETIME
		,@v_fechainiciomes DATETIME

	SET @v_fechahoy = GETDATE()
	SET @v_fechainiciomes = CONVERT(DATETIME, CONVERT(VARCHAR,DATEPART(YEAR, @v_fechahoy)) + RIGHT('0' + CONVERT(VARCHAR,DATEPART(MONTH, @v_fechahoy)), 2) +  '01' )

	SELECT CONVERT(VARCHAR, @v_fechainiciomes, 103) AS fecha_inicio, CONVERT(VARCHAR, @v_fechahoy, 103) AS fecha_fin
END