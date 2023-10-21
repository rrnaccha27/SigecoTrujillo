using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIGEES.Entidades
{
    public partial class regla_bono_trimestral_detalle_dto
    {
        public int codigo_regla_detalle { get; set; }
        public int codigo_regla { get; set; }
        public int codigo_canal { get; set; }
        public int codigo_empresa { get; set; }
        public int codigo_tipo_venta { get; set; }
        public int codigo_campo_santo { get; set; }
    }

    public partial class grilla_regla_bono_trimestral_detalle_dto
    {
        public int codigo_regla_detalle { get; set; }
        public int codigo_regla { get; set; }
        public int codigo_canal { get; set; }
        public string codigo_empresa { get; set; }
        public string codigo_tipo_venta { get; set; }
        public string usuario_registra { get; set; }
        public string nombre_estado_registro { get; set; }
    }
}
