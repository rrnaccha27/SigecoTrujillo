using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIGEES.Entidades
{
    public partial class Planilla_dto:Auditoria_dto
    {
        public Planilla_dto()
        {
            estado_planilla = new estado_planilla_dto();
            tipo_planilla = new tipo_planilla_dto();
        }
        public int codigo_planilla { set; get; }

        public string numero_planilla { set; get; }

        public int codigo_canal { set; get; }
        public string nombre_canal { set; get; }

        public int codigo_tipo_planilla { set; get; }

        public DateTime fecha_inicio { set; get; }
        
        public DateTime fecha_fin { set; get; }

        public DateTime fecha_apertura { set; get; }

        public DateTime? fecha_cierre { set; get; }


        public int codigo_estado_planilla { get; set; }
        public estado_planilla_dto estado_planilla { set; get; }

        
        public string usuario_apertura { get; set; }

        public string usuario_cierre { get; set; }

        public tipo_planilla_dto tipo_planilla { set; get; }



        public DateTime? fecha_anulacion { get; set; }

        public string usuario_anulacion { get; set; }        
        public int codigo_regla_tipo_planilla { get; set; }
        public string nombre_regla_tipo_planilla { get; set; }
        public string permiso_escritura { get; set; }
        public string permiso_generar_txt { get; set; }
        public string permiso_revision_planilla { get; set; }
        public string estilo { get; set; }
        public bool envio_liquidacion { get; set; }
    }
    public partial class grilla_planilla_exclusion_dto : Auditoria_dto
    {
        
        public int codigo_planilla { set; get; }

        public string numero_planilla { set; get; }

        public int codigo_canal { set; get; }
        public string nombre_canal { set; get; }

        public int codigo_tipo_planilla { set; get; }
        public string nombre_tipo_planilla { set; get; }


        public DateTime fecha_inicio { get; set; }

        public DateTime fecha_fin { get; set; }

        public string str_fecha_inicio { get; set; }

        public string str_fecha_fin { get; set; }
        public int codigo_regla_tipo_planilla { get; set; }
        public string nombre_regla_tipo_planilla { get; set; }
    }

    public partial class grilla_cuota_pago_planilla_dto : Auditoria_dto
    {

        public int codigo_planilla { set; get; }

        public string numero_planilla { set; get; }

        public int codigo_canal { set; get; }
        public string nombre_canal { set; get; }

        public int codigo_tipo_planilla { set; get; }
        public string nombre_tipo_planilla { set; get; }


        public DateTime fecha_inicio { get; set; }

        public DateTime fecha_fin { get; set; }

        public string str_fecha_inicio { get; set; }

        public string str_fecha_fin { get; set; }

        public string apellidos_nombres { get; set; }

        public int codigo_tipo_venta { get; set; }

        public int codigo_tipo_pago { get; set; }

        public decimal monto_neto { get; set; }

        public decimal igv { get; set; }

        public decimal monto_bruto { get; set; }

        public int codigo_detalle_cronograma { get; set; }

        public int codigo_cronograma { get; set; }

        public int indica_modificar { get; set; }

        public int nro_cuota { get; set; }

        public string nro_contrato { get; set; }
        public string motivo_registro { get; set; }

        public int codigo_detalle_planilla { get; set; }

        public int codigo_exclusion { get; set; }

        public int codigo_regla_tipo_planilla { get; set; }
        public string nombre_regla_tipo_planilla { get; set; }
        public string nombre_empresa { get; set; }
    }

    public partial class planilla_filtro_dto {

        public DateTime? fecha_inicio { set; get; }
        public DateTime? fecha_fin { set; get; }

        public int codigo_canal_venta { set; get; }

        public string numero_planilla { set; get; }

        public int pageSize {set;get;}
        public int pageNumber { set; get; }
    }

    public partial class Planilla_bono_dto : Auditoria_dto
    {
        public Planilla_bono_dto()
        {
            estado_planilla = new estado_planilla_dto();
            tipo_planilla = new tipo_planilla_dto();
        }
        public int codigo_planilla { set; get; }

        public string numero_planilla { set; get; }

        public int codigo_canal { set; get; }
        public string nombre_canal { set; get; }

        public int codigo_tipo_planilla { set; get; }

        public DateTime fecha_inicio { set; get; }

        public DateTime fecha_fin { set; get; }

        public DateTime fecha_apertura { set; get; }

        public DateTime? fecha_cierre { set; get; }


        public int codigo_estado_planilla { get; set; }
        public estado_planilla_dto estado_planilla { set; get; }



        public DateTime? fecha_aprobacion { get; set; }

        public string usuario_aprobacion { get; set; }

        public string usuario_apertura { get; set; }

        public string usuario_cierre { get; set; }

        public tipo_planilla_dto tipo_planilla { set; get; }
        public DateTime? fecha_anulacion { get; set; }
        public string usuario_anulacion { get; set; }
        public int codigo_regla_tipo_planilla { get; set; }
        public string codigos_canales { get; set; }
        public int es_planilla_jn { get; set; }
        public bool envio_liquidacion { get; set; }
    }

    public partial class planilla_bono_jn_dto : Auditoria_dto
    {
        public int codigo_planilla { set; get; }
        public string numero_planilla { set; get; }
        public int codigo_tipo_planilla { set; get; }
        public string fecha_inicio { set; get; }
        public string fecha_fin { set; get; }
        public int codigo_estado_planilla { get; set; }
        public decimal porcentaje { get; set; }
        public decimal dinero_ingresado { get; set; }
        public decimal bono { get; set; }
        public decimal meta_100 { get; set; }
        public decimal meta_90 { get; set; }
    }


    public partial class grilla_planilla_bono 
    {                       
        public int codigo_planilla { set; get; }
        public string numero_planilla { set; get; }                
        public string nombre_tipo_planilla { set; get; }
        public string fecha_inicio { set; get; }
        public string fecha_fin { set; get; }
        public string nombre_estado_planilla { set; get; }
        public string fecha_creacion{ set; get; }
        public string fecha_cierre{ set; get; }
        public string fecha_apertura { set; get; }
        public string nombre_canal { get; set; }
        public string fecha_anulacion { get; set; }
        public int codigo_estado_planilla { get; set; }
        public string estilo { get; set; }
        public bool envio_liquidacion { get; set; }
    }

    public partial class planilla_bono_trimestral_dto : Auditoria_dto
    {
        public planilla_bono_trimestral_dto()
        {
            estado_planilla = new estado_planilla_dto();
        }

        public int codigo_planilla { set; get; }
        public string numero_planilla { set; get; }
        public int codigo_regla { set; get; }
        public string nombre_regla { set; get; }
        public int codigo_periodo { get; set; }
        public DateTime fecha_apertura { set; get; }
        public DateTime? fecha_cierre { set; get; }
        public int codigo_estado_planilla { get; set; }
        public estado_planilla_dto estado_planilla { set; get; }
        public DateTime? fecha_aprobacion { get; set; }
        public string usuario_aprobacion { get; set; }
        public string usuario_apertura { get; set; }
        public string usuario_cierre { get; set; }
        public DateTime? fecha_anulacion { get; set; }
        public string usuario_anulacion { get; set; }
    }


    public partial class grilla_planilla_bono_trimestral
    {
        public int codigo_planilla { set; get; }
        public string numero_planilla { set; get; }
        public string nombre_tipo_bono { set; get; }
        public string nombre_regla_bono { set; get; }
        public int anio_periodo { set; get; }
        public string periodo { set; get; }
        public string fecha_creacion { set; get; }
        public string fecha_cierre { set; get; }
        public string fecha_apertura { set; get; }
        public string fecha_anulacion { get; set; }
        public int codigo_estado_planilla { get; set; }
        public string nombre_estado_planilla { set; get; }
        public string estilo { get; set; }
    }


    public partial class filtro_grilla_plainilla_bono
    {
        public int iCurrentPage { set; get; }
        public int iPageSize { set; get; }
        public string numero_planilla { set; get; }
       
        
    }

    public partial class planilla_resumen_dto
    {
        public int codigo_planilla { get; set; }
        public string fecha_inicio { get; set; }
        public string fecha_fin { get; set; }
        public string nombre_planilla { get; set; }
        public string estado_planilla { get; set; }
    }

    public partial class planilla_checklist_comision_dto
    { 
        public int codigo_planilla { get; set; }
        public string numero_planilla { get; set; }
        public string nombre_regla_tipo_planilla { get; set; }
        public int codigo_regla_tipo_planilla { get; set; }
        public string fecha_anulacion { get; set; }
        public string fecha_apertura { get; set; }
        public string fecha_cierre { get; set; }
        public int codigo_estado_planilla { get; set; }
        public string nombre_estado_planilla { get; set; }
        public string fecha_inicio { get; set; }
        public string fecha_fin { get; set; }
    }

    public partial class planilla_checklist_bono_dto
    {
        public int codigo_planilla { get; set; }
        public string numero_planilla { get; set; }
        public string fecha_anulacion { get; set; }
        public string fecha_apertura { get; set; }
        public string fecha_cierre { get; set; }
        public int codigo_estado_planilla { get; set; }
        public string nombre_estado_planilla { get; set; }
        public string fecha_inicio { get; set; }
        public string fecha_fin { get; set; }
        public string nombre_tipo_planilla { get; set; }
        public string nombre_canal { get; set; }
    }

    public partial class planilla_checklist_bono_trimestral_dto
    {
        public int codigo_planilla { get; set; }
        public string numero_planilla { get; set; }
        public string fecha_anulacion { get; set; }
        public string fecha_apertura { get; set; }
        public string fecha_cierre { get; set; }
        public int codigo_estado_planilla { get; set; }
        public string nombre_estado_planilla { get; set; }
        public string anio_periodo { get; set; }
        public string periodo { get; set; }
        public string nombre_regla { get; set; }
    }

}
