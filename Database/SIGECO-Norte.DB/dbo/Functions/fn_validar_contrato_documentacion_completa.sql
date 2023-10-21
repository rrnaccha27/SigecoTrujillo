CREATE FUNCTION fn_validar_contrato_documentacion_completa(
	@p_Codigo_empresa NVARCHAR(4), 
	@p_NumAtCard NVARCHAR(100)
)
RETURNS BIT
AS
BEGIN
	DECLARE 
		@c_DOC_COMPLETA		CHAR(2) = '01'
		,@v_flag_documentacion	BIT = 0

	IF EXISTS(
		SELECT TOP 1
			NumAtCard
		FROM 
			dbo.cabecera_contrato cc
		WHERE 
			cc.NumAtCard = @p_NumAtCard AND cc.Codigo_empresa = @p_Codigo_empresa and cc.Flg_Documentacion_Completa = @c_DOC_COMPLETA
		)
		SET @v_flag_documentacion = 1

	RETURN @v_flag_documentacion
END