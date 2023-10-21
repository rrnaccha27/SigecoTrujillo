CREATE PROC dbo.sp_regla_calculo_comision_supervisor_validar
@codigo_campo_santo int,
@codigo_empresa int,
@codigo_canal_grupo int,
@codigo_regla int,
@cantidad int output
AS
BEGIN
	
	SELECT @cantidad = count(r.codigo_regla) FROM regla_calculo_comision_supervisor r
	WHERE r.codigo_campo_santo = @codigo_campo_santo AND
	r.codigo_empresa = @codigo_empresa AND
	r.codigo_canal_grupo = @codigo_canal_grupo AND r.estado_registro = 1 AND
	(@codigo_regla = 0 OR (r.codigo_regla <> @codigo_regla))

	RETURN;
END