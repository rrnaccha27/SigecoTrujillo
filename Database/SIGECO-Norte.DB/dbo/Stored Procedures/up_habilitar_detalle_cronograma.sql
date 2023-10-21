create PROC [dbo].[up_habilitar_detalle_cronograma]
(
	@p_codigo_tipo_planilla	INT,
	@p_fecha_fin			DATETIME
)
AS
BEGIN
	SET NOCOUNT ON

	DECLARE
		@t_Cuotas_Pagadas TABLE
		(
			id					int identity(1, 1),
			codigo_empresa		int,
			empresa				nvarchar(4),
			nro_contrato		nvarchar(100),
			nro_cuota			int,
			fecha_habilitacion	datetime,
			es_contrato_doble	bit,
			codigo_cronograma	int
		)

	DECLARE
		@c_OFSA				CHAR(4) = '0002'
		,@c_FUNJAR			CHAR(4) = '0939'
		,@c_CANCELADA		CHAR(1) = 'C'

	INSERT INTO @t_Cuotas_Pagadas (codigo_empresa, empresa, nro_contrato, nro_cuota, fecha_habilitacion, es_contrato_doble, codigo_cronograma)
	SELECT 
		e.codigo_empresa
		,cc.Codigo_empresa
		,cc.NumAtCard
		,convert(int, (cc.Num_Cuota + 1))
		,cc.Fec_Pago
		,dbo.fn_EsContratoDoble(cc.NumAtCard, cc.Codigo_empresa)
		,cpc.codigo_cronograma
	FROM 
		dbo.contrato_cuota cc
	INNER JOIN
		dbo.empresa_sigeco e
		on e.codigo_equivalencia = cc.Codigo_empresa
	INNER JOIN
		dbo.cronograma_pago_comision cpc
		ON cpc.codigo_empresa = e.codigo_empresa AND cpc.nro_contrato = cc.NumAtCard AND cpc.codigo_tipo_planilla = @p_codigo_tipo_planilla
	WHERE 
		--cc.Num_Cuota > 0
		--AND 
		cc.Cod_Estado = @c_CANCELADA 
		AND cc.Fec_Pago IS NOT NULL
		AND cc.Fec_Pago <= @p_fecha_fin

	--Actualizar contratos FUNJAR que no son contratos dobles
	UPDATE dc
	SET
		dc.fecha_programada = c.fecha_habilitacion
	FROM dbo.detalle_cronograma dc
	INNER JOIN @t_Cuotas_Pagadas c 
		ON dc.codigo_cronograma = c.codigo_cronograma and dc.nro_cuota = c.nro_cuota and c.es_contrato_doble = 0 and c.empresa = @c_FUNJAR
	WHERE
		dc.fecha_programada IS NULL
		AND dc.codigo_estado_cuota = 1

	DELETE FROM @t_Cuotas_Pagadas
	WHERE es_contrato_doble = 0 and empresa = @c_FUNJAR


	--Actualizar contratos FUNJAR que contratos dobles
	UPDATE dc
	SET
		dc.fecha_programada = (SELECT TOP 1 d.fecha_habilitacion from @t_Cuotas_Pagadas d WHERE d.nro_contrato = c.nro_contrato and d.empresa = @c_OFSA AND d.nro_cuota = c.nro_cuota)
	FROM dbo.detalle_cronograma dc
	INNER JOIN 
		(SELECT cpc2.codigo_cronograma, c.nro_contrato, c.nro_cuota FROM dbo.cronograma_pago_comision cpc1
		INNER JOIN dbo.cronograma_pago_comision cpc2 
			on cpc1.codigo_empresa = 1 and cpc2.codigo_empresa = 2 and cpc1.codigo_tipo_planilla = @p_codigo_tipo_planilla and cpc1.nro_contrato = cpc2.nro_contrato and cpc1.codigo_tipo_planilla = cpc2.codigo_tipo_planilla
		INNER JOIN @t_Cuotas_Pagadas c 
			ON cpc1.nro_contrato = c.nro_contrato and c.es_contrato_doble = 1 and c.empresa = @c_OFSA
		WHERE cpc1.codigo_empresa = 1 and cpc2.codigo_empresa = 2) c
		ON dc.codigo_cronograma = c.codigo_cronograma and dc.nro_cuota = c.nro_cuota
	WHERE
		dc.fecha_programada IS NULL
		AND dc.codigo_estado_cuota = 1

	UPDATE dc
	SET
		dc.fecha_programada = 
		CASE WHEN NOT EXISTS(SELECT NumAtCard FROM contrato_cuota cc WHERE cc.NumAtCard = c.nro_contrato AND cc.Codigo_empresa = @c_OFSA AND (cc.Num_Cuota + 1) = c.nro_cuota AND Cod_Estado = @c_CANCELADA AND Fec_Pago IS NOT NULL) then
			(SELECT TOP 1 d.fecha_habilitacion FROM @t_Cuotas_Pagadas d WHERE d.nro_contrato = c.nro_contrato and d.empresa = @c_FUNJAR AND d.nro_cuota = c.nro_cuota)
		ELSE
			NULL
		END 
	FROM dbo.detalle_cronograma dc
	INNER JOIN @t_Cuotas_Pagadas c 
		ON dc.codigo_cronograma = c.codigo_cronograma and dc.nro_cuota = c.nro_cuota and c.es_contrato_doble = 1 and c.empresa = @c_FUNJAR
	WHERE
		dc.fecha_programada IS NULL
		AND dc.codigo_estado_cuota = 1

	DELETE FROM @t_Cuotas_Pagadas
	WHERE es_contrato_doble = 1 and empresa = @c_FUNJAR

	--Actualizar contratos OFSA todos
	UPDATE dc
	SET
		dc.fecha_programada = c.fecha_habilitacion
	FROM dbo.detalle_cronograma dc
	INNER JOIN @t_Cuotas_Pagadas c 
		ON dc.codigo_cronograma = c.codigo_cronograma and dc.nro_cuota = c.nro_cuota and c.empresa = @c_OFSA
	WHERE
		dc.fecha_programada IS NULL
		AND dc.codigo_estado_cuota = 1

	DELETE FROM @t_Cuotas_Pagadas
	WHERE empresa = @c_OFSA

	--Actualizar contratos restantes
	UPDATE dc
	SET
		dc.fecha_programada = c.fecha_habilitacion
	FROM dbo.detalle_cronograma dc
	INNER JOIN @t_Cuotas_Pagadas c 
		ON dc.codigo_cronograma = c.codigo_cronograma and dc.nro_cuota = c.nro_cuota
	WHERE
		dc.fecha_programada IS NULL
		AND dc.codigo_estado_cuota = 1

	DELETE FROM @t_Cuotas_Pagadas

	SET NOCOUNT OFF

END