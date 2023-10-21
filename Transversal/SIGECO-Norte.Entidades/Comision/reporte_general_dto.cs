using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIGEES.Entidades
{


    [Serializable]
    public class reporte_detallado_vendedores_dto
    {

        public int codigo_canal { get; set; }
        public string nombre_canal { get; set; }
        public string tipo { get; set; }
        public int codigo_grupo { get; set; }
        public string nombre_grupo { get; set; }
        
        public int codigo_tipo_venta { get; set; }
        public string nombre_tipo_venta { get; set; }

        public int codigo_tipo_articulo { get; set; }
        public string nombre_tipo_articulo { get; set; }
        public string nro_contrato { get; set; }

        public int codigo_personal { get; set; }
        public string apellido_paterno { get; set; }
        public string apellido_materno { get; set; }
        public string nombre { get; set; }
        public string nombres { get { return $"{apellido_paterno}  {apellido_materno}, {nombre}"; } }
        public decimal? monto_igv { get; set; }
        public decimal? precio_unidad { get; set; }
        public decimal? monto_neto { get; set; }
        public decimal? monto_bruto { get; set; }
        public decimal? monto_total_contrato { get; set; }
        public decimal? monto_total_cuota_inicial { get; set; }
        public decimal? porcentaje_cuota_inicial { get; set; }
        public decimal? porcentaje_regla_comision { get; set; }
        public string nombre_articulo { get; set; }
        public int tope_unidad { get; set; }
        public int cantidad_contrato { get; set; }
        public int nro_cuota { get; set; }

        
        public DateTime? fecha_programado { get; set; }
        public string nro_ruc { get; set; }
        public string direccion { get; set; }
        public string contacto { get; set; }
        public string nro_cuenta { get; set; }
        public string codigo_interbancario { set; get; }
        public string nombre_banco { get; set; }
        public string telefono_celular { get; set; }
        public string telefono_fijo { get; set; }
        public int id_tipo_comision { get; set; }
        public int codigo_detalle_regla_comision { get; set; }

        public bool regla_especial_vtas { get; set; }
        public int es_mejora { get; set; }
        public decimal? porcentaje_ejecutado { get; set; }
        public decimal? monto_prorrateo { get; set; }
        public decimal? comision_x_meta { get; set; }
        public decimal? total_excedente { get; set; }
        public decimal? monto_prorrateo_excedente { get; set; }
        public decimal? comision_excedente { get; set; }
        public decimal? porcentaje_meta { get; set; }
        public decimal? porcentaje_excedente { get; set; }

        public string concepto_comision_articulo { get; set; }
        public string concepto_comision_tipo_venta { get; set; }
        public string concepto_bono_tipo_venta { get; set; }
        public string concepto_excedente_tipo_venta { get; set; }
        public decimal? monto_contrato { get; set; }
        public decimal? monto_pagado { get; set; }
        public decimal? meta_tipo_venta { get; set; }
        public decimal? excedente_tipo_venta { get; set; }

    }

    [Serializable]
    public class reporte_resumen_comercial_dto
    {
        /// <summary>
        /// comision o bono
        /// </summary>
        public string tipo { get; set; }
        public int codigo_personal { get; set; }
        public string apellido_paterno { get; set; }
        public string apellido_materno { get; set; }
        public string nombre { get; set; }
        public string nombres { get { return $"{apellido_paterno}  {apellido_materno}, {nombre}"; } }
        public decimal? monto_igv { get; set; }
        public decimal? monto_neto { get; set; }
        public decimal? monto_bruto { get; set; }
        public decimal? monto_ir { get; set; }

        public decimal? monto_neto_espacio { get; set; }
        public decimal? monto_neto_cremacion { get; set; }
        public decimal? monto_neto_servicio { get; set; }
        public decimal? monto_neto_otros { get; set; }

        public decimal? bono_espacio { get; set; }
        public decimal? bono_cremacion { get; set; }
        public decimal? bono_servicio { get; set; }

        public string nombre_grupo { get; set; }
        public string nombre_supervisor { get; set; }
        public string nro_documento { get; set; }
    }


    [Serializable]
    public class reporte_comercial_dto
    {
        public int tipo { get; set; }
        public string nombre_periodo { get; set; }
        public string nombre_canal { get; set; }
        public string nombre_grupo { get; set; }
        public decimal monto_factura { get; set; }
        public decimal monto_planilla { get; set; }
    }

    [Serializable]
    public class reporte_comercial_busqueda_dto
    {
        public DateTime? fecha_inicio { set; get; }

        public DateTime? fecha_fin { set; get; }

        public int tipo { get; set; }        
        public string codigo_canal { get; set; }
        public int codigo_tipo_planilla { get; set; }

        public int codigo_mes { get; set; }
        public int codigo_anio { get; set; }

        /// <summary>
        /// 1: 1era quincena
        /// 2: 2da quincena
        /// </summary>
        public int codigo_tipo_reporte { get; set; }
        public int? codigo_persona { get; set; }
        public bool generar_orden_pago { get; set; }
    }


    [Serializable]
    public class reporte_resumen_supervisores_dto
    {
        public int codigo_personal { get; set; }
        public string apellido_paterno { get; set; }
        public string apellido_materno { get; set; }
        public string nombre { get; set; }
        public string nombres { get { return $"{apellido_paterno}  {apellido_materno}, {nombre}"; } }
        public decimal? monto_igv { get; set; }
        public decimal? monto_neto { get; set; }
        public decimal? monto_bruto { get; set; }
        public decimal? monto_ir { get; set; }

        public decimal? monto_neto_espacio { get; set; }
        public decimal? monto_neto_cremacion { get; set; }
        public decimal? monto_neto_servicio { get; set; }
        public decimal? monto_neto_otros { get; set; }
    }



    [Serializable]
    public class reporte_finanzas_dto
    {
        public long id { get; set; }
        public int tipo { get; set; }
        public string nombre_periodo { get; set; }
        public string nombre_empresa { get; set; }
        public string nombre_canal { get; set; }
        public string nombre_grupo { get; set; }
        public string nombre_tipo_venta { get; set; }
        public string nombre_unidad_negocio { get; set; }
        public decimal monto_factura { get; set; }
        public decimal monto_planilla { get; set; }
        public string numero_contrato { get; set; }
        public string nombre_personal { get; set; }
        public string tipo_planilla { get; set; }
        public int resumen_detalle { get; set; }
        public int tipo_reporte { get; set; }

    }


    [Serializable]
    public class reporte_finanzas_busqueda_dto
    {
        public int tipo { get; set; }
        public string codigo_canal { get; set; }
        public string codigo_tipo_planilla { get; set; }
        public int codigo_tipo_reporte { get; set; }
        public string periodo { get; set; }
        public int anio { get; set; }
        public int resumen_detalle { get; set; }
    }

    [Serializable]
    public class reporte_finanzas_filtro_dto
    {
        public string tipo { get; set; }
        public string canal { get; set; }
        public string tipo_planilla { get; set; }
        public string tipo_reporte { get; set; }
        public string periodo { get; set; }
        public string anio { get; set; }
    }

    [Serializable]
    public class reporte_migracion_contratos_dto
    {
        public string nombre_empresa { get; set; }
        public string nro_contrato { get; set; }
        public string fecha_contrato { get; set; }
        public int docentry { get; set; }
        public string estado { get; set; }
        public string fecha_migracion { get; set; }
        public string nombre_canal { get; set; }
        public string nombre_grupo { get; set; }
        public string nombre_personal { get; set; }
        public int intentos { get; set; }
    }

    [Serializable]
    public class reporte_migracion_contratos_busqueda_dto
    {
        public string fecha_inicial { get; set; }
        public string fecha_final { get; set; }
    }

    [Serializable]
    public class reporte_cuotas_iniciales_dto
    {
        public int docentry { get; set; }
        public string nombre_empresa { get; set; }
        public string nro_contrato { get; set; }
        public string nombre_tipo_venta { get; set; }
        public string nombre_tipo_pago { get; set; }
        public string fecha_contrato { get; set; }
        public decimal monto_contratado { get; set; }
        public decimal dinero_ingresado { get; set; }
        public string comision_habilitada { get; set; }
        public string estado_proceso { get; set; }
        public string observacion_proceso { get; set; }
    }

    [Serializable]
    public class reporte_cuotas_iniciales_busqueda_dto
    {
        public string fecha_inicial { get; set; }
        public string fecha_final { get; set; }
    }

}
