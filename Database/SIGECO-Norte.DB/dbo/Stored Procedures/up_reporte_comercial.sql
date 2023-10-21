CREATE PROCEDURE [dbo].[up_reporte_comercial]
(
	@p_tipo						INT
	,@p_codigo_canal			VARCHAR(100)
	,@p_codigo_tipo_planilla	INT
	,@p_codigo_tipo_reporte		INT
	,@p_periodo					VARCHAR(50)
	,@p_anio					INT
)
AS
BEGIN
	SET NOCOUNT ON

	--declare 
	--	@p_codigo_canal varchar(100) = '1, 4'
	--	,@p_codigo_tipo_planilla int = 1
	--	,@p_codigo_tipo_reporte int = 1 --pagado
	--	,@p_periodo varchar(50) = '9, 7, 8'
	--	,@p_anio int = 2018
	--	,@p_tipo int = 1 

	DECLARE @t_data_bruto TABLE(
		periodo int
		,nombre_canal varchar(50)
		,nombre_grupo varchar(50)
		,monto_factura decimal(12, 2)
		,monto_planilla decimal(12, 2)
	)

	DECLARE @t_periodo TABLE(
		periodo INT
		,nombre_periodo VARCHAR(20)
	)

	INSERT INTO @t_periodo VALUES(1, 'ENERO');INSERT INTO @t_periodo VALUES(2, 'FEBRERO');INSERT INTO @t_periodo VALUES(3, 'MARZO');INSERT INTO @t_periodo VALUES(4, 'ABRIL');
	INSERT INTO @t_periodo VALUES(5, 'MAYO');INSERT INTO @t_periodo VALUES(6, 'JUNIO');INSERT INTO @t_periodo VALUES(7, 'JULIO');INSERT INTO @t_periodo VALUES(8, 'AGOSTO');
	INSERT INTO @t_periodo VALUES(9, 'SETIEMBRE');INSERT INTO @t_periodo VALUES(10, 'OCTUBRE');INSERT INTO @t_periodo VALUES(11, 'NOVIEMBRE');INSERT INTO @t_periodo VALUES(12, 'DICIEMBRE');

	IF @p_codigo_tipo_reporte = 1/*PAGADO*/ AND @p_tipo = 1/*COMISION*/
	BEGIN
		INSERT INTO @t_data_bruto
		SELECT 
			p.codigo, canal.nombre as canal, grupo.nombre as grupo , 
			case 
				when dpl.codigo_canal = 1 and dpl.codigo_empresa = 1 then --funes ofsa
					dpl.monto_bruto
				when dpl.codigo_canal = 2 and dpl.codigo_empresa = 2 and pl.codigo_tipo_planilla = 2 then --gestores funjar
					dpl.monto_bruto
				when dpl.codigo_canal = 2 and pl.codigo_tipo_planilla = 1 then --vend agencia
					dpl.monto_bruto
				when @p_codigo_tipo_planilla = 1 and dpl.codigo_canal = 4 then --vend representantes
					dpl.monto_bruto
				when @p_codigo_tipo_planilla = 2 and dpl.codigo_canal = 4 and dpl.codigo_empresa  = 1 then --sup representantes ofsa
					dpl.monto_bruto
				else
					0
				end
			as monto_factura
			,
			case 
				when dpl.codigo_canal = 1 and dpl.codigo_empresa = 2 then --funes funjar
					dpl.monto_bruto
				when dpl.codigo_canal = 2 and dpl.codigo_empresa = 1 and pl.codigo_tipo_planilla = 2 then --gestores ofsa
					dpl.monto_bruto
				when @p_codigo_tipo_planilla = 2 and dpl.codigo_canal = 4 and dpl.codigo_empresa  = 2 then --sup representantes funjar
					dpl.monto_bruto
				else
					0
				end
			as monto_planilla
		FROM 
			dbo.planilla pl 
		inner join dbo.detalle_planilla dpl on pl.codigo_estado_planilla = 2 and dpl.codigo_planilla = pl.codigo_planilla
		inner join dbo.fn_SplitReporteGeneral(@p_codigo_canal) c on c.codigo = dpl.codigo_canal
		inner join dbo.canal_grupo canal on canal.codigo_canal_grupo = dpl.codigo_canal and canal.es_canal_grupo = 1
		inner join dbo.canal_grupo grupo on grupo.codigo_canal_grupo = dpl.codigo_grupo and grupo.es_canal_grupo = 0
		inner join dbo.fn_SplitReporteGeneral(@p_periodo) p on p.codigo = month(pl.fecha_fin)
		WHERE 
			dpl.excluido = 0
			and year(pl.fecha_fin) = @p_anio 
			--and month(pl.fecha_fin) = @p_periodo
			and pl.codigo_tipo_planilla = @p_codigo_tipo_planilla
	END

	IF @p_codigo_tipo_reporte = 2/*GENERADO*/ AND @p_tipo = 1 /*COMISION*/
	BEGIN
		INSERT INTO @t_data_bruto	
		SELECT 
			p.codigo, canal.nombre as canal, grupo.nombre as grupo , 
			--cpc.nro_contrato,e.nombre,
			case 
				when pcg.codigo_canal = 1 and cpc.codigo_empresa = 1 then --funes ofsa
					dcr.monto_bruto
				when pcg.codigo_canal = 2 and cpc.codigo_empresa = 2 and cpc.codigo_tipo_planilla = 2 then --gestores funjar
					dcr.monto_bruto
				when pcg.codigo_canal = 2 and cpc.codigo_tipo_planilla = 1 then --vend agencia
					dcr.monto_bruto
				when @p_codigo_tipo_planilla = 1 and pcg.codigo_canal = 4 then --vend representantes
					dcr.monto_bruto
				when @p_codigo_tipo_planilla = 2 and pcg.codigo_canal = 4 and cpc.codigo_empresa  = 1 then --sup representantes ofsa
					dcr.monto_bruto
				else
					0
				end
			as monto_factura
			,
			case 
				when pcg.codigo_canal = 1 and cpc.codigo_empresa = 2 then --funes funjar
					dcr.monto_bruto
				when pcg.codigo_canal = 2 and cpc.codigo_empresa = 1 and cpc.codigo_tipo_planilla = 2 then --gestores ofsa
					dcr.monto_bruto
				when @p_codigo_tipo_planilla = 2 and pcg.codigo_canal = 4 and cpc.codigo_empresa  = 2 then --sup representantes funjar
					dcr.monto_bruto
				else
					0
				end
			as monto_planilla
		FROM 
			dbo.cronograma_pago_comision cpc
		inner join dbo.empresa_sigeco e on e.codigo_empresa = cpc.codigo_empresa
		inner join dbo.cabecera_contrato cc on cc.NumAtCard = cpc.nro_contrato and cc.Codigo_empresa = e.codigo_equivalencia
		inner join dbo.personal_canal_grupo pcg on pcg.codigo_registro = cpc.codigo_personal_canal_grupo
		inner join dbo.fn_SplitReporteGeneral(@p_codigo_canal) c on c.codigo = pcg.codigo_canal
		inner join dbo.canal_grupo canal on canal.codigo_canal_grupo = pcg.codigo_canal and canal.es_canal_grupo = 1
		inner join dbo.canal_grupo grupo on grupo.codigo_canal_grupo = pcg.codigo_canal_grupo and grupo.es_canal_grupo = 0
		inner join dbo.detalle_cronograma dcr on dcr.codigo_cronograma = cpc.codigo_cronograma
		inner join dbo.fn_SplitReporteGeneral(@p_periodo) p on p.codigo = month(cc.CreateDate)
		WHERE 
			cc.Cod_Estado_Contrato = ''
			and year(cc.CreateDate) = @p_anio 
			--and month(cc.CreateDate) = @p_periodo
			and cpc.codigo_tipo_planilla = @p_codigo_tipo_planilla
	END

	IF @p_tipo = 2 /*BONO*/
	BEGIN
		INSERT INTO @t_data_bruto
		SELECT 
			p.codigo, canal.nombre as canal, grupo.nombre as grupo , 
			case 
				when @p_codigo_tipo_planilla = 2 and dpl.codigo_canal = 1 and dpl.codigo_empresa = 2 then --sup funes funjar
					dpl.monto_bruto
				when @p_codigo_tipo_planilla = 2 and dpl.codigo_canal = 2 and dpl.codigo_empresa = 2 then --sup gestores funjar
					dpl.monto_bruto
				when @p_codigo_tipo_planilla = 1 and dpl.codigo_canal = 4 then --vend representantes
					dpl.monto_bruto
				when @p_codigo_tipo_planilla = 2 and dpl.codigo_canal = 4 and dpl.codigo_empresa  = 1 then --sup representantes ofsa
					dpl.monto_bruto
				else
					0
				end
			as monto_factura
			,
			case 
				when @p_codigo_tipo_planilla = 2 and dpl.codigo_canal = 1 and dpl.codigo_empresa = 1 then --sup funes ofsa
					dpl.monto_bruto
				when @p_codigo_tipo_planilla = 2  and dpl.codigo_canal = 2 and dpl.codigo_empresa = 1 then --sup gestores ofsa
					dpl.monto_bruto
				when @p_codigo_tipo_planilla = 2 and dpl.codigo_canal = 4 and dpl.codigo_empresa  = 2 then --sup representantes funjar
					dpl.monto_bruto
				else
					0
				end
			as monto_planilla
		FROM 
			dbo.planilla_bono pl 
		inner join dbo.detalle_planilla_bono dpl on pl.codigo_estado_planilla = 2 and dpl.codigo_planilla = pl.codigo_planilla
		inner join dbo.fn_SplitReporteGeneral(@p_codigo_canal) c on c.codigo = dpl.codigo_canal
		inner join dbo.canal_grupo canal on canal.codigo_canal_grupo = dpl.codigo_canal and canal.es_canal_grupo = 1
		inner join dbo.canal_grupo grupo on grupo.codigo_canal_grupo = dpl.codigo_grupo and grupo.es_canal_grupo = 0
		inner join dbo.fn_SplitReporteGeneral(@p_periodo) p on p.codigo = month(pl.fecha_fin)
		WHERE 
			/*dpl.excluido = 0  --SE DEBE REVISAR COMO HACER CON LAS EXCLUSIONES PUES SON POR ARTICULO  */
			year(pl.fecha_fin) = @p_anio 
			--and month(pl.fecha_fin) = @p_periodo
			and pl.codigo_tipo_planilla = @p_codigo_tipo_planilla
	END

	SELECT 
		@p_tipo as tipo, 
		(SELECT TOP 1 nombre_periodo FROM @t_periodo WHERE periodo = d.periodo) as nombre_periodo, 
		nombre_canal, 
		nombre_grupo, 
		sum(monto_factura) as monto_factura, 
		sum(monto_planilla) as monto_planilla 
	FROM 
		@t_data_bruto d
	GROUP BY 
		periodo, nombre_canal, nombre_grupo
	ORDER BY 
		periodo, nombre_canal, nombre_grupo

	SET NOCOUNT OFF
END;