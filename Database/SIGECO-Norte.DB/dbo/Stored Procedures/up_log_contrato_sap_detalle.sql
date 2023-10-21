CREATE PROCEDURE dbo.up_log_contrato_sap_detalle
(
	 @p_Codigo_empresa	NVARCHAR(4)
	,@p_NumAtCard		NVARCHAR(100)
)
AS
BEGIN
	SELECT
		cc.NumAtCard as nro_contrato
		,e.nombre as nombre_empresa
		,p.nombre + ' ' + p.apellido_paterno + ' ' + p.apellido_materno AS personal
		,(SELECT COUNT(dc.Codigo_empresa) FROM dbo.detalle_contrato dc WHERE dc.Codigo_empresa = cc.Codigo_empresa AND dc.NumAtCard = cc.NumAtCard) AS nro_articulos
		,(SELECT COUNT(ccu.Codigo_empresa) FROM dbo.contrato_cuota ccu WHERE ccu.Codigo_empresa = cc.Codigo_empresa AND ccu.NumAtCard = cc.NumAtCard) AS nro_cuotas
		,CONVERT(VARCHAR(10), cm.Fec_Creacion, 103) + ' ' + CONVERT(VARCHAR(8), cm.Fec_Creacion, 108) AS fecha_migracion
		,CONVERT(VARCHAR(10), cc.CreateDate, 103) + ' ' + CONVERT(VARCHAR(8), cc.CreateDate, 108) AS fecha_contrato
		,ep.nombre AS estado
		,CONVERT(VARCHAR(10), cm.Fec_Proceso, 103) + ' ' + CONVERT(VARCHAR(8), cm.Fec_Proceso, 108) AS fecha_proceso
		,cm.Observacion AS observacion
		,ISNULL(tv.nombre, cc.Cod_Tipo_Venta) AS tipo_venta
		,ISNULL(tp.nombre, cc.Cod_FormaPago) AS tipo_pago
	FROM
		dbo.cabecera_contrato cc
	INNER JOIN
		dbo.contrato_migrado cm
		ON cc.Codigo_empresa = cm.Codigo_empresa AND cc.NumAtCard = cm.NumAtCard
	LEFT JOIN 
		dbo.empresa_sigeco e
		ON e.codigo_equivalencia = cc.Codigo_empresa
	LEFT JOIN
		dbo.personal p
		ON p.codigo_equivalencia = cc.Cod_Vendedor
	LEFT JOIN 
		dbo.tipo_venta tv
		ON tv.codigo_equivalencia = cc.Cod_Tipo_Venta
	LEFT JOIN 
		dbo.tipo_pago tp
		ON tp.codigo_equivalencia = cc.Cod_FormaPago
	LEFT JOIN
		dbo.estado_proceso ep
		ON ep.codigo_estado_proceso = cm.codigo_estado_proceso
	WHERE
		cc.Codigo_empresa = @p_Codigo_empresa
		AND cc.NumAtCard = @p_NumAtCard
END