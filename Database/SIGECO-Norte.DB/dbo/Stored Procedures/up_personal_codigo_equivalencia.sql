CREATE PROCEDURE dbo.up_personal_codigo_equivalencia
	@p_codigo_personal	INT
AS
BEGIN
	SELECT TOP 1
		codigo_equivalencia
	FROM 
		dbo.personal
	WHERE
		codigo_personal = @p_codigo_personal
END;