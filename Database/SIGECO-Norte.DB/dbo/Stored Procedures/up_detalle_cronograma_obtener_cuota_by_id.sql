CREATE PROCEDURE up_detalle_cronograma_obtener_cuota_by_id
(
	@p_codigo_detalle_cronograma	INT
)
AS
BEGIN
	declare 
		@v_codigo_cronograma int,
		@v_codigo_articulo int
		,@v_numero_cuota_maxima int
		,@v_nro_cuota_libre int,
		@v_total_nro_cuota_contrato int,
		@v_nro_contrato varchar(60),
		@v_codigo_equivalencia_empresa varchar(20),
		@v_nombre_articulo varchar(100),
		@v_nombre_empresa varchar(100)
		,@v_nombre_tipo_planilla varchar(50)
		,@v_codigo_planilla int = 0
		,@v_codigo_detalle_planilla int= 0
		,@v_codigo_estado_cuota int = 0;

	select top 1
		@v_codigo_cronograma = codigo_cronograma,
		@v_codigo_articulo = codigo_articulo
		,@v_codigo_estado_cuota = codigo_estado_cuota
	from detalle_cronograma 
	where codigo_detalle=@p_codigo_detalle_cronograma;

	select 
		 @v_numero_cuota_maxima=max(nro_cuota)
	from detalle_cronograma 
	where codigo_cronograma=@v_codigo_cronograma 
	and codigo_articulo=@v_codigo_articulo and codigo_estado_cuota in(1,2,3,4);

	select 
		@v_nro_contrato=cpc.nro_contrato,
		@v_codigo_equivalencia_empresa=e.codigo_equivalencia,
		@v_nombre_empresa=e.nombre,
		@v_nombre_articulo=a.nombre
		,@v_nombre_tipo_planilla = tp.nombre
	from 
		detalle_cronograma dp 
	inner join cronograma_pago_comision cpc
		on dp.codigo_cronograma=cpc.codigo_cronograma
	inner join empresa_sigeco e 
		on cpc.codigo_empresa=e.codigo_empresa
	inner join articulo a 
		on dp.codigo_articulo=a.codigo_articulo
	inner join dbo.tipo_planilla tp
		on tp.codigo_tipo_planilla = cpc.codigo_tipo_planilla
	where dp.codigo_detalle=@p_codigo_detalle_cronograma;

	/**********************************************************
	OBTENIENDO EL NRO DE CUOTA TOTAL DEL CONTRATO
	***********************************************************/
	select 
		@v_total_nro_cuota_contrato=isnull(max(cc.Num_Cuota),0)+1
	from contrato_cuota cc 
	where cc.NumAtCard=@v_nro_contrato and cc.Codigo_empresa=@v_codigo_equivalencia_empresa and UPPER(cc.Cod_Estado) in('C','P') ;
	/******************************************************/

	set @v_nro_cuota_libre=@v_total_nro_cuota_contrato-isnull(@v_numero_cuota_maxima,0);

	/* OBTENIENDO DATOS DE PLANILLA */
	IF (@v_codigo_estado_cuota = 2 OR @v_codigo_estado_cuota = 3)
	BEGIN
		SELECT TOP 1
			@v_codigo_planilla = p.codigo_planilla
			,@v_codigo_detalle_planilla = dp.codigo_detalle_planilla
		FROM
			dbo.detalle_planilla dp
		inner join dbo.planilla p
			on dp.codigo_planilla = p.codigo_planilla and p.codigo_estado_planilla in (1, 2)
		WHERE
			dp.codigo_detalle_cronograma = @p_codigo_detalle_cronograma
			AND dp.excluido = 0
			AND dp.estado_registro = 1;
	END

	SELECT TOP 1
		codigo_detalle,
		nro_cuota,
		@v_nombre_empresa as nombre_empresa,
		@v_nombre_articulo as nombre_articulo,
		@v_nro_contrato as nro_contrato,
		@v_total_nro_cuota_contrato as nro_total_contrato,
		@v_nro_cuota_libre as nro_cuota_libre_refinanciar,
		fecha_programada,
		monto_bruto,
		igv,
		monto_neto,
		codigo_estado_cuota
		,@v_nombre_tipo_planilla as nombre_tipo_planilla
		,@v_codigo_detalle_planilla as codigo_detalle_planilla
		,@v_codigo_planilla as codigo_planilla
	FROM 
		dbo.detalle_cronograma
	WHERE 
		codigo_detalle=@p_codigo_detalle_cronograma;
END;