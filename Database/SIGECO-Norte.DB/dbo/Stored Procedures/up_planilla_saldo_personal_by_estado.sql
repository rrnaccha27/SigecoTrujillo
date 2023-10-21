CREATE PROCEDURE [dbo].[up_planilla_saldo_personal_by_estado](
	@codigo_planilla int,
	@codigo_personal int,
	@codigo_estado_cuota int
)
AS
BEGIN
	declare @tb_descuento table(
		codigo_empresa int,
		monto decimal(10,2),
		motivo varchar(300)
	);

	insert into @tb_descuento
	select
		codigo_empresa,
		sum(monto),
		max(motivo)
	from descuento d where codigo_planilla=@codigo_planilla and codigo_personal=@codigo_personal and estado_registro=1
	group by codigo_empresa;

	select 
		cpc.codigo_empresa,	
		max(e.nombre) as nombre_empresa,
		SUM(dc.monto_bruto) as monto_bruto,
		SUM(dc.igv) as igv,
		SUM(dc.monto_neto) monto_neto,
		isnull(max(td.monto),0) monto_descuento,
		SUM(dc.monto_neto)-isnull(max(td.monto),0) as monto_afecto_descuento,
		max(motivo) motivo,
		(case when max(td.monto)>0 then 1 else 0 end)tiene_descuento
	from cronograma_pago_comision cpc 
	inner join empresa_sigeco e on e. codigo_empresa=cpc.codigo_empresa
	inner join personal_canal_grupo pcg on (cpc.codigo_personal_canal_grupo=pcg.codigo_registro and pcg.codigo_personal=@codigo_personal)
	inner join detalle_cronograma dc on cpc.codigo_cronograma=dc.codigo_cronograma
	inner join detalle_planilla dp on (dp.codigo_detalle_cronograma=dc.codigo_detalle and dp.codigo_planilla=@codigo_planilla)
	left join @tb_descuento td on td.codigo_empresa=cpc.codigo_empresa 
	where dp.codigo_planilla=@codigo_planilla  and pcg.codigo_personal=@codigo_personal and dp.excluido=0
	and dc.codigo_estado_cuota=@codigo_estado_cuota and dc.es_registro_manual_comision = 0
	group by cpc.codigo_empresa
	order by cpc.codigo_empresa
END;