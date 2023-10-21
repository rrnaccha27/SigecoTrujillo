using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIGEES.Entidades.planilla
{
    public partial class detalle_planilla_dto : Auditoria_dto
    {

        public int codigo_detalle_planilla { set; get; }
        public int codigo_planilla { set; get; }
        public int codigo_cronograma { set; get; }
        public int codigo_detalle_cronograma { set; get; }
        public bool excluido { set; get; }
        public int nro_cuota { set; get; }
        public DateTime fecha_pago { set; get; }
        public decimal monto_bruto { set; get; }
        public decimal igv { set; get; }
        public decimal monto_neto { set; get; }


        public string observacion { get; set; }
    }

    public partial class detalle_planilla_resumen_dto : Auditoria_dto
    {

        public int codigo_tipo_busqueda { set; get; }
        public int codigo_detalle_cronograma { set; get; }
        public int codigo_planilla { get; set; }
        public int codigo_articulo { set; get; }
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
        public string apellido_paterno { set; get; }
        public string apellido_materno { set; get; }
        public string nombre_persona { set; get; }
        public string nombre_grupo_canal { set; get; }




        public string apellidos_nombres { get; set; }

        public int codigo_detalle_planilla { get; set; }

        public DateTime fecha_pago { get; set; }

        public string str_fecha_pago { get; set; }

        public string observacion { get; set; }

        public int codigo_personal { get; set; }

        public string nombre_canal { get; set; }

        public int codigo_cronograma { get; set; }

        public int codigo_empresa { get; set; }
        public string nombre_empresa { get; set; }


        public int codigo_grupo { get; set; }



        public decimal comision_total { get; set; }

        public decimal monto_descuento { get; set; }

        public bool tiene_descuento { get; set; }

        public string motivo { get; set; }

        public string nombre_moneda { get; set; }

        public int codigo_estado_cuota { get; set; }

        public string nombre_estado_cuota { get; set; }

        public string usuario_exclusion { get; set; }

        public string motivo_exclusion { get; set; }

        public string fecha_exclusion { get; set; }

        public string usuario_habilita_exclusion { get; set; }

        public string motivo_habilitacion_exclusion { get; set; }

        public decimal monto_afecto_descuento { get; set; }

        public string nombre_estado_exclusion { get; set; }

        public bool es_registro_manual_comision { get; set; }

        public int indica_registro_manual_comision
        {
            get
            {
                return es_registro_manual_comision ? 1 : 0;
            }
        }
        public int es_transferencia { get; set; }
        public string personal_comision_manual { get; set; }
        public string usuario_comision_manual { get; set; }
    }

    public partial class detalle_planilla_inclusion_dto
    {
        public string codigo_detalle_cronograma { get; set; }
    }

    public partial class detalle_planilla_exclusion_dto : Auditoria_dto
    {
        public string motivo { get; set; }
        public bool permanente { get; set; }
        public List<detalle_planilla_inclusion_dto> lst_detalle_cronograma { get; set; }
        public string procesarXML { get; set; }
    }

    public partial class detalle_planilla_bono_exclusion_dto
    {
        public int codigo_planilla_bono { get; set; }
        public int codigo_articulo { get; set; }
        public int codigo_empresa { get; set; }
        public int codigo_personal { get; set; }
        public string nro_contrato { get; set; }
        public string excluido_motivo { get; set; }
        public string excluido_usuario { get; set; }
    }

    public partial class detalle_planilla_bono_trimestral_dto
    { 
		public string nombre_canal { get; set; }
		public string nombre_grupo { get; set; }
		public string nombre_personal { get; set; }
		public string nombre_supervisor { get; set; }
		public decimal monto_contratado { get; set; }
		public int rango { get; set; }
        public decimal monto_bono { get; set; }
    }

    public partial class detalle_liquidacion_planilla_bono_trimestral_dto
    {
		public int codigo_empresa { get; set; }
		public string nombre_empresa { get; set; }
		public string nombre_empresa_largo { get; set; }
		public string direccion_fiscal_empresa { get; set; }
		public string ruc_empresa { get; set; }
		public int codigo_canal_grupo { get; set; }
		public int codigo_canal { get; set; }
		public string nombre_canal { get; set; }
		public int codigo_grupo { get; set; }
		public string nombre_grupo { get; set; }
		public int codigo_personal { get; set; }
		public string nombre_personal { get; set; }
		public string documento_personal { get; set; }
		public string codigo_personal_j { get; set; }
		public int codigo_supervisor { get; set; }
		public string correo_supervisor { get; set; }
		public string nombre_supervisor { get; set; }
		public decimal monto_contratado { get; set; }
		public int rango { get; set; }
		public decimal monto_bono { get; set; }
		public decimal monto_sin_igv { get; set; }
		public decimal monto_igv { get; set; }
		public string monto_bono_letras { get; set; }
		public string concepto_liquidacion { get; set; }
        public int codigo_estado_planilla { get; set; }
    }

}
