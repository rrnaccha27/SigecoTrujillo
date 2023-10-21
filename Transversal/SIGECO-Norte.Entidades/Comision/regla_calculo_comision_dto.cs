using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIGEES.Entidades
{
    public  class regla_calculo_comision_dto:Auditoria_dto
    {
        public int codigo_regla { get; set; }
        public int codigo_precio { get; set; }
        public int codigo_canal { get; set; }
        public int codigo_tipo_pago { get; set; }
        public int codigo_tipo_comision { get; set; }
        public decimal valor { get; set; }
        public DateTime vigencia_inicio { get; set; }
        public DateTime vigencia_fin { get; set; }        

        public string str_vigencia_inicio { get; set; }

        public string str_vigencia_fin { get; set; }

        public string nombre_tipo_venta { get; set; }

        public int actualizado { get; set; }
    }
}
