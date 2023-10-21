CREATE PROCEDURE dbo.up_reporte_migracion_contratos
(
	@p_fecha_inicial	VARCHAR(10)
	,@p_fecha_final		VARCHAR(10)
)
AS
BEGIN
	SET NOCOUNT ON

	DECLARE @t_sap as table(
		codigo_empresa varchar(4)
		,numatcard varchar(100)
		,docentry int
		,db varchar(100)
		,codigo_vendedor varchar(25)
		,codigo_grupo varchar(25)
		,fecha_contrato datetime
	)

	DECLARE
		@v_fecha_inicio	DATETIME = CONVERT(DATETIME, @p_fecha_inicial)
		,@v_fecha_fin	DATETIME = CONVERT(DATETIME, @p_fecha_final + ' 23:59:59')

	--ofsa
	insert into @t_sap (codigo_empresa, numatcard, docentry, db, codigo_vendedor, codigo_grupo, fecha_contrato)
		select '0002', right('0000000000'+numatcard,10), docentry, 'DB_OPERACIONES_FUNERARIAS', U_JDLP_VENDEDOR, U_JDLP_GRUPOVENTA, sap.taxdate
		from SAP.db_operaciones_funerarias.dbo.ordr sap
		where 
			sap.taxdate between @v_fecha_inicio and @v_fecha_fin
			and (numatcard not like '%A%' or numatcard not like '%N%' or numatcard not like '%L%')
			and cardcode <> 'LIQUIDACIONES'

	--funjar
	insert into @t_sap (codigo_empresa, numatcard, docentry, db, codigo_vendedor, codigo_grupo, fecha_contrato)
		select '0939', right('0000000000'+numatcard,10), docentry, 'DB_FUNERARIA_JARDINES', U_JDLP_VENDEDOR, U_JDLP_GRUPOVENTA, sap.taxdate
		from SAP.db_funeraria_jardines.dbo.ordr sap
		where 
			sap.taxdate between @v_fecha_inicio and @v_fecha_fin
			and (numatcard not like '%A%' or numatcard not like '%N%' or numatcard not like '%L%')
			and cardcode <> 'LIQUIDACIONES'

	select 
		e.nombre as nombre_empresa, sap.numatcard as nro_contrato, convert(varchar, sap.fecha_contrato, 103) as fecha_contrato, sap.docentry
		,case when exists(select top 1 docentry from cabecera_contrato cab where cab.codigo_empresa = sap.codigo_empresa and cab.numatcard = sap.numatcard and isnull(cab.Cod_Estado_Contrato, '') <> 'ANL') then 'Migrado' else 'No Migrado' end as estado 
		,isnull((select top 1 convert(varchar, cm.Fec_Creacion,103) from contrato_migrado cm where cm.docentry = sap.docentry),'') as fecha_migracion
		,isnull(p.nombre_canal,'') as nombre_canal, isnull(p.nombre_grupo, '') as nombre_grupo, isnull(p.nombre_personal, '') as nombre_personal
		,sslog.EntityFramework.fn_obtenerintentos(sap.docentry, sap.db, 'RegistrarContrato') as intentos
	from @t_sap sap 
	left join empresa_sigeco e on e.codigo_equivalencia = sap.codigo_empresa
	left join vw_personal p on p.codigo_equivalencia = sap.codigo_vendedor and p.codigo_equivalencia_grupo = sap.codigo_grupo and p.estado_canal_grupo = 1
	order by sap.fecha_contrato, sap.numatcard, sap.codigo_empresa

	SET NOCOUNT OFF
END;