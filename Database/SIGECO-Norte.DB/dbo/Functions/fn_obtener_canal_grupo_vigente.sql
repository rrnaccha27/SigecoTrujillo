CREATE FUNCTION dbo.fn_obtener_canal_grupo_vigente
(
	@p_codigo_personal	INT
)
RETURNS VARCHAR(100)
AS
BEGIN

	DECLARE
		@v_canal_grupo	VARCHAR(100) = ''
	
	select top 1
		@v_canal_grupo = isnull(grupo.nombre, canal.nombre)
	from
		personal_canal_grupo pcg
	inner join canal_grupo canal 
		on canal.codigo_canal_grupo = pcg.codigo_canal
	left join canal_grupo grupo
		on grupo.codigo_canal_grupo = pcg.codigo_canal_grupo
	where
		pcg.codigo_personal = @p_codigo_personal
		and pcg.estado_registro = 1

	RETURN @v_canal_grupo
END;