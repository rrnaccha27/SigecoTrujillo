using System;
using System.Collections.Generic;

using SIGEES.Entidades.Common;

namespace SIGEES.Entidades
{

    public class contrato_dto : Auditoria_dto
    {
        //    public int codigo_empresa { set; get; }
        //    public string nombre_empresa { set; get; }
        //    public string nro_contrato { set; get; }
        //    public string apellidos_nombres_cliente { set; get; }
        //    public string apellidos_nombres_vendedor { set; get; }
        //    public string apellidos_nombres_supervisor { set; get; }


        //    public string nombre_canal_venta { set; get; }
        //    public string nombre_tipo_venta { set; get; }
        //    public string nombre_tipo_pago { set; get; }
    }


    public class analisis_contrato_dto : Auditoria_dto
    {
        public int existe_registro { set; get; }
        public int codigo_empresa { set; get; }
        public string nombre_empresa { set; get; }
        public string nro_contrato { set; get; }
        public string apellidos_nombres_cliente { set; get; }
        public string apellidos_nombres_vendedor { set; get; }
        public string apellidos_nombres_supervisor { set; get; }


        public string nombre_canal_venta { set; get; }
        public string nombre_tipo_venta { set; get; }
        public string nombre_tipo_pago { set; get; }

        public int codigo_cronograma { get; set; }

        public string nombre_grupo { get; set; }
        public string doc_completa { get; set; }

        public string observacion_contrato_migrado { get; set; }

        public string nombre_estado_proceso_migrado { get; set; }
        public string nro_contrato_ref { get; set; }
        public string nombre_empresa_ref { get; set; }
        public string fecha_proceso { get; set; }
        public string fecha_migracion { get; set; }
        public string fecha_contrato { get; set; }
        public string usuario_proceso { get; set; }
        public string tiene_transferencia { get; set; }
        public string nombre_empresa_transferencia { get; set; }
        public string nro_contrato_transferencia { get; set; }
        public decimal monto_transferencia { get; set; }

        public string bloqueo { get; set; }
        public string usuario_bloqueo { get; set; }
        public string fecha_bloqueo { get; set; }
        public string motivo_bloqueo { get; set; }
    }

    public class analisis_contrato_articulo_cronograma_dto : Auditoria_dto
    {
        public int codigo_empresa { set; get; }

        public string numero_contrato { set; get; }


        public int codigo_articulo { get; set; }

        public string nombre_articulo { get; set; }

        public string nombre_moneda { get; set; }

        public int cantidad_articulo { get; set; }

        public decimal monto_total_comision_vendedor { get; set; }

        public decimal monto_total_pagado_vendedor { get; set; }

        public decimal monto_total_excluido_vendedor { get; set; }

        public decimal monto_total_comision_supervisor { get; set; }

        public decimal monto_total_pagado_supervisor { get; set; }

        public decimal monto_total_excluido_supervisor { get; set; }

        public decimal monto_total_saldo_vendedor { get; set; }

        public decimal monto_total_saldo_supervisor { get; set; }

        public int codigo_moneda { get; set; }

        public string anulacion_vendedor { get; set; }

        public string anulacion_supervisor { get; set; }

        public decimal monto_comision_inicial_personal { get; set; }

        public decimal monto_comision_inicial_supervisor { get; set; }

        public decimal monto_comision_inicial_total { get; set; }

        public string codigo_sku { get; set; }
        public int es_hr { get; set; }
        public int genera_comision { get; set; }
    }

    public class filtro_contrato_dto : Auditoria_dto
    {
        public int codigo_articulo { get; set; }

        public int codigo_empresa { get; set; }

        public int codigo_moneda { get; set; }

        public string numero_contrato { get; set; }
    }

    public class analisis_contrato_cronograma_cuotas_dto : Auditoria_dto
    {
        public string tipo_cuota { get; set; }
        public int cuota { get; set; }
        public decimal importe_sin_igv { get; set; }
        public decimal importe_igv { get; set; }
        public decimal importe_total { get; set; }
        public string fec_vencimiento { get; set; }
        public string fec_pago { get; set; }
        public string estado { get; set; }

    }

    public class analisis_contrato_combo_dto
    {
        public int id { get; set; }
        public string text { get; set; }
    }

}
