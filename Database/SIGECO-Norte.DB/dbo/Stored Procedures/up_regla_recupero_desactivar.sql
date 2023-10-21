CREATE PROCEDURE dbo.up_regla_recupero_desactivar
(
	 @p_codigo_regla_recupero	INT
	,@p_usuario_modifica		VARCHAR(50)
)
AS
BEGIN
	
	UPDATE
		dbo.regla_recupero
	SET
		estado_registro = 0
		,fecha_modifica = GETDATE() 
		,usuario_modifica = @p_usuario_modifica
	WHERE
		codigo_regla_recupero = @p_codigo_regla_recupero

END