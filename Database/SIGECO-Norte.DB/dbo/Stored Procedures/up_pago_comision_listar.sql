CREATE PROCEDURE up_pago_comision_listar
(
	@p_codigo_estado_cuota int,
	@p_codigo_grupo int,
	@p_codigo_canal int,
	@p_codigo_tipo_planilla int,
	@p_codigo_personal int,
	@p_fecha_habilitado_inicio date,
	@p_fecha_habilitado_fin  date,
	@p_fecha_contrato_inicio  date,
	@p_fecha_contrato_fin date
)
AS
BEGIN  

	declare @v_fecha_inicio date,
		    @v_fecha_fin date;

	set @v_fecha_inicio=@p_fecha_habilitado_inicio;
	set @v_fecha_fin=@p_fecha_habilitado_fin;

	/*
	print @p_fecha_habilitado_inicio;
	select @v_fecha_inicio=convert(varchar(10),@p_fecha_habilitado_inicio,103),
	@v_fecha_fin=convert(varchar(10),@p_fecha_habilitado_fin,103);
	print @v_fecha_inicio;
	*/

   select 
		@p_codigo_estado_cuota=(case when @p_codigo_estado_cuota>0  then @p_codigo_estado_cuota else null end ),	  
		@p_codigo_grupo=(case when @p_codigo_grupo>0 then @p_codigo_grupo else null end ),	 
		@p_codigo_canal=(case when @p_codigo_canal>0   then @p_codigo_canal else null end ),	 
		@p_codigo_tipo_planilla=(case when @p_codigo_tipo_planilla>0 then @p_codigo_tipo_planilla else null end ),     
		@p_codigo_personal=(case when @p_codigo_personal>0 then @p_codigo_personal  else null end )

	if @p_codigo_estado_cuota=1 begin --- pendiente
		select      
			null as numero_planilla,
			null as nombre_regla,
			art.nombre as nombre_articulo,
			cg.nombre as nombre_canal,
			cgrp.nombre as nombre_grupo,
			isnull(ps.nombre,'')+' '+isnull(ps.apellido_paterno,'')+' '+isnull(ps.apellido_materno,'') as datos_vendedor,
			emp.nombre as nombre_empresa,
			tv.abreviatura as nombre_tipo_venta,
			tp.nombre as nombre_tipo_pago,
			cpc.nro_contrato,	 
			convert(varchar(10),dc.fecha_programada, 103) as fecha_programada,
			dc.monto_bruto,
			dc.monto_neto,
			dc.igv,
			dc.nro_cuota,	 
			ec.nombre as nombre_estado_cuota,
			dc.estado_registro
	   from 
			cronograma_pago_comision cpc
		inner join 
			empresa_sigeco emp on cpc.codigo_empresa=emp.codigo_empresa
		inner join
			cabecera_contrato cct on (cpc.nro_contrato=cct.NumAtCard and cct.Codigo_empresa=emp.codigo_equivalencia)
		inner join 
			tipo_venta tv on cpc.codigo_tipo_pago=tv.codigo_tipo_venta
		inner join 
			tipo_pago tp on cpc.codigo_tipo_pago=tp.codigo_tipo_pago
		inner join 
			personal_canal_grupo pcg on cpc.codigo_personal_canal_grupo=pcg.codigo_registro
		inner join
			personal ps on ps.codigo_personal=pcg.codigo_personal
		inner join 
			canal_grupo cg on pcg.codigo_canal=cg.codigo_canal_grupo
		inner join   
			detalle_cronograma dc  on dc.codigo_cronograma=cpc.codigo_cronograma 
		inner join
			articulo art on dc.codigo_articulo=art.codigo_articulo
		inner join  
			estado_cuota ec on dc.codigo_estado_cuota=ec.codigo_estado_cuota      
		left join
			canal_grupo cgrp on pcg.codigo_canal_grupo=cgrp.codigo_canal_grupo
		where
			dc.codigo_estado_cuota=isnull(@p_codigo_estado_cuota,dc.codigo_estado_cuota )
			and
			pcg.codigo_canal_grupo=isnull(@p_codigo_grupo,pcg.codigo_canal_grupo)
			and
			pcg.codigo_canal=isnull(@p_codigo_canal,pcg.codigo_canal )
			and
			cpc.codigo_tipo_planilla=isnull(@p_codigo_tipo_planilla,cpc.codigo_tipo_planilla)
			and
			pcg.codigo_personal=isnull(@p_codigo_personal,pcg.codigo_personal) 
			and 
			--isnull(dc.fecha_programada,0) between isnull(@v_fecha_inicio,isnull(dc.fecha_programada,0)) and isnull(@v_fecha_fin,isnull(dc.fecha_programada,0))
			--and
			cct.CreateDate between isnull(@p_fecha_contrato_inicio,cct.CreateDate) and isnull(@p_fecha_contrato_fin,cct.CreateDate)
		order by
			emp.codigo_empresa asc,
			cpc.nro_contrato asc,
			art.nombre asc,
			dc.nro_cuota asc
	end;

	if @p_codigo_estado_cuota=2 begin ---en proceso pago
		select      
			p.numero_planilla,
			rtp.nombre as nombre_regla,
			art.nombre as nombre_articulo,
			cg.nombre as nombre_canal,
			cgrp.nombre as nombre_grupo,
			isnull(ps.nombre,'')+' '+isnull(ps.apellido_paterno,'')+' '+isnull(ps.apellido_materno,'') as datos_vendedor,
			emp.nombre as nombre_empresa,
			tv.abreviatura as nombre_tipo_venta,
			tp.nombre as nombre_tipo_pago,
			cpc.nro_contrato,	 
			convert(varchar(10),dc.fecha_programada, 103) as fecha_programada,
			dc.monto_bruto,
			dc.monto_neto,
			dc.igv,
			dc.nro_cuota,	 
			ec.nombre as nombre_estado_cuota,
			dc.estado_registro
		from 
			cronograma_pago_comision cpc
		inner join 
			empresa_sigeco emp on cpc.codigo_empresa=emp.codigo_empresa
		inner join
			cabecera_contrato cct on (cpc.nro_contrato=cct.NumAtCard and cct.Codigo_empresa=emp.codigo_equivalencia)
		inner join 
			tipo_venta tv on cpc.codigo_tipo_pago=tv.codigo_tipo_venta
		inner join 
			tipo_pago tp on cpc.codigo_tipo_pago=tp.codigo_tipo_pago
		inner join 
			personal_canal_grupo pcg on cpc.codigo_personal_canal_grupo=pcg.codigo_registro
		inner join
			personal ps on ps.codigo_personal=pcg.codigo_personal
		inner join 
			canal_grupo cg on pcg.codigo_canal=cg.codigo_canal_grupo
		inner join   
			detalle_cronograma dc  on dc.codigo_cronograma=cpc.codigo_cronograma 
		inner join
			articulo art on dc.codigo_articulo=art.codigo_articulo
		inner join  
			estado_cuota ec on dc.codigo_estado_cuota=ec.codigo_estado_cuota  
		inner join
			detalle_planilla dpl on dc.codigo_detalle=dpl.codigo_detalle_cronograma and dpl.excluido = 0
		inner join 
			planilla p on dpl.codigo_planilla=p.codigo_planilla and p.codigo_estado_planilla=1
		inner join 
			regla_tipo_planilla rtp on rtp.codigo_regla_tipo_planilla=p.codigo_regla_tipo_planilla    
		left join
			canal_grupo cgrp on pcg.codigo_canal_grupo=cgrp.codigo_canal_grupo
		where
			dc.codigo_estado_cuota=isnull(@p_codigo_estado_cuota,dc.codigo_estado_cuota )
			and
			pcg.codigo_canal_grupo=isnull(@p_codigo_grupo,pcg.codigo_canal_grupo)
			and
			pcg.codigo_canal=isnull(@p_codigo_canal,pcg.codigo_canal )
			and
			cpc.codigo_tipo_planilla=isnull(@p_codigo_tipo_planilla,cpc.codigo_tipo_planilla)
			and
			pcg.codigo_personal=isnull(@p_codigo_personal,pcg.codigo_personal) 
			and 
			p.fecha_inicio between isnull(@v_fecha_inicio,p.fecha_inicio) and isnull(@v_fecha_fin,p.fecha_inicio)
			and
			cct.CreateDate between isnull(@p_fecha_contrato_inicio,cct.CreateDate) and isnull(@p_fecha_contrato_fin,cct.CreateDate)
		order by
			emp.codigo_empresa asc,
			cpc.nro_contrato asc,
			art.nombre asc,
			dc.nro_cuota asc
	end;

	if @p_codigo_estado_cuota=3 begin ---pagado
		select      
			p.numero_planilla,
			rtp.nombre as nombre_regla,
			art.nombre as nombre_articulo,
			cg.nombre as nombre_canal,
			cgrp.nombre as nombre_grupo,
			isnull(ps.nombre,'')+' '+isnull(ps.apellido_paterno,'')+' '+isnull(ps.apellido_materno,'') as datos_vendedor,
			emp.nombre as nombre_empresa,
			tv.abreviatura as nombre_tipo_venta,
			tp.nombre as nombre_tipo_pago,
			cpc.nro_contrato,	 
			convert(varchar(10),dc.fecha_programada, 103) as fecha_programada,
			dc.monto_bruto,
			dc.monto_neto,
			dc.igv,
			dc.nro_cuota,	 
			ec.nombre as nombre_estado_cuota,
			dc.estado_registro
		from 
			cronograma_pago_comision cpc
		inner join 	 
			empresa_sigeco emp on cpc.codigo_empresa=emp.codigo_empresa
		inner join
			cabecera_contrato cct on (cpc.nro_contrato=cct.NumAtCard and cct.Codigo_empresa=emp.codigo_equivalencia)     
		inner join 
			tipo_venta tv on cpc.codigo_tipo_pago=tv.codigo_tipo_venta
		inner join 
			tipo_pago tp on cpc.codigo_tipo_pago=tp.codigo_tipo_pago
		inner join 
			personal_canal_grupo pcg on cpc.codigo_personal_canal_grupo=pcg.codigo_registro
		inner join
			personal ps on ps.codigo_personal=pcg.codigo_personal
		inner join 
			canal_grupo cg on pcg.codigo_canal=cg.codigo_canal_grupo
		inner join   
			detalle_cronograma dc  on dc.codigo_cronograma=cpc.codigo_cronograma 
		inner join
			articulo art on dc.codigo_articulo=art.codigo_articulo
		inner join  
			estado_cuota ec on dc.codigo_estado_cuota=ec.codigo_estado_cuota  
		inner join
			detalle_planilla dpl on dc.codigo_detalle=dpl.codigo_detalle_cronograma and dpl.excluido = 0
		inner join 
			planilla p on dpl.codigo_planilla=p.codigo_planilla and p.codigo_estado_planilla=2  
		inner join 
			regla_tipo_planilla rtp on rtp.codigo_regla_tipo_planilla=p.codigo_regla_tipo_planilla    
		left join
			canal_grupo cgrp on pcg.codigo_canal_grupo=cgrp.codigo_canal_grupo
		where
			dc.codigo_estado_cuota=isnull(@p_codigo_estado_cuota,dc.codigo_estado_cuota )
			and
			pcg.codigo_canal_grupo=isnull(@p_codigo_grupo,pcg.codigo_canal_grupo)
			and
			pcg.codigo_canal=isnull(@p_codigo_canal,pcg.codigo_canal )
			and
			cpc.codigo_tipo_planilla=isnull(@p_codigo_tipo_planilla,cpc.codigo_tipo_planilla)
			and
			pcg.codigo_personal=isnull(@p_codigo_personal,pcg.codigo_personal) 
			and 
			p.fecha_fin between isnull(@v_fecha_inicio,p.fecha_inicio) and isnull(@v_fecha_fin,p.fecha_fin)
			and
			cct.CreateDate between isnull(@p_fecha_contrato_inicio,cct.CreateDate) and isnull(@p_fecha_contrato_fin,cct.CreateDate)
		order by
			p.numero_planilla asc,
			emp.codigo_empresa asc,
			cpc.nro_contrato asc,
			art.nombre asc,
			dc.nro_cuota asc
	end;

	if @p_codigo_estado_cuota=4 begin --- excluido
		select      
			null as numero_planilla,
			null as nombre_regla,
			art.nombre as nombre_articulo,
			cg.nombre as nombre_canal,
			cgrp.nombre as nombre_grupo,
			isnull(ps.nombre,'')+' '+isnull(ps.apellido_paterno,'')+' '+isnull(ps.apellido_materno,'') as datos_vendedor,
			emp.nombre as nombre_empresa,
			tv.abreviatura as nombre_tipo_venta,
			tp.nombre as nombre_tipo_pago,
			cpc.nro_contrato,	 
			convert(varchar(10),dc.fecha_programada, 103) as fecha_programada,
			dc.monto_bruto,
			dc.monto_neto,
			dc.igv,
			dc.nro_cuota,	 
			ec.nombre as nombre_estado_cuota,
			dc.estado_registro
		from 
			cronograma_pago_comision cpc
		inner join 	 
			empresa_sigeco emp on cpc.codigo_empresa=emp.codigo_empresa
		inner join
			cabecera_contrato cct on (cpc.nro_contrato=cct.NumAtCard and cct.Codigo_empresa=emp.codigo_equivalencia)     
		inner join 
			tipo_venta tv on cpc.codigo_tipo_pago=tv.codigo_tipo_venta
		inner join 
			tipo_pago tp on cpc.codigo_tipo_pago=tp.codigo_tipo_pago
		inner join 
			personal_canal_grupo pcg on cpc.codigo_personal_canal_grupo=pcg.codigo_registro
		inner join
			personal ps on ps.codigo_personal=pcg.codigo_personal
		inner join 
			canal_grupo cg on pcg.codigo_canal=cg.codigo_canal_grupo
		inner join   
			detalle_cronograma dc  on dc.codigo_cronograma=cpc.codigo_cronograma 
		inner join
			articulo art on dc.codigo_articulo=art.codigo_articulo
		inner join     
			estado_cuota ec on dc.codigo_estado_cuota=ec.codigo_estado_cuota  
		inner join 
			operacion_cuota_comision occ on dc.codigo_detalle=occ.codigo_detalle_cronograma    
		--inner join 
		--	detalle_planilla dpl on dc.codigo_detalle=dpl.codigo_detalle_cronograma
		--inner join 
		--	planilla p on dpl.codigo_planilla=p.codigo_planilla
		--inner join 
		--	regla_tipo_planilla rtp on rtp.codigo_regla_tipo_planilla=p.codigo_regla_tipo_planilla  
		left join
			canal_grupo cgrp on pcg.codigo_canal_grupo=cgrp.codigo_canal_grupo
		where
			occ.codigo_tipo_operacion_cuota=4 
			and
			dc.codigo_estado_cuota=isnull(@p_codigo_estado_cuota,dc.codigo_estado_cuota )
			and
			pcg.codigo_canal_grupo=isnull(@p_codigo_grupo,pcg.codigo_canal_grupo)
			and
			pcg.codigo_canal=isnull(@p_codigo_canal,pcg.codigo_canal )
			and
			cpc.codigo_tipo_planilla=isnull(@p_codigo_tipo_planilla,cpc.codigo_tipo_planilla)
			and
			pcg.codigo_personal=isnull(@p_codigo_personal,pcg.codigo_personal) 
			and 
			convert(date, occ.fecha_movimiento) between isnull(@v_fecha_inicio,occ.fecha_movimiento) and isnull(@v_fecha_fin,occ.fecha_movimiento)
			and
			cct.CreateDate between isnull(@p_fecha_contrato_inicio,cct.CreateDate) and isnull(@p_fecha_contrato_fin,cct.CreateDate)
		order by
			emp.codigo_empresa asc,
			cpc.nro_contrato asc,
			art.nombre asc,
			dc.nro_cuota asc
	end;

	if @p_codigo_estado_cuota=5 begin --- anulado
		select      
			null as numero_planilla,
			null as nombre_regla,
			art.nombre as nombre_articulo,
			cg.nombre as nombre_canal,
			cgrp.nombre as nombre_grupo,
			isnull(ps.nombre,'')+' '+isnull(ps.apellido_paterno,'')+' '+isnull(ps.apellido_materno,'') as datos_vendedor,
			emp.nombre as nombre_empresa,
			tv.abreviatura as nombre_tipo_venta,
			tp.nombre as nombre_tipo_pago,
			cpc.nro_contrato,	 
			convert(varchar(10),dc.fecha_programada, 103) as fecha_programada,
			dc.monto_bruto,
			dc.monto_neto,
			dc.igv,
			dc.nro_cuota,	 
			ec.nombre as nombre_estado_cuota,
			dc.estado_registro
		from 
			cronograma_pago_comision cpc
		inner join
			empresa_sigeco emp on cpc.codigo_empresa=emp.codigo_empresa
		inner join
			cabecera_contrato cct on (cpc.nro_contrato=cct.NumAtCard and cct.Codigo_empresa=emp.codigo_equivalencia)     
		inner join 
			tipo_venta tv on cpc.codigo_tipo_pago=tv.codigo_tipo_venta
		inner join 
			tipo_pago tp on cpc.codigo_tipo_pago=tp.codigo_tipo_pago
		inner join 
			personal_canal_grupo pcg on cpc.codigo_personal_canal_grupo=pcg.codigo_registro
		inner join
			personal ps on ps.codigo_personal=pcg.codigo_personal
		inner join 
			canal_grupo cg on pcg.codigo_canal=cg.codigo_canal_grupo
		inner join   
			detalle_cronograma dc  on dc.codigo_cronograma=cpc.codigo_cronograma 
		inner join
			articulo art on dc.codigo_articulo=art.codigo_articulo
		inner join     
			estado_cuota ec on dc.codigo_estado_cuota=ec.codigo_estado_cuota  
		inner join 
			operacion_cuota_comision occ on dc.codigo_detalle=occ.codigo_detalle_cronograma    
		--inner join 
		--	detalle_planilla dpl on dc.codigo_detalle=dpl.codigo_detalle_cronograma
		--inner join 
		--	planilla p on dpl.codigo_planilla=p.codigo_planilla and p.codigo_estado_planilla=3
		--inner join 
		--	regla_tipo_planilla rtp on rtp.codigo_regla_tipo_planilla=p.codigo_regla_tipo_planilla  
		left join
			canal_grupo cgrp on pcg.codigo_canal_grupo=cgrp.codigo_canal_grupo
		where
			occ.codigo_tipo_operacion_cuota=5 
			and
			dc.codigo_estado_cuota=isnull(@p_codigo_estado_cuota,dc.codigo_estado_cuota )
			and
			pcg.codigo_canal_grupo=isnull(@p_codigo_grupo,pcg.codigo_canal_grupo)
			and
			pcg.codigo_canal=isnull(@p_codigo_canal,pcg.codigo_canal )
			and
			cpc.codigo_tipo_planilla=isnull(@p_codigo_tipo_planilla,cpc.codigo_tipo_planilla)
			and
			pcg.codigo_personal=isnull(@p_codigo_personal,pcg.codigo_personal) 
			and 
			convert(date, occ.fecha_movimiento) between isnull(@v_fecha_inicio,occ.fecha_movimiento) and isnull(@v_fecha_fin,occ.fecha_movimiento)
			and
			cct.CreateDate between isnull(@p_fecha_contrato_inicio,cct.CreateDate) and isnull(@p_fecha_contrato_fin,cct.CreateDate)
		order by
			emp.codigo_empresa asc,
			cpc.nro_contrato asc,
			art.nombre asc,
			dc.nro_cuota asc
	end;
  
END;