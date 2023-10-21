CREATE PROCEDURE dbo.up_reporte_finanzas_filtro
(
	@p_tipo						INT
	,@p_codigo_canal			VARCHAR(100)
	,@p_codigo_tipo_planilla	VARCHAR(100)
	,@p_codigo_tipo_reporte		INT
	,@p_periodo					VARCHAR(50)
	,@p_anio					INT
)
AS
BEGIN
	SET NOCOUNT ON

	DECLARE
		@v_tipo varchar(250) = ''
		,@v_canal varchar(250) = ''
		,@v_tipo_planilla varchar(250) = ''
		,@v_tipo_reporte varchar(250) = ''
		,@v_periodo varchar(250) = ''
		,@v_anio varchar(250) = ''

	DECLARE @t_periodo TABLE(
		periodo INT
		,nombre_periodo VARCHAR(20)
	)

	INSERT INTO @t_periodo VALUES(1, 'ENERO');INSERT INTO @t_periodo VALUES(2, 'FEBRERO');INSERT INTO @t_periodo VALUES(3, 'MARZO');INSERT INTO @t_periodo VALUES(4, 'ABRIL');
	INSERT INTO @t_periodo VALUES(5, 'MAYO');INSERT INTO @t_periodo VALUES(6, 'JUNIO');INSERT INTO @t_periodo VALUES(7, 'JULIO');INSERT INTO @t_periodo VALUES(8, 'AGOSTO');
	INSERT INTO @t_periodo VALUES(9, 'SETIEMBRE');INSERT INTO @t_periodo VALUES(10, 'OCTUBRE');INSERT INTO @t_periodo VALUES(11, 'NOVIEMBRE');INSERT INTO @t_periodo VALUES(12, 'DICIEMBRE');

	SET @v_tipo = (CASE @p_tipo WHEN 1 THEN 'COMISION' WHEN 2 THEN 'BONO' ELSE '' END)

	SELECT @v_canal = @v_canal + ', ' +  UPPER(canal.nombre)
	FROM dbo.canal_grupo canal 
	INNER JOIN dbo.fn_SplitReporteGeneral(@p_codigo_canal) c on canal.codigo_canal_grupo = c.codigo AND canal.es_canal_grupo = 1
	ORDER by c.codigo

	SELECT @v_tipo_planilla = @v_tipo_planilla + ', ' + + UPPER(pl.nombre)
	FROM dbo.tipo_planilla pl
	INNER JOIN dbo.fn_SplitReporteGeneral(@p_codigo_tipo_planilla) tpl on tpl.codigo = pl.codigo_tipo_planilla
	ORDER BY tpl.codigo

	SET @v_tipo_reporte = (CASE @p_codigo_tipo_reporte WHEN 1 THEN 'PAGADO' WHEN 2 THEN 'GENERADO' ELSE '' END)

	SELECT @v_periodo = @v_periodo + ', ' + t.nombre_periodo
	FROM @t_periodo t
	INNER JOIN dbo.fn_SplitReporteGeneral(@p_periodo) p on p.codigo = t.periodo 
	ORDER BY p.codigo

	SET @v_anio = (CONVERT(VARCHAR(250), @p_anio))

	SELECT
		@v_tipo as tipo
		,SUBSTRING(@v_canal, 3, 250) as canal
		,SUBSTRING(@v_tipo_planilla, 3, 250) as tipo_planilla
		,@v_tipo_reporte as tipo_reporte
		,SUBSTRING(@v_periodo, 3, 250) as periodo
		,@v_anio as anio

	SET NOCOUNT OFF
END;