using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIGEES.Entidades
{
    public partial class detalle_cronograma_dto : Auditoria_dto
    {


        public int codigo_detalle_cronograma { set; get; }
        public string nombre_articulo { set; get; }
        public int nro_cuota { set; get; }
        public decimal monto_bruto { set; get; }
        public decimal igv { set; get; }
        public decimal monto_neto { set; get; }
        public string nro_contrato { set; get; }
        public int codigo_tipo_venta { set; get; }
        public string nombre_tipo_venta { set; get; }
        public int codigo_tipo_pago { set; get; }
        public string nombre_tipo_pago { set; get; }

        public int codigo_personal { get; set; }
        public string apellido_paterno { set; get; }
        public string apellido_materno { set; get; }
        public string nombre_persona { set; get; }

        public string nombre_canal { set; get; }
        public string nombre_grupo { set; get; }


        public string apellidos_nombres { get; set; }

        public DateTime fecha_programado { get; set; }
        public string str_fecha_programado { get; set; }

        public string observacion { get; set; }


    }
    public partial class detalle_cronograma_grilla_dto : Auditoria_dto
    {


        public int codigo_detalle_cronograma { set; get; }
        public string nombre_articulo { set; get; }
        public int nro_cuota { set; get; }
        public decimal monto_bruto { set; get; }
        public decimal igv { set; get; }
        public decimal monto_neto { set; get; }
        public string nro_contrato { set; get; }
        public int codigo_tipo_venta { set; get; }
        public string nombre_tipo_venta { set; get; }
        public int codigo_tipo_pago { set; get; }
        public string nombre_tipo_pago { set; get; }

        public int codigo_personal { get; set; }
        public string apellido_paterno { set; get; }
        public string apellido_materno { set; get; }
        public string nombre_persona { set; get; }

        public string nombre_canal { set; get; }
        public string nombre_grupo { set; get; }


        public string apellidos_nombres { get; set; }

        public DateTime fecha_programada { get; set; }
        public string str_fecha_programada { get; set; }

        public string observacion { get; set; }


    }

    public partial class listado_exclusion_grilla_dto : Auditoria_dto
    {


        public int codigo_detalle_cronograma { set; get; }
        public string numero_planilla { set; get; }

        public DateTime fecha_registra { set; get; }
        public string str_fecha_registra { set; get; }


        public DateTime fecha_inicio { set; get; }
        public string str_fecha_inicio { set; get; }

        public DateTime fecha_fin { set; get; }
        public string str_fecha_fin { set; get; }
        public string nro_contrato { set; get; }
        public int codigo_personal { get; set; }
        public string apellido_paterno { set; get; }
        public string apellido_materno { set; get; }
        public string nombre_persona { set; get; }
        public string apellidos_nombres { get; set; }

        public int codigo_estado_cuota { set; get; }
        public string nombre_estado_cuota { get; set; }


        public string observacion { get; set; }



        public string nombre_estado_exclusion { get; set; }

        public int codigo_planilla { get; set; }

        public int codigo_detalle_planilla { get; set; }

        public int codigo_exclusion { get; set; }

        public string usuario_exclusion { get; set; }

        public DateTime? fecha_exclusion { get; set; }

        public string str_fecha_exclusion { get; set; }

        public string motivo_exclusion { get; set; }

        public string usuario_habilita { get; set; }

        public string motivo_habilita { get; set; }

        public DateTime? fecha_habilita { get; set; }

        public string str_fecha_habilita { get; set; }

        public string numero_planilla_incluido { get; set; }
        public int codigo_estado_exclusion { get; set; }
        public int nro_cuota { get; set; }
        public string nombre_empresa { get; set; }
    }

    public partial class detalle_exclusion_dto : Auditoria_dto
    {


        public int codigo_detalle_cronograma { set; get; }
        public string nombre_articulo { set; get; }
        public int nro_cuota { set; get; }
        public decimal monto_bruto { set; get; }
        public decimal igv { set; get; }
        public decimal monto_neto { set; get; }
        public string nro_contrato { set; get; }
        public int codigo_tipo_venta { set; get; }
        public string nombre_tipo_venta { set; get; }
        public int codigo_tipo_pago { set; get; }
        public string nombre_tipo_pago { set; get; }

        public int codigo_personal { get; set; }
        public string apellido_paterno { set; get; }
        public string apellido_materno { set; get; }
        public string nombre_persona { set; get; }

        public string nombre_canal { set; get; }
        public string nombre_grupo { set; get; }


        public string apellidos_nombres { get; set; }

        public DateTime fecha_programado { get; set; }
        public string str_fecha_programado { get; set; }

        public string observacion { get; set; }



        public string motivo_exclusion { get; set; }

        public string usuario_exclusion { get; set; }

        public DateTime? fecha_exclusion { get; set; }
        public string str_fecha_exclusion { get; set; }


        public string motivo_habilitacion { get; set; }

        public string usuario_habilitacion { get; set; }

        public DateTime? fecha_habilitacion { get; set; }
        public string str_fecha_habilitacion { get; set; }

        public int codigo_estado_cuota { get; set; }

        public string nombre_estado_cuota { get; set; }

        public string nombre_estado_exclusion { get; set; }

        public int codigo_exclusion { get; set; }

        //public string usuario_habilita { get; set; }

        //public string motivo_habilita { get; set; }

        //public DateTime? fecha_habilita { get; set; }

        //public string str_fecha_habilita { get; set; }

        public string nombre_empresa { get; set; }

        public string s_monto_bruto { set; get; }
        public string s_igv { set; get; }
        public string s_monto_neto { set; get; }

    }

    public partial class detalle_cronograma_comision_dto : Auditoria_dto
    {
        public int codigo_detalle_cronograma { get; set; }
        public int nro_cuota { set; get; }
        public string nombre_tipo_planilla { set; get; }
        public DateTime? fecha_programada { set; get; }
        public string str_fecha_programada { set; get; }
        public decimal importe_sing_igv { set; get; }
        public decimal igv { set; get; }
        public decimal importe_comision { set; get; }
        public DateTime? fecha_exclusion { set; get; }
        public string str_fecha_exclusion { set; get; }
        public DateTime? fecha_cierre { set; get; }
        public string str_fecha_cierre { set; get; }
        public string observacion { set; get; }
        /*seccion filtro*/
        public string nombre_estado_cuota { get; set; }
        public int codigo_estado_cuota { get; set; }
        public string motivo_anulacion { get; set; }
        public DateTime? fecha_anulado { get; set; }
        public string str_fecha_anulado { get; set; }
        public int nro_total_contrato { get; set; }
        public int nro_cuota_libre_refinanciar { get; set; }
        public string nro_contrato { get; set; }
        public string nombre_empresa { get; set; }
        public string nombre_articulo { get; set; }
        public string s_importe_comision { get; set; }
        public string s_importe_sing_igv { get; set; }
        public string s_igv { get; set; }
        public string numero_planilla { get; set; }
        public bool es_registro_manual_comision { get; set; }
        public int indica_registro_manual_comision
        {
            get
            {
                return es_registro_manual_comision ? 1 : 0;
            }
        }
        public int codigo_detalle_planilla { get; set; }
        public int codigo_planilla { get; set; }
    }

    public partial class detalle_cronograma_adicionar_dto : Auditoria_dto
    {
        public int codigo_detalle_cronograma { get; set; }
        public int codigo_empresa { get; set; }
        public string nro_contrato { get; set; }
        public int codigo_tipo_planilla { get; set; }
        public int codigo_articulo { get; set; }
        public decimal monto_neto { get; set; }
        public string motivo { get; set; }
    }

    public partial class detalle_cronograma_personal_inactivo_dto
    {
        public int codigo_detalle_cronograma { get; set; }
        public string nombre_canal { get; set; }
        public string nombre_grupo { get; set; }
        public string nombre_tipo_planilla { get; set; }
        public string nombre_personal { get; set; }
        public string nombre_empresa { get; set; }
        public string nro_contrato { get; set; }
        public string nombre_tipo_venta { get; set; }
        public string nombre_tipo_pago { get; set; }
        public string nombre_articulo { get; set; }
        public int nro_cuota { get; set; }
        public decimal monto_sin_igv { get; set; }
        public decimal monto_igv { get; set; }
        public decimal monto_con_igv { get; set; }
        public string observacion { get; set; }
        public string fecha_registra { get; set; }
        public string usuario_registra { get; set; }
        public int liquidado { get; set; }
        public string resultado_n1 { get; set; }
        public string resultado_n2 { get; set; }
        public string estilo_n1 { get; set; }
        public string estilo_n2 { get; set; }
        public int codigo_resultado_n1 { get; set; }
        public int codigo_resultado_n2 { get; set; }
    }

    public partial class detalle_cronograma_personal_inactivo_busqueda_dto
    {
        public string fecha_inicio { get; set; }
        public string fecha_fin { get; set; }
        public string codigo_canal { get; set; }
        public int liquidado { get; set; }
    }

    public partial class detalle_cronograma_elemento_dto
    {
        public string codigo_detalle_cronograma { get; set; }
    }

    public partial class detalle_cronograma_deshabilitacion_dto : Auditoria_dto
    {
        public string motivo { get; set; }
        public List<detalle_cronograma_elemento_dto> lst_detalle_cronograma_elemento { get; set; }
        public string procesarXML { get; set; }
    }

    public partial class detalle_cronograma_type_dto
    {
        public int codigo_detalle_cronograma { get; set; }
    }

    public partial class detalle_cronograma_aprobacion_dto
    {
        public int codigo_detalle_cronograma { get; set; }
        public string nombre_canal { get; set; }
        public string nombre_grupo { get; set; }
        public string nombre_tipo_planilla { get; set; }
        public string nombre_personal { get; set; }
        public string nombre_empresa { get; set; }
        public string nro_contrato { get; set; }
        public string nombre_tipo_venta { get; set; }
        public string nombre_tipo_pago { get; set; }
        public string nombre_articulo { get; set; }
        public int nro_cuota { get; set; }
        public decimal monto_sin_igv { get; set; }
        public decimal monto_igv { get; set; }
        public decimal monto_con_igv { get; set; }
        public string observacion { get; set; }
        public string fecha_registra { get; set; }
        public string usuario_registra { get; set; }
        public int liquidado { get; set; }
        public string estado_liquidado { get; set; }
        public string resultado_n1 { get; set; }
        public string resultado_n2 { get; set; }
        public int codigo_resultado_n1 { get; set; }
        public int codigo_resultado_n2 { get; set; }
        public string fecha_aprobacion_n1 { get; set; }
        public string fecha_aprobacion_n2 { get; set; }
        public string usuario_aprobacion_n1 { get; set; }
        public string usuario_aprobacion_n2 { get; set; }
        public string observacion_n1 { get; set; }
        public string observacion_n2 { get; set; }
    }


}
