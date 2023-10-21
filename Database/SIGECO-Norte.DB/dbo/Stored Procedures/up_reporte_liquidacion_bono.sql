create proc up_reporte_liquidacion_bono(
@p_codigo_planilla int,
@p_codigo_personal int
)
as
begin

set @p_codigo_personal=case when @p_codigo_personal=0 then null else @p_codigo_personal end;
print @p_codigo_personal
 declare
 @monto_limite_detraccion decimal(10,2),
 @porcentaje_detraccion decimal(10,2),
 @fecha_inicio varchar(20),
 @fecha_fin varchar(20);
 select @monto_limite_detraccion=Convert(numeric,valor) from parametro_sistema where codigo_parametro_sistema=16;
 select @porcentaje_detraccion=Convert(numeric,valor)/100 from parametro_sistema where codigo_parametro_sistema=15;

 select @fecha_inicio=CONVERT(NVARCHAR(10), fecha_inicio, 103),@fecha_fin=CONVERT(NVARCHAR(10), fecha_fin, 103) from planilla_bono where codigo_planilla=@p_codigo_planilla;

 
 declare @resumen_empresa table(
 codigo_personal int,
 apellidos_nombres varchar(200),
 codigo_empresa int,
 nombre_empresa varchar(200),
 nombre_empresa_largo varchar(200),
 monto_contratado_empresa decimal(10,2),
 monto_ingresado_empresa decimal(10,2),
 monto_bruto_empresa decimal(10,2),
 monto_igv_empresa decimal(10,2),
 detraccion_empresa decimal(10,2),
 monto_neto_empresa decimal(10,2) ,
 monto_bono_total_persona decimal(10,2) 
 );
 insert into @resumen_empresa
 select 
 dp.codigo_personal,
 max(ISNULL(p.apellido_paterno,'') +' '+ISNULL(p.apellido_materno, '')+', '+p.nombre),
 dp.codigo_empresa,
 max(e.nombre),
 max(e.nombre_largo),
 
 sum(dp.monto_contratado),
 sum(dp.monto_ingresado),
 sum(dp.monto_bruto),
 sum(dp.monto_igv), 
 0,
 sum(dp.monto_neto),
 0
 from detalle_planilla_bono dp
 inner join personal p on dp.codigo_personal=p.codigo_personal
 inner join empresa_sigeco e on dp.codigo_empresa=e.codigo_empresa
  where dp.codigo_planilla=@p_codigo_planilla and dp.codigo_personal=isnull(@p_codigo_personal,dp.codigo_personal)
 group by dp.codigo_personal,dp.codigo_empresa;

 
 /************************************************************************************************
 CALCULANDO EL DETRACCION BONO POR EMPRESA
 *************************************************************************************************/
 update @resumen_empresa set detraccion_empresa=case when monto_neto_empresa>@monto_limite_detraccion then @porcentaje_detraccion*monto_neto_empresa else 0 end;
 
 /************************************************************************************************
 CALCULANDO EL BONO TOTAL MENOS DETRACCION  POR EMPRESA
 *************************************************************************************************/
 update @resumen_empresa set monto_neto_empresa=monto_neto_empresa-detraccion_empresa;

 /************************************************************************************************
 CALCULANDO EL BONO TOTAL POR PERSONA
 *************************************************************************************************/
 update @resumen_empresa set monto_bono_total_persona=
 (
 select sum(monto_neto_empresa) from @resumen_empresa  re where re.codigo_personal=codigo_personal
 );
 
 
 
 
 select 
 @fecha_inicio as fecha_inicio,
 @fecha_fin as fecha_fin,
 ------------------------------------------
 t.codigo_personal,
 r.apellidos_nombres,
 t.monto_contratado_personal,
 t.monto_ingresado_personal,
 r.monto_bono_total_persona,
 -----------------------------------------------
 r.codigo_empresa,
 r.nombre_empresa,
 r.nombre_empresa_largo,
 r.monto_contratado_empresa,
 r.monto_ingresado_empresa,
 r.monto_bruto_empresa,
 r.monto_igv_empresa,
 r.detraccion_empresa,
 r.monto_neto_empresa,
 -------------------------------------------------------
 t.codigo_grupo,
 t.nombre_grupo,
 t.numero_contrato,
 t.nombre_tipo_venta,
 t.monto_contratado,
 t.monto_ingresado,
 t.porcentaje_pago,
 t.importe_bono_detalle 
 from (
 SELECT 
 ----------------------------------------
 d.codigo_personal,
 rp.monto_contratado as monto_contratado_personal,
 rp.monto_ingresado as monto_ingresado_personal,
 rp.meta_lograda,
 rp.porcentaje_pago,
 -----------------------------------------
 d.codigo_empresa,
 d.codigo_grupo, 
 b.numero_contrato,
 b.codigo_tipo_venta,
 tv.abreviatura as nombre_tipo_venta, 
 cg.nombre as nombre_grupo,
 b.monto_contratado,
 b.monto_ingresado, 
 b.monto_ingresado*(rp.porcentaje_pago/100) importe_bono_detalle
 FROM detalle_planilla_bono d
 inner join resumen_planilla_bono rp on d.codigo_personal=rp.codigo_personal 
 inner join contrato_planilla_bono b on d.codigo_empresa=b.codigo_empresa and 
 d.codigo_personal=b.codigo_personal and 
 d.codigo_planilla=b.codigo_planilla
 inner join tipo_venta tv on tv.codigo_tipo_venta=b.codigo_tipo_venta
 left join canal_grupo cg on cg.codigo_canal_grupo=d.codigo_grupo
 where d.codigo_planilla=@p_codigo_planilla and d.codigo_personal=isnull(@p_codigo_personal,d.codigo_personal)
 ) t inner join @resumen_empresa r on t.codigo_empresa=r.codigo_empresa and t.codigo_personal=r.codigo_personal
 
  end;