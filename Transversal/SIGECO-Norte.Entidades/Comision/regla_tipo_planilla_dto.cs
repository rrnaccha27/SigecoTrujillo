using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIGEES.Entidades
{
    public partial class regla_tipo_planilla_dto
    {
        public int codigo_regla_tipo_planilla { set; get; }
        public int codigo_tipo_planilla { get; set; }
        public string nombre { set; get; }
        public string descripcion { set; get; }
        public bool estado_registro { set; get; }
        public DateTime fecha_registra { set; get; }
        public DateTime? fecha_modifica { set; get; }
        public string usuario_registra { set; get; }
        public string usuario_modifica { set; get; }
        public string nombre_usuario { set; get; }
        public bool afecto_doc_completa { set; get; }
        public int codigo_tipo_reporte { set; get; }
        public bool detraccion_contrato { set; get; }
        public bool envio_liquidacion { set; get; }
    }


    public partial class grilla_regla_tipo_planilla_dto
    {
        public int codigo_regla_tipo_planilla { set; get; }
        public string nombre { set; get; }
        public string descripcion { set; get; }
        public bool estado_registro { set; get; }
        public string str_fecha_registra { set; get; }
        public string str_fecha_modifica { set; get; }
        public string usuario_registra { set; get; }
        public string usuario_modifica { set; get; }

        public string str_estado_registro { get; set; }

        public string nombre_tipo_planilla { get; set; }
        public string afecto_doc_completa { get; set; } 
    }

    public partial class combo_regla_tipo_planilla_dto
    {
        public int id { set; get; }
        public string text { set; get; }
      
    }
}
