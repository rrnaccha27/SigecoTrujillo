using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIGEES.Entidades
{
    public partial class detalle_regla_tipo_planilla_dto
    {
        public int codigo_detalle_regla_tipo_planilla { get; set; }
        public int codigo_regla_tipo_planilla { get; set; }
        public int codigo_canal { get; set; }
        public int codigo_empresa { get; set; }
        public int codigo_tipo_venta { get; set; }
        public int codigo_campo_santo { get; set; }
    }


    public partial class grilla_detalle_regla_tipo_planilla_dto
    {
        public int codigo_detalle_regla_tipo_planilla { get; set; }
        public int codigo_regla_tipo_planilla { get; set; }
        public int codigo_canal { get; set; }
        public string codigo_empresa { get; set; }
        public string codigo_tipo_venta { get; set; }
        public string codigo_campo_santo { get; set; }

        /*
        public string[] array_codigo_empresa { get; set; }
        public string[] array_codigo_tipo_venta { get; set; }
        public string[] array_codigo_campo_santo { get; set; }   */


        public string usuario_registra { get; set; }

        public string nombre_estado_registro { get; set; }
    }
}
