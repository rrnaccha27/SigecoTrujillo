CREATE PROCEDURE [dbo].up_detalle_planilla_bono_excluido_by_id
(
	@p_codigo_planilla int
)
AS
BEGIN
	SET NOCOUNT ON
	
	DECLARE
		@v_codigo_tipo_planilla INT

	SET @v_codigo_tipo_planilla = (SELECT TOP 1 codigo_tipo_planilla FROM planilla_bono WHERE codigo_planilla = @p_codigo_planilla)

	--SELECT 
	--	g.codigo_canal_grupo,
	--	g.nombre as nombre_grupo,
	--	p.codigo_personal,
	--	p.nombre + ISNULL(' ' + p.apellido_paterno,'') + ISNULL(' ' + p.apellido_materno,'') as nombre_persona,
	--	--dpb.monto_bruto,
	--	--dpb.monto_igv,
	--	--dpb.monto_neto,
	--	e.codigo_empresa,
	--	e.nombre as nombre_empresa,	
	--	dpb.codigo_canal,
	--	c.nombre as nombre_canal,	
	--	m.nombre as nombre_moneda
	--	,cpb.numero_contrato as nro_contrato
	--	,apb.codigo_articulo
	--	,a.nombre as nombre_articulo
	--	,0.00 as monto_bruto
	--	,0.00 as monto_igv
	--	,((apb.dinero_ingresado * rpb.porcentaje_pago) / 100) as monto_neto
	--	,apb.excluido_motivo
	--	,dbo.fn_obtener_nombre_usuario(apb.excluido_usuario) as excluido_usuario
	--	,convert(varchar, apb.excluido_fecha, 103) as excluido_fecha
	--FROM 
	--	detalle_planilla_bono dpb
	--inner join dbo.resumen_planilla_bono rpb on rpb.codigo_planilla = @p_codigo_planilla and rpb.codigo_personal = dpb.codigo_personal
	--inner join dbo.contrato_planilla_bono cpb on cpb.codigo_planilla = @p_codigo_planilla and case when @v_codigo_tipo_planilla = 1 then cpb.codigo_personal else cpb.codigo_supervisor end = dpb.codigo_personal and cpb.codigo_empresa = dpb.codigo_empresa
	--inner join dbo.articulo_planilla_bono apb on apb.codigo_planilla_bono = @p_codigo_planilla and apb.codigo_empresa = cpb.codigo_empresa and apb.nro_contrato = cpb.numero_contrato
	--inner join dbo.articulo a on a.codigo_articulo = apb.codigo_articulo
	--inner join personal p on dpb.codigo_personal=p.codigo_personal
	--inner join canal_grupo c on c.codigo_canal_grupo=dpb.codigo_canal
	--inner join canal_grupo g on g.codigo_canal_grupo=dpb.codigo_grupo
	--inner join empresa_sigeco e on e.codigo_empresa=dpb.codigo_empresa
	--inner join moneda m on m.codigo_moneda=dpb.codigo_moneda
	--WHERE 
	--	dpb.codigo_planilla = @p_codigo_planilla
	--	AND ((apb.dinero_ingresado * rpb.porcentaje_pago) / 100) > 0
	--	AND apb.excluido = 1
	--ORDER BY
	--	dpb.codigo_canal, g.nombre, p.nombre, dpb.codigo_empresa;
	
	SELECT
		codigo_canal_grupo, nombre_grupo, codigo_personal, nombre_persona, codigo_empresa, nombre_empresa, codigo_canal, nombre_canal, nombre_moneda, nro_contrato, codigo_articulo, nombre_articulo, monto_bruto, monto_igv, monto_neto, excluido_motivo, excluido_usuario, convert(varchar(10), excluido_fecha, 103) as excluido_fecha
	FROM 
		dbo.articulo_planilla_bono_excluido
	WHERE 
		codigo_planilla_bono = @p_codigo_planilla
	SET NOCOUNT OFF
END;