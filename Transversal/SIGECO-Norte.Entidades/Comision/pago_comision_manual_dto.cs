using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIGEES.Entidades
{
    public partial class comision_manual_dto:Auditoria_dto
    {
        public int codigo_comision_manual { get; set; }
        public int codigo_estado_cuota { get; set; }
        
        #region FALLECIDO
        public int codigo_fallecido { set; get; }
        public string apellido_paterno_fallecido { set; get; }
        public string apellido_materno_fallecido { set; get; }
        public string nombre_fallecido { set; get; }
        public int? codigo_tipo_documento { set; get; }
        public string nro_documento { set; get; }
        public string nombre_completo_fallecido { get; set; }
        #endregion


        public int codigo_personal { set; get; }
        public string nombre_personal { set; get; }
        public int codigo_canal {get; set;}
        public string nombre_canal { get; set; }

        public int codigo_articulo { set; get; }
        public string nombre_articulo { set; get; }

        public int? codigo_empresa { set; get; }
        public string nro_contrato { set; get; }
        
        public decimal monto_bruto { set; get; }
        public decimal monto_igv { set; get; }
        public decimal monto_neto { set; get; }

        public string comentario { set; get; }

        public int codigo_tipo_venta { get; set; }
        public string nombre_tipo_venta { get; set; }
        public int codigo_tipo_pago { get; set; }
        public string nombre_tipo_pago { get; set; }

        public int codigo_cronograma { set; get; }
        public int codigo_planilla { get; set; }
        public string nombre_usuario { get; set; }
        public decimal igv { get; set; }

        public string nro_factura_vendedor { set; get; }
        public string nombre_empresa { get; set; }
        public string nombre_tipo_documento { get; set; }
        public string nombre_estado_proceso { get; set; }
        public string nombre_planilla { get; set; }
    }

    public partial class comision_manual_listado_dto
    {
        public int codigo_comision_manual { get; set; }
        public string nombre_personal { get; set; }
        public string nombre_fallecido { get; set; }
        public string nro_contrato { get; set; }
        public string nombre_empresa { get; set; }
        public string fecha_registra { get; set; }
        public bool estado_registro { get; set; }
        public string nombre_estado_registro { get; set; }
        public int codigo_estado_cuota { get; set; }
        public string nombre_estado_cuota { get; set; }
        public string nombre_estado_proceso { get; set; }
        public string nro_factura_vendedor { set; get; }
        public string usuario_registra { set; get; }
    }


    public partial class comision_manual_filtro_dto
    {
        public string fecha_inicio { get; set; }
        public string fecha_fin { get; set; }
        public int codigo_empresa { get; set; }
        public int codigo_canal_grupo { get; set; }
        public string usuario { get; set; }
        public int estado_registro { get; set; }
        public string usuario_registra { get; set; }
    }


    public partial class comision_manual_reporte_dto
    {
        public string fecha_inicio { get; set; }
        public string fecha_fin { get; set; }
        public string fecha_registra { get; set; }
        public string empresa { get; set; }
        public string tipo_venta { get; set; }

        public string tipo_pago { get; set; }
        public string vendedor { get; set; }
        public string canal { get; set; }
        public string articulo { get; set; }
        public decimal comision { get; set; }

        public string nro_contrato { get; set; }
        public string nro_factura_vendedor { get; set; }
        public string usuario_registra { set; get; }
    }

    public partial class comision_manual_reporte_param_dto
    {
        public string fecha_impresion { get; set; }
        public string fechas { get; set; }
        public string usuario { get; set; }
    }
}
