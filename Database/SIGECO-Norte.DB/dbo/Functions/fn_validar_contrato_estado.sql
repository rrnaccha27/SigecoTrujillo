CREATE FUNCTION [dbo].fn_validar_contrato_estado(
	@p_Codigo_empresa NVARCHAR(4), 
	@p_NumAtCard NVARCHAR(100)
)
RETURNS BIT
AS
BEGIN
	DECLARE 
		@c_ANULADO	CHAR(3) = 'ANL'
		,@v_flag	BIT = 1

	IF EXISTS(
		SELECT TOP 1
			NumAtCard
		FROM 
			dbo.cabecera_contrato cc
		WHERE 
			cc.NumAtCard = @p_NumAtCard AND cc.Codigo_empresa = @p_Codigo_empresa and cc.Cod_Estado_Contrato = @c_ANULADO
		)
		SET @v_flag = 0

	RETURN @v_flag
END;